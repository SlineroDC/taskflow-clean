using TaskFlow.Application;
using TaskFlow.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Inyectamos la Infraestructura (DbContext, Repositorios)
builder.Services.AddInfrastructure(builder.Configuration);

// 2. Inyectamos la Aplicación (Casos de Uso, Servicios)
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); 

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default", 
    pattern: "{controller=Projects}/{action=Index}/{id?}"); // Cambiamos la ruta por defecto a Projects

app.Run();