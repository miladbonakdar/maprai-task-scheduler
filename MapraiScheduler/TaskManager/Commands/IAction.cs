using System.Threading.Tasks;

namespace MapraiScheduler.TaskManager.Commands
{
    public interface IAction
    {
        Task<IAction> RunAsync();

        IAction SetDatas(object dataToSet);
    }
}