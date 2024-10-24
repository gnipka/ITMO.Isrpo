using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Workout.Metrics;

namespace Workout.Middlewares;

public class ResponseMetricMiddleware
{
    private readonly RequestDelegate _request;

    public ResponseMetricMiddleware(RequestDelegate request)
    {
        _request = request ?? throw new ArgumentNullException(nameof(request));
    }

    public async Task Invoke(HttpContext httpContext, ServerResponseReporter resposeReporter, ServerResponseTimeReporter timeReporter, ServerActiveRequestsReporter requestsReporter)
    {
        var path = httpContext.Request.Path.Value;
        if (path == "/metrics")
        {
            await _request.Invoke(httpContext);
            return;
        }

        requestsReporter.HoldRequest();
        var sw = Stopwatch.StartNew();
        try
        {
            await _request.Invoke(httpContext);
        }
        finally
        {
            sw.Stop();

            resposeReporter.RegisterRequest(httpContext.Response.StatusCode);

            var routeValues = httpContext.Request.RouteValues;

            var hasControllerName = routeValues.TryGetValue("controller", out object controller);
            var hasActionName = routeValues.TryGetValue("action", out object action);
            if (hasControllerName && hasActionName)
            {
                timeReporter.RegisterResponseTime(controller as string, action as string, sw.Elapsed);
            }
            requestsReporter.ReleseRequest();
        }
    }
}