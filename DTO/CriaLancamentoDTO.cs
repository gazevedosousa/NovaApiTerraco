using System.Text.Json.Serialization;

namespace TerracoDaCida.DTO
{
    public class CriaLancamentoDTO
    {
        [JsonPropertyName("comanda")]
        public int Comanda { get; set; }
        [JsonPropertyName("produto")]
        public int Produto { get; set; }
        [JsonPropertyName("quantidade")]
        public int Quantidade { get; set; }
        [JsonPropertyName("isAcrescimo")]
        public bool IsAcrescimo { get; set; }
    }
}
