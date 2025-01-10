using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Models.Admin;
using ShopWithMe.Models.IdentityModels.Accounts;
using ShopWithMe.Models.IdentityModels.Accounts.Admin;
using ShopWithMe.Models.IdentityModels.Accounts.Public;
using ShopWithMe.Tools.Models;

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

        #region GetList()
        [HttpGet]
        [Authorize(Roles = Roles.Administrator)]
        public ResultModel<UserListModel> GetList(
            [FromQuery] int pageIndex,
            [FromQuery] int pageSize)
        {

            var pager = new Pager()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var users = userManager.Users
                    .Skip(pager.Skip)
                    .Take(pager.PageSize)
                    .ToList();

            var result = new List<UserListModel>();

            foreach (var user in users)
            {
                result.Add(new UserListModel
                {
                    Email = user.Email,
                    Id = user.Id,
                    UserName = user.UserName
                });
            }

            return new ResultModel<UserListModel>(result, pager);
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

        #region Create()
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateModel loginModel)
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
                ModelState.AddModelError("", $"Wystąpił problem podczas tworzenia konta: {error.Description}");
            }

            return UnprocessableEntity(ModelState);
        }
        #endregion

        #region Update()
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserFormModel model)
        {
            var editedUser = await userManager.FindByIdAsync(model.Id);

            if (!string.IsNullOrEmpty(model.Password) && model.Password != model.RepeatPassword)
            {
                ModelState.AddModelError("RepeatPassword", "Hasła są różne.");
                return UnprocessableEntity(ModelState);
            }

            var user = await userManager.FindByNameAsync(model.UserName);

            if (user != null && user.Id != editedUser.Id)
            {
                ModelState.AddModelError("UserName", "Użytkownik o takiej nazwie już istnieje.");
                return UnprocessableEntity(ModelState);
            }

            user = await userManager.FindByEmailAsync(model.Email);

            if (user != null && user.Id != editedUser.Id)
            {
                ModelState.AddModelError("Email", "Podany adres e-mail jest już zajęty.");
                return UnprocessableEntity(ModelState);
            }

            editedUser.Email = model.Email;
            editedUser.UserName = model.UserName;

            var result = await userManager.UpdateAsync(editedUser);

            var passwordResult = (IdentityResult)null;

            if (!string.IsNullOrEmpty(model.Password))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(editedUser);

                passwordResult = await userManager.ResetPasswordAsync(editedUser, token, model.Password);
            }

            if (result.Succeeded && (passwordResult == null || passwordResult.Succeeded))
            {
                return Ok();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", $"Wystąpił problem podczas edycji konta: {error.Description}");
            }

            foreach (var error in passwordResult.Errors)
            {
                ModelState.AddModelError("", $"Wystąpił problem podczas edycji konta: {error.Description}");
            }

            return UnprocessableEntity(ModelState);
        }
        #endregion
    }
}
