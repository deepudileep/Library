using Library.Grpc;
using LibraryApis.Data;
using LibraryApis.Serivices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// EF Core SQLite
builder.Services.AddDbContext<LibraryContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")
    ?? "Data Source=library.db"));

// gRPC server
builder.Services.AddGrpc();

// REST API calls to gRPC internally
builder.Services.AddGrpcClient<LibraryService.LibraryServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:7145");
});

// REST API + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Create & seed database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    db.Database.EnsureCreated();
    Seeder.Seed(db);
}

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Map REST API + gRPC
app.MapControllers();
app.MapGrpcService<LibraryServiceImpl>();

// Default root
app.MapGet("/", () => "Library API + gRPC service running");

app.Run();
