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
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LancamentoController : ControllerBase
    {
        private readonly ILogger<LancamentoController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ILancamentoService _lancamentoService;
        private readonly IComandaService _comandaService;
        private readonly IProdutoService _produtoService;
        public LancamentoController(
            ILogger<LancamentoController> logger, IConfiguration configuration,
            ILancamentoService lancamentoService, IComandaService comandaService, IProdutoService produtoService)
        {
            _logger = logger;
            _configuration = configuration;
            _lancamentoService = lancamentoService;
            _lancamentoService = lancamentoService;
            _comandaService = comandaService;
            _produtoService = produtoService;
        }

        [HttpGet]
        [Route("buscaLancamentosComanda")]
        public async Task<ApiResponse<List<LancamentoDTO>>> BuscaLancamentosComanda(int coComanda)
        {
            try
            {
                return await _lancamentoService.BuscaLancamentosComanda(coComanda);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpGet]
        [Route("buscaLancamentosDia")]
        public async Task<ApiResponse<List<LancamentoDTO>>> BuscaLancamentosDia(DateOnly dtLancamento)
        {
            try
            {
                return await _lancamentoService.BuscaLancamentosDia(dtLancamento);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpGet]
        [Route("buscaLancamentosPeriodo")]
        public async Task<ApiResponse<List<LancamentoDTO>>> BuscaLancamentosPeriodo(DateOnly dtInicial, DateOnly dtFinal)
        {
            try
            {
                return await _lancamentoService.BuscaLancamentosPeriodo(dtInicial, dtFinal);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPost]
        [Route("criaLancamento")]
        public async Task<IActionResult> CriaLancamento([FromBody] CriaLancamentoDTO criaLancamentoDTO)
        {
            if (!await _comandaService.ExisteComanda(criaLancamentoDTO.Comanda))
            {
                throw new NotFoundException("Comanda não existente");
            }

            if (!await _produtoService.ExisteProduto(criaLancamentoDTO.Produto))
            {
                throw new NotFoundException("Produto não existente");
            }

            if (_lancamentoService.QtdZerada(criaLancamentoDTO.Quantidade))
            {
                throw new BadRequestException("Quantidade do lançamento deve ser superior a 0");
            }

            if (!criaLancamentoDTO.IsAcrescimo && !await _lancamentoService.ExistePossibilidadeDeDiminuir(criaLancamentoDTO))
            {
                throw new BadRequestException("Não é possível realizar o lançamento negativo solicitado. Verifique quantidade e valor");
            }

            try
            {
                var retorno = await _lancamentoService.CriaLancamento(criaLancamentoDTO);

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
        [Route("excluiLancamento")]
        public async Task<IActionResult> ExcluiLancamento(int coLancamento)
        {
            if (!await _lancamentoService.ExisteLancamento(coLancamento))
            {
                throw new NotFoundException("Lançamento não existente");
            }

            try
            {
                var retorno = await _lancamentoService.ExcluiLancamento(coLancamento);

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
