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

        public void Save(Entry entry)
        {
            _context.Entries.Add(entry);
            _context.SaveChanges();
        }

        public void Update(Entry entry)
        {
            _context.Entries.Update(entry);
            _context.SaveChanges();
        }

        public Entry Find(long id)
        {
            return _context.Entries.Find(id);
        }
    }
}
