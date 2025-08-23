
namespace UniEnroll.Contracts.Documents;

public sealed record UploadRequirementRequest(string StudentId, string Type, string FileName, string ContentBase64);
