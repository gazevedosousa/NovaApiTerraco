using System.Text.Json.Serialization;

namespace TerracoDaCida.DTO
{
    public class TipoProdutoDTO
    {
        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }
        [JsonPropertyName("tipoProduto")]
        public string TipoProduto { get; set; } = string.Empty;
    }
}
