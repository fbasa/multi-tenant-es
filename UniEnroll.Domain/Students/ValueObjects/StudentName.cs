
namespace UniEnroll.Domain.Students.ValueObjects;

public readonly struct StudentName
{
    public string First { get; }
    public string Last { get; }
    public StudentName(string first, string last)
    {
        First = first;
        Last = last;
    }
    public override string ToString() => $"{First} {Last}";
}
