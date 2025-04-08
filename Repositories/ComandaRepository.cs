using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Repositories.Interfaces;
using TerracoDaCida.Util;

namespace TerracoDaCida.Repositories
{
    public class ComandaRepository : IComandaRepository
    {
        private readonly DbEscrita _dbEscrita;
        private readonly DbLeitura _dbLeitura;

        public ComandaRepository(DbEscrita dbEscrita, DbLeitura dbLeitura)
        {
            _dbEscrita = dbEscrita;
            _dbLeitura = dbLeitura;
        }

        public async Task<List<Comandum>> BuscarComandas()
        {
            return await _dbLeitura.Comanda
                .Include(c => c.Pagamentos)
                .Include(c => c.Lancamentos)
                .ToListAsync();
        }

        public async Task<List<Comandum>> BuscarComandasAbertas()
        {
            return await _dbLeitura.Comanda
                .Include(c => c.Pagamentos)
                .Include(c => c.Lancamentos)
                .Where(c => c.CoSituacao == 1)
                .ToListAsync();
        }

        public async Task<List<Comandum>> BuscarComandasFechadas()
        {
            return await _dbLeitura.Comanda
                .Include(c => c.Pagamentos)
                .Include(c => c.Lancamentos)
                .Where(c => c.CoSituacao == 2)
                .ToListAsync();
        }

        public async Task<Comandum?> BuscarComanda(int coComanda)
        {
            return await _dbLeitura.Comanda
                .Where(c => c.CoComanda == coComanda)
                .Include(c => c.Lancamentos)
                .Include(c => c.Pagamentos)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AbrirComanda(Comandum comanda)
        {
            await _dbEscrita.AddAsync(comanda);
            return await SaveChangesAsync();
        }

        public async Task<bool> FecharComanda(int coComanda)
        {
            return await _dbEscrita.Comanda
               .Where(c => c.CoComanda == coComanda)
               .ExecuteUpdateAsync(up => up
                   .SetProperty(c => c.CoSituacao, 2)) == 1;
        }

        public async Task<bool> ExisteComanda(int coComanda)
        {
            return await _dbLeitura.Comanda
                .Where(c => c.CoComanda == coComanda)
                .AnyAsync();
        }

        public async Task<bool> ExisteComandaAbertaParaNomeInformado(string noComanda)
        {
            return await _dbLeitura.Comanda
                .Where(c => c.NoComanda == noComanda)
                .Where(c => c.DhFechamento == null)
                .AnyAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var acaoBanco = await _dbEscrita.SaveChangesAsync();
            return acaoBanco == 1;
        }
    }
}
