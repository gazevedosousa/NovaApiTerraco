using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TerracoDaCida.Configuration;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Repositories;
using TerracoDaCida.Repositories.Interfaces;
using TerracoDaCida.Services.Interfaces;
using TerracoDaCida.Util;

namespace TerracoDaCida.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ILogger<UsuarioService> _logger;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IInfoTokenUser _infoTokenUser;

        public UsuarioService(ILogger<UsuarioService> logger, IUsuarioRepository usuarioRepository, IInfoTokenUser infoTokenUser)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
            _infoTokenUser = infoTokenUser;
        }
        public async Task<ApiResponse<List<UsuarioDTO>>> BuscaUsuarios()
        {

            List<Usuario> usuarios = await _usuarioRepository.BuscarUsuarios();

            List<UsuarioDTO> listaUsuarios = usuarios.Select(u => new UsuarioDTO()
            {
                Usuario = u.NoUsuario,
                Perfil = u.CoPerfilNavigation.NoPerfil
            }).ToList();

            return ApiResponse<List<UsuarioDTO>>.SuccessOk(listaUsuarios);
        }

        public async Task<UsuarioDTO?> BuscaUsuarioPorNome(string noUsuario)
        {

            Usuario? usuarioBanco = await _usuarioRepository.BuscarUsuarioPorNome(noUsuario);

            if(usuarioBanco == null)
            {
                return null;
            }

            return new UsuarioDTO
            {
                Usuario = usuarioBanco.NoUsuario,
                Perfil = usuarioBanco.CoPerfilNavigation.NoPerfil
            };

        }

        public async Task<Usuario?> BuscaUsuarioParaLogin(string noUsuario)
        {

            return await _usuarioRepository.BuscarUsuarioPorNome(noUsuario);

        }

        public async Task<ApiResponse<bool>> CriaUsuario(CriaUsuarioDTO criaUsuarioDTO)
        {
            HashUtil.CriaSenhaHash(criaUsuarioDTO.Senha, out byte[] senhaHash, out byte[] senhaSalt);

            Usuario usuario = new Usuario
            {
                NoUsuario = criaUsuarioDTO.Usuario,
                CoPerfil = criaUsuarioDTO.Perfil,
                Senhahash = senhaHash,
                Senhasalt = senhaSalt,
            };

            if (await _usuarioRepository.CriarUsuario(usuario))
            {

                _logger.LogInformation($"Usuário criado com sucesso - {usuario.NoUsuario}");
                return ApiResponse<bool>.SuccessCreated(true); 
            }
            else
            {
                return ApiResponse<bool>.Error($"Erro ao criar usuário. DTO de entrada: {criaUsuarioDTO.ToJson()}");
            }
        }

        public async Task<ApiResponse<bool>> ExcluiUsuario(int coUsuario)
        {
            Usuario? usuario = await _usuarioRepository.BuscarUsuario(coUsuario);

            if (usuario != null)
            {
                if (await _usuarioRepository.ExcluirUsuario(coUsuario))
                {
                    _logger.LogInformation($"Usuário excluído com sucesso - {usuario.NoUsuario}");
                    return ApiResponse<bool>.NoContent(true);
                }
                else
                {
                    return ApiResponse<bool>.Error($"Erro ao excluir Usuário. coTipoProduto: {coUsuario}");
                }
            }
            else
            {
                return ApiResponse<bool>.Error($"Usuário não existente. coTipoProduto: {coUsuario}");
            }
        }

        public async Task<bool> ExisteUsuarioDuplicado(string noUsuario)
        {
            return await _usuarioRepository.ExisteUsuarioDuplicado(noUsuario);
        }

        public async Task<bool> ExistePerfilSolicitado(int coPerfil)
        {
            return await _usuarioRepository.ExistePerfilSolicitado(coPerfil);
        }

        public bool UsuarioSolicitanteIgualAoDeletado(int coUsuario)
        {
            return _infoTokenUser.CoUsuario == coUsuario;
        }

    }
}
