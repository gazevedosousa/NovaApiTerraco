using TerracoDaCida.DTO;
using TerracoDaCida.Models;

namespace TerracoDaCida.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> BuscarUsuarios();
        Task<Usuario?> BuscarUsuarioPorNome(string noUsuario);
        Task<Perfil?> BuscarPerfil(short coPerfil);
        Task<bool> CriaUsuario(Usuario usuario);
        Task<bool> SaveChangesAsync();
    }
}
