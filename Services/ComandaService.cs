using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TerracoDaCida.DTO;
using TerracoDaCida.Enums;
using TerracoDaCida.Models;
using TerracoDaCida.Repositories;
using TerracoDaCida.Repositories.Interfaces;
using TerracoDaCida.Services.Interfaces;
using TerracoDaCida.Util;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TerracoDaCida.Services
{
    public class ComandaService : IComandaService
    {
        private readonly ILogger<ComandaService> _logger;
        private readonly IComandaRepository _comandaRepository;
        private readonly ICouvertRepository _couvertRepository;

        public ComandaService(ILogger<ComandaService> logger, IComandaRepository comandaRepository, ICouvertRepository couvertRepository)
        {
            _logger = logger;
            _comandaRepository = comandaRepository;
            _couvertRepository = couvertRepository;
        }

        public async Task<ApiResponse<List<ComandaDTO>>> BuscaComandas()
        {

            List<Comandum> comandas = await _comandaRepository.BuscarComandas();

            List<ComandaDTO> listaComandas = comandas.Select(c => new ComandaDTO()
            {
                Codigo = c.CoComanda,
                Nome = c.NoComanda,
                Situacao = (SituacaoComandaEnum)c.CoSituacao,
                DataAbertura = DateOnly.FromDateTime((DateTime)c.DhAbertura!),
                DataFechamento = c.DhFechamento == null ? null : DateOnly.FromDateTime((DateTime)c.DhFechamento)
            }).ToList();

            return ApiResponse<List<ComandaDTO>>.SuccessOk(listaComandas);
        }

        public async Task<ApiResponse<List<ComandaDTO>>> BuscaComandasAbertas()
        {

            List<Comandum> comandas = await _comandaRepository.BuscarComandasAbertas();

            List<ComandaDTO> listaComandas = comandas.Select(c => new ComandaDTO()
            {
                Codigo = c.CoComanda,
                Nome = c.NoComanda,
                Situacao = (SituacaoComandaEnum)c.CoSituacao,
                DataAbertura = DateOnly.FromDateTime((DateTime)c.DhAbertura!),
                DataFechamento = c.DhFechamento == null ? null : DateOnly.FromDateTime((DateTime)c.DhFechamento)
            }).ToList();

            return ApiResponse<List<ComandaDTO>>.SuccessOk(listaComandas);
        }

        public async Task<ApiResponse<List<ComandaDTO>>> BuscaComandasFechadas()
        {

            List<Comandum> comandas = await _comandaRepository.BuscarComandasFechadas();

            List<ComandaDTO> listaComandas = comandas.Select(c => new ComandaDTO()
            {
                Codigo = c.CoComanda,
                Nome = c.NoComanda,
                Situacao = (SituacaoComandaEnum)c.CoSituacao,
                DataAbertura = DateOnly.FromDateTime((DateTime)c.DhAbertura!),
                DataFechamento = c.DhFechamento == null ? null : DateOnly.FromDateTime((DateTime)c.DhFechamento)
            }).ToList();

            return ApiResponse<List<ComandaDTO>>.SuccessOk(listaComandas);
        }

        public async Task<ApiResponse<bool>> AbreComanda(AbreComandaDTO abreComandaDTO)
        {

            Comandum comanda = new Comandum
            {
                NoComanda = abreComandaDTO.Nome,
                CoSituacao = (int)SituacaoComandaEnum.Aberta,
                DhAbertura = new DateTime().GetDataAtual()
            };

            if (await _comandaRepository.AbrirComanda(comanda))
            {
                _logger.LogInformation($"Comanda aberta com sucesso para {abreComandaDTO.Nome}");
                return ApiResponse<bool>.SuccessCreated(true);
            }
            else
            {
                return ApiResponse<bool>.Error($"Erro ao abrir Comanda. DTO de entrada: {abreComandaDTO.ToJson()}");
            }
        }

        public async Task<ApiResponse<DetalhaComandaDTO>> DetalhaComanda(int coComanda)
        {
            Comandum? comanda = await _comandaRepository.BuscarComanda(coComanda);

            if (comanda != null)
            {
                List<LancamentoDTO> lancamentos = comanda.Lancamentos.Select(l => 
                    new LancamentoDTO()
                    {
                        Codigo = l.CoLancamento,
                        Comanda = coComanda,
                        Produto = new ProdutoDTO() 
                        {
                            Codigo = l.CoProdutoNavigation.CoProduto,
                            Produto = l.CoProdutoNavigation.NoProduto,
                            TipoProduto = l.CoProdutoNavigation.CoTipoProdutoNavigation.NoTipoProduto,
                            ValorProduto = l.VrLancamento
                        },
                        Quantidade = l.QtdLancamento,
                        ValorLancamento = l.VrLancamento,
                        DataLancamento = DateOnly.FromDateTime(l.DhCriacao)

                    }).ToList();

                List<PagamentoDTO> pagamentos = comanda.Pagamentos.Select(p =>
                    new PagamentoDTO()
                    {
                        Codigo = p.CoPagamento,
                        Comanda = coComanda,
                        ValorPagamento = p.VrPagamento,
                        DataPagamento = DateOnly.FromDateTime(p.DhCriacao)
                    }).ToList();

                decimal valorCouvert = 0;

                if(comanda.Temcouvert)
                {
                    valorCouvert = await _couvertRepository.BuscarValorCouvert();
                }

                decimal valorLancamentos = lancamentos.Sum(l => l.ValorLancamento);
                decimal valorDezPorCento = comanda.Temdezporcento ? valorLancamentos * (decimal)0.1 : 0;
                decimal valorPagamentos = pagamentos.Sum(p => p.ValorPagamento);
                decimal valorDesconto = comanda.Valordesconto.HasValue ? (decimal)comanda.Valordesconto : 0;
                decimal valorTroco = comanda.Valortroco.HasValue ? (decimal)comanda.Valortroco : 0;

                decimal valorTotalComanda = CalculaValorTotalComanda(
                    valorLancamentos, valorPagamentos, valorDezPorCento, valorCouvert, valorDesconto, valorTroco);

                DetalhaComandaDTO detalhaComandaDTO = new DetalhaComandaDTO
                {
                    Codigo = comanda.CoComanda,
                    Nome = comanda.NoComanda,
                    Situacao = (SituacaoComandaEnum)comanda.CoSituacao,
                    Lancamentos = lancamentos,
                    Pagamentos = pagamentos,
                    ValorLancamentos = valorLancamentos,
                    ValorDezPorCento = valorDezPorCento,
                    ValorCouvert = valorCouvert,
                    ValorPagamentos = valorPagamentos,
                    ValorDescontos = valorDesconto,
                    ValorTroco = valorTroco,
                    DataAbertura = DateOnly.FromDateTime((DateTime)comanda.DhAbertura!),
                    DataFechamento = comanda.DhFechamento == null ? null : DateOnly.FromDateTime((DateTime)comanda.DhAbertura!),
                    ValorTotalComanda = valorTotalComanda
                };

                return ApiResponse<DetalhaComandaDTO>.SuccessOk(detalhaComandaDTO);
            }
            else
            {
                return ApiResponse<DetalhaComandaDTO>.Error($"Comanda não existente. coComanda: {coComanda}");
            }
        }

        public async Task<bool> FechaComanda(int coComanda)
        {
            return await _comandaRepository.FecharComanda(coComanda);
        }

        public async Task<bool> ExisteComanda(int coComanda)
        {
            return await _comandaRepository.ExisteComanda(coComanda);
        }

        public async Task<bool> ExisteComandaAbertaParaNomeInformado(string noComanda)
        {
            return await _comandaRepository.ExisteComandaAbertaParaNomeInformado(noComanda);
        }

        public decimal CalculaValorTotalComanda(
            decimal valorLancamentos, decimal valorPagamentos, decimal valorDezPorCento, decimal valorCouvert, decimal valorDesconto, decimal valorTroco)
        {
            return valorLancamentos + valorDezPorCento + valorCouvert - valorPagamentos - valorDesconto + valorTroco;
        }
    }
}
