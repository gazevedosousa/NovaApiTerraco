using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Repositories.Interfaces;

namespace TerracoDaCida.Repositories
{
    public class UsuarioRepository: IUsuarioRepository
    {
        private readonly DbEscrita _dbEscrita;
        private readonly DbLeitura _dbLeitura;

        public UsuarioRepository(DbEscrita dbEscrita, DbLeitura dbLeitura)
        {
            _dbEscrita = dbEscrita;
            _dbLeitura = dbLeitura;
        }

        public async Task<List<Usuario>> BuscarUsuarios()
        {
            return await _dbLeitura.Usuarios
                .Include(u => u.CoPerfilNavigation)
                .ToListAsync();
        }

        public async Task<Usuario?> BuscarUsuarioPorNome(string noUsuario)
        {
            return await _dbLeitura.Usuarios
                .Where(u => u.NoUsuario == noUsuario)
                .Include(u => u.CoPerfilNavigation)
                .FirstOrDefaultAsync();
        }

        public async Task<Perfil?> BuscarPerfil(short coPerfil)
        {
            return await _dbLeitura.Perfils
                .Where(p => p.CoPerfil == coPerfil)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CriaUsuario(Usuario usuario)
        {
            await _dbEscrita.AddAsync(usuario);
            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var acaoBanco = await _dbEscrita.SaveChangesAsync();
            return acaoBanco == 1;
        }
    }
}
