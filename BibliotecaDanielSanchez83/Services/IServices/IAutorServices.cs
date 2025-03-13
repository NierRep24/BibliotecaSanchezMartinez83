using BibliotecaDanielSanchez83.Models.Domain;


namespace BibliotecaDanielSanchez83.Services.IServices
{
    public interface IAutorServices
    {
        public List<Autor> ObtenerAutores();
        public Autor GetAutorById(int id);
        public bool CrearAutor(Autor autor);
        public Task<bool> EditarAutor(Autor autor);
        public bool EliminarAutor(int id);
    }
}
