using System.Text.Json.Serialization;
using TerracoDaCida.Enums;
using TerracoDaCida.Models;

namespace TerracoDaCida.DTO
{
    public class DetalhaComandaDTO
    {
        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }
        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;
        [JsonPropertyName("situacao")]
        public SituacaoComandaEnum Situacao { get; set; }
        [JsonPropertyName("lancamentos")]
        public List<LancamentoDTO> Lancamentos { get; set; } = new List<LancamentoDTO>();
        [JsonPropertyName("pagamentos")]
        public List<PagamentoDTO> Pagamentos { get; set; } = new List<PagamentoDTO>();
        [JsonPropertyName("valorLancamentos")]
        public decimal ValorLancamentos { get; set; }
        [JsonPropertyName("valorDezPorCento")]
        public decimal ValorDezPorCento { get; set; }
        [JsonPropertyName("valorCouvert")]
        public decimal ValorCouvert { get; set; }
        [JsonPropertyName("qtdCouvert")]
        public int? QtdCouvert { get; set; }
        [JsonPropertyName("valorTotalCouvert")]
        public decimal ValorTotalCouvert { get; set; }
        [JsonPropertyName("valorPagamentos")]
        public decimal ValorPagamentos{ get; set; }
        [JsonPropertyName("valorDescontos")]
        public decimal ValorDescontos { get; set; }
        [JsonPropertyName("valorTroco")]
        public decimal ValorTroco { get; set; }
        [JsonPropertyName("valorTotalComanda")]
        public decimal ValorTotalComanda { get; set; }
        [JsonPropertyName("dtAbertura")]
        public DateOnly DataAbertura { get; set; }
        [JsonPropertyName("dtFechamento")]
        public DateOnly? DataFechamento { get; set; }
        
    }
}
