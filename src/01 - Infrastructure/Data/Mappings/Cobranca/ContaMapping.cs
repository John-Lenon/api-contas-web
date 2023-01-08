using Domain.Entities.Cobranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings.Tesouraria
{
    public class ContaMapping : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> builder)
        {
            builder.ToTable("contas")
                .HasKey(e => e.Id)
                .HasName("PK_id_conta");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Nome)
                .HasColumnType("varchar(60)")
                .HasColumnName("nome")
                .HasMaxLength(60)
                .IsRequired();

            builder.Property(e => e.ValorOriginal)
                .HasColumnType("decimal(19,2)")
                .HasColumnName("valor_original")
                .IsRequired();

            builder.Property(e => e.ValorCorrigido)
                .HasColumnType("decimal(19,2)")
                .HasColumnName("valor_corrigido")
                .IsRequired();

            builder.Property(e => e.DataVencimento)
                .HasColumnType("datetime")
                .HasColumnName("data_vencimento")
                .IsRequired();

            builder.Property(e => e.DataPagamento)
                .HasColumnType("datetime")
                .HasColumnName("data_pagamento")
                .IsRequired();

            builder.Property(e => e.QuantidadeDiasAtraso)
                .HasColumnType("int")
                .HasColumnName("qtd_dias_atraso")
                .IsRequired();

            builder.Property(e => e.Multa)
                .HasColumnType("decimal(19,2)")
                .HasColumnName("multa")
                .IsRequired();

            builder.Property(e => e.JurosDia)
                .HasColumnType("decimal(19,2)")
                .HasColumnName("juros_dia")
                .IsRequired();

            builder.Property(e => e.UsuarioId);

            builder.HasOne(conta => conta.Usuario)
                .WithMany(usuario => usuario.Contas)
                .HasPrincipalKey(usuario => usuario.Id)
                .HasForeignKey(conta => conta.UsuarioId);
        }
    }
}
