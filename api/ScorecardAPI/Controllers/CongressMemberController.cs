using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;
using ScorecardAPI.Models;

namespace ScorecardAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CongressMemberController : ControllerBase
{
    private const string MockDataHeader = "mockdata";
    private readonly IDynamoDBContext _context;

    public CongressMemberController(IDynamoDBContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets congress members by state.
    /// </summary>
    /// <param name="state">The state abbreviation (e.g., MN).</param>
    [HttpGet("state/{state}")]
    [ProducesResponseType(typeof(IEnumerable<CongressMemberResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CongressMemberResponse>>> GetByState(string state)
    {
        var useMockData = Request.Headers.TryGetValue(MockDataHeader, out var value) &&
                          string.Equals(value.ToString(), "true", StringComparison.OrdinalIgnoreCase);

        if (useMockData)
        {
            return Ok(new[] { GetMockMember() });
        }

        // Scan assumes State is not a key. For better performance, use a GSI.
        var conditions = new List<ScanCondition>
        {
            new ScanCondition(nameof(CongressMember.State), Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, state.ToUpper())
        };

        var members = await _context.ScanAsync<CongressMember>(conditions).GetRemainingAsync();

        var response = members.Select(m => new CongressMemberResponse
        {
            FirstName = m.FirstName,
            LastName = m.LastName,
            MiddleName = m.MiddleName,
            State = m.State,
            District = m.District,
            AdvocacyScore = m.AdvocacyScore
        });

        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CongressMemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<CongressMemberResponse> GetById(string id)
    {
        var useMockData = Request.Headers.TryGetValue(MockDataHeader, out var value) &&
                          string.Equals(value.ToString(), "true", StringComparison.OrdinalIgnoreCase);

        if (useMockData)
        {
            return Ok(GetMockMember());
        }

        // Database lookup not yet implemented for ID
        return NotFound();
    }

    private static CongressMemberResponse GetMockMember()
    {
        return new CongressMemberResponse
        {
            FirstName = "Jane",
            LastName = "Smith",
            MiddleName = "Marie",
            State = "CA",
            District = 12,
            AdvocacyScore = 85
        };
    }
}
