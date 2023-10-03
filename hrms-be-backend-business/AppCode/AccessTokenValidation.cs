using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace hrms_be_backend_business.AppCode
{
    public class AccessTokenValidation
    {
        public static Task<IPrincipal> ValidateToken(string token, string Secret, string jwtIssuer, string jwtAudience)
        {
            ClaimsPrincipal principal = GetPrincipal(token, Secret, jwtIssuer, jwtAudience);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
                IPrincipal Iprincipal = new ClaimsPrincipal(identity);
                return Task.FromResult(Iprincipal);
            }
            catch (NullReferenceException)
            {

                return Task.FromResult<IPrincipal>(null);
            }
        }

        private static ClaimsPrincipal GetPrincipal(string token, string Secret, string jwtIssuer, string jwtAudience)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Encoding.ASCII.GetBytes(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
