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

        public async Task<Comandum?> BuscarComanda(int coComanda)
        {
            return await _dbLeitura.Comanda
                .Where(c => c.CoComanda == coComanda)
                .Include(c => c.Lancamentos)
                .ThenInclude(l => l.CoProdutoNavigation)
                .ThenInclude(p => p.CoTipoProdutoNavigation)
                .Include(c => c.Pagamentos)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AbrirComanda(Comandum comanda)
        {
            await _dbEscrita.AddAsync(comanda);
            return await SaveChangesAsync();
        }

        public async Task<bool> AlterarComanda(Comandum comanda, AlteraComandaDTO alteraComandaDTO)
        {
            var query = _dbEscrita.Comanda
                .Where(c => c.CoComanda == alteraComandaDTO.Codigo);

            int totalAtualizacoes = 0;
            bool atualizouDezPorCento = false;

            if(alteraComandaDTO.AlteraDezPorCento)
            {
                totalAtualizacoes += await query.ExecuteUpdateAsync(up => up
                   .SetProperty(c => c.Temdezporcento, c => !c.Temdezporcento));

                if(totalAtualizacoes != 1)
                {
                    return false;
                }
                
                atualizouDezPorCento = true;
            }

            if (alteraComandaDTO.AlteraCouvert)
            {
                int? qntCouvert = null;

                if(!comanda.Temcouvert)
                {
                    qntCouvert = alteraComandaDTO.QtdCouvert;
                }

                totalAtualizacoes += await query.ExecuteUpdateAsync(up => up
                   .SetProperty(c => c.Temcouvert, c => !c.Temcouvert)
                   .SetProperty(c => c.QtdCouvert, qntCouvert));

                if(atualizouDezPorCento)
                {
                    if(totalAtualizacoes != 2)
                    {
                        return false;
                    }
                }
                else
                {
                    if (totalAtualizacoes != 1)
                    {
                        return false;
                    }
                }
            }

            return true;
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
