using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreApi.Extensions;

namespace NetCoreApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(p => p.JsonSerializerOptions.IgnoreNullValues = true)
                .AddXmlDataContractSerializerFormatters();

            services.ConfigureCors();
            services.AddLogger();
            services.ConfigureApi();
            services.ConfigureInfrastructure();
            services.AddApiVersion();
            services.AddSwaggerDoc();
            services.SetJwtAuth();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // Must be first
            app.SetMiddlewares();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.AddSwagger();
            app.UseCors(Constraint.PolicyKey);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
