using System.Text.Json.Serialization;

namespace TerracoDaCida.DTO
{
    public class ProdutoDTO
    {
        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }
        [JsonPropertyName("produto")]
        public string Produto { get; set; } = string.Empty;
        [JsonPropertyName("tipoProduto")]
        public string TipoProduto { get; set; } = string.Empty;
        [JsonPropertyName("valorProduto")]
        public decimal ValorProduto { get; set; }
    }
}
