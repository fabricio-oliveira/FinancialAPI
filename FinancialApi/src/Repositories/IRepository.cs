
namespace FinancialApi.Repositories
{
    public interface IRepository<T>
    {
        T Find(long id);
        void Save(T t, bool commit);
        void Update(T t, bool commit);
    }
}
