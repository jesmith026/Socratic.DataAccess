using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Socratic.DataAccess.Abstractions;

namespace Socratic.DataAccess.DependencyInjection
{
    /// <summary>
    /// Methods used to assist in configuration of dependency injection container
    /// </summary>
    public static class Registration
    {
        /// <summary>
        /// Add standard configuration of the default implementations for Socratic Data Access
        /// </summary>
        /// <param name="services"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static IServiceCollection AddSocraticDataAccess<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IRepositoryFactory<TContext>, EfRepositoryFactory<TContext>>();
            services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();

            return services;
        } 
    }
}