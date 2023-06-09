using DatenProtektion.Web.Controllers;
using DatenProtektion.Web.IPMiddleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Net;

namespace DatenProtektion.Web.Filter
{
    public class CheckWhiteList:ActionFilterAttribute
    {
        private readonly IpList _ipList;
        private readonly ILogger<IpList> _logger;

        public CheckWhiteList(IOptions<IpList> ipList, ILogger<IpList> logger)
        {
            _ipList = ipList.Value;
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var IpAdresse = context.HttpContext.Connection.RemoteIpAddress;


            var istWhiteList = _ipList?.AllowedIPs?.Where(x => IPAddress.Parse(x).Equals(IpAdresse)).Any();


            if (!istWhiteList.HasValue || !istWhiteList.Value)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden); 
                _logger.LogWarning($"Anfrage von IP-Adresse {IpAdresse} ist nicht erlaubt.");
                return;
            }
            else
            {
                _logger.LogInformation($"Anfrage von IP-Adresse {IpAdresse} ist erlaubt.");
            }
            base.OnActionExecuting(context);
        }
    }
}
