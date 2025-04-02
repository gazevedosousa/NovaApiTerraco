using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TerracoDaCida.DTO;
using TerracoDaCida.Services;
using TerracoDaCida.Services.Interfaces;
using TerracoDaCida.Util;

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
        public async Task<ActionResult<string>> Login([FromBody] LoginDTO loginDTO)
        {
            var usuario = await _usuarioService.BuscaUsuarioParaLogin(loginDTO.Usuario);

            if(usuario == null)
            {
                _logger.LogInformation("Usuário não encontrado");
                return BadRequest("Usuário ou senha incorretos");
            }

            if(!_loginService.SenhaCorreta(loginDTO, usuario)) 
            {
                _logger.LogInformation("Senha incorreta");
                return BadRequest("Usuário ou senha incorretos");
            }

            string jwt = _loginService.RealizaLogin(usuario);

            _logger.LogInformation("Login realizado com sucesso");
            return Ok(jwt);
        }

    }
}
