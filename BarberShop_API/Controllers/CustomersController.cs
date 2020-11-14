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
    /// Endpoint used to interact with the Customers in Barber Shop Database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get All Customers
        /// </summary>
        /// <returns>List of Customers</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var customers = await _customerRepository.GetAll();
                return Ok(customers);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong. Please contact the Administrator"); // Internal Service Error
            }
          
        }
        /// <summary>
        /// Get Customer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Customer record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                var customer = await _customerRepository.GetById(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong. Please contact the Administrator");
            }
           
        }
        /// <summary>
        /// Create or update a customer
        /// </summary>
        /// <param name="customerDTO"></param>
        /// <returns>Customer record</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUpdate([FromBody] CustomerDTO customerDTO)
        {
            try
            {
                // The API will return Bad Request if 
                // customerDTO is null or ModelState 
                // is not valid without executing this method
                // I think I don't need the following code
                // start
                if (customerDTO == null)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // end   --- Iyad
                 
                var customer = _mapper.Map<Customer>(customerDTO);
                var response = await _customerRepository.CreateUpdate(customer);
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
        /// Delete a customer record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest();
                }
                //var isExist = await _customerRepository.IsExists(id);
                //if (!isExist)
                //{
                //    return NotFound();
                //}
                var customer = await _customerRepository.GetById(id);
                if (customer == null)
                {
                    return NotFound();
                }
                var isSuccess = await _customerRepository.Delete(customer);
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
