using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Repositories.Interfaces;
using TerracoDaCida.Util;

namespace TerracoDaCida.Repositories
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly DbEscrita _dbEscrita;
        private readonly DbLeitura _dbLeitura;

        public PagamentoRepository(DbEscrita dbEscrita, DbLeitura dbLeitura)
        {
            _dbEscrita = dbEscrita;
            _dbLeitura = dbLeitura;
        }

        public async Task<List<Pagamento>> BuscarPagamentosComanda(int coComanda)
        {
            return await _dbLeitura.Pagamentos
                .Where(p => p.CoComanda == coComanda)
                .ToListAsync();
        }
        public async Task<List<Pagamento>> BuscarPagamentosDia(DateOnly dtPagamento)
        {
            DateTime dhInicial = dtPagamento.ToDateTime(TimeOnly.MinValue);
            DateTime dhFinal = dtPagamento.ToDateTime(TimeOnly.MaxValue);

            return await _dbLeitura.Pagamentos
                .Where(p => p.DhCriacao >= dhInicial && p.DhCriacao <= dhFinal)
                .ToListAsync();
        }

        public async Task<List<Pagamento>> BuscarPagamentosPeriodo(DateOnly dtInicial, DateOnly dtFinal)
        {
            DateTime dhInicial = dtInicial.ToDateTime(TimeOnly.MinValue);
            DateTime dhFinal = dtFinal.ToDateTime(TimeOnly.MaxValue); 

            return await _dbLeitura.Pagamentos
                .Where(p => p.DhCriacao >= dhInicial && p.DhCriacao <= dhFinal)
                .ToListAsync();
        }

        public async Task<bool> CriarPagamento(Pagamento pagamento)
        {
            await _dbEscrita.AddAsync(pagamento);
            return await SaveChangesAsync();
        }

        public async Task<bool> ExcluirPagamento(int coPagamento)
        {
            return await _dbEscrita.Pagamentos
                .Where(l => l.CoPagamento == coPagamento)
                .ExecuteDeleteAsync() == 1;
        }

        public async Task<bool> ExistePagamento(int coPagamento)
        {
            return await _dbLeitura.Pagamentos
                .Where(l => l.CoPagamento == coPagamento)
                .AnyAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var acaoBanco = await _dbEscrita.SaveChangesAsync();
            return acaoBanco == 1;
        }
    }
}
