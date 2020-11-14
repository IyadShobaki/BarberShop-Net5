using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BarberShop_Models.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(250)]
        public string EmailAddress { get; set; }
        // Enforce one to many relationship with Appointment table
        public List<Appointment> Appointments { get; set; }
    }
}
