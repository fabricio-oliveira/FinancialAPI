
using Microsoft.EntityFrameworkCore.Storage;

namespace FinancialApi.Repositories
{
    public interface IRepository<T>
    {
        T Find(long? id);
        void Save(T t);
        void Update(T t);
        IDbContextTransaction BeginTransaction();
        void Commit();
        void Rollback();
        long Count();
    }
}
