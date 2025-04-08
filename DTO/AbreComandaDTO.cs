using System.Text.Json.Serialization;
using TerracoDaCida.Enums;

namespace TerracoDaCida.DTO
{
    public class AbreComandaDTO
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;
        [JsonPropertyName("temDezPorCento")]
        public bool TemDezPorCento { get; set; }
        [JsonPropertyName("temCouvert")]
        public bool TemCouvert { get; set; }
        [JsonPropertyName("qtdCouvert")]
        public int? QtdCouvert { get; set; }
    }
}
