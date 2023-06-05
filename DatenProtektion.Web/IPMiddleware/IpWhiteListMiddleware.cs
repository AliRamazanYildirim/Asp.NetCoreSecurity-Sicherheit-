using Microsoft.Extensions.Options;
using System.Net;

namespace DatenProtektion.Web.IPMiddleware
{
    public class IpWhiteListMiddleware
    {
        private readonly RequestDelegate? _requestDelegate;
        private readonly IpWhiteList _ipWhiteList;
        public IpWhiteListMiddleware(RequestDelegate? requestDelegate, IOptions<IpWhiteList> ipWhiteList)
        {
            _requestDelegate = requestDelegate;
            _ipWhiteList = ipWhiteList.Value;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            var anfrageIpAdresse = httpContext.Connection.RemoteIpAddress;

            var istWhiteList = _ipWhiteList?.AllowedIPs?.Where(x => IPAddress.Parse(x).Equals(anfrageIpAdresse)).Any();


            if (!istWhiteList.HasValue || !istWhiteList.Value)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            if (_requestDelegate != null)
            {
                await _requestDelegate(httpContext);
            }
        }
    }

}
