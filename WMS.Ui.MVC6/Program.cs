
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
using WMS.Ui.Mvc6;
using WMS.Ui.Mvc6.Models;
using WMS.Ui.Mvc6.Auth;
using Azure.Identity;
using RWD.Toolbox.Ui.Middleware.SecurityHeaders;
using RWD.Toolbox.Ui.Middleware.CspHeader;

var appSettings = new AppSettings();
var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    Uri KeyVaultName = new(@"https://WMS-Secrets.vault.azure.net");
    builder.Configuration.AddAzureKeyVault(KeyVaultName, new DefaultAzureCredential());
}

IConfiguration configuration = builder.Configuration;

configuration.GetSection("ApplicationSettings").Bind(appSettings);

// app config settings
builder.Services.Configure<AppSettings>(configuration.GetSection("ApplicationSettings"));

// Cookies same site setting
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});



builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    //options.HeaderName = "PPA-XSRF-TOKEN";
});


// model factories
builder.Services.AddTransient<WMS.Ui.Mvc6.Models.Recipes.IFactory, WMS.Ui.Mvc6.Models.Recipes.Factory>();
builder.Services.AddTransient<WMS.Ui.Mvc6.Models.Yeasts.IFactory, WMS.Ui.Mvc6.Models.Yeasts.Factory>();
builder.Services.AddTransient<WMS.Ui.Mvc6.Models.Conversions.IFactory, WMS.Ui.Mvc6.Models.Conversions.Factory>();
builder.Services.AddTransient<WMS.Ui.Mvc6.Models.Contact.IFactory, WMS.Ui.Mvc6.Models.Contact.Factory>();
builder.Services.AddTransient<WMS.Ui.Mvc6.Models.Calculations.IFactory, WMS.Ui.Mvc6.Models.Calculations.Factory>();
builder.Services.AddTransient<WMS.Ui.Mvc6.Models.Journal.IFactory, WMS.Ui.Mvc6.Models.Journal.Factory>();
builder.Services.AddTransient<WMS.Ui.Mvc6.Models.Admin.IFactory, WMS.Ui.Mvc6.Models.Admin.Factory>();
//builder.Services.AddTransient<WMS.Ui.Mvc6.Models.MaloCulture.IFactory, WMS.Ui.Mvc6.Models.MaloCulture.Factory>();

// comm agents
builder.Services.AddHttpClient<WMS.Communications.ICategoryAgent, WMS.Communications.CategoryAgent>()
    .ConfigureHttpClient(httpClient =>
    {
        httpClient.BaseAddress = new Uri(appSettings.URLs.ServiceAPI);
        httpClient.DefaultRequestHeaders.Add("api-version", "1");
        // using Microsoft.Net.Http.Headers;
        // The GitHub API requires two headers.
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.Accept, "application/vnd.github.v3+json");
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.UserAgent, "HttpRequestsSample");
    });
builder.Services.AddHttpClient<WMS.Communications.IImageAgent, WMS.Communications.ImageAgent>()
    .ConfigureHttpClient(httpClient =>
    {
        httpClient.BaseAddress = new Uri(appSettings.URLs.ServiceAPI);
        httpClient.DefaultRequestHeaders.Add("api-version", "1");
        // using Microsoft.Net.Http.Headers;
        // The GitHub API requires two headers.
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.Accept, "application/vnd.github.v3+json");
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.UserAgent, "HttpRequestsSample");
    });
builder.Services.AddHttpClient<WMS.Communications.IJournalAgent, WMS.Communications.JournalAgent>()
    .ConfigureHttpClient(httpClient =>
    {
        httpClient.BaseAddress = new Uri(appSettings.URLs.ServiceAPI);
        httpClient.DefaultRequestHeaders.Add("api-version", "1");
        // using Microsoft.Net.Http.Headers;
        // The GitHub API requires two headers.
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.Accept, "application/vnd.github.v3+json");
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.UserAgent, "HttpRequestsSample");
    });
