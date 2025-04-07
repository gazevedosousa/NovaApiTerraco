using System.Threading.Tasks;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Util;

namespace TerracoDaCida.Services.Interfaces
{
    public interface ICouvertService
    {
        Task<ApiResponse<List<CouvertDTO>>> BuscaCouverts();
        Task<ApiResponse<bool>> CriaCouvert(CriaCouvertDTO criaCouvertDTO);
    }
}
