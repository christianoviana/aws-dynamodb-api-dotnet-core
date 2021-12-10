using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MovieRank.Api.Mapper;
using MovieRank.Api.Middleware;
using MovieRank.Api.Services;
using MovieRank.Domains.Gateways;
using MovieRank.Domains.Interfaces;
using MovieRank.Infra.Repository;
using System;
using System.IO;
using System.Reflection;

namespace MovieRank.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {         
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddDefaultAWSOptions(new AWSOptions
            {
                Region = RegionEndpoint.GetBySystemName(Configuration["AWS:Region"]),
                Profile = Configuration["AWS:Profile"],
                ProfilesLocation = Configuration["AWS:ProfilesLocation"]
            });
            services.AddSingleton<IMovieRankService, MovieRankService>();
            services.AddSingleton<IMovieRankGateway, MovieRankRepository>();
            services.AddAutoMapper(typeof(MapperProfile));
            
            services.AddControllers();
            services.AddSwaggerGen(o => 
            {
                o.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Movie Rank Api",
                    Description = "Movie Api Storing Data in AWS DynamoDB"
                });

                //Set swagger description documentation
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(o =>
                {
                    o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    o.RoutePrefix = string.Empty;
                });
            }

            app.UseMiddleware<ExceptionHandler>();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
