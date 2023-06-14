using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        [MaxLength(30)]
        public string? Name { get; set; }

        [DisplayName("Address")]
        [MaxLength(100)]
        public string? Address { get; set; }

        [DisplayName("City")]
        [MaxLength(30)]
        public string? City { get; set; }

        [DisplayName("County")]
        [MaxLength(30)]
        public string? County { get; set; }

        [DisplayName("PostalCode")]
        [MaxLength(30)]
        public string? PostalCode { get; set; }

        [DisplayName("PhoneNumber")]
        [MaxLength(30)]
        public string? PhoneNumber { get; set; }
    }
}
