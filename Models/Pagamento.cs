using System;
using System.Collections.Generic;

namespace TerracoDaCida.Models;

public partial class Pagamento
{
    public int CoPagamento { get; set; }

    public int CoComanda { get; set; }

    public decimal VrPagamento { get; set; }

    public int CoTipoPagamento { get; set; }

    public DateTime DhCriacao { get; set; }

    public virtual Comandum CoComandaNavigation { get; set; } = null!;
}
