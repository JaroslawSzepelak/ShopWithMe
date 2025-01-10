using Microsoft.AspNetCore.Identity;
using ShopWithMe.Models.IdentityModels.Accounts.Admin;

namespace ShopWithMe.Models.IdentityModels.Accounts
{
    public class UserModelsMapper
    {
        public void Map(IdentityUser mapFrom, User mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Email = mapFrom.Email;
            mapTo.UserName = mapFrom.UserName;
        }
    }
}
