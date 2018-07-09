using System.Collections.Generic;
using FinancialApi.Models.DTO.Request;
using FinancialApi.src.Models.Entity;

namespace FinancialApi.Repositories
{
    public interface IRepository<T>
    {
        void Save(T t);
        IEnumerable<T> List();
    }
}
