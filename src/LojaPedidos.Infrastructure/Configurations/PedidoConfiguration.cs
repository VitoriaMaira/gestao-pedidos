using LojaPedidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaPedidos.Infrastructure.Configurations;

public sealed class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");

        builder.HasKey(pedido => pedido.Id);

        builder.Property(pedido => pedido.Id)
            .ValueGeneratedNever();

        builder.Property(pedido => pedido.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(pedido => pedido.CriadoEm)
            .IsRequired();

        builder.Property(pedido => pedido.AtualizadoEm);

        builder.Ignore(pedido => pedido.Total);

        builder.HasOne(pedido => pedido.Comprador)
            .WithMany()
            .HasForeignKey(pedido => pedido.CompradorId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasMany(pedido => pedido.Itens)
            .WithOne()
            .HasForeignKey(item => item.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
