using Socratic.DataAccess.Abstractions;
using Socratic.DataAccess.Abstractions.Annotations;

namespace api.Data
{
    [StoredProcedure("GetStudent")]
    public class GetStudentByIdProcedure : IStoredProcedure<StudentDto>
    {
        public int StudentId { get; set; }
    }
}