using MapraiScheduler.Models.Database;
using MapraiScheduler.Models.DTO;
using MapraiScheduler.Notifier;
using MapraiScheduler.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.TaskManager.Commands
{
    public class CheckVeryLateProject : ICheckVeryLateProject
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        public List<IAction> CommandActions { get; set; }
        public List<INotifier> Notifiers { get; }
        private List<Project> _invalidProjs;

        public CheckVeryLateProject(IProjectRepository projectRepository, IUserRepository userRepository,
            IAppNotifier appNotifier, IEmailNotifier emailNotifier, ISmsNotifier smsNotifier)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            Notifiers = new List<INotifier> { appNotifier, emailNotifier/*, smsNotifier*/ };
            CommandActions = new List<IAction>();
        }

        public void CreateActions()
        {
        }

        public async Task Execute()
        {
            _invalidProjs = await GetInvalidProjects();
            await NotifyProblems();
            CreateActions();
            await RunActions();
        }

        public async Task NotifyProblems()
        {
            var notify = await _projectRepository.GetInvalidProjectsNotifies(_invalidProjs.Select(item => item.ProjectID).ToList(),
                NotifySetting.NotifyTypeUniqueName.VeryLateProject);
            var notifyDtos = await GenerateNotifies(notify);
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
                            PhoneNumber = notifyDtos.PhoneNumber,
                            Priority = notifyDtos.Priority,
                            ProjectID = notifyDtos.ProjectID,
                            UserID = notifyDtos.UserID,
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

        public async Task RunActions()
        {
            foreach (var commandAction in CommandActions)
            {
                commandAction.Run();
            }
        }

        private async Task<List<Project>> GetInvalidProjects()
        {
            var projs = await _projectRepository.GetValidActiveProjects(NotifySetting.NotifyTypeUniqueName.VeryLateProject);
            if (projs.Count == 0) return projs;
            List<Project> invalids = new List<Project>();
            foreach (var project in projs)
            {
                if (project.RemainingTime != null && (2 * project.RemainingTime.Value) < (DateTime.Now - project.CreationDate).TotalMinutes)
                {
                    invalids.Add(project);
                }
            }

            return invalids;
        }
    }
}