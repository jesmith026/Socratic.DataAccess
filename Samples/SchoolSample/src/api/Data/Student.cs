using Socratic.DataAccess.Abstractions;

namespace api.Data
{
    public class Student : IDbEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}