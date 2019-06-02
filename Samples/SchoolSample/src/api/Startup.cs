using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Socratic.DataAccess.DependencyInjection;

namespace DockerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SchoolContext>(options =>
                options.UseSqlServer(@"Data Source=db;Initial Catalog=School;User Id=SA;Password=P@ssw0rd"));
            
            services.AddSocraticDataAccess<SchoolContext>();

            services.AddControllers()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UpdateDb();
            }           

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }        
    }    

    static class Extensions
        {
            public static void UpdateDb(this IApplicationBuilder app)
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var provider = scope.ServiceProvider;
                    var db = provider.GetService<SchoolContext>();
                    if (db.Database.GetPendingMigrations().Any())
                        db.Database.Migrate();

                    if (!db.Students.Any())
                    {
                        for (var i = 0; i < 20; i++)
                        {
                            var student = new Student { Name = Guid.NewGuid().ToString() };
                            db.Students.Add(student);
                        }
                        db.SaveChanges();
                    }
                }
            }
        }
}
