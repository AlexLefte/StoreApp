﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApp.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? County { get; set; }
        public string? PostalCode { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [ValidateNever]
        public Company Company { get; set; }

    }
}
