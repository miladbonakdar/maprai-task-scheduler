using MapraiScheduler.Exception;
using MapraiScheduler.TaskManager.Commands;
using MapraiScheduler.TaskManager.IBackgroundTasks;
using System.Collections.Generic;

namespace MapraiScheduler.TaskManager.BackgroundTasks
{
    public abstract class BackgroundTask : IBackgroundTask
    {
        protected List<ICommand> BackgroundCommands;

        protected BackgroundTask()
        {
            InitCommands();
        }

        public virtual void Setup()
        {
            if (BackgroundCommands == null || BackgroundCommands.Count == 0)
                throw new BackgroundTaskException("BackgroundCommands is empty make sure you created the commands first");
        }

        public void InitCommands()
        {
            BackgroundCommands = new List<ICommand>
            {
                new LogCommand(GetType())
            };
        }
    }
}