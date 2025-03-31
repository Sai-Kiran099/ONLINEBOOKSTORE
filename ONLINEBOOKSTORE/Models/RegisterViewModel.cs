﻿using System.ComponentModel.DataAnnotations;

namespace ONLINEBOOKSTORE.Models
{
    public class RegisterViewModel
    {
        [Required]
        
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password",ErrorMessage="Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string? Role { get; set; }
    }
}
