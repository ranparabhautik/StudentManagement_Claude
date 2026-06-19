namespace StudentManagement.Domain.Interfaces;

/// <summary>
/// Defines a contract for entities with a unique identifier.
/// </summary>
public interface IEntity
{
    /// <summary>Gets or sets the unique identifier of the entity.</summary>
    int Id { get; set; }
}
