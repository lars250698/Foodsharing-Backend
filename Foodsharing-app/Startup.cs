using Foodsharing_app.Filters;
using Foodsharing_app.Models;
using Foodsharing_app.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Foodsharing_app
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
            RegisterDatabaseSettings(services);
            RegisterJwtSettings(services);

            RegisterServices(services);
            RegisterScopedServices(services);

            services.AddControllers();
        }

        private void RegisterScopedServices(IServiceCollection services)
        {
            services.AddScoped<UserAuthorizationFilter>();
            services.AddScoped<AdminAuthorizationFilter>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<DatabaseService>();
            services.AddSingleton<FoodItemService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<AuthorizationService>();
        }

        private void RegisterJwtSettings(IServiceCollection services)
        {
            services.Configure<FoodSharingJwtSettings>(
                Configuration.GetSection(nameof(FoodSharingJwtSettings)));
            services.AddSingleton<IFoodSharingJwtSettings>(sp =>
                sp.GetRequiredService<IOptions<FoodSharingJwtSettings>>().Value);
        }

        private void RegisterDatabaseSettings(IServiceCollection services)
        {
            services.Configure<FoodSharingDatabaseSettings>(
                Configuration.GetSection(nameof(FoodSharingDatabaseSettings)));
            services.AddSingleton<IFoodSharingDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<FoodSharingDatabaseSettings>>().Value);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();   
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}