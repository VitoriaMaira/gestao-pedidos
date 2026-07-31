using LojaPedidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaPedidos.Infrastructure.Configurations;

public sealed class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos");

        builder.HasKey(produto => produto.Id);

        builder.Property(produto => produto.Id)
            .ValueGeneratedNever();

        builder.Property(produto => produto.Nome)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(produto => produto.Preco)
            .HasPrecision(18, 2)
            .IsRequired();
    }
}
