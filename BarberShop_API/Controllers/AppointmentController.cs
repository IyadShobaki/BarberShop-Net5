using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BarberShop_DataAccess.Contracts;
using BarberShop_DataAccess.Data;
using BarberShop_Models.DTOs;
using BarberShop_Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShop_API.Controllers
{
    /// <summary>
    /// Endpoint used to interact with Appointments in Barber Shop Database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentController(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get All Appointments
        /// </summary>
        /// <returns>List of Appointments</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAppointments()
        {
            try
            {
                var appointments = await _appointmentRepository.GetAll();
                return Ok(appointments);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong. Please contact the Administrator"); // Internal Service Error
            }

        }
        /// <summary>
        /// Get Appointment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Appointment record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            try
            {
                var appointment = await _appointmentRepository.GetById(id);

                if (appointment == null)
                {
                    return NotFound();
                }
                return Ok(appointment);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong. Please contact the Administrator");
            }
        }
        /// <summary>
        /// Create or Update Appointment
        /// </summary>
        /// <param name="appointmentDTO"></param>
        /// <returns>Appointment Info</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUpdate([FromBody] AppointmentDTO appointmentDTO)
        {
            try
            {
                // The API will return Bad Request if 
                // appointmentDTO is null or ModelState 
                // is not valid without executing this method
                // I think I don't need the following code
                // start
                if (appointmentDTO == null)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // end   --- Iyad
                var isDateTimeAvailable = await _appointmentRepository.IsDateTimeAvailable(appointmentDTO.AppointmentDate);
                if (isDateTimeAvailable)
                {
                    return BadRequest(ModelState);
                }
                var appointment = _mapper.Map<Appointment>(appointmentDTO);
                var response = await _appointmentRepository.CreateUpdate(appointment);
                if (response == null)
                {
                    return StatusCode(500, "Something went wrong. Please try again later!");
                }
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong. Please try again later!");
            }
        }
        /// <summary>
        /// Delete an appointment record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest();
                }
                //var isExist = await _appointmentRepository.IsExists(id);
                //if (!isExist)
                //{
                //    return NotFound();
                //}
                var appointment = await _appointmentRepository.GetById(id);
                if (appointment == null)
                {
                    return NotFound();
                }
                var isSuccess = await _appointmentRepository.Delete(appointment);
                if (!isSuccess)
                {
                    return StatusCode(500, "Something went wrong. Please try again later!");
                }
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong. Please try again later!");
            }

        }
    }
}
