using System.ComponentModel.DataAnnotations;

namespace WebEnablePackage.ViewModels.Accounts
{
    public class VM_Registration
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "Name length cannot be more than 8.")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}