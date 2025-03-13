using BibliotecaDanielSanchez83.Models.Domain;
using BibliotecaDanielSanchez83.ViewModels;


namespace BibliotecaDanielSanchez83.Services.IServices
{
    public interface ILibrosServices
    {
        public List<Libro> ObtenerLibros();
        public IEnumerable<Libro> GetLibrosNew();
        public IEnumerable<Libro> GetLibrosOld();
        public Libro GetLibroById(int id);
        public Task<bool> CrearLibro(LibroCreateViewModel libroModel);
        public Task<bool> EditarLibro(Libro libro);
        public bool EliminarLibro(int id);

    }
}
