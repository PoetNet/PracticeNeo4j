using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using System.Security.Claims;
using WebApi.Models;

namespace Auth.Controllers

{
    [Route("api/v1/auth")]
    [ApiController]
    public class DepartmentController(IGraphClient _client) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var depatments = await _client.Cypher.Match("(n: Department")
                .Return(n => n.As<Department>())
                .ResultsAsync;

            return Ok(depatments);

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var depatment = await _client.Cypher.Match("(d: Department")
                .Where((Department d) =>  d.Id == id)
                .Return(d => d.As<Department>()).ResultsAsync;

            return Ok(depatment.LastOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Department dept)
        {
            await _client.Cypher.Create("(d: Department $dept")
                .WithParam("dept", dept)
                .ExecuteWithoutResultsAsync();

            return Ok();
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Department dept)
        {
            await _client.Cypher.Match("(d: Department")
                .Where((Department d) => d.Id == id)
                .Set("d = $dept")
                .WithParam("dept", dept)
                .ExecuteWithoutResultsAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _client.Cypher.Match("(d: Department")
                .Where((Department d) => d.Id == id)
                .Delete("d")
                .ExecuteWithoutResultsAsync();

            return Ok();
        }

    }
}
