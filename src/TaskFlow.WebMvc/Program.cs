using Microsoft.AspNetCore.Authentication.Cookies; // <--- NUEVO
using TaskFlow.Application;
using TaskFlow.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// --- CONFIGURACIÓN DE COOKIES ---
builder
    .Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Si no estás logueado, te manda aquí
        options.LogoutPath = "/Auth/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(30); // Duración de la sesión
        options.AccessDeniedPath = "/Home/Error?statusCode=403"; // Nuestra página 403
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// --- ACTIVAR AUTENTICACIÓN ---
app.UseAuthentication(); // <--- DEBE IR ANTES DE AUTHORIZATION
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
