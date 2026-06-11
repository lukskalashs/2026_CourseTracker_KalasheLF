using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser
    {
        
        public required string DisplayName { get; set; }
        // Refresh Token Navigation Properties
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        // Navigation Property
        public ICollection<Course> Courses { get; set; } = [];

    }
}