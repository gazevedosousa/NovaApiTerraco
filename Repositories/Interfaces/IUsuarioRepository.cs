using TerracoDaCida.DTO;
using TerracoDaCida.Models;

namespace TerracoDaCida.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> BuscarUsuarios();
        Task<Usuario?> BuscarUsuarioPorNome(string noUsuario);
        Task<bool> ExisteUsuarioDuplicado(string noUsuario);
        Task<bool> ExistePerfilSolicitado(short coPerfil);
        Task<bool> CriaUsuario(Usuario usuario);
        Task<bool> SaveChangesAsync();
    }
}
