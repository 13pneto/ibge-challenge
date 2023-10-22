namespace challenge.ibge.infra.data.Entities;

public abstract class BaseEntity
{
    public int Id { get; private set; }
    public DateTimeOffset CreatedAt { get; protected set; }
    public DateTimeOffset? UpdatedAt { get; protected set; }
}