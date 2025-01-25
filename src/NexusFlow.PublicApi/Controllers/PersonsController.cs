using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NexusFlow.PublicApi.Data.Repositories;
using NexusFlow.PublicApi.Models;
using System;

namespace NexusFlow.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly PersonRepository _repository;

        public PersonsController(PersonRepository personRepository)
        {
            _repository = personRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var persons = await _repository.GetPersons();
            return Ok(persons);
        }

        [HttpGet("{code=-1}")]
        public async Task<IActionResult> Get(int code=-1)
        {
            var persons = await _repository.GetPersons(code);
            return Ok(persons);
        }

        [HttpGet("{searchTerm}/{searchCiteria}")]
        public async Task<IActionResult> Get(string searchTerm, string searchCiteria)
        {
            var person = await _repository.GetPersonByCriteriaAsync(searchTerm, searchCiteria);
            if (person == null)
            {
                return NotFound(new { Message = $"Person with {searchCiteria} = {searchTerm} not found." });
            }
            return Ok(person);
        }

        // POST: api/Persons
        [HttpPost]
        public async Task<IActionResult> AddPerson([FromBody] Person newPerson)
        {
            if (newPerson == null || string.IsNullOrWhiteSpace(newPerson.Name) || string.IsNullOrWhiteSpace(newPerson.Surname))
            {
                return BadRequest(new { Message = "Invalid person data provided." });
            }

            try
            {
                var result = await _repository.CreatePersonAsync(newPerson);
                return Ok(result);
            }
            catch (SqlException ex) when (ex.Number == 2627) // Unique constraint violation
            {
                return Conflict(new { Message = $"A person with ID Number {newPerson.IdNumber} already exists." });
            }
        }

        // PUT: api/Persons/{code}
        [HttpPut("{code}")]
        public async Task<IActionResult> Update(int code, [FromBody] Person updatedPerson)
        {
            if (updatedPerson == null || string.IsNullOrWhiteSpace(updatedPerson.Name) || string.IsNullOrWhiteSpace(updatedPerson.Surname))
            {
                return BadRequest(new { Message = "Invalid person data provided." });
            }

            var result = await _repository.GetPersons(code);
            var existingPerson = result.FirstOrDefault();
            if (existingPerson == null || existingPerson.Code != code)
            {
                return NotFound(new { Message = $"Person with code {code} not found." });
            }

            updatedPerson.Code = code; // Ensure the code remains the same during the update

            await _repository.UpdatePersonAsync(updatedPerson);
            return Ok(updatedPerson);
        }

        // DELETE: api/Persons/{code}
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(int code)
        {
            try
            {
                await _repository.DeletePersonAsync(code);
                return NoContent(); // 204 No Content
            }
            catch (SqlException ex) when (ex.Number == 547) // Foreign key violation
            {
                return Conflict(new { Message = $"Person with code {code} cannot be deleted because they have active accounts." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Person with code {code} not found." });
            }
        }
    }

}
