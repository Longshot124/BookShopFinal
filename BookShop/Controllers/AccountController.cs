using BookShop.BLL.Extensions;
using BookShop.Core.Entities;
using BookShop.Data.Helpers;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManger;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManger = roleManger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existUser = await _userManager.FindByNameAsync(model.UserName);

            if (existUser != null)
            {
                ModelState.AddModelError("", "This User is already exist!");
                return View();
            }

            var gender = "";

            if (model.Gender)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }


            var user = new AppUser
            {
                FisrtName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
                Age = model.Age,
                Adress = model.Adress,
                PhoneNumber = model.PhoneNumber,
                Gender = gender,
            };

            
            if(model.Image is not null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "You must choose image");
                    return View();
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Image size is over 5mb!!!");
                    return View();
                }

                var unicalName = await model.Image.GenerateFile(Constants.UserPath);

                user.ImageUrl = unicalName;
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            var createdUser = await _userManager.FindByNameAsync(model.UserName);

            result = await _userManager.AddToRoleAsync(createdUser,Constants.UserRole);

            return RedirectToAction(nameof(Login));
        }
       
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existUser = await _userManager.FindByNameAsync(model.UserName);

            if (existUser == null)
            {
                ModelState.AddModelError("", "Invalid Credentias");
                return View();
            }

            var signResult = await _signInManager.PasswordSignInAsync(existUser, model.Password, model.RememberMe, true);

            if (!signResult.Succeeded)
            {
                ModelState.AddModelError("", "Invalid Credentias");
                return View();
            }

            return RedirectToAction("Index","Home");
        }

        public IActionResult AccessDenied()
        {
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }


    }
}
