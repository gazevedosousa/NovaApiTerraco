using System.Threading.Tasks;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Util;

namespace TerracoDaCida.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<ApiResponse<List<ProdutoDTO>>> BuscaProdutos();
        Task<ApiResponse<bool>> CriaProduto(CriaProdutoDTO criaProdutoDTO);
        Task<ApiResponse<bool>> ExcluiProduto(int coProduto);
        Task<ApiResponse<List<TipoProdutoDTO>>> BuscaTiposProduto();
        Task<ApiResponse<bool>> CriaTipoProduto(CriaTipoProdutoDTO criaTipoProdutoDTO);
        Task<ApiResponse<bool>> ExcluiTipoProduto(int coTipoProduto);
        Task<bool> ExisteProdutoDuplicado(string noProduto);
        Task<bool> ExisteTipoProduto(int coTipoProduto);
        Task<bool> ExisteTipoProdutoDuplicado(string noTipoProduto);
    }
}
