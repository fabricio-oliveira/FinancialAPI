using System.Collections.Generic;
using System.Linq;
using FinancialApi.Config;
using FinancialApi.Models.Entity;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinancialApi.Repositories
{
    public abstract class GenericRepository
    {
        private readonly DataBaseContext _databaseContext;

        protected GenericRepository(DataBaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _databaseContext.Database.BeginTransaction();
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
