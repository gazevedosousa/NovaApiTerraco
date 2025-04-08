using System.Text.Json.Serialization;
using TerracoDaCida.Enums;

namespace TerracoDaCida.DTO
{
    public class CriaPagamentoDTO
    {
        [JsonPropertyName("comanda")]
        public int Comanda { get; set; }
        [JsonPropertyName("valorPagamento")]
        public decimal ValorPagamento { get; set; }
        [JsonPropertyName("tipoPagamento")]
        public int TipoPagamento { get; set; }

    }
}
