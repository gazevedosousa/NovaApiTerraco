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

        [HttpGet]
        [Route("buscaComandasAbertas")]
        public async Task<ApiResponse<List<ComandaDTO>>> BuscaComandasAbertas()
        {
            try
            {
                return await _comandaService.BuscaComandasAbertas();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpGet]
        [Route("buscaComandasFechadas")]
        public async Task<ApiResponse<List<ComandaDTO>>> BuscaComandasFechadas()
        {
            try
            {
                return await _comandaService.BuscaComandasFechadas();

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
                throw new BadRequestException("Comanda já aberta para esse cliente");
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
                throw new NotFoundException("Comanda não existente");
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

    }
}
