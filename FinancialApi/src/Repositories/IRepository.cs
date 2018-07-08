using System;
using System.Collections.Generic;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public interface IRepository<Entry>
    {
        void Save(Entry entry);
        IEnumerable<Entry> List();
    }
}
