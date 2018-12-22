using MapraiScheduler.Exception;
using MapraiScheduler.Models.Database;
using MapraiScheduler.Models.DTO;
using MapraiScheduler.Notifier;
using MapraiScheduler.Repositories.Contracts;
using MD.PersianDateTime;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        //TESTED AND IT IS WOTKING FINE
        public async Task<List<Project>> GetValidActiveProjects(string uniqueName)
        {
            try
            {
                var notify = await _notifyTypeRepository.GetAsync(uniqueName);
                return await _mapRaiContex.Projects.Where(item => item.ProjectPhaseID != 1 && item.ProjectPhaseID != 3)
                    .GroupJoin(_mapRaiContex.Notifiers, p => p.ProjectID, n => n.ProjectID,
                        (p, n) => new { Project = p, Notify = n })
                    .Where(item => !item.Notify.Any() || item.Notify.All(n => n.NotifyTypeID != notify.NotifyTypeID))
                    .Distinct().Select(item => item.Project).ToListAsync();
            }
            catch (System.Exception e)
            {
                throw new AppDefaultException(e, "GetInvalidPhonesNotifies");
            }
        }

        //TESTED AND IT IS WOTKING FINE
        public IQueryable<Project> GetAll()
        {
            return _mapRaiContex.Projects;
        }

        //TESTED AND IT IS WOTKING FINE
        public async Task<List<NotifyDTO>> GetInvalidProjectsNotifies(List<long> projectIds, string notifyUniqueName)
        {
            try
            {
                var notify = await _notifyTypeRepository.GetAsync(notifyUniqueName);
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
                        ProjectDetail = item.StartStation.StationName.ToString()
                                        + " - " + item.EndStation.StationName.ToString(),
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
                        ProjectAdminDetail = $"{item.User.FullName} - {item.User.PhoneNumber.ToString()}",
                        ProjectPhoneDetailUrl = NotifySetting.EmailStatics.PhoneBaseUrl + item.Phone.PhoneID.ToString(),
                        OrganizationID = item.Depo.OrganPositionID
                    }).Where(project => projectIds.Any(id => id == project.ProjectID)).ToListAsync();
                notifyDtos.ForEach(item =>
                {
                    item.MergeWithNotifyType(notify);
                    item.PersianDateTime = (new PersianDateTime(DateTime.Now)).ToLongDateString();
                });
                return notifyDtos;
            }
            catch (System.Exception e)
            {
                throw new AppDefaultException(e, "GetInvalidPhonesNotifies");
            }
        }

        //TESTED AND IT IS WOTKING FINE
        public async Task StopProjectsAsync(List<Project> invalidProjects)
        {
            try
            {
                foreach (var project in invalidProjects)
                {
                    project.ProjectPhaseID = 3;
                    project.FinishDate = DateTime.Now;
                }
                await _mapRaiContex.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                throw new AppDefaultException(e, "StopProjectsAsync");
            }
        }

        //TESTED AND IT IS WOTKING FINE
        public async Task<List<Project>> GetEmptyReportProjects()
        {
            try
            {
                return
                    await _mapRaiContex.Projects.GroupJoin(_mapRaiContex.Notifiers, p => p.ProjectID, n => n.ProjectID,
                            (p, n) => new { Project = p, Notify = n })
                        .Where(item => !item.Notify.Any())
                        .Where(
                            item =>
                                item.Project.FinishDate != null &&
                                (DateTime.Now - item.Project.FinishDate.Value).Days >= 1 &&
                                (item.Project.ProjectPhaseID == 1 || item.Project.ProjectPhaseID == 3)
                                && (item.Project.ReportRawFileID == 0 || item.Project.ReportFileID == 0))
                        .Distinct().Select(item => item.Project).ToListAsync();
            }
            catch (System.Exception e)
            {
                throw new AppDefaultException(e, "GetEmptyReportProjects");
            }
        }

        //TESTED AND IT IS WOTKING FINE
        public async Task<List<Project>> GetEmptyDamageReportProjects(int level)
        {
            try
            {
                //get notify type
                var notify = await _notifyTypeRepository.GetAsync($"LateDamageReport{level}");

                return await _mapRaiContex.Projects.Where(item => (item.ProjectPhaseID == 1 || item.ProjectPhaseID == 3))
                    .Join(_mapRaiContex.DamageReports,
                        p => p.ProjectID, d => d.ProjectID, (p, n) => new { Project = p, DamageReport = n })
                        .Where(item => (DateTime.Now - item.DamageReport.CreationDate).TotalDays >= 30)
                        .Where(item => (item.DamageReport.LetterFileID == 0 && item.DamageReport.DamageReportLevel == level)
                        && item.DamageReport.IsFinished == 0 && item.DamageReport.IsArchive == 0)
                        .GroupJoin(_mapRaiContex.Notifiers, p => p.Project.ProjectID, n => n.ProjectID,
                        (p, n) => new { p.Project, p.DamageReport, Notify = n })
                        .Where(item => !item.Notify.Any() || item.Notify.All(n => n.NotifyTypeID != notify.NotifyTypeID))
                    .Distinct().Select(item => item.Project).ToListAsync();
            }
            catch (System.Exception e)
            {
                throw new AppDefaultException(e, "GetEmptyDamageReportProjects");
            }
        }
    }
}