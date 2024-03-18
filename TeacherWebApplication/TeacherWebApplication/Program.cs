
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using TeacherWebApplication.Data;
using TeacherWebApplication.Models.EntityModels;
using TeacherWebApplication.Services;

namespace TeacherWebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddDbContext<TeacherDbContext>(option =>
            //{
            //    option.UseSqlServer(builder.Configuration.GetConnectionString("teacherDb"));
            //});

            builder.Services.AddDbContext<TeacherDbContext>(options =>
            {
                options.UseMySQL(builder.Configuration.GetConnectionString("conn"));
            });

            builder.Services.AddTransient<ITeacherService, TeacherService>();

            builder.Services.AddCors(option =>
            {
                option.AddPolicy("pol", pol =>
                {
                    pol.AllowAnyHeader();
                    pol.AllowAnyMethod();
                    pol.AllowAnyOrigin();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("pol");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
