using System.Drawing;
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
    public class PagamentoService : IPagamentoService
    {
        private readonly ILogger<PagamentoService> _logger;
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IComandaRepository _comandaRepository;
        private readonly IComandaService _comandaService;

        public PagamentoService(ILogger<PagamentoService> logger,
            IPagamentoRepository pagamentoRepository, IComandaRepository comandaRepository, IComandaService comandaService)
        {
            _logger = logger;
            _pagamentoRepository = pagamentoRepository;
            _comandaRepository = comandaRepository;
            _comandaService = comandaService;
        }

        public async Task<ApiResponse<List<PagamentoDTO>>> BuscaPagamentosComanda(int coComanda)
        {

            List<Pagamento> pagamentos = await _pagamentoRepository.BuscarPagamentosComanda(coComanda);

            List<PagamentoDTO> listaPagamentos = pagamentos.Select(p => new PagamentoDTO()
            {
                Codigo = p.CoPagamento,
                Comanda = p.CoComanda,
                ValorPagamento = p.VrPagamento,
                TipoPagamento = (TipoPagamentoEnum)p.CoTipoPagamento,
                DataPagamento = DateOnly.FromDateTime(p.DhCriacao)
            }).ToList();

            return ApiResponse<List<PagamentoDTO>>.SuccessOk(listaPagamentos);
        }

        public async Task<ApiResponse<List<PagamentoDTO>>> BuscaPagamentosDia(DateOnly dtPagamento)
        {

            List<Pagamento> pagamentos = await _pagamentoRepository.BuscarPagamentosDia(dtPagamento);

            List<PagamentoDTO> listaPagamentos = pagamentos.Select(p => new PagamentoDTO()
            {
                Codigo = p.CoPagamento,
                Comanda = p.CoComanda,
                ValorPagamento = p.VrPagamento,
                TipoPagamento = (TipoPagamentoEnum)p.CoTipoPagamento,
                DataPagamento = DateOnly.FromDateTime(p.DhCriacao)
            }).ToList();

            return ApiResponse<List<PagamentoDTO>>.SuccessOk(listaPagamentos);
        }

        public async Task<ApiResponse<List<PagamentoDTO>>> BuscaPagamentosPeriodo(DateOnly dtInicial, DateOnly dtFinal)
        {

            List<Pagamento> pagamentos = await _pagamentoRepository.BuscarPagamentosPeriodo(dtInicial, dtFinal);

            List<PagamentoDTO> listaPagamentos = pagamentos.Select(p => new PagamentoDTO()
            {
                Codigo = p.CoPagamento,
                Comanda = p.CoComanda,
                ValorPagamento = p.VrPagamento,
                TipoPagamento = (TipoPagamentoEnum)p.CoTipoPagamento,
                DataPagamento = DateOnly.FromDateTime(p.DhCriacao)
            }).ToList();

            return ApiResponse<List<PagamentoDTO>>.SuccessOk(listaPagamentos);
        }

        public async Task<ApiResponse<bool>> CriaPagamento(CriaPagamentoDTO criaPagamentoDTO)
        {
            bool pagamentoTotal = false;

            decimal valorTotalComanda = await _comandaService.BuscaValorTotalComanda(criaPagamentoDTO.Comanda);

            if (criaPagamentoDTO.ValorPagamento == valorTotalComanda)
            {
                pagamentoTotal = true;
            }

            Pagamento pagamento = new Pagamento
            {
                CoComanda = criaPagamentoDTO.Comanda,
                CoTipoPagamento = criaPagamentoDTO.TipoPagamento,
                VrPagamento = criaPagamentoDTO.ValorPagamento,
                DhCriacao = new DateTime().GetDataAtual()
            };

            if (await _pagamentoRepository.CriarPagamento(pagamento))
            {
                _logger.LogInformation($"Pagamento realizado com sucesso - {criaPagamentoDTO.Comanda}, {criaPagamentoDTO.ValorPagamento}");

                if (pagamentoTotal)
                {
                    if(await _comandaRepository.FecharComanda(criaPagamentoDTO.Comanda))
                    {
                        _logger.LogInformation($"Comanda fechada com sucesso - {criaPagamentoDTO.Comanda}");
                    }
                }

                return ApiResponse<bool>.SuccessOk(true);
            }
            else
            {
                return ApiResponse<bool>.Error($"Erro ao criar Pagamento. DTO de entrada: {criaPagamentoDTO.ToJson()}");
            }
        }

        public async Task<ApiResponse<bool>> ExcluiPagamento(int coPagamento)
        {
            if (await _pagamentoRepository.ExcluirPagamento(coPagamento))
            {
                _logger.LogInformation($"Pagamento excluído com sucesso - {coPagamento}");
                return ApiResponse<bool>.NoContent(true);
            }
            else
            {
                return ApiResponse<bool>.Error($"Erro ao excluir Pagamento. coPagamento: {coPagamento}");
            }
            
        }

        public bool QtdZerada(int qtdPagamento)
        {
            if (qtdPagamento <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ExistePossibilidadeDePagamento(CriaPagamentoDTO criaPagamentoDTO)
        {
            decimal valorTotalComanda = await _comandaService.BuscaValorTotalComanda(criaPagamentoDTO.Comanda);

            if (criaPagamentoDTO.ValorPagamento > valorTotalComanda)
            {
                return false;
            }

            return true;
        }
        public async Task<bool> ExistePagamento(int coPagamento)
        {
            return await _pagamentoRepository.ExistePagamento(coPagamento);
        }

        public bool ExisteTipoPagamento(int tipoPagamento)
        {
            return Enum.IsDefined(typeof(TipoPagamentoEnum), tipoPagamento);
        }

        public bool ValorSuperiorAZero(decimal vrPagamento)
        {
            if (vrPagamento <= 0)
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
