using TerracoDaCida.DTO;
using TerracoDaCida.Models;

namespace TerracoDaCida.Repositories.Interfaces
{
    public interface IComandaRepository
    {
        Task<List<Comandum>> BuscarComandas();
        Task<Comandum?> BuscarComanda(int coComanda);
        Task<bool> AbrirComanda(Comandum comanda);
        Task<bool> ExisteComanda(int coComanda);
        Task<bool> ExisteComandaAbertaParaNomeInformado(string noComanda);
        Task<bool> SaveChangesAsync();
    }
}
