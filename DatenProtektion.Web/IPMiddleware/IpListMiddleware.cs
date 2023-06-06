using Microsoft.Extensions.Options;
using System.Net;

namespace DatenProtektion.Web.IPMiddleware
{
    public class IpListMiddleware
    {
        private readonly RequestDelegate? _requestDelegate;
        private readonly IpList _ipList;
        public IpListMiddleware(RequestDelegate? requestDelegate, IOptions<IpList> ipList)
        {
            _requestDelegate = requestDelegate;
            _ipList = ipList.Value;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            var anfrageIpAdresse = httpContext.Connection.RemoteIpAddress;

            var istWhiteList = _ipList?.AllowedIPs?.Where(x => IPAddress.Parse(x).Equals(anfrageIpAdresse)).Any();


            if (!istWhiteList.HasValue || !istWhiteList.Value)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await httpContext.Response.WriteAsync("Zugriff verweigert. Ihre IP-Adresse steht auf der schwarzen Liste.");
                return;
            }
           
            if (_requestDelegate != null)
            {
                await _requestDelegate(httpContext);
            }
        }
    }

}
