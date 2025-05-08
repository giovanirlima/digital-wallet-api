using System.Text.Json.Serialization;

namespace Domain.Entities.v1;

public class Address
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Street { get; set; } = default!;
    public int Number { get; set; }
    public string City { get; set; } = default!;
    public string Country { get; set; } = default!;
}