using System.ComponentModel.DataAnnotations;

namespace ShopWithMe.Models.IdentityModels.Accounts.Public
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string RepeatPassword { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
