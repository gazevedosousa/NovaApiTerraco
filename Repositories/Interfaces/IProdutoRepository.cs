using TerracoDaCida.DTO;
using TerracoDaCida.Models;

namespace TerracoDaCida.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        Task<List<Produto>> BuscarProdutos();
        Task<bool> CriarProduto(Produto produto);
        Task<bool> ExcluirProduto(int coProduto);
        Task<Produto?> BuscarProduto(int coProduto);
        Task<Produto?> BuscarProdutoPorNome(string noProduto);
        Task<List<TipoProduto>> BuscarTiposProduto();
        Task<TipoProduto?> BuscarTipoProduto(int coTipoProduto);
        Task<TipoProduto?> BuscarTipoProdutoPorNome(string noTipoProduto);
        Task<bool> CriarTipoProduto(TipoProduto tipoProduto);
        Task<bool> ExcluirTipoProduto(int coTipoProduto);
        Task<bool> SaveChangesAsync();
    }
}
