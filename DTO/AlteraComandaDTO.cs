using System.Text.Json.Serialization;
using TerracoDaCida.Enums;

namespace TerracoDaCida.DTO
{
    public class AlteraComandaDTO
    {
        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }
        [JsonPropertyName("alteraDezPorCento")]
        public bool AlteraDezPorCento { get; set; }
        [JsonPropertyName("alteraCouvert")]
        public bool AlteraCouvert { get; set; }
        [JsonPropertyName("qtdCouvert")]
        public int? QtdCouvert { get; set; }
        [JsonPropertyName("valorDesconto")]
        public decimal? ValorDesconto { get; set; }
    }
}
