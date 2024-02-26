using Autenticacion_JWT_RefreshToken.Models.Custom;
using Autenticacion_JWT_RefreshToken.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
