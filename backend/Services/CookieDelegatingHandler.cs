using System.Linq;
using System;
using System.Runtime.CompilerServices;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace backend.Services
{
    //https://stackoverflow.com/questions/52370605/asp-net-core-2-get-cookies-from-httpresponsemessage
    public class CookieDelegatingHandler : DelegatingHandler
    {
        public CookieDelegatingHandler()
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}