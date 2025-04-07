using System.Text.Json.Serialization;
using TerracoDaCida.Enums;

namespace TerracoDaCida.DTO
{
    public class CouvertDTO
    {
        [JsonPropertyName("valorCouvert")]
        public decimal ValorCouvert { get; set; }
        [JsonPropertyName("isAtivo")]
        public bool IsAtivo { get; set; }
    }
}
