using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Reflection;

namespace CqrsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IQueryProcessor, Orchestrator>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CqrsApi", Version = "v1" });
            });

            services.Scan(scan => scan
                  .FromCallingAssembly()
                  .AddClasses(classes => classes.Where(type =>
                  {
                      var allInterfaces = type.GetInterfaces();
                      var handlers = new[]
                      {
                        typeof(IQueryHandler<,>),
                      }.ToList();

                      return handlers
                          .Any(x => allInterfaces
                              .Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == x));
                  }))
                  .AsSelfWithInterfaces()
                  .WithTransientLifetime());

            //The same as 
            //services
            //    .AddTransient<IQueryHandler<GetSomethingQuery, GetSomethingQueryResponse>, GetSomethingQueryHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CqrsApi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
