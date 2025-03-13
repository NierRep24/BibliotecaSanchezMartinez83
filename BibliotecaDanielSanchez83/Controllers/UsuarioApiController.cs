using BibliotecaDanielSanchez83.Models.Domain;
using BibliotecaDanielSanchez83.Services.IServices;
using BibliotecaDanielSanchez83.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaDanielSanchez83.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    public class UsuarioApiController : ControllerBase
    {
        private readonly IUsuarioServices _usuarioServices;
        private readonly IJwtService _jwtService;

        public UsuarioApiController(IUsuarioServices usuarioServices, IJwtService jwtService)
        {
            _usuarioServices = usuarioServices;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Registro de usuario (API)
        /// </summary>
        [HttpPost("registro")]
        public IActionResult Register([FromBody] UsuarioRegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = new Usuario
            {
                Nombre = model.Nombre,
                UserName = model.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                FkRol = model.FkRol
            };

            bool creado = _usuarioServices.CrearUsuario(usuario);

            if (!creado)
                return BadRequest(new { message = "No se pudo registrar el usuario." });

            return Ok(new { message = "Usuario registrado con éxito" });
        }

        /// <summary>
        /// Autenticación de usuario (API)
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Usamos 'await' para obtener el usuario de manera asincrónica
            var usuario = await _usuarioServices.GetByUserNameAsync(model.UserName);

            // Verificamos si el usuario es nulo o si la contraseña no es correcta
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuario.Password))
                return Unauthorized(new { message = "Credenciales incorrectas" });

            var token = _jwtService.GenerateToken(usuario);
            return Ok(new { token });
        }


    }
}
