using System.ComponentModel.DataAnnotations;

namespace ePizzaHub.UI.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Please Enter Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Compare Password doesn't match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
