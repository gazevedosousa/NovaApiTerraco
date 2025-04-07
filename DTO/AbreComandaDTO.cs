using System.Text.Json.Serialization;
using TerracoDaCida.Enums;

namespace TerracoDaCida.DTO
{
    public class AbreComandaDTO
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;
    }
}
