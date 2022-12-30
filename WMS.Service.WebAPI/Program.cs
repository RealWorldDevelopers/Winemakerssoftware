
using Azure.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Identity.Web;
using RWD.Toolbox.Logging.Infrastructure.Filters;
using RWD.Toolbox.Logging.Infrastructure.Middleware;
using RWD.Toolbox.Ui.Middleware.SecurityHeaders;
using Serilog;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using WMS.Business.Recipe.Dto;
using WMS.Service.WebAPI;
using WMS.Service.WebAPI.AuthorizationPolicies;
using WMS.Service.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
   Uri KeyVaultName = new(@"https://WMS-Secrets.vault.azure.net");
   builder.Configuration.AddAzureKeyVault(KeyVaultName, new DefaultAzureCredential());
}

// for use within ConfigureServices
var appSettings = new AppSettings();
builder.Configuration.GetSection("ApplicationSettings").Bind(appSettings);
var openAPISettings = new OpenAPISettings();
builder.Configuration.GetSection("OpenAPISettings").Bind(openAPISettings);

// app config settings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.Configure<OpenAPISettings>(builder.Configuration.GetSection("OpenAPISettings"));


// setup logging
var name = Assembly.GetExecutingAssembly().GetName();
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("Assembly", $"{name.Name}")
    .Enrich.WithProperty("Version", $"{name.Version}")
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// add cors settings
builder.Services.AddCors(options =>
{
   options.AddDefaultPolicy(
       builder =>
       {
          builder
             .WithOrigins("https://localhost:7052", "https://www.winemakerssoftware.com")
             .AllowAnyMethod()
             .AllowAnyHeader();
       });
});


// security
builder.Services.AddAuthenticationWithAuthorizationSupport(builder.Configuration);
builder.Services.AddAuthorization(options =>
{
   options.AddPolicy("AccessAsUser",
           policy => policy.Requirements.Add(new ScopesRequirement("access_as_user")));
});

// add cache options
builder.Services.AddMemoryCache();

// add controllers
builder.Services.AddControllers(options =>
{
   // track contorller usage and performance
   options.Filters.Add(typeof(TrackActionPerformanceFilter));
   options.Filters.Add(typeof(TrackActionUsageFilter));

   // Authorize all controllers
   var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
   options.Filters.Add(new AuthorizeFilter(policy));

});

// Add Validatiors https://code-maze.com/fluentvalidation-in-aspnet/
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RecipeDtoValidator>(ServiceLifetime.Transient);

// Configure the API versioning properties of the project. 
builder.Services.AddApiVersioningConfigured();

// Add a Swagger generator and Automatic Request and Response annotations:
builder.Services.AddSwaggerSwashbuckleConfigured(openAPISettings);

// Add dbContext for Recipe Database
builder.Services.AddDbContext<WMS.Data.SQL.WMSContext>(options =>
{
   options.UseSqlServer(builder.Configuration.GetConnectionString("RecipeDatabase"),
      sqlServerOptionsAction: sqlOptions =>
      {
         sqlOptions.EnableRetryOnFailure(
                     maxRetryCount: 10,
                     maxRetryDelay: TimeSpan.FromSeconds(30),
                     errorNumbersToAdd: null);
      });
});

builder.Services.AddAppStorageConfiguration(builder.Configuration);

// add Cosmos DB repositories
builder.Services.AddCosmosDBServices();

// Query Factories
builder.Services.AddTransient<WMS.Business.Journal.IFactory, WMS.Business.Journal.Factory>();
builder.Services.AddTransient<WMS.Business.Recipe.IFactory, WMS.Business.Recipe.Factory>();
builder.Services.AddTransient<WMS.Business.Yeast.IFactory, WMS.Business.Yeast.Factory>();
builder.Services.AddTransient<WMS.Business.Image.IFactory, WMS.Business.Image.Factory>();
builder.Services.AddTransient<WMS.Business.MaloCulture.IFactory, WMS.Business.MaloCulture.Factory>();


// misc services needed
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



// create application
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();

   //Enable middleware to serve Swagger - UI(HTML, JS, CSS, etc.) by specifying the Swagger JSON files(s).
   var descriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
   app.UseSwaggerUI(options =>
   {
      // Build a swagger endpoint for each discovered API version
      foreach (var description in descriptionProvider.ApiVersionDescriptions)
      {
         options.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
      }

      // TODO clean up
      //options.OAuthAppName("SwaggerUI");
      options.OAuthAppName(openAPISettings.OpenIdClientName);
      //options.OAuthClientId("32d4c4c0-8aef-4bd9-832f-25acdae0074d");
      options.OAuthClientId(openAPISettings.OpenIdClientId);
      options.OAuthUsePkce();
      //options.OAuthClientSecret("EOE8Q~2o2SKeS8xwv4B~2J45~lPp4pHXXF1Nmb83");      
      //options.OAuthUseBasicAuthenticationWithAccessCodeGrant();


   });
}

// TODO un comment when web api is completed
// Return custom safe API errors in production
//app.UseApiExceptionHandler(options =>
//{
//    options.AddResponseDetails = UpdateApiErrorResponse;
//    options.DetermineLogLevel = DetermineLogLevel;
//});


// security headers            
app.UseSecurityHeadersMiddleware(new SecurityHeadersBuilder()
               .RemoveServerHeader());

app.UseHsts();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


// Determine how to classify error
LogLevel DetermineLogLevel(Exception ex)
{
   if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) ||
       ex.Message.StartsWith("a network-related", StringComparison.InvariantCultureIgnoreCase))
   {
      return LogLevel.Critical;
   }

   return LogLevel.Error;
}


// Add Custom Notes to Errors
void UpdateApiErrorResponse(HttpContext context, Exception ex, ApiError error)
{
   if (ex.GetType().Name == nameof(SqlException))
   {
      error.Detail = "Exception was a database exception!";
   }
}
