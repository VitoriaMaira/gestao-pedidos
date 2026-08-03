using LojaPedidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaPedidos.Infrastructure.DataAccess.Mappings;

public sealed class CompradorMapping : IEntityTypeConfiguration<Comprador>
{
    public void Configure(EntityTypeBuilder<Comprador> builder)
    {
        builder.ToTable("Compradores");

        builder.HasKey(comprador => comprador.Id);

        builder.Property(comprador => comprador.Id)
            .ValueGeneratedNever();

        builder.Property(comprador => comprador.Nome)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(comprador => comprador.Cpf)
            .HasMaxLength(11)
            .IsRequired();

        builder.HasIndex(comprador => comprador.Cpf)
            .IsUnique();
    }
}
