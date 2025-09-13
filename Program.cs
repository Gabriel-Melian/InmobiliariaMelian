
using Microsoft.AspNetCore.Authentication.Cookies;//Para autenticacion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Autenticacion con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login";//Si no esta logueado va a esta ruta
        options.LogoutPath = "/Usuario/Logout";
        options.AccessDeniedPath = "/Home/AccesoDenegado";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();//Tambien necesario

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Puede estar autenticado pero NO Autorizado
app.UseAuthentication();//Tambien para autenticacion, antes del UseAuthorization
//Aca se autoriza el usuario (Tema de seguridad y permisos)
app.UseAuthorization();//Tambien necesario

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");
    //pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
