
using Azure.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RWD.Toolbox.Logging.Infrastructure.Middleware;
using RWD.Toolbox.Ui.Middleware.CspHeader;
using RWD.Toolbox.Ui.Middleware.SecurityHeaders;
using Serilog;
using System.Data.SqlClient;
using System.Reflection;
using WMS.Business.Recipe.Dto;
using WMS.Service.WebAPI;
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

// app config settings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("ApplicationSettings"));

// setup logging
var name = Assembly.GetExecutingAssembly().GetName();
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    //.MinimumLevel.Debug() 
    //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    //.Enrich.WithAspnetcoreHttpcontext(serviceProvider)
    //.Enrich.FromLogContext()
    //.Enrich.WithExceptionDetails()
    //.Enrich.WithMachineName()
    //.Enrich.WithEnvironmentName()
    //.Enrich.WithEnvironmentUserName()
    .Enrich.WithProperty("Assembly", $"{name.Name}")
    .Enrich.WithProperty("Version", $"{name.Version}")
    //.WriteTo.File(new RenderedCompactJsonFormatter(), @"E:\Testing\error.json", shared: true)
    // .WriteTo.MSSqlServer(connectionString: AppSettings.ConnString,
    //                      sinkOptions: new MSSqlServerSinkOptions { TableName = "Log_Error", AutoCreateSqlTable = true, BatchPostingLimit = 1 },
    //                      columnOptions: Logger.GetSqlColumnOptions())
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

// Add services to the container.

// auth
//https://www.c-sharpcorner.com/article/jwt-authentication-and-authorization-in-net-6-0-with-identity-framework/


builder.Services.AddControllers();

// TODO https://code-maze.com/fluentvalidation-in-aspnet/
// TODO validator test project
// Add Validatiors
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RecipeDtoValidator>(ServiceLifetime.Transient);
//builder.Services.AddTransient<IValidator<RecipeDto>, RecipeDtoValidator>();

// Configure the API versioning properties of the project. 
builder.Services.AddApiVersioningConfigured();

// Add a Swagger generator and Automatic Request and Response annotations:
builder.Services.AddSwaggerSwashbuckleConfigured();

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

// Query Factories
builder.Services.AddTransient<WMS.Business.Journal.IFactory, WMS.Business.Journal.Factory>();
builder.Services.AddTransient<WMS.Business.Recipe.IFactory, WMS.Business.Recipe.Factory>();
builder.Services.AddTransient<WMS.Business.Yeast.IFactory, WMS.Business.Yeast.Factory>();
builder.Services.AddTransient<WMS.Business.Image.IFactory, WMS.Business.Image.Factory>();
builder.Services.AddTransient<WMS.Business.MaloCulture.IFactory, WMS.Business.MaloCulture.Factory>();


// misc services needed
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());




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
    });
}

// TODO un comment when web api is completed
// Return custom safe API errors in production
//app.UseApiExceptionHandler(options =>
//{
//    options.AddResponseDetails = UpdateApiErrorResponse;
//    options.DetermineLogLevel = DetermineLogLevel;
//});


// TODO security headers            
app.UseSecurityHeadersMiddleware(new SecurityHeadersBuilder()
               .RemoveServerHeader());

app.UseHsts();

app.UseCors();

app.UseHttpsRedirection();

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
