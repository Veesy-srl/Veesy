using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Veesy.Presentation.Response;

namespace Veesy.WebApp.CustomDataAttribute;

public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public CustomAuthorizeAttribute() { }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Recupera il ClaimsPrincipal dal contesto HTTP
        var claimsPrincipal = context.HttpContext.User;

        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token) || !claimsPrincipal.Identity.IsAuthenticated)
        {
            var response = new
            {
                error = new ErrorDto
                {
                    Code = -1,
                    Message = "Unauthorized access. The user is not authenticated."
                }
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = 401
            };
            return;
        }

        try
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);

            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

            if (expClaim == null)
            {
                var response = new
                {
                    error = new ErrorDto
                    {
                        Code = -1,
                        Message = "Unauthorized access. The user is not authenticated."
                    }
                };

                context.Result = new ObjectResult(response)
                {
                    StatusCode = 401
                };
                return;
            }

            var expDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value)).UtcDateTime;

            
            // Verifica se il token è scaduto
            if (DateTime.UtcNow > expDate)
            {
                var response = new
                {
                    error = new ErrorDto
                    {
                        Code = -1,
                        Message = "Unauthorized access. Session is expired."
                    }
                };

                context.Result = new ObjectResult(response)
                {
                    StatusCode = 401
                };
                return;
            }

            // Se necessario, puoi anche aggiungere logica per gestire claims specifici qui
        }
        catch (Exception)
        {
            var response = new
            {
                error = new ErrorDto
                {
                    Code = -1,
                    Message = "Unauthorized access. Error during authentication."
                }
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = 401
            };
            return;
        }
    }
}