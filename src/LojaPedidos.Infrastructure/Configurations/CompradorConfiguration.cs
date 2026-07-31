using LojaPedidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaPedidos.Infrastructure.Configurations;

public sealed class CompradorConfiguration : IEntityTypeConfiguration<Comprador>
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
    }
}
