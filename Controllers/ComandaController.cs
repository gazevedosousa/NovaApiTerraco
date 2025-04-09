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
    public class ComandaController : ControllerBase
    {
        private readonly ILogger<ComandaController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IComandaService _comandaService;
        public ComandaController(ILogger<ComandaController> logger, IConfiguration configuration, IComandaService comandaService)
        {
            _logger = logger;
            _configuration = configuration;
            _comandaService = comandaService;
        }

        [HttpGet]
        [Route("buscaComandas")]
        public async Task<ApiResponse<List<ComandaDTO>>> BuscaComandas()
        {
            try
            {
                return await _comandaService.BuscaComandas();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPost]
        [Route("abreComanda")]
        public async Task<IActionResult> AbreComanda([FromBody] AbreComandaDTO abreComandaDTO)
        {
            if (await _comandaService.ExisteComandaAbertaParaNomeInformado(abreComandaDTO.Nome))
            {
                throw new BadRequestException("Comanda j� aberta para esse cliente");
            }

            if(_comandaService.TemCouvertSemQuantidade(abreComandaDTO))
            {
                throw new BadRequestException("Quantidade de Couverts deve ser superior a 0");
            }

            try
            {
                var retorno = await _comandaService.AbreComanda(abreComandaDTO);

                if (retorno.StatusCode != StatusCodes.Status201Created)
                {
                    throw new BadRequestException(retorno.ErrorMessage!);
                }

                Response.StatusCode = StatusCodes.Status201Created;
                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpGet]
        [Route("detalhaComanda")]
        public async Task<ApiResponse<DetalhaComandaDTO>> DetalhaComanda(int coComanda)
        {
            if (!await _comandaService.ExisteComanda(coComanda))
            {
                throw new NotFoundException("Comanda n�o existente");
            }


            try
            {
                return await _comandaService.DetalhaComanda(coComanda);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }

        }

        [HttpPatch]
        [Route("alteraComanda")]
        public async Task<IActionResult> AlteraComanda(AlteraComandaDTO alteraComandaDTO)
        {
            if (!await _comandaService.ExisteComanda(alteraComandaDTO.Codigo))
            {
                throw new NotFoundException("Comanda n�o existente");
            }

            if (await _comandaService.ComandaJaPaga(alteraComandaDTO.Codigo))
            {
                throw new BadRequestException("Comanda est� paga");
            }

            if(!_comandaService.ExisteAlgumaAtualizacao(alteraComandaDTO))
            {
                throw new BadRequestException("Nenhuma atualiza��o a ser realizada");
            }

            if (await _comandaService.TemCouvertSemQuantidade(alteraComandaDTO))
            {
                throw new BadRequestException("Quantidade de Couverts deve ser superior a 0");
            }

            if (alteraComandaDTO.ValorDesconto.HasValue && _comandaService.DescontoMenorQueZero(alteraComandaDTO))
            {
                throw new BadRequestException("Desconto n�o pode ser negativo");
            }

            if (alteraComandaDTO.ValorDesconto.HasValue && !await _comandaService.ExistePossibilidadeDeDesconto(alteraComandaDTO))
            {
                throw new BadRequestException("Desconto n�o pode maior que o valor total da comanda");
            }

            try
            {
                var retorno = await _comandaService.AlteraComanda(alteraComandaDTO);

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

        [HttpPost]
        [Route("fechaComanda")]
        public async Task<IActionResult> FechaComanda(int coComanda)
        {
            if (!await _comandaService.ExisteComanda(coComanda))
            {
                throw new NotFoundException("Comanda n�o existente");
            }

            if (await _comandaService.ComandaJaPaga(coComanda))
            {
                throw new BadRequestException("Comanda j� est� paga");
            }

            if (!await _comandaService.ComandaPodeSerFechada(coComanda))
            {
                throw new BadRequestException("Comanda com valores pendentes");
            }

            try
            {
                var retorno = await _comandaService.FechaComanda(coComanda);

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

        [HttpPost]
        [Route("reabreComanda")]
        public async Task<IActionResult> ReabreComanda(int coComanda)
        {
            if (!await _comandaService.ExisteComanda(coComanda))
            {
                throw new NotFoundException("Comanda n�o existente");
            }

            if (!await _comandaService.ComandaJaPaga(coComanda))
            {
                throw new BadRequestException("Comanda j� est� aberta");
            }

            try
            {
                var retorno = await _comandaService.ReabreComanda(coComanda);

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
    }
}
