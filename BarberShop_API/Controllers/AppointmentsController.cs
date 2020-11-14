using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BarberShop_DataAccess.Data;
using BarberShop_Models.DTOs;
using BarberShop_Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public AppointmentsController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {           
            List<Appointment> appointments =
                await _db.Appointments
                         .AsNoTracking() // To not include the list of appointment inside Customer class
                         .Include(c => c.Customer)
                         .AsNoTracking() // To not include the list of appointment inside SalonService class
                         .Include(s => s.SalonService)
                         .ToListAsync();

            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _db.Appointments//.FindAsync(id)
                         .AsNoTracking() // Its not tracking anyways but just in case
                         .Include(c => c.Customer)
                         .AsNoTracking()
                         .Include(s => s.SalonService)
                         .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUpdate([FromBody] AppointmentDTO appointmentDTO)
        {
            if (appointmentDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isDateTimeAvailable = await _db.Appointments.AnyAsync(a => a.AppointmentDate == appointmentDTO.AppointmentDate);
            if (isDateTimeAvailable)
            {
                return BadRequest(ModelState);
            }
            var appointment = _mapper.Map<Appointment>(appointmentDTO);
            if (appointment.Id == 0)
            {
                await _db.Appointments.AddAsync(appointment);
            }
            else
            {
                _db.Appointments.Update(appointment);
            }

            var changes = await _db.SaveChangesAsync();
            if (changes < 1)
            {
                return StatusCode(500);
            }

            // Return the appointment with the information
            var appointmentWithInfo = await _db.Appointments
                         .Include(c => c.Customer)
                         .Include(s => s.SalonService)
                         .FirstOrDefaultAsync(a => a.Id == appointment.Id);

            return Ok(appointmentWithInfo);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            var isExist = await _db.Appointments.AnyAsync(a => a.Id == id);
            if (!isExist)
            {
                return NotFound();
            }
            var appointment = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == id);
            _db.Appointments.Remove(appointment);
            var changes = await _db.SaveChangesAsync();
            if (changes < 1)
            {
                return StatusCode(500);
            }
            return Ok();
        }
    }
}
