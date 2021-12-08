using System;
using System.Diagnostics;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Project.Infrastructure;

namespace Project.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/Error")]
        public IActionResult Error([FromServices] IOptions<Project.WebApi.Serilog> serilogOptions)
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (serilogOptions.Value.TryGetSeqServerUrl(out string serverUrl))
            {
                var traceId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
                var filter = HttpUtility.UrlEncode($"SpanId='{traceId}'");
                var detail = $"你可以從 {serverUrl}/#/events?filter={filter} 看到更多 Exception 的詳細資訊。";
                return Problem(
                    detail: detail,
                    title: context.Error.Message);
            }
            else
            {
                return Problem(
                    detail: context.Error.StackTrace,
                    title: context.Error.Message);
            }
        }

        /// <summary>
        /// 取得測試的Exception
        /// </summary>
        [HttpGet("Exception")]
        public void GetException() =>
            throw new Exception("Test");
    }
}