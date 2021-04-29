using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using WMS.Ui.Mvc.Models;
using WMS.Ui.Mvc.Models.Account;

namespace WMS.Ui.Mvc.Controllers
{
   /// <summary>
   /// Web Page Authentication Controller
   /// </summary>
   [Authorize]
   public class AccountController : BaseController
   {
      private readonly IWebHostEnvironment _hostingEnvironment;
      private readonly SignInManager<ApplicationUser> _signInManager;
      private readonly RWD.Toolbox.SMTP.IEmailAgent _emailAgent;
      private readonly AppSettings _appSettings;
      private readonly IStringLocalizer<AccountController> _localizer;

      public AccountController(IWebHostEnvironment environment, IConfiguration configuration,
          UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager,
          IOptions<AppSettings> appSettings, RWD.Toolbox.SMTP.IEmailAgent emailAgent, IStringLocalizer<AccountController> localizer, TelemetryClient telemetry) : base(configuration, userManager, roleManager, telemetry)
      {
         _localizer = localizer;
         _hostingEnvironment = environment;
         _signInManager = signInManager;
         _emailAgent = emailAgent;
         _appSettings = appSettings?.Value;


      }

      [HttpGet]
      [AllowAnonymous]
      public IActionResult AccessDenied()
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
      public async Task<IActionResult> Login(Uri returnUrl = null)
      {
         // Clear the existing external cookie to ensure a clean login process
         await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

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
      public async Task<IActionResult> Login(LoginViewModel model, Uri returnUrl = null)
      {
         ViewData["Title"] = "Login";
         // ViewData["Menu"] = "navAccount";
         ViewData["ReturnUrl"] = returnUrl;
         if (ModelState.IsValid)
         {
            var user = await UserManagerAgent.FindByNameAsync(model?.UserName).ConfigureAwait(false);


            if (user != null)
            {
               // check that user has validated email address
               if (!await UserManagerAgent.IsEmailConfirmedAsync(user).ConfigureAwait(false))
               {
                  ModelState.AddModelError(string.Empty, "Email has not yet been verified!  Please check your Email Inbox and click Verify.");
                  return View(model);
               }

               var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, lockoutOnFailure: false).ConfigureAwait(false);
               if (result.Succeeded)
               {
                  await UserManagerAgent.ResetAccessFailedCountAsync(user).ConfigureAwait(false);
                  return RedirectToLocal(returnUrl?.ToString());
               }

               if (result.IsLockedOut)
               {
                  var availableNext = user.LockoutEnd.Value.ToLocalTime().ToString("g", CultureInfo.CurrentCulture);
                  ModelState.AddModelError("", string.Format(CultureInfo.CurrentCulture, "Due to multiple failed login attempts, your account has been locked out until {0}", availableNext));
               }
               else
               {
                  await UserManagerAgent.AccessFailedAsync(user).ConfigureAwait(false);
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
         await _signInManager.SignOutAsync().ConfigureAwait(false);
         return View();
      }

      /// <summary>
      /// New User Creation Page
      /// </summary>
      /// <param name="returnUrl">URL to proceed to on successful login as <see cref="string"/></param>
      [HttpGet]
      [AllowAnonymous]
      public IActionResult Register(Uri returnUrl = null)
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
      public async Task<IActionResult> Register(RegisterViewModel model, Uri returnUrl = null)
      {
         ViewData["Title"] = "Register";
         // ViewData["Menu"] = "navAccount";
         ViewData["ReturnUrl"] = returnUrl;
         if (ModelState.IsValid)
         {
            var user = new ApplicationUser
            {
               UserName = model?.UserName,
               FirstName = model.FirstName,
               LastName = model.LastName,
               Email = model.Email
            };
            var result = await UserManagerAgent.CreateAsync(user, model.Password).ConfigureAwait(false);
            if (result.Succeeded)
            {
               if (!await RoleManagerAgent.RoleExistsAsync(_appSettings.SecRole.Level1).ConfigureAwait(false))
               {
                  ApplicationRole role = new ApplicationRole
                  {
                     Name = _appSettings.SecRole.Level1,
                     Description = "Perform basic operations."
                  };
                  IdentityResult roleResult = await RoleManagerAgent.CreateAsync(role).ConfigureAwait(false);
                  if (!roleResult.Succeeded)
                  {
                     ModelState.AddModelError(string.Empty, "Error while creating role!");
                     return View(model);
                  }
               }
               UserManagerAgent.AddToRoleAsync(user, _appSettings.SecRole.Level1).Wait();

               // send confirmation email
               string confirmationToken = await UserManagerAgent.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);

               string confirmationLink = Url.Action("ConfirmEmail", "Account",
                   new { userid = user.Id, token = confirmationToken }, protocol: HttpContext.Request.Scheme);

               string[] emailAddresses = { _appSettings.SMTP.AdminEmail, user.Email };
               var emailName = string.IsNullOrWhiteSpace(user.FirstName) ? user.UserName : $"{user.FirstName} {user.LastName}".Trim();
               await _emailAgent.SendEmailAsync(_appSettings.SMTP.FromEmail, _appSettings.SMTP.FromEmail, emailAddresses,
                   "Welcome to Winemakers Software - Please verify your email.", CreateVerifyEmail(confirmationLink, emailName), true, null).ConfigureAwait(false);

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
         ApplicationUser user = await UserManagerAgent.FindByIdAsync(userid).ConfigureAwait(false);
         IdentityResult result = await UserManagerAgent.ConfirmEmailAsync(user, token).ConfigureAwait(false);

         if (result.Succeeded)
         {
            Success(_localizer["EmailConfirmSuccess"], true);
            return View("Login");
         }
         else
         {
            Danger(_localizer["EmailConfirmFail"]);
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
         if (model == null)
            throw new ArgumentNullException(nameof(model));

         var user = await UserManagerAgent.FindByNameAsync(model.UserName).ConfigureAwait(false);
         if (user == null || !await UserManagerAgent.IsEmailConfirmedAsync(user).ConfigureAwait(false))
         {
            ViewBag.Message = "Error while resetting your password!";
            return View("Error");
         }

         var token = await UserManagerAgent.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);

         var resetLink = Url.Action("ResetPassword", "Account", new { token }, protocol: HttpContext.Request.Scheme);

         // code to email the above link
         string[] emailAddresses = { _appSettings.SMTP.AdminEmail, user.Email };
         await _emailAgent.SendEmailAsync(_appSettings.SMTP.FromEmail, _appSettings.SMTP.FromEmail, emailAddresses,
             "Winemakers Software - Password Reset.", CreatePasswordResetEmail(resetLink), true, null).ConfigureAwait(false);

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
         if (model == null)
            throw new ArgumentNullException(nameof(model));

         var user = await UserManagerAgent.FindByNameAsync(model.UserName).ConfigureAwait(false);

         IdentityResult result = await UserManagerAgent.ResetPasswordAsync(user, model.Token, model.Password).ConfigureAwait(false);
         if (result.Succeeded)
         {
            Success(_localizer["PasswordResetSuccess"], true);
            return View("Login");
         }
         else
         {
            Danger(_localizer["PasswordResetFail"]);
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

         var dataFolderName = _appSettings.Paths.EmailTemplateFolder;

         var emailBodyTemplateFileName = _appSettings.EmailTemplate.BodyTemplateFileName;
         var emailBodyTemplateFile = Path.Combine(_hostingEnvironment.ContentRootPath, dataFolderName, emailBodyTemplateFileName);

         var emailWelcomeTemplateFileName = _appSettings.EmailTemplate.WelcomeTemplateFileName;
         var welcomeTemplateFile = Path.Combine(_hostingEnvironment.ContentRootPath, dataFolderName, emailWelcomeTemplateFileName);


         var welcomeNameKey = _appSettings.EmailTemplate.WelcomeName;
         var welcomeNameValue = name;
         string welcomeText = System.IO.File.ReadAllText(welcomeTemplateFile)
             .Replace(welcomeNameKey, welcomeNameValue, StringComparison.CurrentCultureIgnoreCase);


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
             .Replace(headerImageKey, headerImageValue, StringComparison.CurrentCultureIgnoreCase)
             .Replace(messageBodyKey, messageBodyValue, StringComparison.CurrentCultureIgnoreCase)
             .Replace(buttonHrefKey, buttonHrefValue, StringComparison.CurrentCultureIgnoreCase)
             .Replace(buttonTextKey, buttonTextValue, StringComparison.CurrentCultureIgnoreCase);

         return body;
      }

      /// <summary>
      /// Create Email for User to Reset Password
      /// </summary>
      /// <param name="link">Link for Reseting Password as <see cref="string"/></param>
      private string CreatePasswordResetEmail(string link)
      {
         var picUri = new Uri(_appSettings.URLs.HomeDomain + "/" + _appSettings.URLs.ImageSite + "/" + _appSettings.URLs.ImageHeaderFile);

         var dataFolderName = _appSettings.Paths.EmailTemplateFolder;

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
             .Replace(headerImageKey, headerImageValue, StringComparison.CurrentCultureIgnoreCase)
             .Replace(messageBodyKey, messageBodyValue, StringComparison.CurrentCultureIgnoreCase)
             .Replace(buttonHrefKey, buttonHrefValue, StringComparison.CurrentCultureIgnoreCase)
             .Replace(buttonTextKey, buttonTextValue, StringComparison.CurrentCultureIgnoreCase);

         return body;
      }


   }
}