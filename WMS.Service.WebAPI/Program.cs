
using Azure.Identity;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using RWD.Toolbox.Ui.Middleware.CspHeader;
using RWD.Toolbox.Ui.Middleware.SecurityHeaders;
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

// Add Validatiors
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<WMS.Business.Recipe.Dto.RecipeDtoValidator>());


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

// security headers            
app.UseSecurityHeadersMiddleware(new SecurityHeadersBuilder()
               .RemoveServerHeader()
               .AddStrictTransportSecurity()
               );

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

