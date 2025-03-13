namespace BibliotecaDanielSanchez83.Models.Domain
{
    public class LibroCategoria
    {
        public int PkLibro { get; set; }  // Clave primaria de Libro
        public Libro Libro { get; set; }   // Relación con el libro

        public int PkCategoria { get; set; }  // Clave primaria de Categoria
        public Categoria Categoria { get; set; }  // Relación con la categoria
    }
}
