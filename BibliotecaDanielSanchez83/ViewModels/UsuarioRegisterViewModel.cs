namespace BibliotecaDanielSanchez83.ViewModels
{
    public class UsuarioRegisterViewModel
    {
        public string Nombre { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int FkRol { get; set; } // ID del rol seleccionado
    }
}
