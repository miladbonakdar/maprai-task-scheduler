using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Notifier;
using MapraiScheduler.TaskManager.Commands.Action;

namespace MapraiScheduler.TaskManager.Commands
{
    public interface ICommand
    {
        List<IAction> CommandActions { get; set; }
        List<INotifier> Notifiers { get; }

        void CreateActions();

        Task Execute();

        Task NotifyProblems();

        Task RunActions();
    }
}