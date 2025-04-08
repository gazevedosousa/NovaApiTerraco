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
    public class CouvertController : ControllerBase
    {
        private readonly ILogger<CouvertController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICouvertService _couvertService;
        public CouvertController(ILogger<CouvertController> logger, IConfiguration configuration, ICouvertService couvertService)
        {
            _logger = logger;
            _configuration = configuration;
            _couvertService = couvertService;
        }

        [HttpGet]
        //[RequireClaim(IdentityData.AdminUserClaimName, "true")]
        [Route("buscaCouverts")]
        public async Task<ApiResponse<List<CouvertDTO>>> BuscaCouverts()
        {
            try
            {
                return await _couvertService.BuscaCouverts();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPost]
        //[RequireClaim(IdentityData.AdminUserClaimName, "true")]
        [Route("criaCouvert")]
        public async Task<IActionResult> CriaCouvert([FromBody] CriaCouvertDTO criaCouvertDTO)
        {
            try
            {
                var retorno = await _couvertService.CriaCouvert(criaCouvertDTO);

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
    }
}
