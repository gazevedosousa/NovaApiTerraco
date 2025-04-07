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
    public class ProdutoController : ControllerBase
    {
        private readonly ILogger<ProdutoController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IProdutoService _produtoService;
        public ProdutoController(ILogger<ProdutoController> logger, IConfiguration configuration, IProdutoService produtoService) 
        {
            _logger = logger;
            _configuration = configuration;
            _produtoService = produtoService;
        }

        [HttpGet]
        [Route("buscaProdutos")]
        public async Task<ApiResponse<List<ProdutoDTO>>> BuscaProdutos()
        {
            try
            {
                return await _produtoService.BuscaProdutos();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPost]
        [Route("criaProduto")]
        public async Task<IActionResult> CriaProduto([FromBody] CriaProdutoDTO criaProdutoDTO)
        {
            if (await _produtoService.ExisteProdutoDuplicado(criaProdutoDTO.Produto))
            {
                throw new BadRequestException("Produto já existente");
            }

            if (!await _produtoService.ExisteTipoProduto(criaProdutoDTO.TipoProduto))
            {
                throw new NotFoundException("Tipo de Produto não existente");
            }

            if(!_produtoService.ValorSuperiorAZero(criaProdutoDTO.ValorProduto))
            {
                throw new BadRequestException("Valor do produto deve ser superior a R$0,00");
            }

            try
            {
                var retorno = await _produtoService.CriaProduto(criaProdutoDTO);

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

        [HttpPatch]
        [Route("editaValorProduto")]
        public async Task<IActionResult> EditaValorProduto([FromBody] EditaProdutoDTO editaProdutoDTO)
        {
            if (!_produtoService.ValorSuperiorAZero(editaProdutoDTO.ValorProduto))
            {
                throw new BadRequestException("Novo valor do produto deve ser superior a R$0,00");
            }

            try
            {
                var retorno = await _produtoService.EditaProduto(editaProdutoDTO.Codigo, editaProdutoDTO.ValorProduto);

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
        [Route("excluiProduto")]
        public async Task<IActionResult> ExcluiProduto(int coProduto)
        {
            try
            {
                var retorno = await _produtoService.ExcluiProduto(coProduto);

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

        [HttpGet]
        [Route("buscaTiposProduto")]
        public async Task<ApiResponse<List<TipoProdutoDTO>>> BuscaTiposProduto()
        {
            try
            {
                return await _produtoService.BuscaTiposProduto();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPost]
        [Route("criaTipoProduto")]
        public async Task<IActionResult> CriaTipoProduto([FromBody] CriaTipoProdutoDTO criaTipoProdutoDTO)
        {
            if (await _produtoService.ExisteTipoProdutoDuplicado(criaTipoProdutoDTO.TipoProduto))
            {
                throw new BadRequestException("Tipo de produto já existente"); 
            }

            try
            {
                var retorno = await _produtoService.CriaTipoProduto(criaTipoProdutoDTO);

                if(retorno.StatusCode != StatusCodes.Status201Created)
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

        [HttpDelete]
        [Route("excluiTipoProduto")]
        public async Task<IActionResult> ExcluiTipoProduto(int coTipoProduto)
        {
            if(await _produtoService.ExisteTipoProdutoVinculadoAtivo(coTipoProduto))
            {
                throw new BadRequestException("Erro ao excluir Tipo Produto. Existe produto ativo vinculado ao tipo");
            }

            try
            {
                var retorno = await _produtoService.ExcluiTipoProduto(coTipoProduto);

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
