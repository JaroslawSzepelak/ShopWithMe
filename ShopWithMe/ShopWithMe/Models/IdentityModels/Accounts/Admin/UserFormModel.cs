using System.ComponentModel.DataAnnotations;

namespace ShopWithMe.Models.IdentityModels.Accounts.Admin
{
    public class UserFormModel
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
