using BookShop.BLL.Extensions;
using BookShop.BLL.Services;
using BookShop.Core.Entities;
using BookShop.Data.Helpers;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace BookShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly IMailService _mailManager;
        private readonly IConfiguration _config;


        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManger, IMailService mailManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManger = roleManger;
            _mailManager = mailManager;
            _config = config;
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


            if (model.Image is not null)
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

            string token = Guid.NewGuid().ToString();
            user.EmailConfirmationToken = token;

            IdentityResult identityResult = await _userManager.CreateAsync(user, model.Password);


            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            var createdUser = await _userManager.FindByNameAsync(model.UserName);

            await _userManager.AddToRoleAsync(user, "User");

            var link = Url.Action(nameof(Login), "Account", new { id = user.Id, token }, Request.Scheme, Request.Host.ToString());

            EmailViewModel email = _config.GetSection("Email").Get<EmailViewModel>();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(email.SenderEmail, email.SenderName);
            mail.To.Add(user.Email);
            mail.Subject = "VerifyEmail";
            string body = "";
            using (StreamReader reader = new StreamReader("wwwroot/template/verifyemail.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{link}", link);
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = email.Server;
            smtp.Port = email.Port;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(email.SenderEmail, email.Password);
            smtp.Send(mail);

            return RedirectToAction(nameof(EmailVerification));
        }

        public IActionResult EmailVerification() => View();

        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            await _userManager.ConfirmEmailAsync(user, token);

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction(nameof(Login));

        }
        public async Task<IActionResult> VerifyEmail(string id, string token)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            AppUser user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            if (user.EmailConfirmationToken != token)
            {
                return BadRequest();
            }

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, emailConfirmationToken);

            if (result.Succeeded)
            {
                string newToken = Guid.NewGuid().ToString();
                user.EmailConfirmationToken = newToken;
                await _userManager.UpdateAsync(user);
                return View();
            }

            return BadRequest();
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

            return RedirectToAction("Index", "Home");
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

        public IActionResult ChangePassword()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(Login));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (existUser == null)
                return BadRequest();

            var result = await _userManager.ChangePasswordAsync(existUser, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));

        }

        public IActionResult ForgetPassword() 
        {
           return View();
        } 


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Must be write email address");
                return View();
            }

            AppUser user = await _userManager.FindByEmailAsync(model.Mail);

            if (user is null)
            {
                ModelState.AddModelError("", "So the email is not available");
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action(nameof(ResetPassword), "Account", new { id = user.Id, token }, Request.Scheme, Request.Host.ToString());

            EmailViewModel email = _config.GetSection("Email").Get<EmailViewModel>();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(email.SenderEmail, email.SenderName);
            mail.To.Add(model.Mail);
            mail.Subject = "Reset Password";
            mail.Body = $"<a href=\"{link}\">Reset Password</a>";
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = email.Server;
            smtp.Port = email.Port;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(email.SenderEmail, email.Password);
            smtp.Send(mail);

            return RedirectToAction(nameof(EmailVerification));
        }

        public IActionResult ResetPassword(string email, string token)
        {
            return View(new ResetPasswordViewModel { Email = email, Token = token });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ResetPassword")]
        public async Task<IActionResult> ResetPasswordPost(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Must be filled in correctly");
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) return BadRequest();

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);

            }

            return View();
        }
    }
}
