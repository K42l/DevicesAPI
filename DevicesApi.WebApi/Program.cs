using System.Text.Json.Serialization;
using DevicesApi.Application;
using DevicesApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
               

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.");
builder.Services.AddDbContextFactory<DeviceDbContext>(options =>
    options.UseSqlite(connectionString, x => x.MigrationsAssembly("DevicesApi.WebApi")), ServiceLifetime.Scoped);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();

builder.Services.AddOpenApi();

var app = builder.Build();

//I added this to apply migrations automatically on startup if the application can't connect to the database.
//This is simply to showcase the application without requiring manual migration steps.
//I don't think this is a good idea for a production application. Migrations should be handled very carefully in production environments.
using (var scope = app.Services.CreateScope())
{
    using (var db = scope.ServiceProvider.GetRequiredService<DeviceDbContext>())
    {
        if (!db.Database.CanConnect())
            db.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("docs", options =>
    {
        options.WithOpenApiRoutePattern("/openapi/{documentName}.json")
                .Title = "DEVICES API";
    });
    app.Map("/", () => Results.Redirect("/docs"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
