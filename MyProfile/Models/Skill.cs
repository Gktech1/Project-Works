using System;
using System.ComponentModel.DataAnnotations;

namespace MyProfile.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Year { get; set; } = DateTime.Now;
        public string UserId { get; set; }
    }
}
