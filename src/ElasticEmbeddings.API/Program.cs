using ElasticEmbeddings.API;
using ElasticEmbeddings.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddDocumentEndpoints();
app.AddSearchEndpoints();

app.Run();