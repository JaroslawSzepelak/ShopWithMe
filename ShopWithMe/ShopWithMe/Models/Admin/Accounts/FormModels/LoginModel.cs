using System.ComponentModel.DataAnnotations;

namespace ShopWithMe.Models.Admin.Accounts.FormModels
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
