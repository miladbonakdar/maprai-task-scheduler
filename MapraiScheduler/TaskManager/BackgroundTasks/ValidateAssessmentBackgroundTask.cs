using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using MapraiScheduler.TaskManager.Commands;
using MapraiScheduler.TaskManager.IBackgroundTasks;

namespace MapraiScheduler.TaskManager.BackgroundTasks
{
    public class ValidateAssessmentBackgroundTask : BackgroundTask, IValidateAssessmentBackgroundTask
    {
        public string Interval { get; } = Cron.Daily();

        public ValidateAssessmentBackgroundTask() : base()
        {
        }

        public override void InitCommands()
        {
            base.InitCommands();
        }

        public override void Setup()
        {
            RecurringJob.AddOrUpdate(() => HandleValidateAssessment(), Interval);
        }

        public void HandleValidateAssessment()
        {
            foreach (ICommand backgroundCommand in BackgroundCommands)
            {
                backgroundCommand.Execute().GetAwaiter().GetResult();
            }
        }
    }
}