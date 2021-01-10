using System;
using System.Diagnostics;
using System.Linq;
using AutomationEngine;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify;
using E3kWorkReports.Clockify.DomainModel;
using E3kWorkReports.Clockify.Requests;

// ReSharper disable UnusedVariable
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Tests
{
    public class ClockifyRestApiTests
    {
        private RestClient RestClient { get; } = new RestClient(new ClockifyRestApiConfiguration());

        public void Run()
        {
            var taskName = "New core";
            var projectName = "europa3000";
            var workspaceName = "XLAB Research";

            var workspaceId = GetWorkspaceId(workspaceName);
            Debug.WriteLine($"\"{workspaceName}\" workspace id: \"{workspaceId}\"");

            var projectId = GetProjectId(projectName, workspaceId);
            Debug.WriteLine($"\"{projectName}\" project id: \"{projectId}\"");

            var taskId = GetTaskId(taskName, workspaceId, projectId);
            Debug.WriteLine($"\"{taskName}\" task id: \"{taskId}\"");

            var userId1 = GetUserId("Boris Turk", workspaceId);
            var userId2 = GetUserId("Andrej Bratož", workspaceId);

            //LoadTasks(workspaceId, projectId, userId2);
            LoadDetailedReport(workspaceId, projectId);
            //LoadCurrentUser();
        }

        //private void LoadTasks(string workspaceId, string projectId, string userId)
        //{
        //    var request = new TimeEntriesRequest(workspaceId, userId)
        //    {
        //        Start = new DateTime(2020, 12, 20),
        //        End = new DateTime(2020, 12, 31),
        //        Project = projectId
        //    };

        //    var result = SendRequest(request);

        //    foreach (var timeEntry in result)
        //    {
        //        Debug.WriteLine(timeEntry.Description);
        //    }
        //}

        private void LoadDetailedReport(string workspaceId, string projectId)
        {
            var request = new DetailedReportRequest(workspaceId)
            {
                Start = new DateTime(2020, 12, 1),
                End = new DateTime(2020, 12, 31),
                Projects = new EntityFilter(projectId)
            };

            DetailedReport result;
            do
            {
                request.DetailedFilter.Page += 1;

                result = SendRequest(request);

                foreach (var timeEntry in result.TimeEntries)
                {
                    Debug.WriteLine(timeEntry.Description);
                }
            } while (result.IsWholePage(request));
        }

        private string GetWorkspaceId(string workspaceName)
        {
            var result = SendRequest(new WorkspaceListRequest());
            return result.Single(_ => _.Name == workspaceName).Id;
        }

        private string GetProjectId(string projectName, string workspaceId)
        {
            var result = SendRequest(new ProjectListRequest(workspaceId));
            var project = result.Single(_ => _.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
            return project.Id;
        }

        private string GetTaskId(string taskName, string workspaceId, string projectId)
        {
            var result = SendRequest(new TaskListRequest(workspaceId, projectId));
            return result.Single(_ => _.Name.Contains(taskName)).Id;
        }

        private string GetUserId(string userName, string workspaceId)
        {
            var request = new UsersRequest(workspaceId)
            {
                Name = userName
            };

            var result = SendRequest(request);

            return result.Single().Id;
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
    }
}