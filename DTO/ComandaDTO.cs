using System.Text.Json.Serialization;
using TerracoDaCida.Enums;

namespace TerracoDaCida.DTO
{
    public class ComandaDTO
    {
        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }
        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;
        [JsonPropertyName("situacao")]
        public SituacaoComandaEnum Situacao { get; set; }
        [JsonPropertyName("dtAbertura")]
        public DateOnly DataAbertura { get; set; }
        [JsonPropertyName("dtFechamento")]
        public DateOnly? DataFechamento { get; set; }
    }
}
