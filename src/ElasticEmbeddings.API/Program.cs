using ElasticEmbeddings.API;
using ElasticEmbeddings.API.Endpoints;
using ElasticEmbeddings.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.AddDocumentEndpoints();
app.AddSearchEndpoints();

await app.Services.MigrateAsync();

app.Run();