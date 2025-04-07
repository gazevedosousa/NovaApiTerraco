using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Repositories.Interfaces;
using TerracoDaCida.Util;

namespace TerracoDaCida.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly DbEscrita _dbEscrita;
        private readonly DbLeitura _dbLeitura;

        public ProdutoRepository(DbEscrita dbEscrita, DbLeitura dbLeitura)
        {
            _dbEscrita = dbEscrita;
            _dbLeitura = dbLeitura;
        }

        public async Task<List<Produto>> BuscarProdutos()
        {
            return await _dbLeitura.Produtos
                .Where(p => p.DhExclusao == null)
                .Include(p => p.CoTipoProdutoNavigation)
                .ToListAsync();
        }

        public async Task<bool> CriarProduto(Produto produto)
        {
            await _dbEscrita.AddAsync(produto);
            return await SaveChangesAsync();
        }

        public async Task<bool> EditarProduto(int coProduto, decimal vrProduto)
        {
            await _dbEscrita.Produtos
               .Where(p => p.CoProduto == coProduto)
               .ExecuteUpdateAsync(up => up
                   .SetProperty(p => p.VrProduto, vrProduto));

            return true;
        }

        public async Task<bool> ExcluirProduto(int coProduto)
        {
            await _dbEscrita.Produtos
                .Where(p => p.CoProduto == coProduto)
                .ExecuteUpdateAsync(up => up
                    .SetProperty(p => p.DhExclusao, new DateTime().GetDataAtual()));

            return true;
        }

        public async Task<Produto?> BuscarProduto(int coProduto)
        {
            return await _dbLeitura.Produtos
                .Where(p => p.CoProduto == coProduto)
                .Where(p => p.DhExclusao == null)
                .Include(p => p.CoTipoProdutoNavigation)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExisteProdutoDuplicado(string noProduto)
        {
            return await _dbLeitura.Produtos
                .Where(p => p.NoProduto == noProduto)
                .Where(p => p.DhExclusao == null)
                .Include(p => p.CoTipoProdutoNavigation)
                .AnyAsync();
        }


        public async Task<List<TipoProduto>> BuscarTiposProduto()
        {
            return await _dbLeitura.TipoProdutos
                .Where(tp => tp.DhExclusao == null)
                .ToListAsync();
        }

        public async Task<TipoProduto?> BuscarTipoProduto(int coTipoProduto)
        {
            return await _dbLeitura.TipoProdutos
                .Where(tp => tp.CoTipoProduto == coTipoProduto)
                .Where(tp => tp.DhExclusao == null)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExisteVinculoTipoProduto(int coTipoProduto)
        {
            return await _dbLeitura.Produtos
                .Include(p => p.CoTipoProdutoNavigation)
                .Where(p => p.DhExclusao == null)
                .Where(p => p.CoTipoProdutoNavigation.CoTipoProduto.Equals(coTipoProduto))
                .AnyAsync();
        }

        public async Task<bool> ExisteTipoProduto(int coTipoProduto)
        {
            return await _dbLeitura.TipoProdutos
                .Where(p => p.CoTipoProduto == coTipoProduto)
                .Where(p => p.DhExclusao == null)
                .AnyAsync();
        }

        public async Task<bool> ExisteTipoProdutoDuplicado(string noTipoProduto)
        {
            return await _dbLeitura.TipoProdutos
                .Where(tp => tp.NoTipoProduto == noTipoProduto)
                .Where(tp => tp.DhExclusao == null)
                .AnyAsync();
        }

        public async Task<bool> CriarTipoProduto(TipoProduto tipoProduto)
        {
            await _dbEscrita.AddAsync(tipoProduto);
            return await SaveChangesAsync();
        }

        public async Task<bool> ExcluirTipoProduto(int coTipoProduto)
        {
            await _dbEscrita.TipoProdutos
                .Where(p => p.CoTipoProduto == coTipoProduto)
                .ExecuteUpdateAsync(up => up
                    .SetProperty(p => p.DhExclusao, new DateTime().GetDataAtual()));

            return true;
        }


        public async Task<bool> SaveChangesAsync()
        {
            var acaoBanco = await _dbEscrita.SaveChangesAsync();
            return acaoBanco == 1;
        }
    }
}
