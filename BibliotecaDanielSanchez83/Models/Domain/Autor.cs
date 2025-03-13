using System.ComponentModel.DataAnnotations;

namespace BibliotecaDanielSanchez83.Models.Domain
{
    public class Autor
    {
        [Key]
        public int PkAutor { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Biografia { get; set; }

        // Relación con Libros (un autor puede tener muchos libros)
        public ICollection<Libro> Libros { get; set; }
    }
}