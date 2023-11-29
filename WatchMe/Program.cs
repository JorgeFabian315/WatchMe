using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WatchMe.Models.Entities;
using WatchMe.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

builder.Services.AddTransient<WatchMe.Repositories.Repository<Genero>>();
builder.Services.AddTransient<WatchMe.Repositories.Repository<Plataforma>>();
builder.Services.AddTransient<WatchMe.Repositories.Repository<Clasificacion>>();
builder.Services.AddTransient<WatchMe.Repositories.Repository<Participacion>>();
builder.Services.AddTransient<ActoresRepository>();
builder.Services.AddTransient<PeliculasRepository>();

builder.Services.AddDbContext<WatchMeContext>(
   optionsBuilder =>
   optionsBuilder.UseMySql("database=WatchMe;user=root;server=localhost;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql")));



var app = builder.Build();

app.UseFileServer();

app.UseStaticFiles();

app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapDefaultControllerRoute();

app.Run();
