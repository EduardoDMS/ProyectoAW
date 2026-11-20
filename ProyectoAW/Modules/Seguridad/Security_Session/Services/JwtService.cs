using Microsoft.IdentityModel.Tokens;
using ProyectoAW.Modules.Seguridad.Security_Session.DTOs.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProyectoAW.Modules.Seguridad.Security_Session.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;


        public JwtService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string GeneracionToken(SesionResponse sesion)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, sesion.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, sesion.Username),
                new Claim(ClaimTypes.Role, sesion.Rol),
                new Claim("IdRol", sesion.IdRol.ToString())

            };
            claims.AddRange(sesion.Permisos.Select(permiso => new Claim("Permiso", permiso)));

            var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(8),
                    signingCredentials: credentials );

            return new JwtSecurityTokenHandler()
                       .WriteToken(token);
        
        }
    }
}
