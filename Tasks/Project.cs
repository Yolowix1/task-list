
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tasks
{
    internal class Project
    {
        public Dictionary<long, dynamic> _tasksArrays = new Dictionary<long, dynamic>();

        public void Add(long taskIdentifier, string taskDescription, bool taskDone)
        {
            dynamic d1 = new System.Dynamic.ExpandoObject();
            d1.description = taskDescription;
            d1.done = taskDone;

            _tasksArrays.Add(taskIdentifier, d1);
        }

        public void PrintInto(IConsole console)
        {
            foreach (var task in _tasksArrays)
            {
                var taskValue = task.Value;
                var taskKey = task.Key;
                var taskDescrition = taskValue.description;
                var taskDone = taskValue.done;
                
                console.WriteLine($"    [{(taskDone ? 'x' : ' ')}] {taskKey}: {taskDescrition}");
            }
        }

        public void SetDoneIfExists(string identifier, bool done, IConsole console)
        {
            long parseIdentifier = long.Parse(identifier);
            var identifiedTask = _tasksArrays
                .FirstOrDefault(task => task.Key == parseIdentifier);

            var identifiedTaskValue = identifiedTask.Value;
            identifiedTaskValue.done = done;
        }
    }
}
