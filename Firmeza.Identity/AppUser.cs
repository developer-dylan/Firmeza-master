using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Firmeza.Identity
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required, EmailAddress]
        public override string? Email { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
    }
}