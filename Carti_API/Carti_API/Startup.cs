﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carti_API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Carti_API
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(o => o.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            var connectionString = Configuration["connectionStrings:bookDbConnectionString"];
            services.AddDbContext<BookDbContext>(c => c.UseSqlServer(connectionString));

            services.AddScoped<ITaraRepository, TaraRepository>();
            services.AddScoped<ICategorieRepository, CategorieRepository>();
            services.AddScoped<IReviewerRepository,ReviewerRepository>();
            services.AddScoped<IReviewRepository,ReviewRepository>();
            services.AddScoped<IAutorRepository,AutorRepository>();
            services.AddScoped<IBookRepository,BookRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,BookDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //context.SeedDataContext();
            app.UseMvc();
        }
    }
}
