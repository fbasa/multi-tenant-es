
namespace UniEnroll.Application.Features.Documents.Dtos;

public sealed record RequirementDto(string Id, string StudentId, string Type, string Status, string? ReviewerNotes);
