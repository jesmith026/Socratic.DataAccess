using Socratic.DataAccess.Abstractions;
using Socratic.DataAccess.Abstractions.Annotations;

namespace api.Data
{
    [StoredProcedure("DeleteStudent")]
    public class DeleteStudentProcedure : IStoredProcedure<int>
    {
        [StoredProcedureParameter("studentId")]
        public int Id { get; set; }
    }
}