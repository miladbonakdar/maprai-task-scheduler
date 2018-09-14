using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Models.Database;
using MapraiScheduler.Repositories;

namespace MapraiScheduler.TaskManager.Commands.Action
{
    public class StopProjectsAction : IStopProjectsAction
    {
        private List<Project> InvalidProjects { set; get; }
        private readonly IProjectRepository _projectRepository;

        public StopProjectsAction(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public IAction SetDatas(object dataToSet)
        {
            InvalidProjects = (List<Project>)dataToSet;
            return this;
        }

        public async Task<IAction> Run()
        {
            await _projectRepository.StopProjects(InvalidProjects);
            return this;
        }
    }
}