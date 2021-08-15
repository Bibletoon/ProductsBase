using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ProductsBase.Data.Seeding;
using ProductsBase.Api.Mapping;
using ProductsBase.Api.Middlewares;
using ProductsBase.Api.Middlewares.Filters;
using ProductsBase.Api.Utility.Extensions;
using ProductsBase.Data.Contexts;
using ProductsBase.Domain.Security.Hashing;
using ProductsBase.Domain.Security.Tokens;
using ProductsBase.Domain.Services;
using ProductsBase.Domain.Services.Interfaces;
using TokenHandler = ProductsBase.Domain.Security.Tokens.TokenHandler;

namespace ProductsBase.Api
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
            services.AddDbContext<AppDbContext>(o =>
                                                {
                                                    o.UseInMemoryDatabase("products-base-db");
                                                });

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ITokenHandler, TokenHandler>();
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            var signingConfigurations = new SigningConfigurations(tokenOptions.Secret);
            services.AddSingleton(signingConfigurations);

            services.AddSingleton<DatabaseSeeder>();
            
            services.AddControllers();
            services.AddCustomSwagger();

            services.AddMvc(opts => { opts.Filters.Add(typeof(ModelStateFilter)); });
            services.Configure<TokenOptions>(Configuration.GetSection("TokenOptions"));
            services.AddAutoMapper(typeof(ModelToResourceProfile));
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(jwtBearerOptions =>
                                 {
                                     jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                                     {
                                         ValidateAudience = true,
                                         ValidateLifetime = true,
                                         ValidateIssuerSigningKey = true,
                                         ValidIssuer = tokenOptions.Issuer,
                                         ValidAudience = tokenOptions.Audience,
                                         IssuerSigningKey = signingConfigurations.SecurityKey,
                                         ClockSkew = TimeSpan.Zero
                                     };
                                 });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCustomSwagger();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<RequestLoggingMiddleware>();


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}