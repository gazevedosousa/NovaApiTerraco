using TerracoDaCida.DTO;
using TerracoDaCida.Models;

namespace TerracoDaCida.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> BuscarUsuarios();
        Task<Usuario?> BuscarUsuario(int coUsuario);
        Task<Usuario?> BuscarUsuarioPorNome(string noUsuario);
        Task<bool> ExisteUsuario(int coUsuario);
        Task<bool> ExisteUsuarioDuplicado(string noUsuario);
        Task<bool> ExistePerfilSolicitado(int coPerfil);
        Task<bool> CriarUsuario(Usuario usuario);
        Task<bool> ExcluirUsuario(int coUsuario);
        Task<bool> SaveChangesAsync();
    }
}
