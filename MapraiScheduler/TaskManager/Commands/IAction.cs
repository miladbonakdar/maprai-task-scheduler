using System.Threading.Tasks;

namespace MapraiScheduler.TaskManager.Commands
{
    public interface IAction
    {
        Task<IAction> Run();

        IAction SetDatas(object dataToSet);
    }
}