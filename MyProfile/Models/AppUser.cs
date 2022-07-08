using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MyProfile.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

    }
}
