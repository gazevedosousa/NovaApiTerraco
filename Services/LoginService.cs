using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Repositories.Interfaces;
using TerracoDaCida.Services.Interfaces;
using TerracoDaCida.Util;

namespace TerracoDaCida.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILogger<UsuarioService> _logger;
        private readonly IConfiguration _configuration;
        private static readonly TimeSpan tokenLifeTime = TimeSpan.FromHours(10);

        public LoginService(ILogger<UsuarioService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public string RealizaLogin(Usuario usuario)
        {
            string secret = _configuration.GetSection("Tkn:key").Value!;

            return CreateToken(usuario, secret);
        }

        public bool SenhaCorreta(LoginDTO loginDTO, Usuario usuario)
        {
            if (!HashUtil.VerificaSenhaHash(loginDTO.Senha, usuario.SenhaHash, usuario.SenhaSalt))
            {
                return false;
            }

            return true;
        }

        public string CreateToken(Usuario usuario, string secret)
        {
            byte[] key = Encoding.UTF8.GetBytes(secret);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string checkAdmin = "false";

            if (usuario.CoPerfil == 1)
            {
                checkAdmin = "true";
            }

            List<Claim> claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Sub, usuario.NoUsuario),
                new ("coUsuario", usuario.CoUsuario.ToString()), 
                new ("noUsuario", usuario.NoUsuario),
                new ("admin", checkAdmin)
            };

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(tokenLifeTime),
                Issuer = "https://terracodacida.com.br",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            SecurityToken token = handler.CreateToken(securityTokenDescriptor);

            return handler.WriteToken(token);
        }

    }
}
