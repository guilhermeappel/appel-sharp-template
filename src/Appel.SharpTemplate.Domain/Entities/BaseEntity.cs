namespace Appel.SharpTemplate.Domain.Entities;

public abstract class BaseEntity
{
    public DateTimeOffset CreatedOn;
    public DateTimeOffset LastUpdatedOn;
    public int Id { get; set; }
}
