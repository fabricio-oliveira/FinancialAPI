using System.Collections.Generic;
using System.Linq;
using FinancialApi.Config;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public abstract class GenericRepository
    {
        private readonly DataBaseContext _databaseContext;

        public GenericRepository(DataBaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public void BeginTransaction()
        {
            this._databaseContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            this._databaseContext.Database.CommitTransaction();
        }

        public void Rollback()
        {
            this._databaseContext.Database.RollbackTransaction();
        }
    }
}
