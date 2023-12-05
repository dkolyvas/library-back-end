using library_app.Data;
using library_app.Repositories;
using library_app.Services;
using library_app.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using static System.Net.WebRequestMethods;

namespace library_app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddDbContext<LibraryContext>(optionsAction => optionsAction.UseSqlServer(connectionString));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAppServices, AppServices>();
            builder.Services.AddAutoMapper(typeof(MapperConfig));
          

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                byte[] signingKeyBytes = Encoding.UTF8
                .GetBytes(builder.Configuration.GetValue<string>("AppSecurityKey")!);

                x.IncludeErrorDetails = true; ;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    /*ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false,
                    SignatureValidator = (token, validator) => { return new JwtSecurityToken(token); }*/
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:5001",
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                };
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

                var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
           
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("MyPolicy");

            app.MapControllers();

            app.Run();
        }
    }
}