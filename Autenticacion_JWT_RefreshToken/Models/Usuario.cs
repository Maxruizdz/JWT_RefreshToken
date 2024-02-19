using System;
using System.Collections.Generic;

namespace Autenticacion_JWT_RefreshToken.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? NombreUsuario { get; set; }

    public string? Clave { get; set; }
}
