using Serilog;
using Serilog.Formatting.Compact;
using Squares.Application;
using Squares.Host.Configurations;
using Squares.Host.Controllers;
using Squares.Infrastructure;
using Squares.Infrastructure.Common;
using Squares.Infrastructure.Logging.Serilog;
using Newtonsoft.Json.Converters;
using Squares.Application.Common.Utility;

[assembly: ApiConventionType(typeof(ApiConventions))]

StaticLogger.EnsureInitialized();
Log.Information("Server starting...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddConfigurations().RegisterSerilog();
    builder.Services.AddControllers()
        .AddNewtonsoftJson(o =>
        {
            o.SerializerSettings.Converters.Add(new StringEnumConverter());
            o.SerializerSettings.Converters.Add(new DateOnlyConverter());
            o.SerializerSettings.Converters.Add(new TimeOnlyConverter());
        });

    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    var app = builder.Build();

    await app.Services.InitializeDatabasesAsync();

    app.UseInfrastructure(builder.Configuration);
    app.MapEndpoints();
    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}