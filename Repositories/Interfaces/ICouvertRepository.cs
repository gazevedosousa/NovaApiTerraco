using TerracoDaCida.DTO;
using TerracoDaCida.Models;

namespace TerracoDaCida.Repositories.Interfaces
{
    public interface ICouvertRepository
    {
        Task<List<Couvert>> BuscarCouverts();
        Task<Couvert?> BuscaCouvertAtivo();
        Task<bool> DesativarCouvertAtivo(int coCouvert);
        Task<bool> CriarCouvert(Couvert couvert);
        Task<decimal> BuscarValorCouvert();
        Task<bool> SaveChangesAsync();
    }
}
