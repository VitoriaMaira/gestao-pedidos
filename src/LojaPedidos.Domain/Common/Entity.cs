namespace LojaPedidos.Domain.Common;

public abstract class Entity
{
    protected Entity()
    {
        Id = Guid.CreateVersion7();
    }

    protected Entity(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("O identificador não pode ser vazio.", nameof(id));
        }

        Id = id;
    }

    public Guid Id { get; private set; }
}
