using Domain.Entities.Cobranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings.Tesouraria
{
    public class RegraDiaAtrasoMapping : IEntityTypeConfiguration<RegraDiaAtraso>
    {
        public void Configure(EntityTypeBuilder<RegraDiaAtraso> builder)
        {
            builder.ToTable("regras_dias_atrasos")
                .HasKey(e => e.Id)
                .HasName("PK_id_regra_dias_atraso");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.DiasAtrasoMinimo)
                .HasColumnType("int")
                .HasColumnName("dias_atraso_minimo")
                .IsRequired();

            builder.Property(e => e.DiasAtrasoMaximo)
               .HasColumnType("int")
               .HasColumnName("dias_atraso_maximo");               

            builder.Property(e => e.Multa)
                .HasColumnType("decimal(10,2)")
                .HasColumnName("multa")
                .IsRequired();

            builder.Property(e => e.JurosDia)
                .HasColumnType("decimal(10,2)")
                .HasColumnName("juros_dia")
                .IsRequired();
        }
    }
}
