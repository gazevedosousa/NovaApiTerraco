using System.Text.Json.Serialization;
using TerracoDaCida.Enums;

namespace TerracoDaCida.DTO
{
    public class PagamentoDTO
    {
        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }
        [JsonPropertyName("comanda")]
        public int Comanda { get; set; }
        [JsonPropertyName("dtPagamento")]
        public DateOnly DataPagamento { get; set; }
        [JsonPropertyName("valorPagamento")]
        public decimal ValorPagamento { get; set; }

    }
}
