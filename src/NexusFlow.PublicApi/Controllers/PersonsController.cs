using Microsoft.AspNetCore.Mvc;
using NexusFlow.PublicApi.Models;

namespace NexusFlow.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private List<Person> _persons = new List<Person>
        {
            new Person { Code = 1, Name = "John Doe" },
            new Person { Code = 2, Name = "Jane Doe" }
        };

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllPersons()
        {
            return Ok(_persons);
        }
    }
}
