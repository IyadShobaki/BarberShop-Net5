using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BarberShop_Models.Models
{
    //[Table("tb_Service")]  // If you need yo change the name of the table
    public class SalonService
    {
        public int Id { get; set; }
        //[Column("Name")] // If you need to change the column name
        [Required]
        [MaxLength(50)]
        public string ServiceName { get; set; }
        [Required]
        [MaxLength(4000)]
        public string ServiceDescription { get; set; }
        [Required]
        public int ServiceDuration { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ServicePrice { get; set; }

        public List<Appointment> Appointments { get; set; }

    }
}
