using VaultHelpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

// get vault address and vault token from the app settings
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

var VaultSection = configuration.GetSection("Vault");

var vaultAddress = VaultSection["VaultAddress"];
var vaultToken = VaultSection["VaultToken"];

// if vault address or vault token is not provided, throw an exception
if (vaultToken == null)
{
    throw new Exception("Vault token is not provided");
}
var vault = new VaultHelper(vaultAddress, vaultToken);

app.MapGet("secret/{path}", (string path) =>
{
    var readSecrets = vault.GetSecrets(path);
    return readSecrets;
}).WithName("Secrets")
    .WithOpenApi();

app.MapGet("secret/{path}/{key}", (string path, string key) =>
{
    var readSecrets = vault.GetSecrets(path);
    return readSecrets[key];
}).WithOpenApi();

app.Run();