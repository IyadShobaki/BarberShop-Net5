using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BarberShop_Models.DTOs
{
    public class SalonServiceDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string ServiceName { get; set; }
        [Required]
        [MaxLength(4000)]
        public string ServiceDescription { get; set; }
        [Required]
        public int ServiceDuration { get; set; }
        [Required]
        public decimal ServicePrice { get; set; }
    }
}
