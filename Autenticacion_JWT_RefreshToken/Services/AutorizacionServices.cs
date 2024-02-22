using Autenticacion_JWT_RefreshToken.Models;
using Autenticacion_JWT_RefreshToken.Models.Custom;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Autenticacion_JWT_RefreshToken.Services
{
    public class AutorizacionServices : IAutorizacionServices
    {
        private readonly DbpruebaContext _dbpruebaContext;
        private readonly IConfiguration _configuration;

        public AutorizacionServices(DbpruebaContext dbpruebaContext, IConfiguration configuration)
        {
            _dbpruebaContext = dbpruebaContext;
            _configuration = configuration;
        }
        private string GenerarToken(string idUsuario)
        {

            var key = _configuration.GetValue<string>("JwtSettings:Key");
            var keyBytes = Encoding.ASCII.GetBytes(key);
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUsuario));

            var credencialesToken = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature);

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = credencialesToken
            };


            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenConfig = tokenHandler.CreateToken(TokenDescriptor);


            string tokenCreado = tokenHandler.WriteToken(tokenConfig);


            return tokenCreado;



        }
        public async Task<AutorizacionResponse> DevolverToken(AutorizacionRequest autorizacion)
        {
            var usuario_encontrado = _dbpruebaContext.Usuarios.FirstOrDefault(x => x.NombreUsuario == autorizacion.Nombre_Usuario && x.Clave == autorizacion.Clave);
            if (usuario_encontrado is null)
            {
                return await Task.FromResult<AutorizacionResponse>(null);



                    

            }
            string tokencreado = GenerarToken(usuario_encontrado.IdUsuario.ToString());
            return new AutorizacionResponse() {Token= tokencreado,Resultado=true, Msg="Ok" };
        }
    }
    
    
}
