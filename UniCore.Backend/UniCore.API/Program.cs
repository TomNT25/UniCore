using UniCore.API.Environment;
using UniCore.API.Extensions;
using UniCore.API.Logging;
using UniCore.API.Middlewares;
using UniCore.Application;
using UniCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace UniCore.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        CustomAdoNetAppender.ConnectionStringOverride = builder.Configuration["Database:ConnectionString"];

        builder.Logging.ClearProviders();

        builder.Logging.AddLog4Net("log4net.config");

        builder.Services.AddAPIServices(builder.Configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDevelopmentSwagger();
        }

        app.ConfigureMiddlewarePipeline(builder.Environment);

        app.Run();
    }
}