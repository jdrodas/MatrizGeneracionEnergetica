using mge.API.DbContexts;
using mge.API.Interfaces;
using mge.API.Repositories;
using mge.API.Services;
using mge.API.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ******************************************
// --- Configuración de la base de datos --
// ******************************************

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

var databaseSettings = builder.Configuration
    .GetSection("DatabaseSettings")
    .Get<DatabaseSettings>();

var pgsqlConnectionString = databaseSettings?.BuildConnectionString();    

//Agregar la cadena de conexión a la configuración
builder.Configuration["ConnectionStrings:mgePL"] = pgsqlConnectionString;

// ***********************************
// --- Configuración del DB Context --
// ***********************************

builder.Services.AddSingleton<PgsqlDbContext>();

// ****************************************
// --- Configuración de los repositorios --
// ****************************************

builder.Services.AddScoped<ITipoRepository, TipoRepository>();

// ************************************************
// --- Configuración de los servicios asociados  --
// ************************************************

builder.Services.AddScoped<TipoService>();

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
        Title = "MGE - Matriz Generación Energética - Versión en PostgreSQL",
        Description = "API para la gestión de Información sobre la Generación de Energía"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
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
