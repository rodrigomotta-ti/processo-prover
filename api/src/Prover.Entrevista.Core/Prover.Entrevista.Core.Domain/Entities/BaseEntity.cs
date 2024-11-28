using System.Text.Json.Serialization;

namespace Prover.Entrevista.Core.Domain.Entities;

public abstract class BaseEntity
{
    public BaseEntity() => CreateAt = DateTime.Now;

    [JsonIgnore]
    public int Id { get; set; }

    public DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; } = null;

    public void Update() => UpdateAt = DateTime.Now;
}