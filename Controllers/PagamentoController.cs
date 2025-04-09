using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TerracoDaCida.DTO;
using TerracoDaCida.Exceptions;
using TerracoDaCida.Models;
using TerracoDaCida.Services;
using TerracoDaCida.Services.Interfaces;
using TerracoDaCida.Util;

namespace TerracoDaCida.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentoController : ControllerBase
    {
        private readonly ILogger<PagamentoController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPagamentoService _pagamentoService;
        private readonly IComandaService _comandaService;
        private readonly IProdutoService _produtoService;
        public PagamentoController(
            ILogger<PagamentoController> logger, IConfiguration configuration,
            IPagamentoService pagamentoService, IComandaService comandaService, IProdutoService produtoService)
        {
            _logger = logger;
            _configuration = configuration;
            _pagamentoService = pagamentoService;
            _comandaService = comandaService;
            _produtoService = produtoService;
        }

        [HttpGet]
        [Route("buscaPagamentosComanda")]
        public async Task<ApiResponse<List<PagamentoDTO>>> BuscaPagamentosComanda(int coComanda)
        {
            try
            {
                return await _pagamentoService.BuscaPagamentosComanda(coComanda);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpGet]
        [Route("buscaPagamentosDia")]
        public async Task<ApiResponse<List<PagamentoDTO>>> BuscaPagamentosDia(DateOnly dtPagamento)
        {
            try
            {
                return await _pagamentoService.BuscaPagamentosDia(dtPagamento);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpGet]
        [Route("buscaPagamentosPeriodo")]
        public async Task<ApiResponse<List<PagamentoDTO>>> BuscaPagamentosPeriodo(DateOnly dtInicial, DateOnly dtFinal)
        {
            try
            {
                return await _pagamentoService.BuscaPagamentosPeriodo(dtInicial, dtFinal);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPost]
        [Route("criaPagamento")]
        public async Task<IActionResult> CriaPagamento([FromBody] CriaPagamentoDTO criaPagamentoDTO)
        {
            if (!await _comandaService.ExisteComanda(criaPagamentoDTO.Comanda))
            {
                throw new NotFoundException("Comanda não existente");
            }

            if (await _comandaService.ComandaJaPaga(criaPagamentoDTO.Comanda))
            {
                throw new BadRequestException("Comanda está paga");
            }

            if (!_pagamentoService.ValorSuperiorAZero(criaPagamentoDTO.ValorPagamento))
            {
                throw new BadRequestException("Valor do Pagamento deve ser superior a R$0,00");
            }

            if (!_pagamentoService.ExisteTipoPagamento(criaPagamentoDTO.TipoPagamento))
            {
                throw new NotFoundException("Tipo de Pagamento informado não existente");
            }

            if (!await _pagamentoService.ExistePossibilidadeDePagamento(criaPagamentoDTO))
            {
                throw new BadRequestException("Valor do Pagamento superior ao total da comanda");
            }

            try
            {
                var retorno = await _pagamentoService.CriaPagamento(criaPagamentoDTO);

                if (retorno.StatusCode != StatusCodes.Status200OK)
                {
                    throw new BadRequestException(retorno.ErrorMessage!);
                }

                Response.StatusCode = StatusCodes.Status200OK;
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpDelete]
        [Route("excluiPagamento")]
        public async Task<IActionResult> ExcluiPagamento(int coPagamento)
        {
            if (!await _pagamentoService.ExistePagamento(coPagamento))
            {
                throw new NotFoundException("Lançamento não existente");
            }

            try
            {
                var retorno = await _pagamentoService.ExcluiPagamento(coPagamento);

                if (retorno.StatusCode != StatusCodes.Status204NoContent)
                {
                    throw new BadRequestException(retorno.ErrorMessage!);
                }

                Response.StatusCode = StatusCodes.Status204NoContent;
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }

        }

    }
}
