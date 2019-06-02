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

        /// <summary>
        /// Demonstrate getting values from the database using the EF context and IQueryable
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return uow.Context<Student>()
                .GetAll()
                .Select(x => x.Name).
                ToList();
        }

        /// <summary>
        /// Demonstrate getting values from the database using the EF context generic repository
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Demonstrate getting values from the database using object-modeled stored procedure. this functionality is 
        /// specific to the Socratic.DataAccess library as it is an abstraction over either EF or Dapper
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/proc")]
        public async Task<StudentDto> GetByProc(int id)
        {           
            var proc = new GetStudentByIdProcedure { StudentId = id };
            return await uow.Database.QuerySingleAsync(proc);                     
        }

        /// <summary>
        /// Deomonstrate adding value to the database using EF context and generic repository
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Post([FromBody] string value)
        {
            var student = new Student { Name = value };
            await uow.Context<Student>().AddAsync(student);
            await uow.CommitAsync();
            return student.Id;
        }

        /// <summary>
        /// Deomonstrate updating value in the database using EF context and generic repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] string value)
        {
            var repo = uow.Context<Student>();

            var student = await repo.GetByIdAsync(id);            
            student.Name = value;
            repo.Update(student);

            await uow.CommitAsync();
        }

        /// <summary>
        /// Demonstrate deleting value in the database using EF context and generic repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await uow.Context<Student>().DeleteAsync(id);
            await uow.CommitAsync();
        }

        /// <summary>
        /// Deomonstrate deleting value in the database using  object-modeled stored procedure as well as the unit of work
        /// functionality for commit and rollback extending to stored procedure execution. this functionality is 
        /// specific to the Socratic.DataAccess library as it is an abstraction over either EF or Dapper
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        [HttpDelete("{id}/proc")]
        public async Task<int> DeleteByProc(int id, [FromQuery]bool commit)
        {
            if (!uow.Context<Student>().GetAll().Any(x => x.Id == id))
                return 0;

            var deleteProc = new DeleteStudentProcedure { Id = id };
            var result = await uow.Database.ExecuteAsync(deleteProc);

            if (commit)
                await uow.CommitAsync();
            else {
                uow.Rollback();
                result = 0;
            }

            return result;
        }
    }
}
