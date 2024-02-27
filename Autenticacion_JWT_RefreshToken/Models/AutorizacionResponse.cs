using Autenticacion_JWT_RefreshToken.Models.Custom;
using Autenticacion_JWT_RefreshToken.Services;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Autenticacion_JWT_RefreshToken.Models
{
    public class AutorizacionResponse
    {

        public string? Token { get; set; }

        public string? RefreshToken { get; set; }

        public bool? Resultado { get; set; }

        public string? Msg { get; set; }
    }
}