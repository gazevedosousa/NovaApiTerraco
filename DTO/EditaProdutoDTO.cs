using System.Text.Json.Serialization;

namespace TerracoDaCida.DTO
{
    public class EditaProdutoDTO
    {
        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }
        [JsonPropertyName("valorProduto")]
        public decimal ValorProduto { get; set; }
    }
}
