using System;

namespace Socratic.DataAccess.Exceptions
{
    /// <summary>
    /// Representation of an issue when attempting to serialize the stored procedure parameters.
    /// </summary>
    public class ParameterSerializationException : Exception
    {
        public ParameterSerializationException(string paramName)
            : this(paramName, null)
        {
        }

        public ParameterSerializationException(string paramName, Exception ex)
            : base($"Error serializing parameter with name {paramName}", ex)
        {
        }
    }
}