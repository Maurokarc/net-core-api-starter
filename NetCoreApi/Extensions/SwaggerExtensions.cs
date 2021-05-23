using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",
                    name: $"{DbConstraint.DefaultIssuer} API v1.0.0"
                );
            });
        }

        public static void AddSwaggerDoc(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    name: "v1",
                    info: new OpenApiInfo
                    {
                        Title = DbConstraint.DefaultIssuer,
                        Version = "1.0.0",
                        Description = $"{DbConstraint.DefaultIssuer} API documentation"
                    }
                );
                c.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date-time" });

                string serverDoc = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{DbConstraint.DefaultNamespace}.xml");
                string frameworkDoc = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{DbConstraint.DefaultNamespace}.Infrastructure.xml");

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
