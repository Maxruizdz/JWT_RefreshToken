using Autenticacion_JWT_RefreshToken.Models;
using Autenticacion_JWT_RefreshToken.Models.Custom;

namespace Autenticacion_JWT_RefreshToken.Services
{
    public interface IAutorizacionServices
    {

        Task<AutorizacionResponse> DevolverToken(AutorizacionRequest autorizacion);
    }
}
