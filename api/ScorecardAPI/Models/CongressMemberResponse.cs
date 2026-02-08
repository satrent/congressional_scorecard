namespace ScorecardAPI.Models;

/// <summary>
/// Response model for a congress member's profile and advocacy score.
/// </summary>
public class CongressMemberResponse
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? MiddleName { get; init; }
    public required string State { get; init; }
    /// <summary>
    /// District number for House members; null for Senators.
    /// </summary>
    public int? District { get; init; }
    /// <summary>
    /// Numeric advocacy score for farm animal welfare issues.
    /// </summary>
    public required int AdvocacyScore { get; init; }
}
