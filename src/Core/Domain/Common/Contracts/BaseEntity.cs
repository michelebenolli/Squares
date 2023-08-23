using MassTransit;
using System.ComponentModel.DataAnnotations.Schema;

namespace Squares.Domain.Common.Contracts;
public abstract class BaseEntity : IEntity
{
    public int Id { get; set; } = default!;

    [NotMapped]
    public List<DomainEvent> DomainEvents { get; } = new();
}