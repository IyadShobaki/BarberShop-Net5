using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BarberShop_Models.DTOs
{
    public class LoginDTO
    {

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Your password is limited to {1} to {0} characters", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
