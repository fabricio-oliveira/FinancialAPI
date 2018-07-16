
using Microsoft.EntityFrameworkCore.Storage;

namespace FinancialApi.Repositories
{
    public interface IRepository<T>
    {
        T Find(int id);
        void Save(T t);
        void Update(T t);
        IDbContextTransaction BeginTransaction();
        void Commit();
        void Rollback();
    }
}
