using System.Linq;
using FinancialApi.Config;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public class InterestRepository : GenericRepository, IInterestRepository
    {
        readonly DataBaseContext _context;

        public InterestRepository(DataBaseContext context) : base(context) => _context = context;

        public void Save(Interest interest)
        {
            _context.Interests.Add(interest);
            _context.SaveChanges();
        }

        public void Update(Interest interest)
        {
            _context.Interests.Update(interest);
            _context.SaveChanges();
        }

<<<<<<< HEAD
        public Interest Find(long? id) => _context.Interests.Find(id);
=======
        public Interest Find(long? id)
        {
            return _context.Interests.Find(id);
        }
>>>>>>> 0c62ff5d5e8d523918c08c5c5617e23bf795b704

        public long Count()
        {
            return _context.Interests.Count();
        }
    }
}
