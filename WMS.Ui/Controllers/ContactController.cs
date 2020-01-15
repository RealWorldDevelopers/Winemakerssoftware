
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using RWD.Toolbox.SMTP;
using System;
using System.Threading.Tasks;
using WMS.Ui.Models;
using WMS.Ui.Models.Contact;

namespace WMS.Ui.Controllers
{
    [Authorize(Roles = "GeneralUser")]
    public class ContactController : BaseController
    {
        private readonly IEmailAgent _emailAgent;
        private readonly IFactory _modelFactory;
        private readonly AppSettings _appSettings;

        private readonly IStringLocalizer<ContactController> _localizer;

        public ContactController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            IOptions<AppSettings> appSettings, IFactory modelFactory, IEmailAgent emailAgent, IStringLocalizer<ContactController> localizer, TelemetryClient telemetry) : base(configuration, userManager, roleManager, telemetry)
        {
            _localizer = localizer;
            _emailAgent = emailAgent;
            _modelFactory = modelFactory;
            _appSettings = appSettings?.Value;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Contact Us";
            ViewData["PageDesc"] = "Communicate with the folks at Winemakers Software";

            var submittedBy = await UserManagerAgent.GetUserAsync(User).ConfigureAwait(false);
            var model = _modelFactory.CreateContactModel();
            model.User = submittedBy;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Email(ContactViewModel model)
        {
            ViewData["Title"] = "Contact Us";
            ViewData["PageDesc"] = "Communicate with the folks at Winemakers Software";

            // create email for admin
            var msg = $"<p>User: {model?.User.UserName} <br />Email: {model.User.Email} <br />Last Name: {model.User.LastName} <br />First Name: {model.User.FirstName}</p> " +
                $"<p>Subject: {model.Subject}</p><p>Message: {model.Message.Replace(Environment.NewLine, "<br />", StringComparison.CurrentCultureIgnoreCase)}</p>";

            // send email 
            await _emailAgent.SendEmailAsync(_appSettings.SMTP.FromEmail, "Contact Page of WMS", _appSettings.SMTP.AdminEmail, model.Subject, msg, true, null).ConfigureAwait(false);

            if (!ModelState.IsValid)
            {
                Warning(_localizer["ContactGeneralError"], true);
                return View("Index", model);
            }

            Success(_localizer["ContactSuccess"], true);
            model.Subject = string.Empty;
            model.Message = string.Empty;
            return View("Index", model);

        }


    }
}