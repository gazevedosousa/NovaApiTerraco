using System;
using System.Collections.Generic;

namespace TerracoDaCida.Models;

public partial class Lancamento
{
    public int CoLancamento { get; set; }

    public int CoComanda { get; set; }

    public int CoProduto { get; set; }

    public int QtdLancamento { get; set; }

    public DateTime DhCriacao { get; set; }

    public virtual Comandum CoComandaNavigation { get; set; } = null!;

    public virtual Produto CoProdutoNavigation { get; set; } = null!;
}
