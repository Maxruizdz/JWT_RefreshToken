using Autenticacion_JWT_RefreshToken.Models;
using Autenticacion_JWT_RefreshToken.Models.Custom;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
            var key = _configuration.GetValue<string>("JwtSettings:key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            // Crear un conjunto de reclamaciones (claims)
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUsuario));

            // Crear credenciales para la firma del token utilizando la clave
            var credencialesToken = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature
            );

            // Configurar el descriptor del token, que incluye las reclamaciones, la expiración y las credenciales de firma
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = credencialesToken
            };

            // Crear un token utilizando el token handler y el descriptor configurado
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            // Escribir el token en formato string
            string tokenCreado = tokenHandler.WriteToken(tokenConfig);

            return tokenCreado;


        }
        public async Task<AutorizacionResponse> DevolverToken(AutorizacionRequest autorizacion)
        {
            var usuario_encontrado =  _dbpruebaContext.Usuarios.FirstOrDefault(x => x.NombreUsuario == autorizacion.Nombre_Usuario && x.Clave == autorizacion.Clave);
            if (usuario_encontrado is null)
            {
                return await Task.FromResult<AutorizacionResponse>(null);



                    

            }
            string tokencreado = GenerarToken(usuario_encontrado.IdUsuario.ToString());
            string RefreshTokenCreado = GenerarRefreshToken();
            return await GuardarHistorialRefreshToken(usuario_encontrado.IdUsuario, tokencreado, RefreshTokenCreado);
        }

        private string GenerarRefreshToken() {
            var byteArray = new byte[64];
            var refreshToken = "";
            using (var mg = RandomNumberGenerator.Create())
            {

                mg.GetBytes(byteArray);
                refreshToken = Convert.ToBase64String(byteArray);


            }

            return refreshToken;

        }

        private async Task<AutorizacionResponse> GuardarHistorialRefreshToken(int idUsuario , string token, string refreshToken) {


            var historialRefreshToken = new HistorialRefreshToken
            {

                IdUsuario = idUsuario,
                Token = token,
                RefreshToken = refreshToken,
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddMinutes(2)




            };
          
            await _dbpruebaContext.HistorialRefreshTokens.AddAsync(historialRefreshToken);
            await _dbpruebaContext.SaveChangesAsync();

            return new AutorizacionResponse() { Token = token, RefreshToken = refreshToken, Resultado = true, Msg = "Ok" };


        
        
        }
        public async Task<AutorizacionResponse> DevolverRefreshToken(RefreshTokenRequest refreshTokenRequest, int id_usuario)
        {
            var refreshTokenEncontrado = _dbpruebaContext.HistorialRefreshTokens.FirstOrDefault(refresh=> refresh.Token== refreshTokenRequest.TokenExpirado && refresh.RefreshToken== refreshTokenRequest.RefreshToken && refresh.IdUsuario == id_usuario);


            if (refreshTokenEncontrado is null) {

                return new AutorizacionResponse { Resultado = false, Msg="No existe RefresToken" };
            
            }

            var refreshTokenCreado = GenerarRefreshToken();
            var tokenCreado = GenerarToken(id_usuario.ToString());




            return await GuardarHistorialRefreshToken(id_usuario,tokenCreado,refreshTokenCreado);
        }
    }
    
    
}
