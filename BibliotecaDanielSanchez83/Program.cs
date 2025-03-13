using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using BibliotecaDanielSanchez83.Context;
using BibliotecaDanielSanchez83.Services.IServices;
using BibliotecaDanielSanchez83.Services.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Agregar configuración de JWT desde appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

// Configurar autenticación con JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Account/Login";  // Ruta para el login
    options.LogoutPath = "/Account/Logout"; // Ruta para el logout
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);  // Expiración de sesión
});

builder.Services.AddAuthorization(options =>
{
    // Política para el rol de Administrador
    options.AddPolicy("Administrador", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador"));
    //Politica para el rol de usuario
    options.AddPolicy("Usuario", policy => policy.RequireClaim(ClaimTypes.Role, "Usuario"));
});

// Agregar servicios
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IUsuarioServices, UsuarioServices>();
builder.Services.AddTransient<ILibrosServices, LibrosServices>();
builder.Services.AddTransient<IAutorServices, AutorServices>();
builder.Services.AddScoped<IRolServices, RolServices>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar autenticación y autorización
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Login",
    pattern: "{controller=Account}/{action=Login}/{id?}");


app.MapControllerRoute(
    name: "Register",
    pattern: "{controller=Account}/{action=Register}/{id?}");

app.MapControllerRoute(
    name: "Usuarios",
    pattern: "{controller=Usuario}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Libros",
    pattern: "{controller=Libros}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "LibrosUser",
    pattern: "{controller=Libros}/{action=LibrosUser}/{id?}");

app.MapControllerRoute(
    name: "Autores",
    pattern: "{controller=Autor}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Rol",
    pattern: "{controller=Rol}/{action=Index}/{id?}");

app.Run();
