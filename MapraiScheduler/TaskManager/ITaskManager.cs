using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.TaskManager
{
    public interface ITaskManager
    {
        ITaskManager StartBackgroundTasks();
    }
}