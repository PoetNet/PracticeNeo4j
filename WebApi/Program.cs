using Neo4jClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var client = new BoltGraphClient(new Uri("bolt://localhost:7687"), "neo4j", "root");
client.ConnectAsync().Wait();

builder.Services.AddSingleton<IGraphClient>(client);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
