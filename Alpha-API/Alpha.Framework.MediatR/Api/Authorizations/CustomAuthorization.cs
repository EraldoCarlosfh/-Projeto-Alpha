using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;

namespace Alpha.Framework.MediatR.Api.Authorizations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorization : Attribute, IActionFilter
    {
        private readonly int[] _accesses;

        public CustomAuthorization(params int[] accesses)
        {
            _accesses = accesses;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

            if (hasAllowAnonymous) return;

            var headers = context.HttpContext.Request.Headers;


            var authKey = headers.Keys.FirstOrDefault(k => k.ToLowerInvariant() == "authorization");

            if (authKey != null)
            {
                var tokenContent = headers[authKey].ToString().Replace("bearer ", "", StringComparison.InvariantCultureIgnoreCase);
                var token = new JwtSecurityToken(tokenContent);

                if (token != null || token.ValidTo >= DateTime.UtcNow.ToLocalTime())
                {
                    if (_accesses != null && _accesses.Any())
                    {
                        var userRoles = token.Claims.Where(c => c.Type == "role")?.Select(c => Convert.ToInt32(c.Value))?.ToList();

                        if (userRoles != null && userRoles.Any())
                        {
                            if (_accesses.Intersect(userRoles).Any())
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }

                }
            }

            context.Result = new UnauthorizedResult();
        }
    }
}