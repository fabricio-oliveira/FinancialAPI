using FinancialApi.Config;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public class InterestRepository : GenericRepository, IInterestRepository
    {
        readonly DataBaseContext context;

        public InterestRepository(DataBaseContext context) : base(context)
        {
            this.context = context;
        }

        public void Save(Interest interest)
        {
            context.Interests.Add(interest);
            context.SaveChanges();
        }

        public void Update(Interest interest)
        {
            context.Interests.Update(interest);
            context.SaveChanges();
        }

        public Interest Find(int id)
        {
            return context.Interests.Find(id);
        }
    }
}
