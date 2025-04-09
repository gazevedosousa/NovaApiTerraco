using System;
using System.Collections.Generic;

namespace TerracoDaCida.Models;

public partial class Comandum
{
    public int CoComanda { get; set; }

    public int CoSituacao { get; set; }

    public string NoComanda { get; set; } = null!;

    public decimal Valordesconto { get; set; }

    public decimal Valortroco { get; set; }

    public DateTime? DhAbertura { get; set; }

    public DateTime? DhFechamento { get; set; }

    public bool Temdezporcento { get; set; }

    public bool Temcouvert { get; set; }

    public int? QtdCouvert { get; set; }

    public virtual ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();

    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
