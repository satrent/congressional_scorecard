using Amazon.DynamoDBv2.DataModel;

namespace ScorecardAPI.Models;

[DynamoDBTable("CongressMember")]
public class CongressMember
{
    [DynamoDBHashKey("MemberID")]
    public string Id { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string State { get; set; } = string.Empty;
    public int? District { get; set; }
    public int AdvocacyScore { get; set; }
}
