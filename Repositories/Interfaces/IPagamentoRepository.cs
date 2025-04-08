using TerracoDaCida.DTO;
using TerracoDaCida.Models;

namespace TerracoDaCida.Repositories.Interfaces
{
    public interface IPagamentoRepository
    {
        Task<List<Pagamento>> BuscarPagamentosComanda(int coComanda);
        Task<List<Pagamento>> BuscarPagamentosDia(DateOnly dtPagamento);
        Task<List<Pagamento>> BuscarPagamentosPeriodo(DateOnly dtInicial, DateOnly dtFinal);
        Task<bool> CriarPagamento(Pagamento lancamento);
        Task<bool> ExcluirPagamento(int coPagamento);
        Task<bool> ExistePagamento(int coPagamento);
        Task<bool> SaveChangesAsync();
    }
}
