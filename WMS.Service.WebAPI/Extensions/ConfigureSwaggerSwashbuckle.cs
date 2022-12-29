using System.Reflection;
using FluentValidation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

// Reference
// https://www.dotnetnakama.com/blog/all-about-web-api-versioning-in-asp-dotnet-core/
// https://www.dotnetnakama.com/blog/enriched-web-api-documentation-using-swagger-openapi-in-asp-dotnet-core/


namespace WMS.Service.WebAPI.Extensions
{
   /// <summary>
   /// Configure the Swagger generator.
   /// </summary>
   public static class ConfigureSwaggerSwashbuckle
   {
      /// <summary>
      /// Configure the Swagger generator with XML comments, bearer authentication, etc.
      /// Additional configuration files:
      /// <list type="bullet">
      ///     <item>ConfigureSwaggerSwashbuckleOptions.cs</item>
      /// </list>
      /// </summary>
      /// <param name="services"></param>
      public static void AddSwaggerSwashbuckleConfigured(this IServiceCollection services, OpenAPISettings openAPISettings)
      {
         services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerSwashbuckleOptions>();

         // Configures ApiExplorer (needed from ASP.NET Core 6.0).
         services.AddEndpointsApiExplorer();

         // Register the Swagger generator, defining one or more Swagger documents.
         // Read more here: https://github.com/domaindrivendev/Swashbuckle.AspNetCore
         services.AddSwaggerGen(options =>
         {
            //If we would like to provide request and response examples(Part 1 / 2)
            //    Enable the Automatic(or Manual) annotation of the[SwaggerRequestExample] and[SwaggerResponseExample].
            //    Read more here: https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters
            options.ExampleFilters();

            // If we would like to include documentation comments in the OpenAPI definition file and SwaggerUI.
            // Set the comments path for the XmlComments file.
            // Read more here: https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio#xml-comments
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);


            // add security to swagger
            var scopes = new Dictionary<string, string> { { openAPISettings.ApiScope, "General Access" } };
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
               Description = "OAuth2.0 Auth Code with PKCE",
               Name = "oauth2",
               Type = SecuritySchemeType.OAuth2,
               Flows = new OpenApiOAuthFlows
               {
                  AuthorizationCode = new OpenApiOAuthFlow
                  {
                     AuthorizationUrl = new Uri(openAPISettings.AuthorizationUrl),
                     TokenUrl = new Uri(openAPISettings.TokenUrl),
                     Scopes = scopes
                  }
               }
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
               {
                  new OpenApiSecurityScheme
                  {
                     Reference = new OpenApiReference 
                     { 
                        Type = ReferenceType.SecurityScheme, 
                        Id = "oauth2" 
                     }
                  },
                  new[] { openAPISettings.ApiScope }
               }
            });


         });

         // If we would like to provide request and response examples (Part 2/2)
         // Register examples with the ServiceProvider based on the location assembly or example type.
         services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

         // If we are using FluentValidation, then we can register the following service to add the  fluent validation rules to swagger.
         // Adds FluentValidationRules staff to Swagger. (Minimal configuration)
         services.AddFluentValidationRulesToSwagger();

      }

   }
}

