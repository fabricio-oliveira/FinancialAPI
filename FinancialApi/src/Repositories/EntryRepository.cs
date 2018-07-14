using System.Collections.Generic;
using FinancialApi.Config;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public class EntryRepository: GenericRepository, IRepository<Entry>
    {
        private DataBaseContext _context;

        public EntryRepository(DataBaseContext context):base(context)
        {
            _context = context;
        }

        public void Save(Entry entry, bool commit = true)
        {
            _context.Entrys.Add(entry);
            if (commit) _context.SaveChanges();
        }

        public void Update(Entry entry, bool commit = true)
        {
            _context.Entrys.Update(entry);
            if (commit) _context.SaveChanges();
        }

        public Entry Find(long id)
        {
            return _context.Entrys.Find(id);
        }
    }
}
