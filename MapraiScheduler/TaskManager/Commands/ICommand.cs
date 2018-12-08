using MapraiScheduler.Notifier;
using System.Collections.Generic;
using System.Threading.Tasks;

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