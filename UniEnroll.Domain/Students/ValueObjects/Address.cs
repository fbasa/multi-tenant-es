
namespace UniEnroll.Domain.Students.ValueObjects;

public sealed class Address
{
    public string Line1 { get; }
    public string? Line2 { get; }
    public string City { get; }
    public string Province { get; }
    public string Country { get; }
    public string Zip { get; }

    public Address(string line1, string? line2, string city, string province, string country, string zip)
    {
        Line1 = line1; Line2 = line2; City = city; Province = province; Country = country; Zip = zip;
    }
}
