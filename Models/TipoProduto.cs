using System;
using System.Collections.Generic;

namespace TerracoDaCida.Models;

public partial class TipoProduto
{
    public int CoTipoProduto { get; set; }

    public string NoTipoProduto { get; set; } = null!;

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
