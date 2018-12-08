using MapraiScheduler.Exception;
using MapraiScheduler.TaskManager.Commands;
using MapraiScheduler.TaskManager.IBackgroundTasks;
using System.Collections.Generic;

namespace MapraiScheduler.TaskManager.BackgroundTasks
{
    public class BackgroundTask : IBackgroundTask
    {
        protected List<ICommand> BackgroundCommands;

        public BackgroundTask()
        {
            InitCommands();
        }

        public virtual void Setup()
        {
            if (BackgroundCommands == null || BackgroundCommands.Count == 0)
                throw new BackgroundTaskException("BackgroundCommands is empty make sure you created the commands first");
        }

        public virtual void InitCommands()
        {
            BackgroundCommands = new List<ICommand>
            {
                new LogCommand(this.GetType())
            };
        }
    }
}