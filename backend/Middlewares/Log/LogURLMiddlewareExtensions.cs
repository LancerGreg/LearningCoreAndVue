using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Middlewares.Log
{
    public static class LogURLMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogUrl(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LogURLMiddleware>();
        }
    }
}
