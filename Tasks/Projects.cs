using System;
using System.Collections.Generic;
using System.Linq;

namespace Tasks
{
    internal class Projects
    {
        private readonly IDictionary<string, Project> _projects = new Dictionary<string, Project>();

        public void Add(string name)
        {
            _projects.Add(name, new Project());
        }

        public void PrintInto(IConsole console)
        {
            foreach (var project in _projects)
            {
                var projectKey = project.Key;
                console.WriteLine(projectKey);
                var projectValue = project.Value;
                projectValue.PrintInto(console);
                console.WriteLine();
            }
        }

        public void AddTaskToProject(string projectName, long taskIdentifier, string taskDescription, bool taskDone, IConsole console)
        {
            if (!_projects.TryGetValue(projectName, out Project project))
            {
                console.WriteLine($"Could not find a project with the name \"{projectName}\".");
                return;
            }

            project.Add(taskIdentifier, taskDescription, taskDone);
        }

        public void SetTaskDone(string taskIdentifier, bool done, IConsole console)
        {
            foreach (var project in _projects.Values)
                project.SetDoneIfExists(taskIdentifier, done, console);
        }
    }
}
