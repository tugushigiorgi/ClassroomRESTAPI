using GoogleClassroom.Data_Transfer_Objects;
using GoogleClassroom.Database.Models;
using GoogleClassroom.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoogleClassroom.Identity
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public AuthenticationResponseDto CreateJwtToken(user user)
        {
             DateTime expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));

           

 


            Claim[] claims = new Claim[] {



   new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),  
      new Claim(JwtRegisteredClaimNames.UniqueName, user.Email!),  
      };








             SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!) );

             SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

           

            var tokenGenerator = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              claims,
              expires: expiration,
              signingCredentials: signingCredentials);


             JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(tokenGenerator);

             return new AuthenticationResponseDto  { Token = token, Email = user.Email, PersonName = user.UserName, Expiration = expiration };

        }



             
        
    }
}
