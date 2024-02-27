using Autenticacion_JWT_RefreshToken.Models;
using Autenticacion_JWT_RefreshToken.Models.Custom;
using Autenticacion_JWT_RefreshToken.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;

namespace Autenticacion_JWT_RefreshToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IAutorizacionServices _autorizacionServices;


    public UsuarioController(IAutorizacionServices autorizacionServices)
        {
            this._autorizacionServices = autorizacionServices;
        }

        [HttpPost]
        [Route("Autenticar")]
        public async Task<IActionResult> Autenticar([FromBody] AutorizacionRequest autorizacionRequest) {
            var result_autorizacion = await _autorizacionServices.DevolverToken(autorizacionRequest);
            if (result_autorizacion is null)
            {

                return Unauthorized(result_autorizacion);

            }
            return Ok(result_autorizacion);
            

        
        
        }

        [HttpPost]
        [Route("ObtenerRefreshToken")]

        public async Task<IActionResult> obtenerRefreshToken([FromBody]RefreshTokenRequest request) { 
        
        var tokenHandler= new JwtSecurityTokenHandler();
            var tokenExpiradoSupuestamente = tokenHandler.ReadJwtToken(request.TokenExpirado);
            if (tokenExpiradoSupuestamente.ValidTo>DateTime.UtcNow) {

                return BadRequest(new AutorizacionResponse {Resultado=false, Msg="Token no ha expirado" });

            }

            string idUsuario = tokenExpiradoSupuestamente.Claims.First(x=> x.Type== JwtRegisteredClaimNames.NameId).Value.ToString();

            var autorizacion_response = await _autorizacionServices.DevolverRefreshToken(request, int.Parse(idUsuario));


            if (autorizacion_response.Resultado is true) {
                return Ok(autorizacion_response);
            
            }
            else
                return BadRequest(autorizacion_response);
        
        }
    }
}
