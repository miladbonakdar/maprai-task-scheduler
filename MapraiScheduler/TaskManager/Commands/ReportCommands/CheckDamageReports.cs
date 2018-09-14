using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Models.Database;
using MapraiScheduler.Notifier;
using MapraiScheduler.Repositories;

namespace MapraiScheduler.TaskManager.Commands.ReportCommands
{
    public class CheckDamageReports : ICheckDamageReports
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        public List<IAction> CommandActions { get; set; }
        public List<INotifier> Notifiers { get; }
        private List<Project> _invalidReport1Projs;
        private List<Project> _invalidReport2Projs;
        private List<Project> _invalidReport3Projs;

        public CheckDamageReports(IProjectRepository projectRepository, IUserRepository userRepository,
            IAppNotifier appNotifier, IEmailNotifier emailNotifier, ISmsNotifier smsNotifier)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            Notifiers = new List<INotifier> { appNotifier, emailNotifier/*, smsNotifier */};
            //TODO: add action like this => CommandActions = new List<IAction> { stopProjectsAction };
        }

        public void CreateActions()
        {
            throw new NotImplementedException();
        }

        public async Task Execute()
        {
            _invalidReport1Projs = await _projectRepository.GetEmptyDamageReportProjects(1);
            _invalidReport2Projs = await _projectRepository.GetEmptyDamageReportProjects(2);
            _invalidReport3Projs = await _projectRepository.GetEmptyDamageReportProjects(3);
            await NotifyProblems();
            //CreateActions();
            //await RunActions();
        }

        public Task NotifyProblems()
        {
            throw new NotImplementedException();
        }

        public Task RunActions()
        {
            throw new NotImplementedException();
        }
    }
}