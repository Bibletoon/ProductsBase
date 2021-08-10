using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using ProductsBase.Api.Mapping;
using ProductsBase.Api.Middlewares;
using ProductsBase.Api.Middlewares.Filters;
using ProductsBase.Data.Contexts;
using ProductsBase.Data.Repositories;
using ProductsBase.Data.Repositories.Interfaces;
using ProductsBase.Domain.Services;
using ProductsBase.Domain.Services.Interfaces;

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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.AddDbContext<AppDbContext>(o => { o.UseInMemoryDatabase("my-app-db"); });

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
                                   {
                                       c.SwaggerDoc(
                                           "v1", new OpenApiInfo { Title = "ProductsBase.Api", Version = "v1" });
                                   });

            services.AddMvc(opts => { opts.Filters.Add(typeof(ModelStateFilter)); });

            services.AddAutoMapper(typeof(ModelToResourceProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductsBase.Api v1"));
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