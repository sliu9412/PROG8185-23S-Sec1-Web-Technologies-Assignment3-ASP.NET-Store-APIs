using GroupAssignment3.Dto;
using GroupAssignment3.Exception;
using GroupAssignment3.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GroupAssignment3.Helper
{
    public class JwtHelper
    {
        public static readonly string UserIdClaim = "userid";
        public static readonly TimeSpan TokenLifeTime = TimeSpan.FromHours(8);
        private static string TokenSecret = null;
        private static string TokenIssuer = null;

        public static String CreateUserToken(string userId, string userMail, bool isScopeAdmin)
        {
            if (TokenSecret == null || TokenIssuer == null)
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                TokenSecret = config["JwtSettings:Key"]!;
                TokenIssuer = config["JwtSettings:Issuer"]!;
            }

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, userMail),
                new(JwtRegisteredClaimNames.Email, userMail),
                new(UserIdClaim, userId),
                new(IdentityData.ScopeClaimName, isScopeAdmin ? IdentityData.ScopeAdminValue : IdentityData.ScopeUserValue)
            };

            var key = Encoding.UTF8.GetBytes(TokenSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifeTime),
                Issuer = TokenIssuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static JwtUser ValidateUserIDByScope(IHttpContextAccessor httpContext, String requestUserId)
        {
            var jwtUser = GetSessionFromToken(httpContext);
            if (jwtUser.scope == IdentityData.ScopeUserValue)
            {
                if (jwtUser.userId.CompareTo(requestUserId) != 0)
                {
                    throw new HttpException(401, "Unauthorize action over user ID '" + requestUserId + "'");
                }
            }
            return jwtUser;
        }

        public static JwtUser GetSessionFromToken(IHttpContextAccessor httpContext)
        {
            JwtUser jwtUser = null;
            if (httpContext.HttpContext is not null){
                var userContext = httpContext.HttpContext.User;
                string email = userContext.FindFirstValue(ClaimTypes.Email);
                string userId = userContext.FindFirstValue(UserIdClaim);
                string scope = userContext.FindFirstValue(IdentityData.ScopeClaimName);
                jwtUser = new JwtUser();
                jwtUser.scope = scope;
                jwtUser.userId = userId;
                jwtUser.email = email;
            }
            if (jwtUser == null)
            {
                throw new HttpException(401, "Invalid JWT Token");
            }
            return jwtUser;
        }

    }
}
