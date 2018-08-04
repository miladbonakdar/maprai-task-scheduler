﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Models.Database;
using MapraiScheduler.Models.DTO;

namespace MapraiScheduler.Repositories
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetValidActiveProjects(string uniqueName);

        IQueryable<Project> GetAll();

        Task<List<NotifyDTO>> GetInvalidProjectsNotifies(List<long> projectIds, string notifyUniqueName);

        Task StopProjects(List<Project> invalidProjects);
    }
}