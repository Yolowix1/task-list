using System;
using System.Collections.Generic;
using System.Threading;

namespace Tasks
{
	public sealed class TaskList
	{
		private const string Quit = "quit";

		private readonly Projects _projects = new ();
		private readonly IConsole _console;

		private long _lastIdentifier;

		public static void Main()
		{
			var cancel = CancellationToken.None;
			new TaskList(new RealConsole()).Run(cancel);
		}

		public TaskList(IConsole console)
		{
			this._console = console;
		}

		public void Run(CancellationToken token)
        {
            while (RunOnce())
            {
				token.ThrowIfCancellationRequested();
            }
        }

        private bool RunOnce()
        {
            _console.Write("> ");
            var command = _console.ReadLine();
            if (command == Quit) return false;

            Execute(command);

			return true;
        }

		private void Execute(string commandLine)
		{
			var commandString = " ".ToCharArray();
			var commandRest = commandLine.Split(commandString, 2);
			var command = commandRest[0];
			switch (command) {
			case "show":
				Show();
				return;
			case "add":
				Add(commandRest[1]);
				return;
			case "check":
				Check(commandRest[1]);
				return;
			case "uncheck":
				Uncheck(commandRest[1]);
				return;
			case "help":
				Help();
				return;
			}

            Error(command);
        }

        private void Show() => _projects.PrintInto(_console);

		private void Add(string commandLine)
		{
			var subCommandString = " ".ToCharArray();
			var subcommandRest = commandLine.Split(subCommandString, 2);
			var subcommand = subcommandRest[0];

			if (subcommand == "project") {
				AddProject(subcommandRest[1]);
                return;
            }
            
            if (subcommand == "task") {
				var projectTask = subcommandRest[1].Split(subCommandString, 2);
				AddTask(projectTask[0], projectTask[1]);
            }
		}

		private void AddProject(string name) => _projects.Add(name);

		private void AddTask(string project, string description)
		{
			_projects.AddTaskToProject(project,
			NextId(),
			description,
			false,
            _console
            );
        }

		private void Check(string idString)
		{
			SetDone(idString, true);
		}

		private void Uncheck(string idString)
		{
			SetDone(idString, false);
		}

        private void SetDone(string idString, bool done) => _projects.SetTaskDone(idString, done, _console);

		private void Help()
		{
			_console.WriteLine("Commands:");
			_console.WriteLine("  show");
			_console.WriteLine("  add project <project name>");
			_console.WriteLine("  add task <project name> <task description>");
			_console.WriteLine("  check <task ID>");
			_console.WriteLine("  uncheck <task ID>");
			_console.WriteLine();
		}

		private void Error(string command)
		{
			_console.WriteLine($"I don't know what the command \"{command}\" is.");
		}

		private long NextId()
		{
			return ++_lastIdentifier;
		}
	}
}
