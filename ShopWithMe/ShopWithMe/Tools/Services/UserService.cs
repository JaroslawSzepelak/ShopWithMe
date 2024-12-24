using System.Security.Claims;

namespace ShopWithMe.Tools.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _context;

        #region UserService()
        public UserService(IHttpContextAccessor context)
        {
            _context = context;
        }
        #endregion

        #region GetUser()
        public string GetUserId()
        {
            return _context.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        #endregion
    }
}
