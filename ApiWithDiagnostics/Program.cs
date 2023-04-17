using ApiWithDiagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureHttpClients();

var app = builder.Build();

app.UseHttpsRedirection();

app.ConfigureApi();

app.Run();
