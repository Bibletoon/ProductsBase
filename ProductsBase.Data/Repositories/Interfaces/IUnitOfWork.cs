using System.Threading.Tasks;

namespace ProductsBase.Data.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}