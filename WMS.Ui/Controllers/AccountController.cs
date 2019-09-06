using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using WMS.Ui.Models;
using WMS.Ui.Models.Account;

namespace WMS.Ui.Controllers
{
    /// <summary>
    /// Web Page Authentication Controller
    /// </summary>
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private RWD.Toolbox.SMTP.IEmailAgent _emailAgent;
        private readonly AppSettings _appSettings;

        public AccountController(IHostingEnvironment environment, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager,
            IOptions<AppSettings> appSettings, RWD.Toolbox.SMTP.IEmailAgent emailAgent) : base(configuration, userManager, roleManager)
        {
            _hostingEnvironment = environment;
            _signInManager = signInManager;
            _emailAgent = emailAgent;
            _appSettings = appSettings.Value;

            
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            ViewData["Title"] = "Access Denied";
            return View();
        }

        /// <summary>
        /// Login Entry Point
        /// </summary>
        /// <param name="returnUrl">URL to proceed to on successful login as <see cref="string"/></param>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["Title"] = "Login";
            // ViewData["Menu"] = "navAccount";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Submit Login Details
        /// </summary>
        /// <param name="model">Login Details as <see cref="LoginViewModel"/></param>
        /// <param name="returnUrl">URL to proceed to on successful login as <see cref="string"/></param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["Title"] = "Login";
           // ViewData["Menu"] = "navAccount";
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);


                if (user != null)
                {
                    // check that user has validated email address
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "Email has not yet been verified!  Please check your Email Inbox and click Verify.");
                        return View(model);
                    }

                    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        return RedirectToLocal(returnUrl);
                    }

                    if (result.IsLockedOut)
                    {
                        var availableNext = user.LockoutEnd.Value.ToLocalTime().ToString("g");
                        ModelState.AddModelError("", string.Format("Due to multiple failed login attempts, your account has been locked out until {0}", availableNext));
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(user);
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }

                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Logged Out Acknowledgment Page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            ViewData["Title"] = "Logout";
           // ViewData["Menu"] = "navAccount";
            await _signInManager.SignOutAsync();
            return View();
        }

        /// <summary>
        /// New User Creation Page
        /// </summary>
        /// <param name="returnUrl">URL to proceed to on successful login as <see cref="string"/></param>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["Title"] = "Register";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Submit New User Information to Create New Login User
        /// </summary>
        /// <param name="model">New User Data as <see cref="RegisterViewModel"/></param>
        /// <param name="returnUrl">URL to proceed to on successful login as <see cref="string"/></param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["Title"] = "Register";
            // ViewData["Menu"] = "navAccount";
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(_appSettings.SecRole.Level1))
                    {
                        ApplicationRole role = new ApplicationRole
                        {
                            Name = _appSettings.SecRole.Level1,
                            Description = "Perform basic operations."
                        };
                        IdentityResult roleResult = await _roleManager.CreateAsync(role);
                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError(string.Empty, "Error while creating role!");
                            return View(model);
                        }
                    }
                    _userManager.AddToRoleAsync(user, _appSettings.SecRole.Level1).Wait();

                    // send confirmation email
                    string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    string confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userid = user.Id, token = confirmationToken }, protocol: HttpContext.Request.Scheme);

                    string[] emailAddresses = { _appSettings.SMTP.AdminEmail, user.Email };
                    var emailName = string.IsNullOrWhiteSpace(user.FirstName) ? user.UserName : $"{user.FirstName} {user.LastName}".Trim();
                    await _emailAgent.SendEmailAsync(_appSettings.SMTP.FromEmail, _appSettings.SMTP.FromEmail, emailAddresses,
                        "Welcome to Winemakers Software - Please verify your email.", CreateVerifyEmail(confirmationLink, emailName), true, null);

                    // redirect to limbo page
                    return RedirectToAction("RegisterLimbo", "Account");

                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Acknowledgment of Submitted Registration
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterLimbo()
        {
            ViewData["Title"] = "Register";
            //ViewData["Menu"] = "navAccount";
            return View();
        }

        /// <summary>
        /// Acknowledgment of Submitted Password Change
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordLimbo()
        {
            ViewData["Title"] = "Register";
            // ViewData["Menu"] = "navAccount";
            return View();
        }

        /// <summary>
        /// Acknowledgment of Confirmed Email
        /// </summary>
        /// <param name="userid">User Primary Key as <see cref="string"/></param>
        /// <param name="token">Security Token as <see cref="string"/></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userid, string token)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userid);
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                Success("You Email is Confirmed.  Please Log In.", true);
                return View("Login");
            }
            else
            {
                Danger("You Email could not be Confirmed.  Please register again.");
                return View("Register");
            }
        }

        /// <summary>
        /// Entry Point for User to Request New Password
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RequestNewPassword()
        {
            ViewData["Title"] = "Reset Password";
            return View();
        }

        /// <summary>
        /// Submits a link in an email for the User create a new password
        /// </summary>
        /// <param name="model">Password Reset Data as <see cref="RequestPasswordResetViewModel"/></param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendPasswordResetLink(RequestPasswordResetViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                ViewBag.Message = "Error while resetting your password!";
                return View("Error");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = Url.Action("ResetPassword", "Account", new { token }, protocol: HttpContext.Request.Scheme);

            // code to email the above link
            string[] emailAddresses = { _appSettings.SMTP.AdminEmail, user.Email };
            await _emailAgent.SendEmailAsync(_appSettings.SMTP.FromEmail, _appSettings.SMTP.FromEmail, emailAddresses,
                "Winemakers Software - Password Reset.", CreatePasswordResetEmail(resetLink), true, null);

            // redirect to limbo page
            return RedirectToAction("PasswordLimbo", "Account");

        }

        /// <summary>
        /// Entry point for Email of Reset Password
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token)
        {
            ViewData["Title"] = "Reset Password";
            var model = new ResetPasswordViewModel { Token = token };
            return View(model);
        }

        /// <summary>
        ///  Acknowledge Password Reset to the USer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                Success("You password has been reset.  Please log in.", true);
                return View("Login");
            }
            else
            {
                Danger("You password could not be reset.  Please request a new password again.");
                return View("RequestNewPassword");
            }

        }

        /// <summary>
        /// Validation Error Handling
        /// </summary>
        /// <param name="result"></param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        /// <summary>
        /// Return to Specific Page after login
        /// </summary>
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// Create Email to User for Verifying User Email Address
        /// </summary>
        /// <param name="link">Link for Confirming Email as <see cref="string"/></param>
        /// <param name="name">User Name for Greeting as <see cref="string"/></param>
        private string CreateVerifyEmail(string link, string name)
        {
            var picUri = new Uri(_appSettings.URLs.HomeDomain + "/" + _appSettings.URLs.ImageSite + "/" + _appSettings.URLs.ImageHeaderFile);

            var dataFolderName = _appSettings.Paths.DataFolder;

            var emailBodyTemplateFileName = _appSettings.EmailTemplate.BodyTemplateFileName;
            var emailBodyTemplateFile = Path.Combine(_hostingEnvironment.ContentRootPath, dataFolderName, emailBodyTemplateFileName);

            var emailWelcomeTemplateFileName = _appSettings.EmailTemplate.WelcomeTemplateFileName;
            var welcomeTemplateFile = Path.Combine(_hostingEnvironment.ContentRootPath, dataFolderName, emailWelcomeTemplateFileName);


            var welcomeNameKey = _appSettings.EmailTemplate.WelcomeName;
            var welcomeNameValue = name;
            string welcomeText = System.IO.File.ReadAllText(welcomeTemplateFile)
                .Replace(welcomeNameKey, welcomeNameValue);


            var headerImageKey = _appSettings.EmailTemplate.BodyHeaderImage;
            var headerImageValue = picUri.ToString();

            var messageBodyKey = _appSettings.EmailTemplate.BodyMessage;
            var messageBodyValue = welcomeText;

            var buttonTextKey = _appSettings.EmailTemplate.BodyButtonText;
            var buttonTextValue = @"VERIFY EMAIL";

            var buttonHrefKey = _appSettings.EmailTemplate.BodyButtonHref;
            var buttonHrefValue = link;

            string templateText = System.IO.File.ReadAllText(emailBodyTemplateFile);
            var body = templateText
                .Replace(headerImageKey, headerImageValue)
                .Replace(messageBodyKey, messageBodyValue)
                .Replace(buttonHrefKey, buttonHrefValue)
                .Replace(buttonTextKey, buttonTextValue);

            return body;
        }

        /// <summary>
        /// Create Email for User to Reset Password
        /// </summary>
        /// <param name="link">Link for Reseting Password as <see cref="string"/></param>
        private string CreatePasswordResetEmail(string link)
        {
            var picUri = new Uri(_appSettings.URLs.HomeDomain + "/" + _appSettings.URLs.ImageSite + "/" + _appSettings.URLs.ImageHeaderFile);

            var dataFolderName = _appSettings.Paths.DataFolder;

            var emailBodyTemplateFileName = _appSettings.EmailTemplate.BodyTemplateFileName;
            var emailBodyTemplateFile = Path.Combine(_hostingEnvironment.ContentRootPath, dataFolderName, emailBodyTemplateFileName);

            var emailPasswordResetTemplateFileName = _appSettings.EmailTemplate.PasswordResetTemplateFileName;
            var passwordResetTemplateFile = Path.Combine(_hostingEnvironment.ContentRootPath, dataFolderName, emailPasswordResetTemplateFileName);

            string welcomeText = System.IO.File.ReadAllText(passwordResetTemplateFile);


            string headerImageKey = _appSettings.EmailTemplate.BodyHeaderImage;
            var headerImageValue = picUri.ToString();

            var messageBodyKey = _appSettings.EmailTemplate.BodyMessage;
            var messageBodyValue = welcomeText;

            var buttonTextKey = _appSettings.EmailTemplate.BodyButtonText;
            var buttonTextValue = @"Reset Password";

            var buttonHrefKey = _appSettings.EmailTemplate.BodyButtonHref;
            var buttonHrefValue = link;

            string templateText = System.IO.File.ReadAllText(emailBodyTemplateFile);
            var body = templateText
                .Replace(headerImageKey, headerImageValue)
                .Replace(messageBodyKey, messageBodyValue)
                .Replace(buttonHrefKey, buttonHrefValue)
                .Replace(buttonTextKey, buttonTextValue);

            return body;
        }


    }
}