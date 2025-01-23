using Microsoft.AspNetCore.Mvc;
using NexusFlow.PublicApi.Models;

namespace NexusFlow.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        // Simulated in-memory data store
        private static List<Person> _persons = new()
        {
            new Person { Code = 1, Name = "John", Surname = "Doe", IdNumber = "123456789" },
            new Person { Code = 2, Name = "Jane", Surname = "Doe", IdNumber = "987654321" }
        };

        // GET: api/Persons
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_persons);
        }

        // GET: api/Persons/{code}
        [HttpGet("{code}")]
        public IActionResult Get(int code)
        {
            var person = _persons.FirstOrDefault(p => p.Code == code);
            if (person == null)
            {
                return NotFound(new { Message = $"Person with code {code} not found." });
            }
            return Ok(person);
        }

        // POST: api/Persons
        [HttpPost]
        public IActionResult AddPerson([FromBody] Person newPerson)
        {
            if (newPerson == null || string.IsNullOrWhiteSpace(newPerson.Name) || string.IsNullOrWhiteSpace(newPerson.Surname))
            {
                return BadRequest(new { Message = "Invalid person data provided." });
            }

            if (_persons.Any(p => p.Code == newPerson.Code))
            {
                return Conflict(new { Message = $"A person with code {newPerson.Code} already exists." });
            }

            _persons.Add(newPerson);
            return CreatedAtAction(nameof(Get), new { code = newPerson.Code }, newPerson);
        }

        // PUT: api/Persons/{code}
        [HttpPut("{code}")]
        public IActionResult Update(int code, [FromBody] Person updatedPerson)
        {
            if (updatedPerson == null || string.IsNullOrWhiteSpace(updatedPerson.Name) || string.IsNullOrWhiteSpace(updatedPerson.Surname))
            {
                return BadRequest(new { Message = "Invalid person data provided." });
            }

            var existingPerson = _persons.FirstOrDefault(p => p.Code == code);
            if (existingPerson == null)
            {
                return NotFound(new { Message = $"Person with code {code} not found." });
            }

            // Update properties
            existingPerson.Name = updatedPerson.Name;
            existingPerson.Surname = updatedPerson.Surname;
            existingPerson.IdNumber = updatedPerson.IdNumber;

            return Ok(existingPerson);
        }

        // DELETE: api/Persons/{code}
        [HttpDelete("{code}")]
        public IActionResult Delete(int code)
        {
            var person = _persons.FirstOrDefault(p => p.Code == code);
            if (person == null)
            {
                return NotFound(new { Message = $"Person with code {code} not found." });
            }

            _persons.Remove(person);
            return NoContent(); // 204 No Content
        }
    }

}
