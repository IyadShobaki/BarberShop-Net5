using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BarberShop_Models.DTOs
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        [Required]
        public int Customer_Id { get; set; }
        [Required]
        public int SalonService_Id { get; set; }
    }
}
