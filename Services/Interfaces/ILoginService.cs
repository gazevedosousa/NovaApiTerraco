using Microsoft.AspNetCore.Mvc;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Util;

namespace TerracoDaCida.Services.Interfaces
{
    public interface ILoginService
    {
        string RealizaLogin(Usuario usuario);
        bool SenhaCorreta(LoginDTO loginDTO, Usuario usuario);
        string CreateToken(Usuario usuario, string secret);
    }
}
