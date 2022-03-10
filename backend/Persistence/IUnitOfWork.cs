using System.Threading.Tasks;

namespace backend.Persistence
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}