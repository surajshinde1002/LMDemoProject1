
using Microsoft.EntityFrameworkCore;
using UserController.data;
using UserController.Services;

namespace UserController
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

            //builder.Services.AddDbContext<UserDbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("UserSqlConnection"));
            //});


            builder.Services.AddDbContext<UserDbContext>(options =>
            {
                options.UseMySQL(builder.Configuration.GetConnectionString("UserSqlConnection"));
            });


            builder.Services.AddTransient<IUserServices, UserServices>();

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

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
