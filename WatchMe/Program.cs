using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WatchMe.Models.Entities;
using WatchMe.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

builder.Services.AddTransient<WatchMe.Repositories.Repository<Genero>>();
builder.Services.AddTransient<WatchMe.Repositories.Repository<Plataforma>>();
builder.Services.AddTransient<WatchMe.Repositories.Repository<Usuario>>();
builder.Services.AddTransient<WatchMe.Repositories.Repository<Clasificacion>>();
builder.Services.AddTransient<WatchMe.Repositories.Repository<Reseņa>>();
builder.Services.AddTransient<ReseņaRepositorio>();
builder.Services.AddTransient<WatchMe.Repositories.Repository<Participacion>>();
builder.Services.AddTransient<ActoresRepository>();
builder.Services.AddTransient<PeliculasRepository>();

builder.Services.AddDbContext<WatchMeContext>(
   optionsBuilder =>
   optionsBuilder.UseMySql("database=websitos_WatchMe;user=websitos_AdminWatchMe;server=websitos256.com;password=WatchMe2023", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(x =>
    {
        x.AccessDeniedPath = "/Home/Denied";
        x.LoginPath = "/Login";
        x.LogoutPath = "/Home/Logout";
        x.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        x.Cookie.Name = "FruteriaCookie";
    });

var app = builder.Build();

app.UseFileServer();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapDefaultControllerRoute();

app.Run();
