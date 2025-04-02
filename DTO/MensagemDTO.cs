using System.Text.Json.Serialization;

namespace TerracoDaCida.DTO
{
    public class MensagemDTO
    {
        [JsonPropertyName("codigo")]
        public int? Codigo { get; set; }
        [JsonPropertyName("mensagem")]
        public required object Mensagem { get; set; }
        [JsonPropertyName("exception")]
        public string? Exception { get; set; }
    }
}
