using Hangfire;
using MapraiScheduler.TaskManager.Commands;
using MapraiScheduler.TaskManager.Commands.ReportCommands;

namespace MapraiScheduler.TaskManager.BackgroundTasks
{
    public class ValidateReportsBackgroundTask : BackgroundTask, IValidateReportsBackgroundTask
    {
        public string Interval { get; } = Cron.Daily();

        public ValidateReportsBackgroundTask(ICheckProjectReports checkProjectReports
            , ICheckDamageReports CheckDamageReports) : base()
        {
            InitCommands(checkProjectReports, CheckDamageReports);
        }

        public void InitCommands(ICheckProjectReports checkProjectReports
            , ICheckDamageReports CheckDamageReports)
        {
            base.InitCommands();
            BackgroundCommands.Add(checkProjectReports);
            BackgroundCommands.Add(CheckDamageReports);
        }

        public override void Setup()
        {
            RecurringJob.AddOrUpdate(() => HandleValidateReports(), Interval);
        }

        public void HandleValidateReports()
        {
            foreach (ICommand backgroundCommand in BackgroundCommands)
            {
                backgroundCommand.Execute().GetAwaiter().GetResult();
            }
        }
    }
}