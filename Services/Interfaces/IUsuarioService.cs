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
        Task<ApiResponse<bool>> ExcluiUsuario(int coUsuario);
        Task<bool> ExisteUsuarioDuplicado(string noUsuario);
        Task<bool> ExistePerfilSolicitado(int coPerfil);
        bool UsuarioSolicitanteIgualAoDeletado(int coUsuario);
    }
}
