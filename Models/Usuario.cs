using System;
using System.Collections.Generic;

namespace TerracoDaCida.Models;

public partial class Usuario
{
    public int CoUsuario { get; set; }

    public int CoPerfil { get; set; }

    public string NoUsuario { get; set; } = null!;

    public byte[] Senhahash { get; set; } = null!;

    public byte[] Senhasalt { get; set; } = null!;

    public virtual Perfil CoPerfilNavigation { get; set; } = null!;
}
