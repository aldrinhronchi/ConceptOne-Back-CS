using Coopersam_WebAPI_CS.Extensions.Middleware;
using Coopersam_WebAPI_CS.Models.Usuario;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Coopersam_WebAPI_CS.Extensions.Helpers
{
    /// <summary>
    /// Helper para efetuar as ações relacionadas ao Token JWT
    /// </summary>
    public class TokenHelper
    {
        /// <summary>
        /// Gera o Token JWT daquele usuario
        /// </summary>
        /// <param name="usuario">O usuario a ser efetuado a autenticação</param>
        /// <returns>O Token JWT</returns>
        public static String GerarToken(Usuario usuario)
        {
            IConfiguration appsettings = ServiceLocator.Current.BuscarServico<IConfiguration>();
            JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(appsettings["Jwt:Key"]);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.Name, usuario.Nome),
                        new Claim(ClaimTypes.Email, usuario.Email),
                        new Claim(ClaimTypes.NameIdentifier, usuario.ID.ToString())
                    }),
                Issuer = appsettings["Jwt:Issuer"],
                Audience = appsettings["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = Handler.CreateToken(tokenDescriptor);
            return Handler.WriteToken(token);
        }
    }
}