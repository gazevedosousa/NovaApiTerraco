using System.Text.Json.Serialization;
using TerracoDaCida.Enums;

namespace TerracoDaCida.DTO
{
    public class CriaCouvertDTO
    {
        [JsonPropertyName("valorCouvert")]
        public decimal ValorCouvert { get; set; }
    }
}
