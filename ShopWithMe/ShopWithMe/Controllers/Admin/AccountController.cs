using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Models.IdentityModels.Accounts;
using ShopWithMe.Models.IdentityModels.Accounts.Admin;

namespace ShopWithMe.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        protected UserManager<IdentityUser> userManager;
        protected SignInManager<IdentityUser> signInManager;
        protected UserModelsMapper _mapper;

        #region AccountController()
        public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr, UserModelsMapper mapper)
        {
            userManager = userMgr;
            signInManager = signInMgr;
            _mapper = mapper;
        }
        #endregion

        #region GetUser()
        [HttpGet("get-user")]
        [Authorize]
        public async Task<User> GetUser()
        {
            var user = new User();

            _mapper.Map(await userManager.GetUserAsync(User), user);

            return user;
        }
        #endregion

        #region Login()
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(loginModel.Name);

                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                    {
                        return Ok();
                    }
                }
            }

            ModelState.AddModelError("", "Nieprawidłowa nazwa użytkownika lub hasło");
            return BadRequest();
        }
        #endregion

        #region logout()
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
        #endregion
    }
}
