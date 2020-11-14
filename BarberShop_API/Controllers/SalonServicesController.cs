using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BarberShop_DataAccess.Data;
using BarberShop_Models.DTOs;
using BarberShop_Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalonServicesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public SalonServicesController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            List<SalonService> services = await _db.SalonServices.ToListAsync();

            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var service = await _db.SalonServices.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return Ok(service);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUpdate([FromBody] SalonServiceDTO serviceDTO)
        {
            if (serviceDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var service = _mapper.Map<SalonService>(serviceDTO);
            if (service.Id == 0)
            {
                await _db.SalonServices.AddAsync(service);
            }
            else
            {
                _db.SalonServices.Update(service);
            }
            
            var changes = await _db.SaveChangesAsync();
            if (changes < 1)
            {
                return StatusCode(500);
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            var isExist = await _db.SalonServices.AnyAsync(s => s.Id == id);
            if (!isExist)
            {
                return NotFound();
            }
            var service = await _db.SalonServices.FirstOrDefaultAsync(s => s.Id == id);
            _db.SalonServices.Remove(service);
            var changes = await _db.SaveChangesAsync();
            if (changes < 1)
            {
                return StatusCode(500);
            }
            return Ok();
        }
    }
}
