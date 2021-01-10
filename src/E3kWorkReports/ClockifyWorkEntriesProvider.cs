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

        public IEnumerable<ReportEntry> GetAllEntries()
        {
            var project = GetProject();
            var workspace = GetWorkspace();

            var request = new DetailedReportRequest(workspace.Id)
            {
                Start = new DateTime(2020, 12, 1),
                End = new DateTime(2020, 12, 31),
                Projects = new EntityFilter(project.Id)
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
                        Description = timeEntry.Description,
                        EmployeeFullName = timeEntry.UserName,
                        Chargeable = IsChargeable(timeEntry),
                        Hours = timeEntry.TimeInterval.Hours,
                        Task = timeEntry.TaskName,
                        ProjectCode = GetProjectCode(timeEntry.TaskName)
                    };
                }
            } while (result.IsWholePage(request));
        }

        private bool IsChargeable(TimeEntry timeEntry)
        {
            return timeEntry.TaskName.Contains(ChargeableNamePart);
        }

        private string GetProjectCode(string taskName)
        {
            taskName = taskName.Replace(ChargeableNamePart, "");

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

        private Project GetProject()
        {
            return Load(() =>
            {
                var requestedProjectName = "europa3000";
                var workspace = GetWorkspace();

                var result = SendRequest(new ProjectListRequest(workspace.Id));

                var candidates = (
                    from project in result
                    let projectName = project.Name.ToLower()
                    where projectName == requestedProjectName
                    select project
                ).ToList();

                ThrowExactlyOneExceptionIfNecessary(candidates, $"name {requestedProjectName}");

                return candidates.Single();
            });
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
