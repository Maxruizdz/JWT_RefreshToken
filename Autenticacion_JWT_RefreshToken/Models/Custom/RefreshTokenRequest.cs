namespace Autenticacion_JWT_RefreshToken.Models.Custom
{
    public class RefreshTokenRequest
    {
      public string TokenExpirado { get; set; }

      public string RefreshToken { get; set; }

    }
}
