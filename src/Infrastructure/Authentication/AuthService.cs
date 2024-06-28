using ExpenseTracker.Application.Authentication.Common;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Domain.Models.Common.Authentication;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<string> RegisterAsync(string email, string password, string firstName, string lastName, string avatar)
        {
            try
            {
                var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs()
                {
                    Email = email,
                    Password = password,
                    DisplayName = $"{firstName} {lastName}",
                });
                return userRecord.Uid;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string?> LoginWithEmailAndPasswordAsync(string email, string password)
        {
            var request = new
            {
                email,
                password,
                returnSecureToken = true
            };

            var baseUrl = _configuration["JwtSettings:TokenUri"];
            var response = await _httpClient.PostAsJsonAsync(baseUrl, request);
            var authToken = await response.Content.ReadFromJsonAsync<AuthToken>();

            return authToken?.IdToken;
        }
        public async Task<VerifyTokenResult?> VerifyTokenAsync(string idToken)
        {
            try
            {
                var result = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
                if (result != null)
                {
                    var email = result.Claims.First(x => x.Key == JwtRegisteredClaimNames.Email).Value?.ToString();
                    var name = result.Claims.First(x => x.Key == JwtRegisteredClaimNames.Name).Value?.ToString();
                    var emailVerified = result.Claims.First(x => x.Key == "email_verified").Value?.ToString();
                    
                    return new VerifyTokenResult
                    {
                        IdentityId = result.Uid,
                        Email = email,
                        Name = name,
                        EmailVerified = bool.TryParse(emailVerified, out var emailVerifiedBool) && emailVerifiedBool
                    };
                }

            }
            catch (Exception)
            {
                // ??
            }

            return null;
        }
        public async Task SetCustomClaims(string uid, IReadOnlyDictionary<string, object> customClaims)
        {
            await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, customClaims);
        }
    }
}
