using Application;
using Application.DbServices;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Common;
using Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Persistence.Models;
using RTLTechTask.Extensions;

namespace TvMazeScraper
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
            services.AddControllers();
            services.AddHttpClient<IHttpClient, TvMazeHttpClient>();
            services.AddTransient<IScraperService, TvMazeService>();
            services.AddTransient<IDbService<Show>, ShowDbService >();
            services.AddTransient<IDbService<Actor>, ActorDbService >();
            services.AddTransient<IDbService<ActorShow>, CastDbService >();
            services.AddTransient<IShowService<ShowModel>, ShowService>();
            services.AddTransient<IShowCastService, ShowCastService>();
            services.AddAutoMapper(typeof(TvMazeService));

            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddJsonOptions(options => 
                    options.JsonSerializerOptions.IgnoreNullValues = true);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext context)
        {
            if (env.IsDevelopment())
            {
                context.Database.EnsureCreated();
                app.UseDeveloperExceptionPage();
            }
            
            app.ConfigureCustomExceptionMiddleware();
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}