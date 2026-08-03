using LojaPedidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaPedidos.Infrastructure.DataAccess.Mappings;

public sealed class ItemPedidoMapping : IEntityTypeConfiguration<ItemPedido>
{
    public void Configure(EntityTypeBuilder<ItemPedido> builder)
    {
        builder.ToTable("ItensPedido");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .ValueGeneratedNever();

        builder.Property(item => item.Quantidade)
            .IsRequired();

        builder.Property(item => item.PrecoUnitario)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Ignore(item => item.Subtotal);

        builder.HasOne(item => item.Produto)
            .WithMany()
            .HasForeignKey(item => item.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasIndex(item => new { item.PedidoId, item.ProdutoId })
            .IsUnique();
    }
}
