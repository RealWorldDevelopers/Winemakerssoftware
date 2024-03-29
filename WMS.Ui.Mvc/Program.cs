﻿
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;

namespace WMS.Ui.Mvc
{
   public static class Program
   {
      public static void Main(string[] args)
      {
         CreateHostBuilder(args).Build().Run();
      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((context, config) =>
               {
                  var keyVaultEndpoint = GetKeyVaultEndpoint();
                  if (!string.IsNullOrEmpty(keyVaultEndpoint))
                  {
                     var azureServiceTokenProvider = new AzureServiceTokenProvider();
                     var keyVaultClient = new KeyVaultClient(
                           new KeyVaultClient.AuthenticationCallback(
                               azureServiceTokenProvider.KeyVaultTokenCallback));
                     config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                  }
               })
               .ConfigureWebHostDefaults(webBuilder =>
               {
                  webBuilder.UseStartup<Startup>();
               });

      private static string GetKeyVaultEndpoint() => "https://WMS-Secrets.vault.azure.net";

   }
}

// https://docs.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?view=aspnetcore-6.0

