using System.Text.Json.Serialization;

namespace Domain.Entities.v1;

public class User
{
    public int Id { get; set; }

    [JsonIgnore]
    public int AddressId { get; set; }

    public string Name { get; set; } = default!;
    public DateTime Birthday { get; set; }
    public string Email { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Address? Address { get; set; }
}