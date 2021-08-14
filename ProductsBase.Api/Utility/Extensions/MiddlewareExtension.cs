using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ProductsBase.Api.Utility.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(cfg =>
                                   {
                                       cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                                       {
                                           In = ParameterLocation.Header,
                                           Description = "JSON Web Token to access resources. Example: Bearer {token}",
                                           Name = "Authorization",
                                           Type = SecuritySchemeType.ApiKey
                                       });

                                       cfg.AddSecurityRequirement(new OpenApiSecurityRequirement
                                       {
                                           {
                                               new OpenApiSecurityScheme
                                               {
                                                   Reference = new OpenApiReference
                                                       { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                                               },
                                               new[] { string.Empty }
                                           }
                                       });
                                   });

            return services;
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger().UseSwaggerUI(options =>
                                          {
                                              options.SwaggerEndpoint("/swagger/v1/swagger.json", "Products Base API");
                                              options.DocumentTitle = "Products Base API";
                                          });

            return app;
        }
    }
}