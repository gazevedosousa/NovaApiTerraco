using System.Threading.Tasks;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Util;

namespace TerracoDaCida.Services.Interfaces
{
    public interface ILancamentoService
    {
        Task<ApiResponse<List<LancamentoDTO>>> BuscaLancamentosComanda(int coComanda);
        Task<ApiResponse<List<LancamentoDTO>>> BuscaLancamentosDia(DateOnly dtLancamento);
        Task<ApiResponse<List<LancamentoDTO>>> BuscaLancamentosPeriodo(DateOnly dtInicial, DateOnly dtFinal);
        Task<ApiResponse<bool>> CriaLancamento(CriaLancamentoDTO criaLancamentoDTO);
        Task<ApiResponse<bool>> ExcluiLancamento(int coLancamento);
        bool QtdZerada(int qtdLancamento);
        Task<bool> ExisteLancamento(int coLancamento);
        Task<bool> ExistePossibilidadeDeDiminuir(CriaLancamentoDTO criaLancamentoDTO);
        Task<bool> ExisteLancamentoDeProdutoNaComanda(int coComanda, int coProduto);
    }
}
