using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace WebApp.Services.Authorization
{
    public class EditAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private const string HEADER_SPECIAL_STUFF = "specialHeaderStuff";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var UserId = context.HttpContext.Session.GetString("UserId");

            if(UserId == null || UserId.Length==0)
            {
                context.Result = new RedirectResult(string.Format("/login?url={0}", context.HttpContext.Request.GetEncodedUrl()));
            }
            return;
        }
    }
}
