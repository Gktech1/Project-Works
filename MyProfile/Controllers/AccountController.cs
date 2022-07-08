using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using MyProfile.Models;
using MyProfile.ViewModel;

namespace MyProfile.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly  IEmailSender _emailSender;


        public AccountController(UserManager<IdentityUser> userManager , SignInManager<IdentityUser> signInManager, IEmailSender IemailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = IemailSender;
        }

        [HttpGet]
       // [Authorize]
        public IActionResult Register(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            RegisterViewModel registerViewModel = new RegisterViewModel();
            return View(registerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            if (ModelState.IsValid)

            {
                var user = new AppUser { UserName = model.Email, Email = model.Email, Name = model.Name };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                 return   LocalRedirect(returnurl);
                } 
                AddError(result);
            }

            return View(model);

             

        }

        [HttpGet]
        public IActionResult Login(string returnurl=null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            var user = await _userManager.FindByNameAsync(model.Email);
            if (ModelState.IsValid)

            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    return LocalRedirect(returnurl);
                }

                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }

                else
                {
                    ModelState.AddModelError("", "Invalid Login attempt");
                    return View(model);
                }

            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassWord(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    RedirectToAction("ForgotPasswordConfirmation"); 
                }
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);    
                var callBackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol:HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password - Identity Manager",
                    "Please reset your password by clicking here: <a href=\"" + callBackUrl + "\">link</a>");
                return RedirectToAction("ForgotPasswordConfirmation");

            }
            return View(model);
        } 

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpPost] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index),"Home");

           // return RedirectToAction("Index", "Home");

        }
        private void AddError(IdentityResult result)
        {
            foreach (var error in result.Errors )
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}


/*ViewData["ReturnUrl"] = returnurl;
returnurl = returnurl ?? Url.Content("~/");
if (!ModelState.IsValid)

{
    return View(model);

}
//Check if email exist
var user = await _userManager.FindByNameAsync(model.Email);
if (user == null)
{
    ModelState.AddModelError("", "Invalid Login attempt");
    return View(model);
}

var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
if (!result.Succeeded)
{
    ModelState.AddModelError("", "Invalid Login attempt");
    return View(model);
}

if (result.IsLockedOut)
{
    return View("Lockout");
}
return LocalRedirect(returnurl);
// return RedirectToAction("Index", "Home");
}
*/