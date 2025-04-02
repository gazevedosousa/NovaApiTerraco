using System;
using System.Collections.Generic;

namespace TerracoDaCida.Models;

public partial class Perfil
{
    public int CoPerfil { get; set; }

    public string NoPerfil { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
