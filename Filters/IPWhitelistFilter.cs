using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace TaxiDispacher.Filters;

public class IPWhitelistFilter : IActionFilter
{
    private IConfiguration _configuration;

    public IPWhitelistFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
        var allowedList = _configuration.GetSection("WhitelistedIPs");
        var al = allowedList.GetChildren();
        var allowedCnt = al.Where(i => i.Value.Equals(ip))
            .ToList().Count();


        if (allowedCnt == 0)
        {
            var requestedPath = context.HttpContext.Request.Path;

          //  _logger.LogWarning("Not whitelisted IP (" + ip + ") tried to access system route:" + requestedPath);
            context.Result = new ForbidResult();
            return;
        }
    }
}
