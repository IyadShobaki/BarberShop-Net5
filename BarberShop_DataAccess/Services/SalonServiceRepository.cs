using BarberShop_DataAccess.Contracts;
using BarberShop_DataAccess.Data;
using BarberShop_Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarberShop_DataAccess.Services
{
    public class SalonServiceRepository : ISalonServiceRepository
    {
        private readonly ApplicationDbContext _db;

        public SalonServiceRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IList<SalonService>> GetAll()
        {
            List<SalonService> services = await _db.SalonServices.ToListAsync();
            return services;
        }

        public async Task<SalonService> GetById(int id)
        {
            var service = await _db.SalonServices.FindAsync(id);
            return service;
        }
        public async Task<SalonService> CreateUpdate(SalonService entity)
        {
            if (entity.Id < 1)
            {
                try
                {
                    await _db.SalonServices.AddAsync(entity);
                    if (!await SaveChanges())
                    {
                        return null;
                    }
                    return entity;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                _db.SalonServices.Update(entity);
                if (!await SaveChanges())
                {
                    return null;
                }
                return entity;
            }
        }

        public async Task<bool> Delete(SalonService entity)
        {
            _db.SalonServices.Remove(entity);
            return await SaveChanges();
        }
        public async Task<bool> SaveChanges()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }
        public async Task<bool> IsExists(int id)
        {
            var isExists = await _db.SalonServices.AnyAsync(p => p.Id == id);
            return isExists;
        }

      
    }
}
