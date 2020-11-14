using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarberShop_DataAccess.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IList<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> CreateUpdate(T entity);
        Task<bool> Delete(T entity);
        Task<bool> SaveChanges();
        Task<bool> IsExists(int id);
    }
}
