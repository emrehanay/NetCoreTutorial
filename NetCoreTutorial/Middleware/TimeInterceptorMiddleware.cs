using System.Diagnostics;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Http;

namespace NetCoreTutorial.Middleware
{
    public class TimeInterceptorMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(TimeInterceptorMiddleware));

        public TimeInterceptorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var watch = Stopwatch.StartNew();
            var url = context.Request.Path;
            await _next(context);
            _logger.Info($"Time for {url} in {watch.ElapsedMilliseconds} ms.");
        }
    }
}