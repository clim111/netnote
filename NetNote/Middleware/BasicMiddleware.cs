using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace NetNote.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BasicMiddleware
    {
        private readonly RequestDelegate _next;
        public const string AuthorizationHeader = "Authorization";
        public const string WWWAuthorizationHeader = "WWW-Authenticate";
        private BasicUser _user;
        public BasicMiddleware(RequestDelegate next,BasicUser user)
        {
            _next = next;
            _user = user;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;
            string auth = request.Headers[AuthorizationHeader];
            if (auth==null)
            {
                return BasicResult(httpContext);
            }

            string[] authParts = auth.Split(' ');
            if (authParts.Length!=2)
            {
                return BasicResult(httpContext);
            }
            string basic64 = authParts[1];
            string authValue;

            try
            {
                byte[] bytes = Convert.FromBase64String(basic64);
                authValue = Encoding.ASCII.GetString(bytes);
            }
            catch (Exception ex)
            {
                authValue = null;
            }
            if (string.IsNullOrEmpty(authValue))
            {
                return BasicResult(httpContext);
            }

            string userName;
            string password;
            int sepIndex = authValue.IndexOf(':');
            if (sepIndex==-1)
            {
                userName = authValue;
                password = string.Empty;
            }
            else
            {
                userName = authValue.Substring(0, sepIndex);
                password = authValue.Substring(sepIndex + 1);
            }

            if (_user.UserName.Equals(userName)&&_user.Password.Equals(password))
            {
                return _next(httpContext);
            }

            return BasicResult(httpContext);
        }

        private static Task BasicResult(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 401;
            httpContext.Response.Headers.Add(WWWAuthorizationHeader,"Basic realm \"localhost\"");
            return Task.FromResult(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BasicMiddlewareExtensions
    {
        public static IApplicationBuilder UseBasicMiddleware(this IApplicationBuilder builder,BasicUser user)
        {
            if (user==null)
            {
                throw new ArgumentException("需设置Basic用户");
            }
            return builder.UseMiddleware<BasicMiddleware>(user);
        }
    }
}
