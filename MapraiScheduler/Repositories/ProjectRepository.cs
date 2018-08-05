using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Models.Database;
using MapraiScheduler.Models.DTO;
using MapraiScheduler.Notifier;
using MapraiScheduler.TaskManager.Commands;
using MD.PersianDateTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace MapraiScheduler.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly INotifyTypeRepository _notifyTypeRepository;
        private readonly MapRaiContex _mapRaiContex;

        public ProjectRepository(MapRaiContex mapRaiContex, INotifyTypeRepository notifyTypeRepository)
        {
            _mapRaiContex = mapRaiContex;
            _notifyTypeRepository = notifyTypeRepository;
        }

        public async Task<List<Project>> GetValidActiveProjects(string uniqueName)
        {
            return await _mapRaiContex.Projects.Join(_mapRaiContex.Notifiers, p => p.ProjectID, n => n.ProjectID,
                    (p, n) => new { Project = p, Notify = n })
                .Join(_mapRaiContex.NotifyTypes, p => p.Notify.NotifyTypeID, n => n.NotifyTypeID,
                    (p, n) => new { p.Project, p.Notify, n.UniqueName })
                .Where(item => (item.UniqueName == null || item.UniqueName != uniqueName) &&
                               item.Project.ProjectPhaseID != 1 && item.Project.ProjectPhaseID != 3)
                .Select(item => item.Project).ToListAsync();
        }

        public IQueryable<Project> GetAll()
        {
            return _mapRaiContex.Projects;
        }

        public async Task<List<NotifyDTO>> GetInvalidProjectsNotifies(List<long> projectIds, string notifyUniqueName)
        {
            var notify = await _notifyTypeRepository.Get(notifyUniqueName);
            var notifyDtos = await _mapRaiContex.Projects.Join(_mapRaiContex.Users, p => p.UserID, u => u.UserID,
                    (p, u) => new { Project = p, User = u })
                .Join(_mapRaiContex.Phones, p => p.Project.PhoneID, ph => ph.PhoneID,
                    (p, ph) => new { p.Project, p.User, Phone = ph })
                .Join(_mapRaiContex.Stations, p => p.Project.StartStationID, s => s.StationID,
                    (p, s) => new { p.Project, p.User, p.Phone, StartStation = s })
                .Join(_mapRaiContex.Stations, p => p.Project.EndStationID, s => s.StationID,
                    (p, s) => new { p.Project, p.User, p.Phone, p.StartStation, EndStation = s })
                .Join(_mapRaiContex.Depos, p => p.Phone.DepoID, d => d.DepoID,
                    (p, d) => new { p.Project, p.User, p.Phone, p.StartStation, p.EndStation, Depo = d })
                .Select(item => new NotifyDTO
                {
                    EventDescription = "",
                    PriorityName = "",
                    NotifyID = 0,
                    NotifyTypeID = 0,
                    NotifyColor = "",
                    ProjectDetail = ((item.StartStation == null) ? string.Empty : item.StartStation.StationName)
                                     + " - " +
                                    ((item.EndStation == null) ? string.Empty : item.EndStation.StationName),
                    ToEmail = "",
                    Priority = notify.Priority,
                    PhoneID = item.Phone.PhoneID,
                    Seen = 0,
                    UserID = item.User.UserID,
                    ProjectID = item.Project.ProjectID,
                    PhoneNumber = item.Phone.PhoneNumber,
                    PersianDateTime = "",
                    ProjectPhoneDetail = $"{item.Phone.FullName} - {item.Phone.PhoneNumber.ToString()}",
                    ProjectDetailUrl = NotifySetting.EmailStatics.ProjectBaseUrl + item.Project.ProjectID.ToString(),
                    ProjectAdminDetail = $"{item.Phone.FullName} - {item.Phone.PhoneNumber.ToString()}",
                    ProjectPhoneDetailUrl = NotifySetting.EmailStatics.PhoneBaseUrl + item.Phone.PhoneID.ToString(),
                    OrganizationID = item.Depo.OrganPositionID
                }).Where(project => projectIds.Any(id => id == project.ProjectID)).ToListAsync();
            notifyDtos.ForEach(item => item.MergeWithNotifyType(notify)); ;
            notifyDtos.ForEach(item => item.PersianDateTime = (new PersianDateTime(DateTime.Now)).ToLongDateString()); ;
            return notifyDtos;
        }

        public async Task StopProjects(List<Project> invalidProjects)
        {
            foreach (var project in invalidProjects)
            {
                project.ProjectPhaseID = 3;
            }
            _mapRaiContex.SaveChangesAsync();
        }
    }
}