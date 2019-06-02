using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Mvc;
using Socratic.DataAccess.Abstractions;

namespace DockerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IUnitOfWork<SchoolContext> uow;

        public StudentsController(IUnitOfWork<SchoolContext> uow)
        {
            this.uow = uow;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return uow.Context<Student>()
                .GetAll()
                .Select(x => x.Name).
                ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            try {
                var student = await uow.Context<Student>().GetByIdAsync(id);
                return student.Name;
            }
            catch(Exception ex)
            {
                return "Unable to find student with that id";
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<int> Post([FromBody] string value)
        {
            var student = new Student { Name = value };
            await uow.Context<Student>().AddAsync(student);
            await uow.CommitAsync();
            return student.Id;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] string value)
        {
            var repo = uow.Context<Student>();

            var student = await repo.GetByIdAsync(id);            
            student.Name = value;
            repo.Update(student);

            await uow.CommitAsync();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await uow.Context<Student>().DeleteAsync(id);
            await uow.CommitAsync();
        }
    }
}