builder.Services.AddHttpClient<WMS.Communications.IMaloCultureAgent, WMS.Communications.MaloCultureAgent>()
    .ConfigureHttpClient(httpClient =>
    {
        httpClient.BaseAddress = new Uri(appSettings.URLs.ServiceAPI);
        httpClient.DefaultRequestHeaders.Add("api-version", "1");
        // using Microsoft.Net.Http.Headers;
        // The GitHub API requires two headers.
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.Accept, "application/vnd.github.v3+json");
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.UserAgent, "HttpRequestsSample");
    });
builder.Services.AddHttpClient<WMS.Communications.IRatingAgent, WMS.Communications.RatingAgent>()
    .ConfigureHttpClient(httpClient =>
    {
        httpClient.BaseAddress = new Uri(appSettings.URLs.ServiceAPI);
        httpClient.DefaultRequestHeaders.Add("api-version", "1");
        // using Microsoft.Net.Http.Headers;
        // The GitHub API requires two headers.
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.Accept, "application/vnd.github.v3+json");
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.UserAgent, "HttpRequestsSample");
    });
builder.Services.AddHttpClient<WMS.Communications.IRecipeAgent, WMS.Communications.RecipeAgent>()
    .ConfigureHttpClient(httpClient =>
    {
        httpClient.BaseAddress = new Uri(appSettings.URLs.ServiceAPI);
        httpClient.DefaultRequestHeaders.Add("api-version", "1");
        // using Microsoft.Net.Http.Headers;
        // The GitHub API requires two headers.
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.Accept, "application/vnd.github.v3+json");
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.UserAgent, "HttpRequestsSample");
    });
builder.Services.AddHttpClient<WMS.Communications.ISugarUOMAgent, WMS.Communications.SugarUOMAgent>()
    .ConfigureHttpClient(httpClient =>
    {
        httpClient.BaseAddress = new Uri(appSettings.URLs.ServiceAPI);
        httpClient.DefaultRequestHeaders.Add("api-version", "1");
        // using Microsoft.Net.Http.Headers;
        // The GitHub API requires two headers.
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.Accept, "application/vnd.github.v3+json");
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.UserAgent, "HttpRequestsSample");
    }); ;
builder.Services.AddHttpClient<WMS.Communications.ITargetAgent, WMS.Communications.TargetAgent>()
    .ConfigureHttpClient(httpClient =>
    {
        httpClient.BaseAddress = new Uri(appSettings.URLs.ServiceAPI);
        httpClient.DefaultRequestHeaders.Add("api-version", "1");
        // using Microsoft.Net.Http.Headers;
        // The GitHub API requires two headers.
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.Accept, "application/vnd.github.v3+json");
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.UserAgent, "HttpRequestsSample");
    });
builder.Services.AddHttpClient<WMS.Communications.ITempUOMAgent, WMS.Communications.TempUOMAgent>()
    .ConfigureHttpClient(httpClient =>
    {
        httpClient.BaseAddress = new Uri(appSettings.URLs.ServiceAPI);
        httpClient.DefaultRequestHeaders.Add("api-version", "1");
        // using Microsoft.Net.Http.Headers;
        // The GitHub API requires two headers.
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.Accept, "application/vnd.github.v3+json");
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.UserAgent, "HttpRequestsSample");
    });
builder.Services.AddHttpClient<WMS.Communications.IVarietyAgent, WMS.Communications.VarietyAgent>()
    .ConfigureHttpClient(httpClient =>
    {
        httpClient.BaseAddress = new Uri(appSettings.URLs.ServiceAPI);
        httpClient.DefaultRequestHeaders.Add("api-version", "1");
        // using Microsoft.Net.Http.Headers;
        // The GitHub API requires two headers.
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.Accept, "application/vnd.github.v3+json");
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.UserAgent, "HttpRequestsSample");
    });
builder.Services.AddHttpClient<WMS.Communications.IVolumeUOMAgent, WMS.Communications.VolumeUOMAgent>()
    .ConfigureHttpClient(httpClient =>
    {
        httpClient.BaseAddress = new Uri(appSettings.URLs.ServiceAPI);
        httpClient.DefaultRequestHeaders.Add("api-version", "1");
        // using Microsoft.Net.Http.Headers;
        // The GitHub API requires two headers.
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.Accept, "application/vnd.github.v3+json");
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.UserAgent, "HttpRequestsSample");
    });
