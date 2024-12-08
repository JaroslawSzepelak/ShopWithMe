using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopWithMe.Models.IdentityModels.Accounts;
using ShopWithMe.Models.IdentityModels.Accounts.Admin;
using ShopWithMe.Models.IdentityModels.Accounts.Public;

namespace ShopWithMe.Controllers
{
    [Route("api/[controller]")]
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

        #region Logout()
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
        #endregion

        #region Register()
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel loginModel)
        {
            if (loginModel.Password != loginModel.RepeatPassword)
            {
                ModelState.AddModelError("RepeatPassword", "Hasła są różne.");
                return UnprocessableEntity(ModelState);
            }

            var user = await userManager.FindByNameAsync(loginModel.UserName);

            if (user != null)
            {
                ModelState.AddModelError("UserName", "Użytkownik o takiej nazwie już istnieje.");
                return UnprocessableEntity(ModelState);
            }

            user = await userManager.FindByEmailAsync(loginModel.Email);

            if (user != null)
            {
                ModelState.AddModelError("Email", "Podany adres e-mail jest już zajęty.");
                return UnprocessableEntity(ModelState);
            }

            user = new IdentityUser(loginModel.UserName)
            {
                Email = loginModel.Email
            };

            var result = await userManager.CreateAsync(user, loginModel.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", $"Wystąpił problem podczas tworzenia konta: {error}");
            }

            return UnprocessableEntity(ModelState);
        }
        #endregion
    }
}
