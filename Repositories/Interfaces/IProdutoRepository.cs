using TerracoDaCida.DTO;
using TerracoDaCida.Models;

namespace TerracoDaCida.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        Task<List<Produto>> BuscarProdutos();
        Task<bool> CriarProduto(Produto produto);
        Task<bool> EditarProduto(int coProduto, decimal vrProduto);
        Task<bool> ExcluirProduto(int coProduto);
        Task<Produto?> BuscarProduto(int coProduto);
        Task<bool> ExisteProdutoDuplicado(string noProduto);
        Task<List<TipoProduto>> BuscarTiposProduto();
        Task<TipoProduto?> BuscarTipoProduto(int coTipoProduto);
        Task<bool> ExisteVinculoTipoProduto(int coTipoProduto);
        Task<bool> ExisteTipoProduto(int coTipoProduto);
        Task<bool> ExisteTipoProdutoDuplicado(string noTipoProduto);
        Task<bool> CriarTipoProduto(TipoProduto tipoProduto);
        Task<bool> ExcluirTipoProduto(int coTipoProduto);
        Task<bool> SaveChangesAsync();
    }
}
