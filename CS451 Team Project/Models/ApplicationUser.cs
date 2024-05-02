using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS451_Team_Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string UserName { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string? TwoFactorKey { get; set; }
        public bool RememberMe { get; set; } = false;
        //public string Key { get; set; }
    }

    public class Photo
    {
        public int PhotoId { get; set; }
        public string UserId { get; set; }
        public string PhotoName { get; set; }
        public string PhotoPath { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
