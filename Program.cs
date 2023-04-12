using DataAccess;
using Microsoft.Extensions.Options;
using Serilog;
using TaxiDispacher.Extensions;
using TaxiDispacher.Filters;
using TaxiDispacher.Services;
using TaxiDispacher.Services.Hosted;

namespace TaxiDispacher;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDataAccess();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddLogging(loggingBuilder => {
             loggingBuilder.ClearProviders();
             loggingBuilder.AddSerilog(new LoggerConfiguration()
                .MinimumLevel
                .Debug()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console( outputTemplate: "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
                .CreateLogger());
         });
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<IDriverService, DriverService>();
        builder.Services.AddSingleton<IOrderService, OrderService>();
        builder.Services.AddSingleton<INotificationsService, NotificationsService>();
        builder.Services.AddTransient<ISocketClientService, SocketClientService>();

        builder.Services.AddHostedService<DispatchOrderService>();
        builder.Services.AddHostedService<ApacheKafkaConsumerService>();
   
        builder.Services.AddMvc(options => {
            options.Filters.Add(typeof(IPWhitelistFilter));
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddJwt(builder.Configuration);
        builder.Services.AddL10n();
        
     
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