using BibliotecaDanielSanchez83.Context;
using BibliotecaDanielSanchez83.Models.Domain;
using BibliotecaDanielSanchez83.Services.IServices;
using BibliotecaDanielSanchez83.Services.Services;
using BibliotecaDanielSanchez83.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BibliotecaDanielSanchez83.Controllers
{
    public class LibrosController : Controller
    {
        private readonly ILibrosServices _librosServices;
        private readonly IAutorServices _autorServices;
        private readonly ICategoriaService _categoriaServices;
        private readonly IUsuarioServices _usuarioServices;
        private readonly ApplicationDbContext _context;

        public LibrosController(ILibrosServices librosServices, IAutorServices autorServices, ICategoriaService categoriaServices, IUsuarioServices usuarioServices, ApplicationDbContext context)
        {
            _librosServices = librosServices;
            _autorServices = autorServices;
            _categoriaServices = categoriaServices;
            _usuarioServices = usuarioServices;
            _context = context;
        }

        // [Authorize(Roles = "Usuario")]  
        public IActionResult LibrosUser(string orden)
        {

            IEnumerable<Libro> libros;

            if (orden == "reciente")
            {
                libros = _librosServices.GetLibrosNew();
            }
            else if (orden == "antiguo")
            {
                libros = _librosServices.GetLibrosOld();
            }
            else
            {
                libros = _librosServices.GetLibrosNew(); // Por defecto, más recientes
            }

            return View(libros);
        }

        public IActionResult Index()
        {
            var token = Request.Cookies["JwtToken"];

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var userName = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var usuarioAutenticado = _usuarioServices.GetByUserNameAsync(userName);

            switch (role)
            {
                case "Administrador":
                    var libros = _librosServices.ObtenerLibros();

                    // Si no hay libros, regresar una lista vacía
                    if (libros == null || !libros.Any())
                    {
                        return View(new List<Libro>());
                    }

                    // Lógica de relacion Libros-categoria
                    foreach (var libro in libros)
                    {
                        if (libro.LibroCategorias == null)
                        {
                            libro.LibroCategorias = new List<LibroCategoria>();
                        }
                    }

                    return View(libros);

                case "Usuario":
                    return RedirectToAction("LibrosUser", "Libros");

                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Crear()
        {
            var autores = _autorServices.ObtenerAutores();
            var categorias = _categoriaServices.ObtenerCategoria();

            ViewBag.Autores = new SelectList(autores, "PkAutor", "Nombre");
            ViewBag.Categorias = new SelectList(categorias, "PkCategoria", "Nombre");
            return View();
        }

        [HttpPost]
        public IActionResult Crear(LibroCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var libro = new Libro
                {
                    Titulo = viewModel.Titulo,
                    FkAutor = viewModel.FkAutor,
                    FechaPublicacion = viewModel.FechaPublicacion,
                    Descripcion = viewModel.Descripcion
                };

                _context.Libros.Add(libro);
                _context.SaveChanges();

                if (viewModel.CategoriaIds != null && viewModel.CategoriaIds.Any())
                {
                    foreach (var categoriaId in viewModel.CategoriaIds)
                    {
                        var libroCategoria = new LibroCategoria
                        {
                            PkLibro = libro.PkLibro,
                            PkCategoria = categoriaId
                        };

                        _context.LibroCategorias.Add(libroCategoria);
                    }

                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            // Si hay errores, regresa con los datos
            var autores = _autorServices.ObtenerAutores();
            var categorias = _categoriaServices.ObtenerCategoria();
            ViewBag.Autores = new SelectList(autores, "PkAutor", "Nombre", viewModel.FkAutor);
            ViewBag.Categorias = new SelectList(categorias, "PkCategoria", "Nombre", viewModel.CategoriaIds);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var libro = _librosServices.GetLibroById(id);
            if (libro == null)
            {
                return NotFound();
            }

            var categorias = _categoriaServices.ObtenerCategoria();
            var autores = _autorServices.ObtenerAutores();

            var libroViewModel = new LibroCreateViewModel
            {
                PkLibro = libro.PkLibro,
                Titulo = libro.Titulo,
                FkAutor = libro.FkAutor,
                FechaPublicacion = libro.FechaPublicacion,
                Descripcion = libro.Descripcion,
                CategoriaIds = libro.LibroCategorias?.Select(c => c.PkCategoria).ToList() ?? new List<int>(),
            };

            // Pasar las categorías y autores a la vista mediante ViewBag
            ViewBag.Categorias = new SelectList(categorias, "PkCategoria", "Nombre", libroViewModel.CategoriaIds);
            ViewBag.Autores = new SelectList(autores, "PkAutor", "Nombre", libroViewModel.FkAutor);

            return View(libroViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(LibroCreateViewModel libroViewModel)
        {
            if (ModelState.IsValid)
            {
                var libro = new Libro
                {
                    PkLibro = libroViewModel.PkLibro,
                    Titulo = libroViewModel.Titulo,
                    FkAutor = libroViewModel.FkAutor,
                    FechaPublicacion = libroViewModel.FechaPublicacion,
                    Descripcion = libroViewModel.Descripcion,
                    LibroCategorias = libroViewModel.CategoriaIds?.Select(categoriaId => new LibroCategoria
                    {
                        PkCategoria = categoriaId,
                        PkLibro = libroViewModel.PkLibro
                    }).ToList()
                };

                bool isUpdated = await _librosServices.EditarLibro(libro);

                if (isUpdated)
                {
                    return RedirectToAction("Index");
                }
            }

            var autores = _autorServices.ObtenerAutores();
            var categorias = _categoriaServices.ObtenerCategoria();
            ViewBag.Autores = new SelectList(autores, "PkAutor", "Nombre", libroViewModel.FkAutor);
            ViewBag.Categorias = new SelectList(categorias, "PkCategoria", "Nombre", libroViewModel.CategoriaIds);

            return View(libroViewModel);
        }

        [HttpDelete]
        public IActionResult Eliminar(int id)
        {
            bool result = _librosServices.EliminarLibro(id);
            if (result == false)
            {
                return Json(new { succes = true });
            }
            else
            {
                return Json(new { succes = false });
            }
        }
    }
}
