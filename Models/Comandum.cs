using System;
using System.Collections.Generic;

namespace TerracoDaCida.Models;

public partial class Comandum
{
    public int CoComanda { get; set; }

    public int CoSituacao { get; set; }

    public string NoComanda { get; set; } = null!;

    public DateTime? DhCriacao { get; set; }

    public DateTime? DhExclusao { get; set; }

    public virtual ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();

    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
