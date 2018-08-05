using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using MapraiScheduler.TaskManager.Commands;
using MapraiScheduler.TaskManager.IBackgroundTasks;

namespace MapraiScheduler.TaskManager.BackgroundTasks
{
    public class ValidateProjectBackgroundTask : BackgroundTask, IValidateProjectBackgroundTask
    {
        public int Interval { get; } = 15;// 15 minute

        public ValidateProjectBackgroundTask(ICheckAutoStopProject checkAutoStopProject,
            ICheckOutOfTimeCommand checkOutOfTimeCommand, ICheckProjectUserActivity checkProjectUserActivity,
            ICheckVeryLateProject checkVeryLateProject) : base()
        {
            InitCommands(checkAutoStopProject, checkOutOfTimeCommand, checkProjectUserActivity, checkVeryLateProject);
        }

        public void InitCommands(ICheckAutoStopProject checkAutoStopProject,
            ICheckOutOfTimeCommand checkOutOfTimeCommand, ICheckProjectUserActivity checkProjectUserActivity,
            ICheckVeryLateProject checkVeryLateProject)
        {
            base.InitCommands();
            BackgroundCommands.Add(checkAutoStopProject);
            BackgroundCommands.Add(checkOutOfTimeCommand);
            BackgroundCommands.Add(checkProjectUserActivity);
            BackgroundCommands.Add(checkVeryLateProject);
        }

        public override void Setup()
        {
            RecurringJob.AddOrUpdate(() => HandleValidateProject(), $"*/{Interval} * * * *");
        }

        public void HandleValidateProject()
        {
            foreach (ICommand backgroundCommand in BackgroundCommands)
            {
                //maybe it should be backgroundCommand.Execute().GetAwaiter().GetResult()
                backgroundCommand.Execute();
            }
        }
    }
}