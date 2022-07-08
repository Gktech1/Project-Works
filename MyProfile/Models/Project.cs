using System;
using System.ComponentModel.DataAnnotations;

namespace MyProfile.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProjectLink { get; set; }
        public DateTime Year { get; set; } = DateTime.Now;
        public string UserId { get; set; }
    }
}
