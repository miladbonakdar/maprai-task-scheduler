using Hangfire;
using MapraiScheduler.Exception;
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
            try
            {
                RecurringJob.AddOrUpdate(() => HandleValidateReports(), Interval);
            }
            catch (System.Exception exception)
            {
                throw new AppDefaultException(exception);
            }
        }

        public void HandleValidateReports()
        {
            try
            {
                foreach (ICommand backgroundCommand in BackgroundCommands)
                    backgroundCommand.Execute().GetAwaiter().GetResult();
            }
            catch (System.Exception exception)
            {
                throw new AppDefaultException(exception);
            }
        }
    }
}