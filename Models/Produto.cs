using System;
using System.Collections.Generic;

namespace TerracoDaCida.Models;

public partial class Produto
{
    public int CoProduto { get; set; }

    public int CoTipoProduto { get; set; }

    public string NoProduto { get; set; } = null!;

    public decimal VrProduto { get; set; }

    public DateTime? DhCriacao { get; set; }

    public DateTime? DhExclusao { get; set; }

    public virtual TipoProduto CoTipoProdutoNavigation { get; set; } = null!;

    public virtual ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();
}
