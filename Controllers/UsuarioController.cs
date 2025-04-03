using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TerracoDaCida.DTO;
using TerracoDaCida.Exceptions;
using TerracoDaCida.Identity;
using TerracoDaCida.Services;
using TerracoDaCida.Services.Interfaces;
using TerracoDaCida.Util;

namespace TerracoDaCida.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(ILogger<UsuarioController> logger, IConfiguration configuration, IUsuarioService usuarioService) 
        {
            _logger = logger;
            _configuration = configuration;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Route("buscaUsuarios")]
        public async Task<ApiResponse<List<UsuarioDTO>>> BuscaUsuarios()
        {
            try
            {
                return await _usuarioService.BuscaUsuarios();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPost]
        [RequireClaim(IdentityData.AdminUserClaimName, "true")]
        [Route("criaUsuario")]
        public async Task<IActionResult> CriaUsuario([FromBody] CriaUsuarioDTO criaUsuarioDTO)
        {
            if(await _usuarioService.ExisteUsuarioDuplicado(criaUsuarioDTO.Usuario))
            {
                throw new BadRequestException("Usuário já existente");
            }

            if(!await _usuarioService.ExistePerfilSolicitado(criaUsuarioDTO.Perfil))
            {
                throw new NotFoundException("Perfil não existente");
            }
            try
            {
                var retorno = await _usuarioService.CriaUsuario(criaUsuarioDTO);

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

        [HttpDelete]
        [RequireClaim(IdentityData.AdminUserClaimName, "true")]
        [Route("excluiUsuario")]
        public async Task<IActionResult> ExcluiUsuario(int coUsuario)
        {
            if(_usuarioService.UsuarioSolicitanteIgualAoDeletado(coUsuario))
            {
                throw new BadRequestException("Erro ao deletar Usuario. Não é possível deletar o próprio Usuário");
            }

            try
            {
                var retorno = await _usuarioService.ExcluiUsuario(coUsuario);

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
