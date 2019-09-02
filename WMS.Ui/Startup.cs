using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using WMS.Ui.Data;
using WMS.Ui.Middleware.CspHeader;
using WMS.Ui.Middleware.SecurityHeaders;
using WMS.Ui.Models;

namespace WMS.Ui
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            var _environment = env;
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
                DefaultLockoutTimeSpan = TimeSpan.FromHours(int.Parse(appSettings.SecRole.LockoutHours)),
                MaxFailedAccessAttempts = int.Parse(appSettings.SecRole.MaxLoginAttempts)
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
            var port = int.Parse(appSettings.SMTP.Port);
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
            services.AddTransient<Business.Recipe.Queries.IFactory, Business.Recipe.Queries.Factory>();
            services.AddTransient<Business.Yeast.Queries.IFactory, Business.Yeast.Queries.Factory>();
            services.AddTransient<Business.Image.Queries.IFactory, Business.Image.Queries.Factory>();

            // Command Factories
            services.AddTransient<Business.Recipe.Commands.IFactory, Business.Recipe.Commands.Factory>();
            services.AddTransient<Business.Yeast.Commands.IFactory, Business.Yeast.Commands.Factory>();

            // DTO Factories
            services.AddTransient<Business.Recipe.Dto.IFactory, Business.Recipe.Dto.Factory>();
            services.AddTransient<Business.Yeast.Dto.IFactory, Business.Yeast.Dto.Factory>();

            // misc services needed
            services.AddAutoMapper();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // security headers
            app.UseSecurityHeadersMiddleware(new SecurityHeadersBuilder()
                .AddDefaultSecurePolicy()
                //.AddCustomHeader("X-My-Custom-Header", "So cool")
                );

            // csp header
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

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });

        }

    }
}
