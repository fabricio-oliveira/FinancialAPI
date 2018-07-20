using FinancialApi.Config;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinancialApi.Repositories
{
    public abstract class GenericRepository
    {
        readonly DataBaseContext _databaseContext;

        protected GenericRepository(DataBaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _databaseContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            _databaseContext.Database.CommitTransaction();
        }

        public void Rollback()
        {
            _databaseContext.Database.RollbackTransaction();
        }
    }
}