builder.Services.AddHttpClient<WMS.Communications.IYeastAgent, WMS.Communications.YeastAgent>()
    .ConfigureHttpClient(httpClient =>
    {
        httpClient.BaseAddress = new Uri(appSettings.URLs.ServiceAPI);
        httpClient.DefaultRequestHeaders.Add("api-version", "1");
        // using Microsoft.Net.Http.Headers;
        // The GitHub API requires two headers.
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.Accept, "application/vnd.github.v3+json");
        //httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.UserAgent, "HttpRequestsSample");
    });

// misc services needed
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Entity Framework for Authorization
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("RecipeDatabase"),
       sqlServerOptionsAction: sqlOptions =>
       {
           sqlOptions.EnableRetryOnFailure(
                      maxRetryCount: 10,
                      maxRetryDelay: TimeSpan.FromSeconds(30),
                      errorNumbersToAdd: null);
       });
});

var lockoutOptions = new LockoutOptions()
{
    AllowedForNewUsers = true,
    DefaultLockoutTimeSpan = TimeSpan.FromHours(int.Parse(appSettings.SecRole.LockoutHours, CultureInfo.CurrentCulture)),
    MaxFailedAccessAttempts = int.Parse(appSettings.SecRole.MaxLoginAttempts, CultureInfo.CurrentCulture)
};

// For Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Lockout = lockoutOptions;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Adding Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtBearerOptions =>
    {
        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateActor = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JwtToken:Issuer"],
            ValidAudience = configuration["JwtToken:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtToken:Key"]))
        };
    });



// email agent
var port = int.Parse(appSettings.SMTP.Port, CultureInfo.CurrentCulture);
var ssl = bool.Parse(appSettings.SMTP.SSL);
builder.Services.AddTransient<RWD.Toolbox.SMTP.IEmailAgent>(s => new RWD.Toolbox.SMTP.EmailAgent(appSettings.SMTP.IP, port, ssl, appSettings.SMTP.UserName, appSettings.SMTP.UserPassword));

builder.Services.AddMvc();
// TODO services.AddMvc()
//   .AddNewtonsoftJson(options =>
//      options.SerializerSettings.ContractResolver = new DefaultContractResolver());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// security headers            
app.UseSecurityHeadersMiddleware(new SecurityHeadersBuilder()
               .AddDefaultSecurePolicy()
               .AddStrictTransportSecurity()
               .RemoveServerHeader()
               );

// TODO content security header
app.UseCspMiddleware(builder =>
{
    builder.Default_Src
              .AllowSelf();

    builder.Scripts_Src
              .AllowSelf();

    builder.Styles_Src
              .AllowSelf()
              .Allow("https://maxcdn.bootstrapcdn.com");

    builder.Fonts_Src
              .AllowSelf()
              .Allow("https://maxcdn.bootstrapcdn.com");

    builder.Imgs_Src
              .AllowSelf()
              .AllowData()
              .Allow("https://realworlddevelopers.com");

    builder.Connect_Src
           .AllowSelf()
           .Allow("wss://localhost:44367/WMS.Ui.MVC6/")
           .Allow("https://realworlddevelopers.com");

    builder.Object_Src
           .AllowSelf();

    builder.Frame_Ancestors
            .AllowNone();

    builder.Frame_Src
            .AllowNone();

    builder.Form_Action
            .AllowSelf();

    builder.Worker_Src
            .AllowSelf();

    builder.Manifest_Src
            .AllowSelf();

    builder.Prefetch_Src
            .AllowSelf();

    builder.Navigate_To
            .AllowSelf();

});

app.UseHttpsRedirection();

//app.UseStaticFiles();
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".webmanifest"] = "application/manifest+json";
app.UseStaticFiles(new StaticFileOptions()
{
    ContentTypeProvider = provider
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
