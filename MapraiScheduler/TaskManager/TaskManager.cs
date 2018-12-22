using MapraiScheduler.Exception;
using MapraiScheduler.TaskManager.BackgroundTasks;
using MapraiScheduler.TaskManager.IBackgroundTasks;
using System.Collections.Generic;

namespace MapraiScheduler.TaskManager
{
    public class TaskManager : ITaskManager
    {
        private readonly List<IBackgroundTask> _backgroundTasks;

        public TaskManager(IValidateProjectBackgroundTask validateProjectBackgroundTask,
            IValidateReportsBackgroundTask validateReportsBackgroundTask)
        {
            _backgroundTasks = new List<IBackgroundTask> { validateProjectBackgroundTask, validateReportsBackgroundTask };
        }

        public ITaskManager StartBackgroundTasks()
        {
            try
            {
                if (_backgroundTasks.Count == 0)
                    throw new BackgroundTaskException("The tasks list is empty . please create some task first");
                foreach (var backgroundTask in _backgroundTasks)
                {
                    backgroundTask.Setup();
                }
            }
            catch (System.Exception e)
            {
                throw new AppDefaultException(e);
            }
            return this;
        }
    }
}