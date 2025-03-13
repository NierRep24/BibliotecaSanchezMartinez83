using BibliotecaDanielSanchez83.Models.Domain;
using BibliotecaDanielSanchez83.Services.IServices;
using BibliotecaDanielSanchez83.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BibliotecaDanielSanchez83.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsuarioServices _usuarioServices;
        private readonly IRolServices _rolServices;
        private readonly IJwtService _jwtService;

        public AccountController(IUsuarioServices usuarioServices, IRolServices rolServices, IJwtService jwtService)
        {
            _usuarioServices = usuarioServices;
            _rolServices = rolServices;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Muestra la vista de registro
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View(new UsuarioRegisterViewModel());
        }

        /// <summary>
        /// Registrar usuario
        /// </summary>
        [HttpPost]
        public IActionResult Register(UsuarioRegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rolUsuario = _rolServices.ObtenerRoles().FirstOrDefault(r => r.Nombre == "Usuario");
            if (rolUsuario == null)
                return BadRequest(new { message = "El rol 'Usuario' no existe." });

            var usuario = new Usuario
            {
                Nombre = model.Nombre,
                UserName = model.UserName,
                Password = model.Password,
                FkRol = rolUsuario.PkRol
            };

            bool creado = _usuarioServices.CrearUsuario(usuario);
            if (!creado)
                return BadRequest(new { message = "Hubo un problema al crear el usuario." });

            return RedirectToAction("Login");
        }

        /// <summary>
        /// Muestra la vista de login
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            return View(new UsuarioLoginViewModel());
        }

        /// <summary>
        /// Iniciar sesión
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UsuarioLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _usuarioServices.GetByUserNameAsync(model.UserName);
            if (usuario == null)
                return Unauthorized(new { message = "Usuario no encontrado" });

            // Verificación de la contraseña con BCrypt
            var result = BCrypt.Net.BCrypt.Verify(model.Password, usuario.Password);
            if (!result)
                return Unauthorized(new { message = "Credenciales incorrectas" });

            var token = _jwtService.GenerateToken(usuario);

            Response.Cookies.Append("JwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            });

            switch (usuario.Roles?.Nombre)
            {
                case "Administrador":
                    return RedirectToAction("Index", "Usuario");

                case "Usuario":
                    return RedirectToAction("LibrosUser", "Libros");

                default:
                    return RedirectToAction("Index", "Home");
            }
        }


        /// <summary>
        /// Cerrar sesión
        /// </summary>
        public IActionResult Logout()
        {
            // Eliminar el token de las cookies
            Response.Cookies.Delete("JwtToken");
            //TempData.Remove("Token");
            return RedirectToAction("Login", "Account");
        }

    }
}
