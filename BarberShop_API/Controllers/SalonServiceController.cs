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
    /// Endpoint used to interact with the SalonServices in Barber Shop Database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SalonServiceController : ControllerBase
    {
        private readonly ISalonServiceRepository _salonServiceRepository;
        private readonly IMapper _mapper;

        public SalonServiceController(ISalonServiceRepository salonServiceRepository, IMapper mapper)
        {
            _salonServiceRepository = salonServiceRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get All Salon Services
        /// </summary>
        /// <returns>List of the Salon Services</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetServices()
        {
            try
            {
                var services = await _salonServiceRepository.GetAll();
                return Ok(services);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong. Please contact the Administrator"); // Internal Service Error
            }
        }

        /// <summary>
        /// Get Salon Service By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Salon Service record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetServiceById(int id)
        {
            try
            {
                var service = await _salonServiceRepository.GetById(id);
                if (service == null)
                {
                    return NotFound();
                }
                return Ok(service);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong. Please contact the Administrator");
            }

        }
        /// <summary>
        /// Create or Update a salon service
        /// </summary>
        /// <param name="serviceDTO"></param>
        /// <returns>Salon Service Info</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUpdate([FromBody] SalonServiceDTO serviceDTO)
        {

            try
            {
                // The API will return Bad Request if 
                // customerDTO is null or ModelState 
                // is not valid without executing this method
                // I think I don't need the following code
                // start
                if (serviceDTO == null)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // end   --- Iyad

                var service = _mapper.Map<SalonService>(serviceDTO);
                var response = await _salonServiceRepository.CreateUpdate(service);
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
        /// Delete a salon service record
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
                //var isExist = await _salonServiceRepository.IsExists(id);
                //if (!isExist)
                //{
                //    return NotFound();
                //}
                var service = await _salonServiceRepository.GetById(id);
                if (service == null)
                {
                    return NotFound();
                }
                var isSuccess = await _salonServiceRepository.Delete(service);
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
