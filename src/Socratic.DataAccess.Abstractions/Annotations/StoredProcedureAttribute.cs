using System;

namespace Socratic.DataAccess.Abstractions.Annotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StoredProcedureAttribute : Attribute
    {
        public StoredProcedureAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}