using MapraiScheduler.Models.Database;
using MapraiScheduler.Models.DTO;
using MapraiScheduler.Notifier;
using MapraiScheduler.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task NotifyProblems()
        {
            var notify1 = await _projectRepository.GetInvalidProjectsNotifies(_invalidReport1Projs.Select(item => item.ProjectID).ToList(),
                   NotifySetting.NotifyTypeUniqueName.LateDamageReport1);

            var notify2 = await _projectRepository.GetInvalidProjectsNotifies(_invalidReport2Projs.Select(item => item.ProjectID).ToList(),
                   NotifySetting.NotifyTypeUniqueName.LateDamageReport2);

            var notify3 = await _projectRepository.GetInvalidProjectsNotifies(_invalidReport3Projs.Select(item => item.ProjectID).ToList(),
                   NotifySetting.NotifyTypeUniqueName.LateDamageReport3);
            var notifyDtos = await GenerateNotifies(notify1);
            foreach (var notifier in Notifiers)
            {
                notifier.CreateNotifyRange(notifyDtos).SendNotifyRange();
            }
            notifyDtos = await GenerateNotifies(notify2);
            foreach (var notifier in Notifiers)
            {
                notifier.CreateNotifyRange(notifyDtos).SendNotifyRange();
            }
            notifyDtos = await GenerateNotifies(notify3);
            foreach (var notifier in Notifiers)
            {
                notifier.CreateNotifyRange(notifyDtos).SendNotifyRange();
            }
        }

        private async Task<List<NotifyDTO>> GenerateNotifies(List<NotifyDTO> baseNotifyDtos)
        {
            var notifies = new List<NotifyDTO>();
            foreach (var notifyDtos in baseNotifyDtos)
            {
                var users = await _userRepository.GetRelatedUsers(notifyDtos.OrganizationID);
                notifies.AddRange(users.Select(user =>
                        new NotifyDTO
                        {
                            PersianDateTime = notifyDtos.PersianDateTime,
                            NotifyID = notifyDtos.NotifyID,
                            PhoneID = notifyDtos.PhoneID,
                            EventDescription = notifyDtos.EventDescription,
                            PriorityName = notifyDtos.PriorityName,
                            Priority = notifyDtos.Priority,
                            ProjectID = notifyDtos.ProjectID,
                            UserID = notifyDtos.UserID,
                            PhoneNumber = notifyDtos.PhoneNumber,
                            OrganizationID = notifyDtos.OrganizationID,
                            NotifyTypeID = notifyDtos.NotifyTypeID,
                            NotifyColor = notifyDtos.NotifyColor,
                            ProjectDetailUrl = notifyDtos.ProjectDetailUrl,
                            ProjectPhoneDetailUrl = notifyDtos.ProjectPhoneDetailUrl,
                            ProjectDetail = notifyDtos.ProjectDetail,
                            Seen = notifyDtos.Seen,
                            ProjectAdminDetail = notifyDtos.ProjectAdminDetail,
                            ProjectPhoneDetail = notifyDtos.ProjectPhoneDetail,
                            ToEmail = user.Email
                        }).ToList()
                    );
            }

            return notifies;
        }

        public Task RunActions()
        {
            throw new NotImplementedException();
        }
    }
}