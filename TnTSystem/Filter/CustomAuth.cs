using System.Collections.Generic;
using System.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System;
using System.Threading;

namespace TnTSystem.Filter
{
    public class CustomAuth : AuthorizationFilterAttribute
    {

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            string token = null;
            HttpStatusCode statusCode = HttpStatusCode.OK;

            if (!GetFromHeader(actionContext, out token)) { 
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }

            try
            {
                string seceretKey = ConfigurationManager.AppSettings["Secret"].ToString();

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(seceretKey));
                JwtSecurityTokenHandler _objHandler = new JwtSecurityTokenHandler();
                SecurityToken _objSecurityToken;

                TokenValidationParameters objTokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = ConfigurationManager.AppSettings["Audience"].ToString(),
                    ValidIssuer = ConfigurationManager.AppSettings["Issuer"].ToString(),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    LifetimeValidator = LifetimeValidator
                };

                Thread.CurrentPrincipal = _objHandler.ValidateToken(token, objTokenValidationParameters, out _objSecurityToken);
                return;
            }
            catch (SecurityTokenValidationException)
            {
                statusCode = HttpStatusCode.NotAcceptable;
            }
            catch (Exception)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            actionContext.Response = new HttpResponseMessage(statusCode);
        }

        private bool GetFromHeader(HttpActionContext actionContext, out string token)
        {
            token = null;
            IEnumerable<string> headers;

            if (!actionContext.Request.Headers.TryGetValues("Authorization", out headers))
            { 
                return false;
            }
            var bearerToken = headers.ElementAt(0);
            token = bearerToken.StartsWith("Bearer") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        private bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters tokenValidationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires)
                    return true;
            }
            return false;
        }
    }
}