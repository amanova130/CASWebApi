using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models.DbModels;
using CASWebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace CASWebApi
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
            services.AddMvc();
            //Services:
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IFacultyService, FacultyService>();
            services.AddScoped<IExamService, ExamService>();


            services.AddSingleton<BookService>();


            //Configuration for Mongo Db
            //services.Configure<DbSettings>(
            //Configuration.GetSection(nameof(DbSettings)));
            //services.AddSingleton<IDbSettings>(sp =>
            //    sp.GetRequiredService<IOptions<DbSettings>>().Value);
            services.Configure<Settings>(options =>
            {
                options.ConnectionString
                    = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database
                    = Configuration.GetSection("MongoConnection:DatabaseName").Value;
            });
            services.AddSingleton<IDbSettings, DbSettings>();

            //Adding Swagger Generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "College Administration",
                    Description ="College system administration"
                });
            });

            services.AddControllers()
                .AddNewtonsoftJson(options => options.UseMemberCasing());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c=> 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CasWebApi");
                c.RoutePrefix = string.Empty;
            });

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
