using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Util;

namespace TerracoDaCida.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<ApiResponse<List<UsuarioDTO>>> BuscaUsuarios();
        Task<Usuario?> BuscaUsuarioParaLogin(string noUsuario);
        Task<ApiResponse<bool>> CriaUsuario(CriaUsuarioDTO criaUsuarioDTO);
        Task<bool> ExisteUsuarioDuplicado(string noUsuario);
        Task<bool> ExistePerfilSolicitado(short coPerfil);
    }
}
