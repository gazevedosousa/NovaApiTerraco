using System.Text.Json.Serialization;
using TerracoDaCida.Enums;

namespace TerracoDaCida.DTO
{
    public class LancamentoDTO
    {
        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }
        [JsonPropertyName("comanda")]
        public int Comanda { get; set; }
        [JsonPropertyName("produto")]
        public ProdutoDTO Produto { get; set; } = new ProdutoDTO();
        [JsonPropertyName("valorUnitario")]
        public decimal ValorUnitario { get; set; }
        [JsonPropertyName("quantidade")]
        public int Quantidade { get; set; }
        [JsonPropertyName("valorLancamento")]
        public decimal ValorLancamento { get; set; }
        [JsonPropertyName("dtLancamento")]
        public DateOnly DataLancamento { get; set; }

    }
}
