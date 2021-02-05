using System;
using System.Collections.Generic;
using System.Linq;
using AutomationEngine;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify;
using E3kWorkReports.Clockify.DomainModel;
using E3kWorkReports.Clockify.Requests;

// ReSharper disable StringLiteralTypo

namespace E3kWorkReports
{
    public class ClockifyWorkEntriesProvider : IWorkEntriesProvider
    {
        private static readonly List<object> Cache = new List<object>();

        private RestClient RestClient { get; } = new RestClient(new ClockifyRestApiConfiguration());

        private const string ChargeableNamePart = " - Chargeable";

        private DateTime Start => DateTime.Now.GetPreviousMonthStart();

        private DateTime End => DateTime.Now.GetPreviousMonthEnd();

        public IEnumerable<ReportEntry> GetAllEntries()
        {
            var projectIds = GetProjectIds();
            var workspace = GetWorkspace();

            var request = new DetailedReportRequest(workspace.Id)
            {
                Start = Start,
                End = End,
                Projects = new EntityFilter(projectIds)
            };

            DetailedReport result;
            do
            {
                request.DetailedFilter.Page += 1;

                result = SendRequest(request);

                foreach (var timeEntry in result.TimeEntries)
                {
                    yield return new ReportEntry
                    {
                        Date = timeEntry.TimeInterval.Start.Date,
                        Description = GetTaskDescription(timeEntry),
                        EmployeeFullName = timeEntry.UserName,
                        Chargeable = IsChargeable(timeEntry),
                        Hours = timeEntry.TimeInterval.Hours,
                        Task = timeEntry.TaskName,
                        ProjectCode = GetProjectCode(timeEntry)
                    };
                }
            } while (result.IsWholePage(request));
        }

        private bool IsChargeable(TimeEntry timeEntry)
        {
            return timeEntry.TaskName.Contains(ChargeableNamePart);
        }

        private string GetTaskDescription(TimeEntry timeEntry)
        {
            var description = timeEntry.Description?.Trim().ToUpper() ?? "";

            if (description == "RPV")
                return "XLAB project leaders meeting.";

            if (description == "RSTAFF")
                return "XLAB research department meeting.";

            return timeEntry.Description;
        }

        private string GetProjectCode(TimeEntry timeEntry)
        {
            var taskName = timeEntry.TaskName.Replace(ChargeableNamePart, "");

            var internalTasks = new[]
            {
                "Know how exchange",
                "Meetings"
            };

            var standardTasks = new[]
            {
                "Code review",
                "New core",
                "Old core",
                "Project management",
                "Voucher editor",
            };

            if (standardTasks.Contains(taskName))
                return "STANDARD";

            if (internalTasks.Contains(taskName))
                return "INTERNAL";

            if (taskName == "Custom developments")
                return taskName.ToUpper();

            if (taskName == "Troubleshooting")
                return taskName.ToUpper();

            throw new InvalidOperationException($"Could not map task \"{taskName}\" into appropriate project code.");
        }

        private Workspace GetWorkspace()
        {
            return Load(() =>
            {
                var result = SendRequest(new WorkspaceListRequest());
                ThrowExactlyOneExceptionIfNecessary(result);
                return result.Single();
            });
        }

        private string[] GetProjectIds()
        {
            var workspace = GetWorkspace();

            var result = SendRequest(new ProjectListRequest(workspace.Id));
            var ids = result.Where(ShouldIncludeProject).Select(_ => _.Id).ToArray();

            return ids;
        }

        private bool ShouldIncludeProject(Project project)
        {
            var projectName = project.Name.ToLower();

            if (projectName.Contains("europa3000"))
                return true;

            if (projectName.Contains("general"))
                return true;

            return false;
        }

        public TResult SendRequest<TResult>(IRequest<TResult> request)
        {
            var result = GenericMethodInvoker.Instance(this)
                .Method(nameof(SendRequest))
                .WithGenericTypes(request.GetType(), typeof(TResult))
                .WithArguments(request)
                .Invoke();

            return (TResult)result;
        }

        private TResult SendRequest<TRequest, TResult>(TRequest request) where TRequest : IRequest<TResult>
        {
            return RestClient.SendRequest<TRequest, TResult>(request);
        }

        private T Load<T>(Func<T> func) where T : class
        {
            lock (Cache)
            {
                T instance = Cache.OfType<T>().FirstOrDefault();

                if (instance == null)
                {
                    instance = func();
                    Cache.Add(instance);
                }

                return instance;
            }
        }

        private void ThrowExactlyOneExceptionIfNecessary<T>(List<T> candidates, string text = null)
        {
            if (candidates.Count == 1)
                return;

            var typeName = typeof(T).Name;

            var message = $"There should be exacly one {typeName}{(text == null ? "" : $" with \"{text}\"")}, ";

            if (candidates.Count == 0)
                message += "but found none.";
            else
                message += "but found multiple.";

            if (text != null)
                message = $"{message} \"{text}\"";

            throw new InvalidOperationException(message);
        }

    }
}
