using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace NetCoreApi.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwagger(this IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;
            var provider = serviceProvider.GetService<IApiVersionDescriptionProvider>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint(url: $"/swagger/{description.GroupName}/swagger.json",
                         name: $"{Constraint.DefaultIssuer} API {description.GroupName}");
                }
            });
        }

        public static void AddSwaggerDoc(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var provider = serviceProvider.GetService<IApiVersionDescriptionProvider>();

            services.AddSwaggerGen(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(
                        name: description.GroupName,
                        info: new OpenApiInfo
                        {
                            Title = Constraint.DefaultIssuer,
                            Version = description.ApiVersion.ToString(),
                            Description = $"{Constraint.DefaultIssuer} API documentation"
                        });
                }

                c.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date-time" });

                string serverDoc = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Constraint.DefaultNamespace}.xml");
                string frameworkDoc = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Constraint.DefaultNamespace}.Infrastructure.xml");

                if (File.Exists(serverDoc))
                {
                    c.IncludeXmlComments(serverDoc);
                }

                if (File.Exists(frameworkDoc))
                {
                    c.IncludeXmlComments(frameworkDoc);
                }

                OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                {
                    Name = JwtBearerDefaults.AuthenticationScheme,
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(),//should be lower case 
                    BearerFormat = "JWT",
                    Description = "Please enter into by JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                };
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityDefinition);

                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, Array.Empty<string>() } });
            });
        }
    }
}
