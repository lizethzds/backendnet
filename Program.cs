using Microsoft.EntityFrameworkCore;
using backendnet.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DataContext");

builder.Services.AddDbContext<DataContext>( options =>
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

    }

);

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();

