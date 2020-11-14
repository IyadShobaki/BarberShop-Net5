using AutoMapper;
using BarberShop_Models.DTOs;
using BarberShop_Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarberShop_Models.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<SalonService, SalonServiceDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Appointment, AppointmentDTO>().ReverseMap();
        }
    }
}
