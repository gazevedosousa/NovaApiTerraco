using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TerracoDaCida.DTO;
using TerracoDaCida.Services;
using TerracoDaCida.Services.Interfaces;
using TerracoDaCida.Util;
using TerracoDaCida.Exceptions;

namespace TerracoDaCida.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ILoginService _loginService;
        private readonly IUsuarioService _usuarioService;
        public LoginController(ILogger<LoginController> logger, IConfiguration configuration, ILoginService loginService, 
            IUsuarioService usuarioService) 
        {
            _logger = logger;
            _configuration = configuration;
            _loginService = loginService;
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var usuario = await _usuarioService.BuscaUsuarioParaLogin(loginDTO.Usuario);

            if(usuario == null)
            {
                _logger.LogInformation("Usuário não encontrado");
                throw new BadRequestException("Usuário ou senha incorretos");
            }

            if(!_loginService.SenhaCorreta(loginDTO, usuario)) 
            {
                _logger.LogInformation("Senha incorreta");
                throw new BadRequestException("Usuário ou senha incorretos");
            }

            try
            {
                string jwt = _loginService.RealizaLogin(usuario);
                return Ok(jwt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

    }
}
