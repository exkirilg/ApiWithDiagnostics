using ApiWithDiagnostics;
using ApiWithDiagnostics.DbAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.ConfigureHttpClients();
builder.ConfigureLogging();

builder.Services.AddScoped<IDbAccess, DbAccess>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.ConfigureApi();

await PopulateDatabase(app.Services.GetRequiredService<IConfiguration>());

app.Run();

async Task PopulateDatabase(IConfiguration config)
{
    var dbAccess = new DbAccess(config);
    await dbAccess.CreateQuotesTable();
    await dbAccess.PopulateQuotes();
}
