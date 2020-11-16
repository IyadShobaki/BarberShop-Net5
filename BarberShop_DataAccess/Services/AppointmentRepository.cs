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
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _db;

        public AppointmentRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IList<Appointment>> GetAll()
        {
            List<Appointment> appointments =
               await _db.Appointments
                        .AsNoTracking() // To not include the list of appointment inside Customer class
                        .Include(c => c.Customer)
                        .AsNoTracking() // To not include the list of appointment inside SalonService class
                        .Include(s => s.SalonService)
                        .ToListAsync();
            return appointments;
        }

        public async Task<Appointment> GetById(int id)
        {
            var appointment = await _db.Appointments//.FindAsync(id)
                         .AsNoTracking() // Its not tracking anyways but just in case
                         .Include(c => c.Customer)
                         .AsNoTracking()
                         .Include(s => s.SalonService)
                         .FirstOrDefaultAsync(a => a.Id == id);
            return appointment;
        }
        public async Task<Appointment> CreateUpdate(Appointment entity)
        {
            if (entity.Id < 1)
            {              
                try
                {
                    await _db.Appointments.AddAsync(entity);
                    if (!await SaveChanges())
                    {
                        return null;
                    }

                    var appointmentInfo = await GetAppointmentInfo(entity.Id);

                    return appointmentInfo;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                _db.Appointments.Update(entity);
                if (!await SaveChanges())
                {
                    return null;
                }
                // Return the appointment with the information
                var appointmentInfo = await GetAppointmentInfo(entity.Id);

                return appointmentInfo;
            }
            
        }

        public async Task<bool> Delete(Appointment entity)
        {
            _db.Appointments.Remove(entity);
            return await SaveChanges();
        }
       
        public async Task<bool> SaveChanges()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }
        public async Task<bool> IsExists(int id)
        {
            var isExists = await _db.Appointments.AnyAsync(p => p.Id == id);
            return isExists;
        }

        public async Task<bool> IsDateTimeAvailable(DateTime dateTime)
        {
            var isDateTimeAvailable = await _db.Appointments.AnyAsync(a => a.AppointmentDate == dateTime);
            return isDateTimeAvailable;
        }
        private async Task<Appointment> GetAppointmentInfo(int id)
        {
            // Return the appointment with the information
            var appointmentInfo = await _db.Appointments
                             .Include(c => c.Customer)
                             .Include(s => s.SalonService)
                             .FirstOrDefaultAsync(a => a.Id == id);

            return appointmentInfo;
        }
    }
}
