
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
using TaxiDispacher.Extensions;
using TaxiDispacher.Filters;
using TaxiDispacher.Services;
using TaxiDispacher.Services.Hosted;

namespace TaxiDispacher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => {
                    builder.WithOrigins("https://one.lt")
                           .AllowDefaultMethodsExtension("PATCH")
                           .AllowAnyHeader();
                });
            });

            var defaultCulture = new CultureInfo("en-US");
            var supportedCultures = new[]
            {
                defaultCulture,
                new CultureInfo("lt")
            };

            // Localization
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.Configure<RequestLocalizationOptions>(options => {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture.CompareInfo.Name);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            // Add services to the container.
            builder.Services.AddDataAccess();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IDriverService, DriverService>();
            builder.Services.AddSingleton<IOrderService, OrderService>();

            builder.Services.AddHostedService<DispatchOrderService>();
       
            builder.Services.AddMvc(options => {
                options.Filters.Add(typeof(IPWhitelistFilter));
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            var app = builder.Build();

            var scope = app.Services.CreateScope();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();


            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.MapControllers();
            app.Run();
        }
    }
}