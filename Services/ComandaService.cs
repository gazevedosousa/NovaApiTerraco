using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TerracoDaCida.DTO;
using TerracoDaCida.Enums;
using TerracoDaCida.Exceptions;
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

        public async Task<ApiResponse<bool>> AbreComanda(AbreComandaDTO abreComandaDTO)
        {

            Comandum comanda = new Comandum
            {
                NoComanda = abreComandaDTO.Nome,
                CoSituacao = (int)SituacaoComandaEnum.Aberta,
                Temdezporcento = abreComandaDTO.TemDezPorCento,
                Temcouvert = abreComandaDTO.TemCouvert,
                QtdCouvert = abreComandaDTO.QtdCouvert,
                Valordesconto = 0,
                Valortroco = 0,
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
                            ValorProduto = l.CoProdutoNavigation.VrProduto
                        },
                        ValorUnitario = l.VrUnitario,
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
                        TipoPagamento = (TipoPagamentoEnum)p.CoTipoPagamento,
                        DataPagamento = DateOnly.FromDateTime(p.DhCriacao)
                    }).ToList();

                decimal valorCouvert = 0;
                decimal valorTotalCouvert = 0;

                if (comanda.Temcouvert)
                {
                    valorCouvert = await _couvertRepository.BuscarValorCouvert();

                    valorTotalCouvert = valorCouvert * (int)comanda.QtdCouvert!;
                }

                decimal valorLancamentos = lancamentos.Sum(l => l.ValorLancamento);
                decimal valorDezPorCento = comanda.Temdezporcento ? valorLancamentos * (decimal)0.1 : 0;
                decimal valorPagamentos = pagamentos.Sum(p => p.ValorPagamento);
                decimal valorDesconto = comanda.Valordesconto;
                decimal valorTroco = comanda.Valortroco;

                decimal valorTotalComanda = CalculaValorTotalComanda(
                    valorLancamentos, valorPagamentos, valorDezPorCento, valorTotalCouvert, valorDesconto, valorTroco);

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
                    QtdCouvert = comanda.QtdCouvert,
                    ValorTotalCouvert = valorTotalCouvert,
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

        public async Task<ApiResponse<bool>> AlteraComanda(AlteraComandaDTO alteraComandaDTO)
        {
            Comandum? comanda = await _comandaRepository.BuscarComanda(alteraComandaDTO.Codigo);

            if (await _comandaRepository.AlterarComanda(comanda!, alteraComandaDTO))
            {
                _logger.LogInformation($"Comanda alterada com sucesso - {alteraComandaDTO.Codigo}");
                return ApiResponse<bool>.SuccessOk(true);
            }
            else
            {
                return ApiResponse<bool>.Error($"Erro ao alterar comanda. coComanda: {alteraComandaDTO.Codigo}");
            }

        }

        public async Task<decimal> BuscaValorTotalComanda(int coComanda)
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
                            ValorProduto = l.CoProdutoNavigation.VrProduto
                        },
                        ValorUnitario = l.VrUnitario,
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
                        TipoPagamento = (TipoPagamentoEnum)p.CoTipoPagamento,
                        DataPagamento = DateOnly.FromDateTime(p.DhCriacao)
                    }).ToList();

                decimal valorCouvert = 0;
                decimal valorTotalCouvert = 0;

                if (comanda.Temcouvert)
                {
                    valorCouvert = await _couvertRepository.BuscarValorCouvert();

                    valorTotalCouvert = valorCouvert * (int)comanda.QtdCouvert!;
                }

                decimal valorLancamentos = lancamentos.Sum(l => l.ValorLancamento);
                decimal valorDezPorCento = comanda.Temdezporcento ? valorLancamentos * (decimal)0.1 : 0;
                decimal valorPagamentos = pagamentos.Sum(p => p.ValorPagamento);
                decimal valorDesconto = comanda.Valordesconto;
                decimal valorTroco = comanda.Valortroco;

                return CalculaValorTotalComanda(
                    valorLancamentos, valorPagamentos, valorDezPorCento, valorTotalCouvert, valorDesconto, valorTroco);
            }
            else
            {
                throw new NotFoundException("Comanda não existente");
            }
        }

        public async Task<ApiResponse<bool>> FechaComanda(int coComanda)
        {
            if (await _comandaRepository.FecharComanda(coComanda))
            {
                _logger.LogInformation($"Comanda fechada com sucesso - {coComanda}");
                return ApiResponse<bool>.SuccessOk(true);
            }
            else
            {
                return ApiResponse<bool>.Error($"Erro ao fechar comanda. coComanda: {coComanda}");
            }
        }

        public async Task<ApiResponse<bool>> ReabreComanda(int coComanda)
        {
            if (await _comandaRepository.ReabrirComanda(coComanda))
            {
                _logger.LogInformation($"Comanda reaberta com sucesso - {coComanda}");
                return ApiResponse<bool>.SuccessOk(true);
            }
            else
            {
                return ApiResponse<bool>.Error($"Erro ao reabrir comanda. coComanda: {coComanda}");
            }
        }

        public async Task<bool> ExisteComanda(int coComanda)
        {
            return await _comandaRepository.ExisteComanda(coComanda);
        }

        public async Task<bool> ComandaJaPaga(int coComanda)
        {
            return await _comandaRepository.ComandaJaPaga(coComanda);
        }

        public async Task<bool> ComandaPodeSerFechada(int coComanda)
        {
            return await BuscaValorTotalComanda(coComanda) == 0;
        }

        public async Task<bool> ExisteComandaAbertaParaNomeInformado(string noComanda)
        {
            return await _comandaRepository.ExisteComandaAbertaParaNomeInformado(noComanda);
        }

        public bool ExisteAlgumaAtualizacao(AlteraComandaDTO alteraComandaDTO)
        {
            if(!alteraComandaDTO.AlteraDezPorCento && !alteraComandaDTO.AlteraCouvert && !alteraComandaDTO.ValorDesconto.HasValue)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ExistePossibilidadeDeDesconto(AlteraComandaDTO alteraComandaDTO)
        {
            decimal valorTotalComanda = await BuscaValorTotalComanda(alteraComandaDTO.Codigo);

            if (alteraComandaDTO.ValorDesconto > valorTotalComanda)
            {
                return false;
            }

            return true;
        }

        public bool TemCouvertSemQuantidade(AbreComandaDTO abreComandaDTO)
        {
            if (abreComandaDTO.TemCouvert)
            {
                if (!abreComandaDTO.QtdCouvert.HasValue || (abreComandaDTO.QtdCouvert.HasValue && abreComandaDTO.QtdCouvert <= 0))
                {
                    return true;
                }
            }
            
            return false;
        }

        public async Task<bool> TemCouvertSemQuantidade(AlteraComandaDTO alteraComandaDTO)
        {
            Comandum? comanda = await _comandaRepository.BuscarComanda(alteraComandaDTO.Codigo);

            if (!comanda!.Temcouvert && alteraComandaDTO.AlteraCouvert)
            {
                if (!alteraComandaDTO.QtdCouvert.HasValue || (alteraComandaDTO.QtdCouvert.HasValue && alteraComandaDTO.QtdCouvert <= 0))
                {
                    return true;
                }
            }

            return false;
        }

        public bool DescontoMenorQueZero(AlteraComandaDTO alteraComandaDTO)
        {
            if(alteraComandaDTO.ValorDesconto < 0)
            {
                return true;
            }

            return false;
        }

        public decimal CalculaValorTotalComanda(
            decimal valorLancamentos, decimal valorPagamentos, decimal valorDezPorCento, decimal valorTotalCouvert, decimal valorDesconto, decimal valorTroco)
        {
            return valorLancamentos + valorDezPorCento + valorTotalCouvert - valorPagamentos - valorDesconto + valorTroco;
        }
    }
}
