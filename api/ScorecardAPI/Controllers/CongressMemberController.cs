using Microsoft.AspNetCore.Mvc;
using ScorecardAPI.Models;

namespace ScorecardAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CongressMemberController : ControllerBase
{
    private const string MockDataHeader = "mockdata";

    /// <summary>
    /// Gets a congress member by ID. When the "mockdata" header is set,
    /// returns a mock record without querying the database.
    /// </summary>
    /// <param name="id">The congress member identifier.</param>
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

        // Database lookup not yet implemented
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
