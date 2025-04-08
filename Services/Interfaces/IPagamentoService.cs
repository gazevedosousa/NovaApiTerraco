using System.Threading.Tasks;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Util;

namespace TerracoDaCida.Services.Interfaces
{
    public interface IPagamentoService
    {
        Task<ApiResponse<List<PagamentoDTO>>> BuscaPagamentosComanda(int coComanda);
        Task<ApiResponse<List<PagamentoDTO>>> BuscaPagamentosDia(DateOnly dtPagamento);
        Task<ApiResponse<List<PagamentoDTO>>> BuscaPagamentosPeriodo(DateOnly dtInicial, DateOnly dtFinal);
        Task<ApiResponse<bool>> CriaPagamento(CriaPagamentoDTO criaPagamentoDTO);
        Task<ApiResponse<bool>> ExcluiPagamento(int coPagamento);
        bool QtdZerada(int qtdPagamento);
        Task<bool> ExistePossibilidadeDePagamento(CriaPagamentoDTO criaPagamentoDTO);
        Task<bool> ExistePagamento(int coPagamento);
        bool ExisteTipoPagamento(int tipoPagamento);
        bool ValorSuperiorAZero(decimal vrPagamento);
    }
}
