using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TerracoDaCida.Models;

namespace TerracoDaCida.Models;

public class DbEscrita : DbContextBase<DbEscrita>
{
    public DbEscrita(DbContextOptions<DbEscrita> options) : base(options) { }
}

public class DbLeitura : DbContextBase<DbLeitura>
{
    public DbLeitura(DbContextOptions<DbLeitura> options) : base(options) { }
}

public abstract class DbContextBase<T> : DbContext
    where T : DbContext
{
    public DbContextBase(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Comandum> Comanda { get; set; }

    public virtual DbSet<Couvert> Couverts { get; set; }

    public virtual DbSet<Lancamento> Lancamentos { get; set; }

    public virtual DbSet<Pagamento> Pagamentos { get; set; }

    public virtual DbSet<Perfil> Perfils { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<TipoProduto> TipoProdutos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:Leitura");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comandum>(entity =>
        {
            entity.HasKey(e => e.CoComanda).HasName("comanda_pk");

            entity.ToTable("comanda", "dbo");

            entity.Property(e => e.CoComanda).HasColumnName("co_comanda");
            entity.Property(e => e.CoSituacao).HasColumnName("co_situacao");
            entity.Property(e => e.DhFechamento)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dh_fechamento");
            entity.Property(e => e.DhAbertura)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dh_abertura");
            entity.Property(e => e.NoComanda)
                .HasColumnType("character varying")
                .HasColumnName("no_comanda");
            entity.Property(e => e.QtdCouvert).HasColumnName("qtd_couvert");
            entity.Property(e => e.Temcouvert).HasColumnName("temcouvert");
            entity.Property(e => e.Temdezporcento).HasColumnName("temdezporcento");
            entity.Property(e => e.Valordesconto).HasColumnName("valordesconto");
            entity.Property(e => e.Valortroco).HasColumnName("valortroco");
        });

        modelBuilder.Entity<Couvert>(entity =>
        {
            entity.HasKey(e => e.CoCouvert).HasName("couvert_pk");

            entity.ToTable("couvert", "dbo");

            entity.Property(e => e.CoCouvert).HasColumnName("co_couvert");
            entity.Property(e => e.IsAtivo).HasColumnName("is_ativo");
            entity.Property(e => e.VrCouvert).HasColumnName("vr_couvert");
        });

        modelBuilder.Entity<Lancamento>(entity =>
        {
            entity.HasKey(e => e.CoLancamento).HasName("lancamento_pk");

            entity.ToTable("lancamento", "dbo");

            entity.Property(e => e.CoLancamento).HasColumnName("co_lancamento");
            entity.Property(e => e.CoComanda).HasColumnName("co_comanda");
            entity.Property(e => e.CoProduto).HasColumnName("co_produto");
            entity.Property(e => e.DhCriacao)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dh_criacao");
            entity.Property(e => e.QtdLancamento).HasColumnName("qtd_lancamento");
            entity.Property(e => e.VrLancamento).HasColumnName("vr_lancamento");
            entity.Property(e => e.VrUnitario).HasColumnName("vr_unitario");

            entity.HasOne(d => d.CoComandaNavigation).WithMany(p => p.Lancamentos)
                .HasForeignKey(d => d.CoComanda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lancamento_comanda_fk");

            entity.HasOne(d => d.CoProdutoNavigation).WithMany(p => p.Lancamentos)
                .HasForeignKey(d => d.CoProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lancamento_produto_fk");
        });

        modelBuilder.Entity<Pagamento>(entity =>
        {
            entity.HasKey(e => e.CoPagamento).HasName("pagamento_pk");

            entity.ToTable("pagamento", "dbo");

            entity.Property(e => e.CoPagamento).HasColumnName("co_pagamento");
            entity.Property(e => e.CoComanda).HasColumnName("co_comanda");
            entity.Property(e => e.CoTipoPagamento).HasColumnName("co_tipo_pagamento");
            entity.Property(e => e.DhCriacao)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dh_criacao");
            entity.Property(e => e.VrPagamento).HasColumnName("vr_pagamento");

            entity.HasOne(d => d.CoComandaNavigation).WithMany(p => p.Pagamentos)
                .HasForeignKey(d => d.CoComanda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pagamento_comanda_fk");
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.HasKey(e => e.CoPerfil).HasName("perfil_pk");

            entity.ToTable("perfil", "dbo");

            entity.HasIndex(e => e.NoPerfil, "perfil_unique").IsUnique();

            entity.Property(e => e.CoPerfil).HasColumnName("co_perfil");
            entity.Property(e => e.NoPerfil)
                .HasColumnType("character varying")
                .HasColumnName("no_perfil");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.CoProduto).HasName("produto_pk");

            entity.ToTable("produto", "dbo");

            entity.Property(e => e.CoProduto).HasColumnName("co_produto");
            entity.Property(e => e.CoTipoProduto).HasColumnName("co_tipo_produto");
            entity.Property(e => e.DhCriacao)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dh_criacao");
            entity.Property(e => e.DhExclusao)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dh_exclusao");
            entity.Property(e => e.NoProduto)
                .HasColumnType("character varying")
                .HasColumnName("no_produto");
            entity.Property(e => e.VrProduto).HasColumnName("vr_produto");

            entity.HasOne(d => d.CoTipoProdutoNavigation).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.CoTipoProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_tipoproduto_fk");
        });

        modelBuilder.Entity<TipoProduto>(entity =>
        {
            entity.HasKey(e => e.CoTipoProduto).HasName("tipoproduto_pk");

            entity.ToTable("tipoproduto", "dbo");

            entity.HasIndex(e => e.NoTipoProduto, "tipoproduto_unique").IsUnique();

            entity.Property(e => e.CoTipoProduto).HasColumnName("co_tipo_produto");
            entity.Property(e => e.DhCriacao)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dh_criacao");
            entity.Property(e => e.DhExclusao)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dh_exclusao");
            entity.Property(e => e.NoTipoProduto)
                .HasColumnType("character varying")
                .HasColumnName("no_tipo_produto");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.CoUsuario).HasName("usuario_pk");

            entity.ToTable("usuario", "dbo");

            entity.HasIndex(e => e.NoUsuario, "usuario_unique").IsUnique();

            entity.Property(e => e.CoUsuario).HasColumnName("co_usuario");
            entity.Property(e => e.CoPerfil).HasColumnName("co_perfil");
            entity.Property(e => e.NoUsuario)
                .HasColumnType("character varying")
                .HasColumnName("no_usuario");
            entity.Property(e => e.Senhahash).HasColumnName("senhahash");
            entity.Property(e => e.Senhasalt).HasColumnName("senhasalt");

            entity.HasOne(d => d.CoPerfilNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.CoPerfil)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_perfil_fk");
        });
    }
}
