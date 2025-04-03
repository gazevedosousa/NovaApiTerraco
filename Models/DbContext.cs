using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

    public virtual DbSet<Lancamento> Lancamentos { get; set; }

    public virtual DbSet<Pagamento> Pagamentos { get; set; }

    public virtual DbSet<Perfil> Perfils { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<TipoProduto> TipoProdutos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Leitura");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comandum>(entity =>
        {
            entity.HasKey(e => e.CoComanda);

            entity.ToTable("COMANDA");

            entity.Property(e => e.CoComanda).HasColumnName("CO_COMANDA");
            entity.Property(e => e.CoSituacao).HasColumnName("CO_SITUACAO");
            entity.Property(e => e.DhCriacao).HasColumnName("DH_CRIACAO");
            entity.Property(e => e.DhExclusao).HasColumnName("DH_EXCLUSAO");
            entity.Property(e => e.NoComanda)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NO_COMANDA");
        });

        modelBuilder.Entity<Lancamento>(entity =>
        {
            entity.HasKey(e => e.CoLancamento);

            entity.ToTable("LANCAMENTO");

            entity.Property(e => e.CoLancamento).HasColumnName("CO_LANCAMENTO");
            entity.Property(e => e.CoComanda).HasColumnName("CO_COMANDA");
            entity.Property(e => e.CoProduto).HasColumnName("CO_PRODUTO");
            entity.Property(e => e.DhCriacao).HasColumnName("DH_CRIACAO");
            entity.Property(e => e.QtdLancamento).HasColumnName("QTD_LANCAMENTO");

            entity.HasOne(d => d.CoComandaNavigation).WithMany(p => p.Lancamentos)
                .HasForeignKey(d => d.CoComanda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LANCAMENTO_COMANDA");

            entity.HasOne(d => d.CoProdutoNavigation).WithMany(p => p.Lancamentos)
                .HasForeignKey(d => d.CoProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LANCAMENTO_PRODUTO");
        });

        modelBuilder.Entity<Pagamento>(entity =>
        {
            entity.HasKey(e => e.CoPagamento);

            entity.ToTable("PAGAMENTO");

            entity.Property(e => e.CoPagamento).HasColumnName("CO_PAGAMENTO");
            entity.Property(e => e.CoComanda).HasColumnName("CO_COMANDA");
            entity.Property(e => e.CoTipoPagamento).HasColumnName("CO_TIPO_PAGAMENTO");
            entity.Property(e => e.DhCriacao).HasColumnName("DH_CRIACAO");
            entity.Property(e => e.VrPagamento)
                .HasColumnType("numeric(23, 2)")
                .HasColumnName("VR_PAGAMENTO");

            entity.HasOne(d => d.CoComandaNavigation).WithMany(p => p.Pagamentos)
                .HasForeignKey(d => d.CoComanda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PAGAMENTO_COMANDA");
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.HasKey(e => e.CoPerfil);

            entity.ToTable("PERFIL");

            entity.Property(e => e.CoPerfil).HasColumnName("CO_PERFIL");
            entity.Property(e => e.NoPerfil)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NO_PERFIL");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.CoProduto);

            entity.ToTable("PRODUTO");

            entity.Property(e => e.CoProduto).HasColumnName("CO_PRODUTO");
            entity.Property(e => e.CoTipoProduto).HasColumnName("CO_TIPO_PRODUTO");
            entity.Property(e => e.DhCriacao).HasColumnName("DH_CRIACAO");
            entity.Property(e => e.DhExclusao).HasColumnName("DH_EXCLUSAO");
            entity.Property(e => e.NoProduto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NO_PRODUTO");
            entity.Property(e => e.VrProduto)
                .HasColumnType("numeric(23, 2)")
                .HasColumnName("VR_PRODUTO");

            entity.HasOne(d => d.CoTipoProdutoNavigation).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.CoTipoProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRODUTO_TIPO_PRODUTO");
        });

        modelBuilder.Entity<TipoProduto>(entity =>
        {
            entity.HasKey(e => e.CoTipoProduto);

            entity.ToTable("TIPO_PRODUTO");

            entity.HasIndex(e => e.NoTipoProduto, "IX_TIPO_PRODUTO").IsUnique();

            entity.Property(e => e.CoTipoProduto).HasColumnName("CO_TIPO_PRODUTO");
            entity.Property(e => e.DhCriacao).HasColumnName("DH_CRIACAO");
            entity.Property(e => e.DhExclusao).HasColumnName("DH_EXCLUSAO");
            entity.Property(e => e.NoTipoProduto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NO_TIPO_PRODUTO");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.CoUsuario);

            entity.ToTable("USUARIO");

            entity.HasIndex(e => e.NoUsuario, "IX_USUARIO").IsUnique();

            entity.Property(e => e.CoUsuario).HasColumnName("CO_USUARIO");
            entity.Property(e => e.CoPerfil).HasColumnName("CO_PERFIL");
            entity.Property(e => e.NoUsuario)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NO_USUARIO");
            entity.Property(e => e.SenhaHash)
                .HasMaxLength(64)
                .HasColumnName("SENHA_HASH");
            entity.Property(e => e.SenhaSalt)
                .HasMaxLength(128)
                .HasColumnName("SENHA_SALT");

            entity.HasOne(d => d.CoPerfilNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.CoPerfil)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USUARIO_PERFIL");
        });
    }
}
