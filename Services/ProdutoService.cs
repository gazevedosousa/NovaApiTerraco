using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Repositories;
using TerracoDaCida.Repositories.Interfaces;
using TerracoDaCida.Services.Interfaces;
using TerracoDaCida.Util;

namespace TerracoDaCida.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly ILogger<ProdutoService> _logger;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(ILogger<ProdutoService> logger, IProdutoRepository produtoRepository)
        {
            _logger = logger;
            _produtoRepository = produtoRepository;
        }

        public async Task<ApiResponse<List<ProdutoDTO>>> BuscaProdutos()
        {

            List<Produto> produtos = await _produtoRepository.BuscarProdutos();

            List<ProdutoDTO> listaProdutos = produtos.Select(p => new ProdutoDTO()
            {
                Codigo = p.CoProduto,
                Produto = p.NoProduto,
                TipoProduto = p.CoTipoProdutoNavigation.NoTipoProduto,
                ValorProduto = p.VrProduto
            }).ToList();

            return ApiResponse<List<ProdutoDTO>>.SuccessOk(listaProdutos);
        }

        public async Task<ApiResponse<bool>> CriaProduto(CriaProdutoDTO criaProdutoDTO)
        {
            Produto produto = new Produto
            {
                NoProduto = criaProdutoDTO.Produto,
                CoTipoProduto = criaProdutoDTO.TipoProduto,
                VrProduto = criaProdutoDTO.ValorProduto,
                DhCriacao = new DateTime().GetDataAtual()
            };

            if (await _produtoRepository.CriarProduto(produto))
            {
                _logger.LogInformation($"Produto criado com sucesso - {criaProdutoDTO.Produto}");
                return ApiResponse<bool>.SuccessCreated(true);
            }
            else
            {
                return ApiResponse<bool>.Error($"Erro ao criar Produto. DTO de entrada: {criaProdutoDTO.ToJson()}");
            }
        }

        public async Task<ApiResponse<bool>> ExcluiProduto(int coProduto)
        {
            Produto? produto = await _produtoRepository.BuscarProduto(coProduto);

            if (produto != null)
            {
                if (await _produtoRepository.ExcluirProduto(coProduto))
                {
                    _logger.LogInformation($"Produto excluído com sucesso - {produto.NoProduto}");
                    return ApiResponse<bool>.NoContent(true);
                }
                else
                {
                    return ApiResponse<bool>.Error($"Erro ao excluir Produto. coProduto: {coProduto}");
                }
            }
            else
            {
                return ApiResponse<bool>.Error($"Produto não existente. coProduto: {coProduto}");
            }
        }

        public async Task<ApiResponse<List<TipoProdutoDTO>>> BuscaTiposProduto()
        {

            List<TipoProduto> tiposProduto = await _produtoRepository.BuscarTiposProduto();

            List<TipoProdutoDTO> listaTiposProduto = tiposProduto.Select(tp => new TipoProdutoDTO()
            {
                Codigo = tp.CoTipoProduto,
                TipoProduto = tp.NoTipoProduto
            }).ToList();

            return ApiResponse<List<TipoProdutoDTO>>.SuccessOk(listaTiposProduto);
        }

        public async Task<ApiResponse<bool>> CriaTipoProduto(CriaTipoProdutoDTO criaTipoProdutoDTO)
        {
            TipoProduto tipoProduto = new TipoProduto
            {
                NoTipoProduto = criaTipoProdutoDTO.TipoProduto,
                DhCriacao = new DateTime().GetDataAtual()
            };

            if (await _produtoRepository.CriarTipoProduto(tipoProduto))
            {
                _logger.LogInformation($"Tipo Produto criado com sucesso - {criaTipoProdutoDTO.TipoProduto}");
                return ApiResponse<bool>.SuccessCreated(true);
            }
            else
            {
                return ApiResponse<bool>.Error($"Erro ao criar Tipo Produto. DTO de entrada: {criaTipoProdutoDTO.ToJson()}");
            }
        }

        public async Task<ApiResponse<bool>> ExcluiTipoProduto(int coTipoProduto)
        {
            TipoProduto? tipoProduto = await _produtoRepository.BuscarTipoProduto(coTipoProduto);

            if (tipoProduto != null)
            {
                if (await _produtoRepository.ExcluirTipoProduto(coTipoProduto))
                {
                    _logger.LogInformation($"Tipo Produto excluído com sucesso - {tipoProduto.NoTipoProduto}");
                    return ApiResponse<bool>.NoContent(true);
                }
                else
                {
                    return ApiResponse<bool>.Error($"Erro ao excluir Tipo Produto. coTipoProduto: {coTipoProduto}");
                }
            }
            else
            {
                return ApiResponse<bool>.Error($"Tipo Produto não existente. coTipoProduto: {coTipoProduto}");
            }
        }

        public async Task<bool> ExisteProdutoDuplicado(string noProduto)
        {
            Produto? produto = await _produtoRepository.BuscarProdutoPorNome(noProduto);

            if (produto == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> ExisteTipoProduto(int coTipoProduto)
        {
            TipoProduto? tipoProduto = await _produtoRepository.BuscarTipoProduto(coTipoProduto);

            if (tipoProduto == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> ExisteTipoProdutoDuplicado(string noTipoProduto)
        {
            TipoProduto? tipoProduto = await _produtoRepository.BuscarTipoProdutoPorNome(noTipoProduto);

            if (tipoProduto == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValorSuperiorAZero(decimal vrProduto)
        {
            if (vrProduto <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
