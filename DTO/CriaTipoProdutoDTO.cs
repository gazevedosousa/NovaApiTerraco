using System.Text.Json.Serialization;

namespace TerracoDaCida.DTO
{
    public class CriaTipoProdutoDTO
    {
        [JsonPropertyName("tipoProduto")]
        public string TipoProduto { get; set; } = string.Empty;
    }
}
