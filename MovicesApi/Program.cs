using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MovicesApi.Models;

namespace MovicesApi
{
    public class Program { 
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options=>
            options.UseSqlServer(connectionstring)
            );
            builder.Services.AddControllers();

            builder.Services.AddCors(); 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo {
                
                    Version = "v1",
                    Title = "TestApi",
                    Description="My first api ",
                    TermsOfService=new Uri("https://www.google.com"),
                    Contact=new OpenApiContact
                    {
                        Name="Salah", 
                        Email="test@domain.com",
                        Url= new Uri("https://www.google.com"), 
                    },
                    License=new OpenApiLicense
                    {
                        Name="My Licence", 
                        Url = new Uri("https://www.google.com"),
                    }
                });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name="Authorization",
                    Type=SecuritySchemeType.ApiKey,
                    Scheme="Bearer",
                    BearerFormat="JWT",
                    In=ParameterLocation.Header, 
                    Description="Enter you JWT Key "
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer",
                            },
                            Name="Bearer",
                              In=ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                }) ;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(c=>c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}