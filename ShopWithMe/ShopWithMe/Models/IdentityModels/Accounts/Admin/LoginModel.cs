using System.ComponentModel.DataAnnotations;

namespace ShopWithMe.Models.IdentityModels.Accounts.Admin
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
