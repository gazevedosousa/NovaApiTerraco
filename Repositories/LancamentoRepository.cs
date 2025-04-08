using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Repositories.Interfaces;
using TerracoDaCida.Util;

namespace TerracoDaCida.Repositories
{
    public class LancamentoRepository : ILancamentoRepository
    {
        private readonly DbEscrita _dbEscrita;
        private readonly DbLeitura _dbLeitura;

        public LancamentoRepository(DbEscrita dbEscrita, DbLeitura dbLeitura)
        {
            _dbEscrita = dbEscrita;
            _dbLeitura = dbLeitura;
        }

        public async Task<List<Lancamento>> BuscarLancamentosComanda(int coComanda)
        {
            return await _dbLeitura.Lancamentos
                .Where(l => l.CoComanda == coComanda)
                .Include(l => l.CoProdutoNavigation)
                .ThenInclude(p => p.CoTipoProdutoNavigation)
                .ToListAsync();
        }
        public async Task<List<Lancamento>> BuscarLancamentosDia(DateOnly dtLancamento)
        {
            DateTime dhInicial = dtLancamento.ToDateTime(TimeOnly.MinValue);
            DateTime dhFinal = dtLancamento.ToDateTime(TimeOnly.MaxValue);

            return await _dbLeitura.Lancamentos
                .Where(l => l.DhCriacao >= dhInicial && l.DhCriacao <= dhFinal)
                .Include(l => l.CoProdutoNavigation)
                .ThenInclude(p => p.CoTipoProdutoNavigation)
                .ToListAsync();
        }

        public async Task<List<Lancamento>> BuscarLancamentosPeriodo(DateOnly dtInicial, DateOnly dtFinal)
        {
            DateTime dhInicial = dtInicial.ToDateTime(TimeOnly.MinValue);
            DateTime dhFinal = dtFinal.ToDateTime(TimeOnly.MaxValue); 

            return await _dbLeitura.Lancamentos
                .Where(l => l.DhCriacao >= dhInicial && l.DhCriacao <= dhFinal)
                .Include(l => l.CoProdutoNavigation)
                .ThenInclude(p => p.CoTipoProdutoNavigation)
                .ToListAsync();
        }

        public async Task<Lancamento?> BuscarLancamentoDeProdutoNaComanda(int coComanda, int coProduto)
        {
            return await _dbLeitura.Lancamentos
                .Where(l => l.CoComanda == coComanda)
                .Where(l => l.CoProduto == coProduto)
                .Include(l => l.CoProdutoNavigation)
                .ThenInclude(p => p.CoTipoProdutoNavigation)
                .Include(l => l.CoComandaNavigation)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CriarLancamento(Lancamento lancamento)
        {
            await _dbEscrita.AddAsync(lancamento);
            return await SaveChangesAsync();
        }

        public async Task<bool> AtualizarLancamento(Lancamento lancamento, int quantidade, decimal vrProduto, bool isAcrescimo)
        {
            decimal vrUnitario = lancamento.VrUnitario;
            decimal novoValor = 0;
            int novaQuantidade = 0;

            if (isAcrescimo)
            {
                novaQuantidade = lancamento.QtdLancamento + quantidade;
            }
            else
            {
                novaQuantidade = lancamento.QtdLancamento - quantidade;
            }

            if (vrUnitario != vrProduto)
            {
                vrUnitario = vrProduto;
            }

            novoValor = vrUnitario * novaQuantidade;

            if (novaQuantidade <= 0 || novoValor <= 0)
            {
                return await ExcluirLancamento(lancamento.CoLancamento);
            }

            return await _dbEscrita.Lancamentos
               .Where(l => l.CoProduto == lancamento.CoProduto)
               .ExecuteUpdateAsync(up => up
                   .SetProperty(l => l.QtdLancamento, novaQuantidade)
                   .SetProperty(l => l.VrLancamento, novoValor)
                   .SetProperty(l => l.VrUnitario, vrUnitario)) == 1;
        }

        public async Task<bool> ExcluirLancamento(int coLancamento)
        {
            return await _dbEscrita.Lancamentos
                .Where(l => l.CoLancamento == coLancamento)
                .ExecuteDeleteAsync() == 1;
        }

        public async Task<bool> ExisteLancamento(int coLancamento)
        {
            return await _dbLeitura.Lancamentos
                .Where(l => l.CoLancamento == coLancamento)
                .AnyAsync();
        }

        public async Task<bool> ExisteLancamentoDeProdutoNaComanda(int coComanda, int coProduto)
        {
            return await _dbLeitura.Lancamentos
                .Where(l => l.CoComanda == coComanda)
                .Where(l => l.CoProduto == coProduto)
                .AnyAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var acaoBanco = await _dbEscrita.SaveChangesAsync();
            return acaoBanco == 1;
        }
    }
}
