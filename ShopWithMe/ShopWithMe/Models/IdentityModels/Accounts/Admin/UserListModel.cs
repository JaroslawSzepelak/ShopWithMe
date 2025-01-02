using System.ComponentModel.DataAnnotations;

namespace ShopWithMe.Models.IdentityModels.Accounts.Admin
{
    public class UserListModel
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
