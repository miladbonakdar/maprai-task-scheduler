using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Exception;
using MapraiScheduler.TaskManager.BackgroundTasks;
using MapraiScheduler.TaskManager.IBackgroundTasks;

namespace MapraiScheduler.TaskManager
{
    public class TaskManager : ITaskManager
    {
        private List<IBackgroundTask> _backgroundTasks;

        public TaskManager(IValidateProjectBackgroundTask validateProjectBackgroundTask)
        {
            _backgroundTasks = new List<IBackgroundTask> { validateProjectBackgroundTask };
        }

        public ITaskManager StartBackgroundTasks()
        {
            if (_backgroundTasks.Count == 0)
                throw new BackgroundTaskException("The tasks list is empty . please create some task first");
            foreach (var backgroundTask in _backgroundTasks)
            {
                backgroundTask.Setup();
            }
            return this;
        }
    }
}