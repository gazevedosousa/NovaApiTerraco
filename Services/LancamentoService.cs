using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TerracoDaCida.DTO;
using TerracoDaCida.Enums;
using TerracoDaCida.Models;
using TerracoDaCida.Repositories;
using TerracoDaCida.Repositories.Interfaces;
using TerracoDaCida.Services.Interfaces;
using TerracoDaCida.Util;

namespace TerracoDaCida.Services
{
    public class LancamentoService : ILancamentoService
    {
        private readonly ILogger<LancamentoService> _logger;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly IProdutoRepository _produtoRepository;

        public LancamentoService(ILogger<LancamentoService> logger, ILancamentoRepository lancamentoRepository, IProdutoRepository produtoRepository)
        {
            _logger = logger;
            _lancamentoRepository = lancamentoRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<ApiResponse<List<LancamentoDTO>>> BuscaLancamentosComanda(int coComanda)
        {

            List<Lancamento> lancamentos = await _lancamentoRepository.BuscarLancamentosComanda(coComanda);

            List<LancamentoDTO> listaLancamentos = lancamentos.Select(l => new LancamentoDTO()
            {
                Codigo = l.CoLancamento,
                Comanda = l.CoComanda,
                Produto = new ProdutoDTO()
                {
                    Codigo = l.CoProdutoNavigation.CoProduto,
                    Produto = l.CoProdutoNavigation.NoProduto,
                    TipoProduto = l.CoProdutoNavigation.CoTipoProdutoNavigation.NoTipoProduto,
                    ValorProduto = l.CoProdutoNavigation.VrProduto
                },
                ValorUnitario = l.VrUnitario,
                Quantidade = l.QtdLancamento,
                ValorLancamento = l.VrLancamento,
                DataLancamento = DateOnly.FromDateTime(l.DhCriacao)
            }).ToList();

            return ApiResponse<List<LancamentoDTO>>.SuccessOk(listaLancamentos);
        }

        public async Task<ApiResponse<List<LancamentoDTO>>> BuscaLancamentosDia(DateOnly dtLancamento)
        {

            List<Lancamento> lancamentos = await _lancamentoRepository.BuscarLancamentosDia(dtLancamento);

            List<LancamentoDTO> listaLancamentos = lancamentos.Select(l => new LancamentoDTO()
            {
                Codigo = l.CoLancamento,
                Comanda = l.CoComanda,
                Produto = new ProdutoDTO()
                {
                    Codigo = l.CoProdutoNavigation.CoProduto,
                    Produto = l.CoProdutoNavigation.NoProduto,
                    TipoProduto = l.CoProdutoNavigation.CoTipoProdutoNavigation.NoTipoProduto,
                    ValorProduto = l.CoProdutoNavigation.VrProduto
                },
                ValorUnitario = l.VrUnitario,
                Quantidade = l.QtdLancamento,
                ValorLancamento = l.VrLancamento,
                DataLancamento = DateOnly.FromDateTime(l.DhCriacao)
            }).ToList();

            return ApiResponse<List<LancamentoDTO>>.SuccessOk(listaLancamentos);
        }

        public async Task<ApiResponse<List<LancamentoDTO>>> BuscaLancamentosPeriodo(DateOnly dtInicial, DateOnly dtFinal)
        {

            List<Lancamento> lancamentos = await _lancamentoRepository.BuscarLancamentosPeriodo(dtInicial, dtFinal);

            List<LancamentoDTO> listaLancamentos = lancamentos.Select(l => new LancamentoDTO()
            {
                Codigo = l.CoLancamento,
                Comanda = l.CoComanda,
                Produto = new ProdutoDTO()
                {
                    Codigo = l.CoProdutoNavigation.CoProduto,
                    Produto = l.CoProdutoNavigation.NoProduto,
                    TipoProduto = l.CoProdutoNavigation.CoTipoProdutoNavigation.NoTipoProduto,
                    ValorProduto = l.CoProdutoNavigation.VrProduto
                },
                ValorUnitario = l.VrUnitario,
                Quantidade = l.QtdLancamento,
                ValorLancamento = l.VrLancamento,
                DataLancamento = DateOnly.FromDateTime(l.DhCriacao)
            }).ToList();

            return ApiResponse<List<LancamentoDTO>>.SuccessOk(listaLancamentos);
        }

        public async Task<ApiResponse<bool>> CriaLancamento(CriaLancamentoDTO criaLancamentoDTO)
        {
            Produto? produto = await _produtoRepository.BuscarProduto(criaLancamentoDTO.Produto);
            Lancamento? lancamento = await _lancamentoRepository.BuscarLancamentoDeProdutoNaComanda(criaLancamentoDTO.Comanda, criaLancamentoDTO.Produto);

            if (lancamento != null) 
            {
                if (await _lancamentoRepository.AtualizarLancamento(lancamento, criaLancamentoDTO.Quantidade, produto!.VrProduto, criaLancamentoDTO.IsAcrescimo))
                {
                    _logger.LogInformation($"Lançamento realizado com sucesso - {criaLancamentoDTO.Comanda}, {criaLancamentoDTO.Produto}");
                    return ApiResponse<bool>.SuccessOk(true);
                }
                else
                {
                    return ApiResponse<bool>.Error($"Erro ao realizar lançamento. DTO de entrada: {criaLancamentoDTO.ToJson()}");
                }
            }
            else
            {
                Lancamento novoLancamento = new Lancamento
                {
                    CoComanda = criaLancamentoDTO.Comanda,
                    CoProduto = criaLancamentoDTO.Produto,
                    QtdLancamento = criaLancamentoDTO.Quantidade,
                    VrUnitario = produto!.VrProduto,
                    VrLancamento = produto!.VrProduto * criaLancamentoDTO.Quantidade,
                    DhCriacao = new DateTime().GetDataAtual()
                };

                if (await _lancamentoRepository.CriarLancamento(novoLancamento))
                {
                    _logger.LogInformation($"Lançamento realizado com sucesso - {criaLancamentoDTO.Comanda}, {criaLancamentoDTO.Produto}");
                    return ApiResponse<bool>.SuccessOk(true);
                }
                else
                {
                    return ApiResponse<bool>.Error($"Erro ao criar Lançamento. DTO de entrada: {criaLancamentoDTO.ToJson()}");
                }
            }
        }

        public async Task<ApiResponse<bool>> ExcluiLancamento(int coLancamento)
        {
            if (await _lancamentoRepository.ExcluirLancamento(coLancamento))
            {
                _logger.LogInformation($"Lançamento excluído com sucesso - {coLancamento}");
                return ApiResponse<bool>.NoContent(true);
            }
            else
            {
                return ApiResponse<bool>.Error($"Erro ao excluir Lançamento. coLancamento: {coLancamento}");
            }
            
        }

        public bool QtdZerada(int qtdLancamento)
        {
            if (qtdLancamento <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> ExisteLancamento(int coLancamento)
        {
            return await _lancamentoRepository.ExisteLancamento(coLancamento);
        }

        public async Task<bool> ExistePossibilidadeDeDiminuir(CriaLancamentoDTO criaLancamentoDTO)
        {
            Produto? produto = await _produtoRepository.BuscarProduto(criaLancamentoDTO.Produto);
            Lancamento? lancamento = await _lancamentoRepository.BuscarLancamentoDeProdutoNaComanda(criaLancamentoDTO.Comanda, criaLancamentoDTO.Produto);

            if (lancamento == null)
            {
                return false;
            }

            if(lancamento.QtdLancamento < criaLancamentoDTO.Quantidade)
            {
                return false;
            }

            return true;

        }
       
        public async Task<bool> ExisteLancamentoDeProdutoNaComanda(int coComanda, int coProduto)
        {
            return await _lancamentoRepository.ExisteLancamentoDeProdutoNaComanda(coComanda, coProduto);
        }
    }
}
