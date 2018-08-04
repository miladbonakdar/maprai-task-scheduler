using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Notifier;
using MapraiScheduler.TaskManager.Commands.Action;

namespace MapraiScheduler.TaskManager.Commands
{
    public class LogCommand : ILogCommand
    {
        public string ClassName { get; set; }

        public LogCommand(string className)
        {
            ClassName = className;
        }

        public LogCommand(Type classType)
        {
            ClassName = classType.Name;
        }

        public void CreateActions()
        {
            throw new NotImplementedException();
        }

        public async Task Execute()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"executing the commands for class '{ClassName}'");
            Console.WriteLine($"executing at {DateTimeOffset.Now.ToString()}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public async Task NotifyProblems()
        {
            throw new NotImplementedException();
        }

        public async Task RunActions()
        {
            throw new NotImplementedException();
        }

        public List<IAction> CommandActions { get; set; }
        public List<INotifier> Notifiers { get; }
    }
}