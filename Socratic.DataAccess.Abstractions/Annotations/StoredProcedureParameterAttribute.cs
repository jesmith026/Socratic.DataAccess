using System;

namespace Socratic.DataAccess.Abstractions.Annotations
{
    public class StoredProcedureParameterAttribute : Attribute
    {
        public StoredProcedureParameterAttribute(string name)
        {
            Name = name;
        }        
        
        public string Name { get; set; }
    }
}