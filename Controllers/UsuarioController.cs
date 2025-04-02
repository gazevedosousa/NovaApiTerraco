using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TerracoDaCida.DTO;
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
            return await _usuarioService.BuscaUsuarios();
        }

        [HttpPost]
        [Route("criaUsuario")]
        public async Task<IActionResult> CriaUsuario([FromBody] CriaUsuarioDTO criaUsuarioDTO)
        {
            if(await _usuarioService.ExisteUsuarioDuplicado(criaUsuarioDTO.Usuario))
            {
                return BadRequest("Usuário já existente");
            }

            if(!await _usuarioService.ExistePerfilSolicitado(criaUsuarioDTO.Perfil))
            {
                return BadRequest("Perfil não encontrado");
            }

            var retorno = await _usuarioService.CriaUsuario(criaUsuarioDTO);

            return StatusCode(retorno.StatusCode, retorno);
        }

    }
}
