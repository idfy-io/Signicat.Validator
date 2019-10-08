using System.Net;
using Microsoft.AspNetCore.Http;

namespace Signicat.Validator.IdfyPades.Infrastructure.Extensions
{
    public static class HttpRequestExtensions
    {
        private const string NullIPv6 = "::1";

        public static string IpAddress(this HttpRequest httpRequest)
        {
            return httpRequest.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public static string UserAgent(this HttpRequest request)
        {
            return request.Headers["User-Agent"];
        }

        public static bool IsLocal(this HttpRequest request)
        {
            var conn = request.HttpContext.Connection;
            if (!conn.RemoteIpAddress.IsSet())
                return true;

            // we have a remote address set up
            // is local is same as remote, then we are local
            if (conn.LocalIpAddress.IsSet())
                return conn.RemoteIpAddress.Equals(conn.LocalIpAddress);

            // else we are remote if the remote IP address is not a loopback address
            return IPAddress.IsLoopback(conn.RemoteIpAddress);
        }

        private static bool IsSet(this IPAddress address)
        {
            return address != null && address.ToString() != NullIPv6;
        }
    }
}