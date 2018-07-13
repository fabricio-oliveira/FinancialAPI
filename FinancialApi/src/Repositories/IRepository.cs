using System.Collections.Generic;


namespace FinancialApi.Repositories
{
    public interface IRepository<T>
    {
        T Find(long id);
        void Save(T t);
        void Update(T t);
    }
}
