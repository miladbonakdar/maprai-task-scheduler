using MapraiScheduler.Exception;
using MapraiScheduler.Models.Database;
using MapraiScheduler.Models.DTO;
using MapraiScheduler.Notifier;
using MapraiScheduler.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.TaskManager.Commands.ProjectCommands
{
    public class CheckProjectUserActivity : ICheckProjectUserActivity
    {
        ///related tables
        /// project alarm
        /// no project alarm
        /// project event
        /// no project event
        /// better way is using last user modifie
        ///

        private readonly IPhoneRepository _phoneRepository;
        private readonly IUserRepository _userRepository;
        public List<IAction> CommandActions { get; set; }
        public List<INotifier> Notifiers { get; }
        private List<Phone> _invalidPhones;
        private readonly int _invalidUserMiniutesThreshhold = NotifySetting.InvalidUserMiniutesThreshhold;

        public CheckProjectUserActivity(IPhoneRepository phoneRepository, IUserRepository userRepository,
            IAppNotifier appNotifier, IEmailNotifier emailNotifier, ISmsNotifier smsNotifier)
        {
            _phoneRepository = phoneRepository;
            _userRepository = userRepository;
            Notifiers = new List<INotifier> { appNotifier, emailNotifier/*, smsNotifier */};
            CommandActions = new List<IAction>();
        }

        public void CreateActions()
        {
            throw new AppDefaultException(new NotImplementedException(), "CreateActions");
        }

        public async Task Execute()
        {
            _invalidPhones = await GetInvalidPhones();
            await NotifyProblems();
            //CreateActions();
            await RunActions();
        }

        public async Task NotifyProblems()
        {
            var notify = await _phoneRepository.GetInvalidPhonesNotifies(_invalidPhones.Select(item => item.PhoneID).ToList()
            , NotifySetting.NotifyTypeUniqueName.NoUserActivity);
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
                if (notifyDtos.OrganizationID != null)
                {
                    var users = await _userRepository.GetRelatedUsers(notifyDtos.OrganizationID.Value);
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
                                UserID = user.UserID,
                                OrganizationID = notifyDtos.OrganizationID,
                                PhoneNumber = notifyDtos.PhoneNumber,
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
            }
            return notifies;
        }

        public async Task RunActions()
        {
            foreach (var commandAction in CommandActions)
            {
                await commandAction.RunAsync();
            }
        }

        private async Task<List<Phone>> GetInvalidPhones()
        {
            var phones = await _phoneRepository.GetInvalidPhones(_invalidUserMiniutesThreshhold);
            return phones;
        }
    }
}