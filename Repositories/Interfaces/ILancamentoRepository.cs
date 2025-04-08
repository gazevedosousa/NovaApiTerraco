using TerracoDaCida.DTO;
using TerracoDaCida.Models;

namespace TerracoDaCida.Repositories.Interfaces
{
    public interface ILancamentoRepository
    {
        Task<List<Lancamento>> BuscarLancamentosComanda(int coComanda);
        Task<List<Lancamento>> BuscarLancamentosDia(DateOnly dtLancamento);
        Task<List<Lancamento>> BuscarLancamentosPeriodo(DateOnly dtInicial, DateOnly dtFinal);
        Task<Lancamento?> BuscarLancamentoDeProdutoNaComanda(int coComanda, int coProduto);
        Task<bool> ExisteLancamentoDeProdutoNaComanda(int coComanda, int coProduto);
        Task<bool> CriarLancamento(Lancamento lancamento);
        Task<bool> AtualizarLancamento(Lancamento lancamento, int quantidade, decimal vrProduto, bool isAcrescimo);
        Task<bool> ExcluirLancamento(int coLancamento);
        Task<bool> ExisteLancamento(int coLancamento);
        Task<bool> SaveChangesAsync();
    }
}
