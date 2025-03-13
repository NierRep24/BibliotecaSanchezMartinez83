using System.ComponentModel.DataAnnotations;

namespace BibliotecaDanielSanchez83.Models.Domain
{
    public class Libro
    {
        [Key]
        public int PkLibro { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public int FkAutor { get; set; } 

        [Required]
        public int FechaPublicacion { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.UtcNow; // Fecha de inserción en la BD

        // Relación con Autor (un libro pertenece a un autor)
        public Autor Autor { get; set; }
        // Relación con Categorías (muchos a muchos)
        public ICollection<LibroCategoria> LibroCategorias { get; set; }
    }
}