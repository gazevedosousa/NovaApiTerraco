using System.Text.Json.Serialization;

namespace TerracoDaCida.DTO
{
    public class CriaProdutoDTO
    {
        [JsonPropertyName("produto")]
        public string Produto { get; set; } = string.Empty;
        [JsonPropertyName("tipoProduto")]
        public int TipoProduto { get; set; }
        [JsonPropertyName("valorProduto")]
        public decimal ValorProduto { get; set; }
    }
}
