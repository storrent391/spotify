using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Spotify.Services;
using Spotify.Endpoints;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configuració JSON
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
DatabaseConnection dbConn = new DatabaseConnection(connectionString);

WebApplication app = builder.Build();

// Registrar endpoints
app.MapUserEndpoints(dbConn);
app.MapSongEndpoints(dbConn);
app.MapPlaylistEndpoints(dbConn);

app.Run("http://localhost:5000");