using ApiWithDiagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureHttpClients();
builder.ConfigureLogging();

var app = builder.Build();

app.UseHttpsRedirection();

app.ConfigureApi();

app.Run();
