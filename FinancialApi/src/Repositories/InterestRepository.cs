using FinancialApi.Config;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public class ChargeRepository: GenericRepository, IRepository<Interest>
    {
        private DataBaseContext _context;

        public ChargeRepository(DataBaseContext context):base(context)
        {
            _context = context;
        }

        public void Save(Interest interest)
        {
            _context.Interests.Add(interest);
            _context.SaveChanges();
        }

        public void Update(Interest interest){
            _context.Interests.Update(interest);
            _context.SaveChanges();
        }

        public Interest Find(long id)
        {
           return  _context.Interests.Find(id);
        }
    }
}
