using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

// Configure AWS SDK
var client = new AmazonDynamoDBClient();
var context = new DynamoDBContext(client);

Console.WriteLine($"Region: {client.Config.RegionEndpoint?.SystemName ?? "Unknown"}");

// Check if table exists
var tableName = "CongressMember";
try
{
    await client.DescribeTableAsync(tableName);
    Console.WriteLine($"Table '{tableName}' exists.");
}
catch (ResourceNotFoundException)
{
    Console.WriteLine($"Table '{tableName}' does not exist. Creating...");
    await client.CreateTableAsync(new CreateTableRequest
    {
        TableName = tableName,
        KeySchema = new List<KeySchemaElement>
        {
            new KeySchemaElement("MemberID", KeyType.HASH) // Partition key
        },
        AttributeDefinitions = new List<AttributeDefinition>
        {
            new AttributeDefinition("MemberID", ScalarAttributeType.S)
        },
        ProvisionedThroughput = new ProvisionedThroughput
        {
            ReadCapacityUnits = 5,
            WriteCapacityUnits = 5
        }
    });

    // Wait for table to be active
    Console.WriteLine("Waiting for table to become active...");
    var status = TableStatus.CREATING;
    while (status != TableStatus.ACTIVE)
    {
        System.Threading.Thread.Sleep(500);
        var res = await client.DescribeTableAsync(tableName);
        status = res.Table.TableStatus;
    }
    Console.WriteLine("Table created successfully.");
}

Console.WriteLine("Reading legislators.json...");
var jsonString = File.ReadAllText("legislators.json");
var legislators = JsonSerializer.Deserialize<List<Legislator>>(jsonString);

if (legislators == null)
{
    Console.WriteLine("No legislators found in JSON.");
    return;
}

Console.WriteLine($"Found {legislators.Count} legislators. Filtering for current ones...");

var congressMembers = new List<CongressMember>();

foreach (var leg in legislators)
{
    // We only care about the most recent term to determine current state/district
    var lastTerm = leg.terms.LastOrDefault();
    if (lastTerm == null) continue;

    // Filter for those currently in office (end date >= today or 2025-01-03 for 118th)
    // The dataset is "legislators-current", so we can assume they are current.
    
    var member = new CongressMember
    {
        Id = leg.id.bioguide,
        FirstName = leg.name.first,
        LastName = leg.name.last,
        MiddleName = leg.name.middle,
        State = lastTerm.state,
        District = lastTerm.district,
        AdvocacyScore = 0 // Default score
    };

    congressMembers.Add(member);
}

Console.WriteLine($"Prepared {congressMembers.Count} members for insertion.");

var batch = context.CreateBatchWrite<CongressMember>();

int count = 0;
foreach (var member in congressMembers)
{
    batch.AddPutItem(member);
    count++;

    // DynamoDB batch write limit is 25, but the high-level API handles batching.
    // However, it's good practice to execute in chunks to manage memory/throughput.
    if (count % 25 == 0)
    {
        await batch.ExecuteAsync();
        batch = context.CreateBatchWrite<CongressMember>();
        Console.Write(".");
    }
}

// Execute remaining
await batch.ExecuteAsync();

Console.WriteLine("\nSeeding complete!");

// --- Models for JSON Parsing ---

public class Legislator
{
    public Id id { get; set; }
    public Name name { get; set; }
    public List<Term> terms { get; set; }
}

public class Id
{
    public string bioguide { get; set; }
}

public class Name
{
    public string first { get; set; }
    public string last { get; set; }
    public string middle { get; set; }
}

public class Term
{
    public string type { get; set; }
    public string start { get; set; }
    public string end { get; set; }
    public string state { get; set; }
    public int? district { get; set; }
}

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
