using System.ComponentModel.DataAnnotations;

namespace MyProfile.ViewModel
{
    public class ForgotPasswordViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
