
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace WMS.Ui.Mvc6.Controllers.Api
{
   /// <summary>
   /// API Controller for CSP Violation Reporting
   /// </summary>
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   [Route("api/[controller]")]
   [ApiController]
   public class CspReportController : ControllerBase
   {
      [HttpPost("~/cspreport")]
      public IActionResult CspViolation([FromBody] CspReportRequest request)
      {
         // TODO log request to a datastore somewhere 
         // add Telemetry
         //_logger.LogWarning($"CSP Violation: {request.CspReport.DocumentUri}, {request.CspReport.BlockedUri}");

         return Ok();
      }

   }

   public class CspReportRequest
   {
      [JsonProperty(PropertyName = "csp-report")]
      public CspReport CspReport { get; set; }
   }

   public class CspReport
   {
      [JsonProperty(PropertyName = "document-uri")]
      public System.Uri DocumentUri { get; set; }

      [JsonProperty(PropertyName = "referrer")]
      public string Referrer { get; set; }

      [JsonProperty(PropertyName = "violated-directive")]
      public string ViolatedDirective { get; set; }

      [JsonProperty(PropertyName = "effective-directive")]
      public string EffectiveDirective { get; set; }

      [JsonProperty(PropertyName = "original-policy")]
      public string OriginalPolicy { get; set; }

      [JsonProperty(PropertyName = "blocked-uri")]
      public System.Uri BlockedUri { get; set; }

      [JsonProperty(PropertyName = "status-code")]
      public int StatusCode { get; set; }
   }

}