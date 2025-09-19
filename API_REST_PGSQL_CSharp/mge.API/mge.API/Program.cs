using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using mge.API.DbContexts;
using mge.API.Interfaces;
using mge.API.Models;
using mge.API.Repositories;
using mge.API.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ***************************************************************************
// --- Configuración de la base de datos --
// ***************************************************************************

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

var databaseSettings = builder.Configuration
    .GetSection("DatabaseSettings")
    .Get<DatabaseSettings>();

var pgsqlConnectionString = databaseSettings?.BuildConnectionString();

//Agregar la cadena de conexión a la configuración
builder.Configuration["ConnectionStrings:mgePL"] = pgsqlConnectionString;

// ***************************************************************************
// --- Configuración del DB Context --
// ***************************************************************************

builder.Services.AddSingleton<PgsqlDbContext>();

// ***************************************************************************
// --- Configuración de los repositorios --
// ***************************************************************************

builder.Services.AddScoped<ITipoRepository, TipoRepository>();
builder.Services.AddScoped<IPlantaRepository, PlantaRepository>();
builder.Services.AddScoped<IUbicacionRepository, UbicacionRepository>();
builder.Services.AddScoped<IProduccionRepository, ProduccionRepository>();
builder.Services.AddScoped<IEstadisticaRepository, EstadisticaRepository>();

// ***************************************************************************
// --- Configuración de los servicios asociados  --
// ***************************************************************************

builder.Services.AddScoped<TipoService>();
builder.Services.AddScoped<PlantaService>();
builder.Services.AddScoped<UbicacionService>();
builder.Services.AddScoped<ProduccionService>();
builder.Services.AddScoped<EstadisticaService>();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "mge.API v1 - PostgreSQL",
        Description = "API para la gestión de Información sobre Matriz Generación Energética"
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "mge.API v2 - PostgreSQL",
        Description = "API para la gestión de Información sobre Matriz Generación Energética"
    });
});

// ***************************************************************************
// --- Configuración del versionamiento para el API  --
// ***************************************************************************

builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new HeaderApiVersionReader("api-version"),
            new QueryStringApiVersionReader("api-version")
        );
    }
)
    .AddMvc()
    .AddApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(config =>
        {
            // Configuración manual de cada endpoint
            config.SwaggerEndpoint("/swagger/v1/swagger.json", "mge.API v1");
            config.SwaggerEndpoint("/swagger/v2/swagger.json", "mge.API v2");
        }
    );
}

//Modificamos el encabezado de las peticiones para ocultar el web server utilizado
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Server", "MGEServer");
    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
