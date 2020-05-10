using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RWD.Toolboox.Ui.Middleware.SecurityHeaders;
using RWD.Toolbox.Ui.Middleware.CspHeader;
using System;
using System.Globalization;
using System.Text;
using WMS.Ui.Data;
using WMS.Ui.Models;

namespace WMS.Ui
{
   public class Startup
   {

      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         // for use within ConfigureServices
         var appSettings = new AppSettings();
         Configuration.GetSection("ApplicationSettings").Bind(appSettings);

         // app config settings
         services.Configure<AppSettings>(Configuration.GetSection("ApplicationSettings"));

         // business Db Context
         services.AddDbContext<WMS.Data.WMSContext>(options =>
         {
            options.UseSqlServer(Configuration.GetConnectionString("RecipeDatabase"),
               sqlServerOptionsAction: sqlOptions =>
               {
                sqlOptions.EnableRetryOnFailure(
                   maxRetryCount: 10,
                   maxRetryDelay: TimeSpan.FromSeconds(30),
                   errorNumbersToAdd: null);
             });
         });


         //services.AddDbContext<ApplicationDbContext>(options =>
         //    options.UseSqlServer(Configuration.GetConnectionString("RecipeDatabase")));
         // UI Security
         services.AddDbContext<ApplicationDbContext>(options =>
         {
            options.UseSqlServer(Configuration.GetConnectionString("RecipeDatabase"),
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

         services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
         {
            options.Lockout = lockoutOptions;
         })
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

         services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddCookie()
             .AddJwtBearer(jwtBearerOptions =>
             {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                   ValidateActor = false,
                   ValidateAudience = false,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = Configuration["JwtToken:Issuer"],
                   ValidAudience = Configuration["JwtToken:Audience"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtToken:Key"]))
                };
             });


         // email agent
         var port = int.Parse(appSettings.SMTP.Port, CultureInfo.CurrentCulture);
         var ssl = bool.Parse(appSettings.SMTP.SSL);
         services.AddTransient<RWD.Toolbox.SMTP.IEmailAgent>(s => new RWD.Toolbox.SMTP.EmailAgent(appSettings.SMTP.IP, port, ssl, appSettings.SMTP.UserName, appSettings.SMTP.UserPassword));

         // model factories
         services.AddTransient<Models.Recipes.IFactory, Models.Recipes.Factory>();
         services.AddTransient<Models.Yeasts.IFactory, Models.Yeasts.Factory>();
         services.AddTransient<Models.Conversions.IFactory, Models.Conversions.Factory>();
         services.AddTransient<Models.Contact.IFactory, Models.Contact.Factory>();
         services.AddTransient<Models.Calculations.IFactory, Models.Calculations.Factory>();
         services.AddTransient<Models.Journal.IFactory, Models.Journal.Factory>();
         services.AddTransient<Models.Admin.IFactory, Models.Admin.Factory>();


         // Query Factories
         services.AddTransient<Business.Journal.Queries.IFactory, Business.Journal.Queries.Factory>();
         services.AddTransient<Business.Recipe.Queries.IFactory, Business.Recipe.Queries.Factory>();
         services.AddTransient<Business.Yeast.Queries.IFactory, Business.Yeast.Queries.Factory>();
         services.AddTransient<Business.Image.Queries.IFactory, Business.Image.Queries.Factory>();

         // Command Factories
         services.AddTransient<Business.Journal.Commands.IFactory, Business.Journal.Commands.Factory>();
         services.AddTransient<Business.Recipe.Commands.IFactory, Business.Recipe.Commands.Factory>();
         services.AddTransient<Business.Yeast.Commands.IFactory, Business.Yeast.Commands.Factory>();

         // DTO Factories
         services.AddTransient<Business.Journal.Dto.IFactory, Business.Journal.Dto.Factory>();
         services.AddTransient<Business.Recipe.Dto.IFactory, Business.Recipe.Dto.Factory>();
         services.AddTransient<Business.Yeast.Dto.IFactory, Business.Yeast.Dto.Factory>();

         // misc services needed
         services.AddAutoMapper(typeof(Startup));

         // application insights
         services.AddApplicationInsightsTelemetry();

         // localization
         services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

         services.AddMvc()
             .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
             .AddDataAnnotationsLocalization();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         else
         {
            app.UseExceptionHandler("/Home/Error");
         }

         // security headers            
         app.UseSecurityHeadersMiddleware(new SecurityHeadersBuilder()
                        .AddDefaultSecurePolicy()
                        .AddStrictTransportSecurity()
                        );

         // content security header
         app.UseCspMiddleware(builder =>
         {
            builder.Default_Src
                      .AllowSelf();

            builder.Scripts_Src
                      .AllowSelf()
                      .Allow("https://cdnjs.cloudflare.com");

            builder.Styles_Src
                      .AllowSelf()
                      .Allow("https://maxcdn.bootstrapcdn.com")
                      .Allow("https://cdnjs.cloudflare.com");

            builder.Fonts_Src
                      .AllowSelf()
                      .Allow("https://maxcdn.bootstrapcdn.com");

            builder.Imgs_Src
                      .AllowSelf()
                      .AllowData()
                      .Allow("https://realworlddevelopers.com");

            builder.Connect_Src
                   .AllowSelf()
                   .Allow("https://cdnjs.cloudflare.com")
                   .Allow("https://maxcdn.bootstrapcdn.com")
                   .Allow("https://realworlddevelopers.com");

            builder.Object_Src
                   .AllowSelf();

            builder.Frame_Ancestors
                   .AllowNone();

               // builder.ReportUri = "api/CspReport/report";
            });

         var supportedCultures = new[]
         {
                new CultureInfo("en-US"), 
                //new CultureInfo("es"), 
                //new CultureInfo("fr"), 
            };

         app.UseRequestLocalization(options =>
         {
            options.DefaultRequestCulture = new RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
         });


         app.UseStaticFiles();

         app.UseHttpsRedirection();
         app.UseRouting();
         app.UseAuthentication();
         app.UseAuthorization();
         app.UseEndpoints(endpoints =>
         {
            endpoints.MapDefaultControllerRoute();
         });

      }

   }
}
