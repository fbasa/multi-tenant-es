
using UniEnroll.Domain.Common;

namespace UniEnroll.Domain.Instructors;

public sealed class Instructor : EntityBase, IAggregateRoot
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Department { get; private set; }
    public string TenantId { get; private set; }

    public Instructor(string id, string firstName, string lastName, string email, string department, string tenantId) : base(id)
    {
        FirstName = firstName; LastName = lastName; Email = email; Department = department; TenantId = tenantId;
    }
}
