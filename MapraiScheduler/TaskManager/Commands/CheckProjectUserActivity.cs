﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Models.Database;
using MapraiScheduler.Models.DTO;
using MapraiScheduler.Notifier;
using MapraiScheduler.Repositories;
using MapraiScheduler.TaskManager.Commands.Action;

namespace MapraiScheduler.TaskManager.Commands
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
        private readonly int _invalidUserMiniutesThreshhold = 4 * 24 * 60;

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
        }

        public async Task Execute()
        {
            _invalidPhones = await GetInvalidPhones();
            await NotifyProblems();
            CreateActions();
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
            return notifies;
        }

        public async Task RunActions()
        {
            foreach (var commandAction in CommandActions)
            {
                await commandAction.Run();
            }
        }

        private async Task<List<Phone>> GetInvalidPhones()
        {
            var phones = await _phoneRepository.GetInvalidPhones(_invalidUserMiniutesThreshhold);
            return phones;
        }
    }
}