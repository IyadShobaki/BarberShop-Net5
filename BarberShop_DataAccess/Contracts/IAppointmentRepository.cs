using BarberShop_Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarberShop_DataAccess.Contracts
{
    public interface IAppointmentRepository : IRepositoryBase<Appointment>
    {
        Task<bool> IsDateTimeAvailable(DateTime dateTime);
    }
}
