using Azure.Core;
using BibliotecaDanielSanchez83.Context;
using BibliotecaDanielSanchez83.Models.Domain;
using BibliotecaDanielSanchez83.Services.IServices;
using BibliotecaDanielSanchez83.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaDanielSanchez83.Services.Services
{
    public class LibrosServices : ILibrosServices
    {
        private readonly ApplicationDbContext _context;

        public LibrosServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Libro> ObtenerLibros()
        {
            return _context.Libros
                .Include(l => l.Autor)  
                .Include(l => l.LibroCategorias) 
                    .ThenInclude(lc => lc.Categoria)  
                .ToList();
        }

        public IEnumerable<Libro> GetLibrosNew()
        {
            // Ordena forma descendente (más recientes primero)
            return _context.Libros.OrderByDescending(l => l.Fecha).ToList();
        }

        public IEnumerable<Libro> GetLibrosOld()
        {
            // Ordena de forma ascendente (más antiguos primero)
            return _context.Libros.OrderBy(l => l.Fecha).ToList();
        }


        public Libro GetLibroById(int id)
        {
            return _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.LibroCategorias)
                .FirstOrDefault(l => l.PkLibro == id);
        }

        public async Task<bool> CrearLibro(LibroCreateViewModel libroModel)
        {
            try
            {
                var libro = new Libro
                {
                    Titulo = libroModel.Titulo,
                    FkAutor = libroModel.FkAutor,
                    FechaPublicacion = libroModel.FechaPublicacion,
                    Descripcion = libroModel.Descripcion
                };

                _context.Libros.Add(libro);
                int result = await _context.SaveChangesAsync(); 

                if (result > 0)
                {
                    foreach (var categoriaId in libroModel.CategoriaIds)
                    {
                        var libroCategoria = new LibroCategoria
                        {
                            PkLibro = libro.PkLibro, 
                            PkCategoria = categoriaId
                        };

                        _context.LibroCategorias.Add(libroCategoria);
                    }

                    await _context.SaveChangesAsync(); 
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el libro: " + ex.Message);
            }
        }

        public async Task<bool> EditarLibro(Libro libro)
        {
            try
            {
                var libroExistente = await _context.Libros.FindAsync(libro.PkLibro);
                if (libroExistente == null) return false;

                libroExistente.Titulo = libro.Titulo;
                libroExistente.FkAutor = libro.FkAutor;
                libroExistente.FechaPublicacion = libro.FechaPublicacion;
                libroExistente.Descripcion = libro.Descripcion;

                _context.LibroCategorias.RemoveRange(_context.LibroCategorias.Where(lc => lc.PkLibro == libro.PkLibro));

                foreach (var categoriaId in libro.LibroCategorias.Select(c => c.PkCategoria))
                {
                    var libroCategoria = new LibroCategoria
                    {
                        PkLibro = libro.PkLibro,
                        PkCategoria = categoriaId
                    };

                    _context.LibroCategorias.Add(libroCategoria);
                }

                _context.Libros.Update(libroExistente);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el libro: " + ex.Message);
            }
        }

        public bool EliminarLibro(int id)
        {
            try
            {
                var libro = _context.Libros.Find(id);
                if (libro != null)
                {
                    var relacionesCategoria = _context.LibroCategorias.Where(lc => lc.PkLibro == id);
                    _context.LibroCategorias.RemoveRange(relacionesCategoria);

                    _context.Libros.Remove(libro);
                    int result = _context.SaveChanges();
                    if (result > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el libro: " + ex.Message);
            }
        }
    }

}
