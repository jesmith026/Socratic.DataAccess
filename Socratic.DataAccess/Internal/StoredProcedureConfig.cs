using System.Dynamic;
using Socratic.DataAccess.Abstractions;
using System.Linq;
using Socratic.DataAccess.Abstractions.Annotations;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Socratic.DataAccess.Exceptions;

namespace Socratic.DataAccess.Internal
{
    /// <summary>
    /// Represents the needed information to call a stored procedure including:
    ///     1. Name of stored procedure
    ///     2. Parameters object
    ///     3. Return type
    /// </summary>
    public class StoredProcedureConfig
    {
        private static readonly IDictionary<Type, IList<(string paramName, PropertyInfo property)>> parameterLookup = new Dictionary<Type, IList<(string paramName, PropertyInfo property)>>();

        private StoredProcedureConfig(string name, ExpandoObject parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        /// <summary>
        /// Name of the stored procedure
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// Dynamic object representing the parameters to pass into the stored procedure
        /// </summary>
        /// <value></value>
        public ExpandoObject Parameters { get; set; }

        /// <summary>
        /// Build the object to represent the provided stored procedure.
        /// </summary>
        /// <param name="storedProc"></param>
        /// <typeparam name="T">The type of the expected result from the stored procedure. This is inferred via the parameter.</typeparam>
        /// <returns cref="Socratic.DataAccess.Internal.StoredProcedureConfig"></returns>
        /// <exception cref="Socratic.DataAccess.Exceptions.ParameterSerializationException"></exception>
        public static StoredProcedureConfig Build<T>(IStoredProcedure<T> storedProc)
        {
            var attribute = GetStoredProcedureAttribute(storedProc);

            var procName = attribute?.Name ?? nameof(storedProc);

            var parameters = BuildParameters(storedProc, attribute);

            return new StoredProcedureConfig(procName, parameters);
        }

        private static StoredProcedureAttribute GetStoredProcedureAttribute<T>(IStoredProcedure<T> storedProc) => 
            storedProc.GetType()
                .GetCustomAttributes(false)
                .OfType<StoredProcedureAttribute>()
                .FirstOrDefault();

        private static ExpandoObject BuildParameters<T>(IStoredProcedure<T> storedProc, StoredProcedureAttribute attribute)
        {
            var result = new ExpandoObject();
            var procType = storedProc.GetType();

            var parameterInfo = GetParamList(storedProc.GetType());

            foreach (var (param, property) in parameterInfo)
                if (!result.TryAdd(param, property.GetValue(storedProc)))
                    throw new ParameterSerializationException(param);
            
            return result;
        } 

        private static IList<(string paramName, PropertyInfo property)> GetParamList(Type type)
        {
            if (parameterLookup.TryGetValue(type, out var parameters))
                return parameters;

            var paramList = new List<(string, PropertyInfo)>();

            foreach (var prop in type.GetProperties())
            {
                var attribute = prop.GetCustomAttributes()
                    .OfType<StoredProcedureParameterAttribute>()
                    .FirstOrDefault();
                
                paramList.Add((attribute?.Name ?? prop.Name, prop));
            }

            parameterLookup.Add(type, paramList);

            return paramList;
        }
    }
}