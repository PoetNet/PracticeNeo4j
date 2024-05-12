using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using WebApi.Models;

namespace Auth.Controllers

{
    [Route("api/v1/auth")]
    [ApiController]
    public class EmployeeController(IGraphClient _client) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Employee emp)
        {
            await _client.Cypher.Create("(e:Employee $emp)")
                .WithParam("emp", emp)
                .ExecuteWithoutResultsAsync();

            return Ok();
        }

        [HttpGet("{eid:int}/assignemployee/{did:int}")]
        public async Task<IActionResult> AssignDepartment(int did, int eid)
        {
            await _client.Cypher.Match("(e:Employee)")
                .Where((Department d, Employee e) => d.Id == did && e.Id == eid)
                .Create("(d)-[r:hasEmployee]->(e)")
                .ExecuteWithoutResultsAsync();

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Employee emp)
        {
            await _client.Cypher.Match("(d:Employee)")
                .Where((Employee d) => d.Id == id)
                .Set("d = $emp")
                .WithParam("emp", emp)
                .ExecuteWithoutResultsAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _client.Cypher.Match("(d:Employee)")
                .Where((Employee d) => d.Id == id)
                .Delete("d")
                .ExecuteWithoutResultsAsync();

            return Ok();
        }
    }
}
