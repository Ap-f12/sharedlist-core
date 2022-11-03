using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedListApi.Services;
using System.Text.Json;

namespace SharedListApi.Filters
{
    public class ValidateTokenFilter : IAuthorizationFilter
    {
        IUserRegistrationService _userRegistrationService;

        public ValidateTokenFilter(IUserRegistrationService userRegistrationService)
        {
            _userRegistrationService = userRegistrationService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {


            context.HttpContext.Request.EnableBuffering();
            var httpRequest = context.HttpContext.Request;
            var bodyReader = new StreamReader(httpRequest.Body);
            var bodyJson = bodyReader.ReadToEndAsync().Result;
            context.HttpContext.Request.Body.Position = 0;

            string? userId;

            //get user id from query or body
            if (httpRequest.Query.ContainsKey("userId"))
            {
                userId = httpRequest.Query["userId"];
                //httpRequest.QueryString = new QueryString(httpRequest.QueryString.Value.Replace($"userId={userId}", ""));

            }
            else if (bodyJson != "" && JsonDocument.Parse(bodyJson).RootElement.TryGetProperty("userId", out JsonElement userIdElement) == true)
            {
                userId = userIdElement.GetString();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
            var isValidRequest = false;

            var isTokenPresentInRequest = TryGetAuthorizationTokenFromRequest(httpRequest, out string? token);


            if (isTokenPresentInRequest == true && token != null && userId != null)
            {
                {
                    isValidRequest = _userRegistrationService.IsTokenValid(userId, token);
                }
            }


            if (isValidRequest == false)
            {
                throw new UnauthorizedAccessException();
            }






        }
        public bool TryGetAuthorizationTokenFromRequest(HttpRequest request, out string? token)
        {
            if (request.Headers.TryGetValue("Authorization", out var authToken) == true)
            {
                token = authToken;
                return true;
            }
            else
            {
                token = null;
                return false;
            }
         ;
        }


    }

}
