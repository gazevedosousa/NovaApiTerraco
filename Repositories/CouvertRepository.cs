using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Repositories.Interfaces;
using TerracoDaCida.Util;

namespace TerracoDaCida.Repositories
{
    public class CouvertRepository : ICouvertRepository
    {
        private readonly DbEscrita _dbEscrita;
        private readonly DbLeitura _dbLeitura;

        public CouvertRepository(DbEscrita dbEscrita, DbLeitura dbLeitura)
        {
            _dbEscrita = dbEscrita;
            _dbLeitura = dbLeitura;
        }

        public async Task<Couvert?> BuscaCouvertAtivo()
        {
            return await _dbLeitura.Couverts
                .Where(c => c.IsAtivo == true)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Couvert>> BuscarCouverts()
        {
            return await _dbLeitura.Couverts
                .ToListAsync();
        }

        public async Task<decimal> BuscarValorCouvert()
        {
            return await _dbLeitura.Couverts
                .Where(c => c.IsAtivo == true)
                .Select(c => c.VrCouvert)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CriarCouvert(Couvert couvert)
        {
            await _dbEscrita.AddAsync(couvert);
            return await SaveChangesAsync();
        }

        public async Task<bool> DesativarCouvertAtivo(int coCouvert)
        {
            return await _dbEscrita.Couverts
               .Where(c => c.CoCouvert == coCouvert)
               .ExecuteUpdateAsync(up => up
                   .SetProperty(c => c.IsAtivo, false)) == 1;
        }

        public async Task<bool> ExisteCouvertAtivo()
        {
            return await _dbLeitura.Couverts
                .Where(c => c.IsAtivo == true)
                .AnyAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var acaoBanco = await _dbEscrita.SaveChangesAsync();
            return acaoBanco == 1;
        }
    }
}
