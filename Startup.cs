using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReadMeter.Api.Businesses;
using ReadMeter.Api.Configuration;
using ReadMeter.Api.Data;
using ReadMeter.Api.OracleModels;

namespace ReadMeter.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        Console.OutputEncoding = Encoding.UTF8;

        services.AddControllers().AddJsonOptions(ConfigureJson);
        services.AddHealthChecks();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API App Chỉ Số Nước",
                Version = "v1",
                Description = "Created by pthanhdng@gmail.com\n\nPhone: 0905006999",
                Contact = new OpenApiContact
                {
                    Name = "pthanhdng@gmail.com",
                    Email = "pthanhdng@gmail.com"
                }
            });
        });
        services.AddHttpClient();
        RegisterOracleContexts(services);
        RegisterDependencies(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(policy => policy
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials());
        app.UseAuthorization();
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/network");
            endpoints.MapFallbackToFile("index.html");
        });
    }

    private void ConfigureOracle(DbContextOptionsBuilder options, string connectionName) =>
        options.UseOracle(GetRequiredConnectionString(connectionName), oracle =>
            oracle.UseOracleSQLCompatibility("11"));

    private void RegisterOracleContexts(IServiceCollection services)
    {
        services.AddDbContext<ReadMeterDbContext>(options =>
            ConfigureOracle(options, "ReadMeterDatabase"));
        services.AddDbContext<BillingDbContext>(options =>
            ConfigureOracle(options, "BillingDatabase"));
    }

    private string GetRequiredConnectionString(string name)
    {
        var value = Configuration.GetConnectionString(name);
        return !string.IsNullOrWhiteSpace(value)
            ? RemoveUnsupportedOracleAttributes(value)
            : throw new InvalidOperationException(
                $"Chưa cấu hình ConnectionStrings:{name} trong appsettings hoặc biến môi trường.");
    }

    private static string RemoveUnsupportedOracleAttributes(string connectionString) =>
        string.Join(";",
            connectionString
                .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(part => !part.StartsWith("Unicode=", StringComparison.OrdinalIgnoreCase)));

    private static void RegisterDependencies(IServiceCollection services)
    {
        services.AddScoped<IReadMeterBusinesses, ReadMeterBusinesses>();
    }

    private static void ConfigureJson(JsonOptions options)
    {
        options.JsonSerializerOptions.PropertyNamingPolicy =
            LegacySnakeCaseNamingPolicy.Instance;
        options.JsonSerializerOptions.DictionaryKeyPolicy =
            LegacySnakeCaseNamingPolicy.Instance;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }
}
