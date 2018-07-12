using System.Collections.Generic;


namespace FinancialApi.Repositories
{
    public interface IRepository<T>
    {
        void Save(T t);
        IEnumerable<T> List();
    }
}
