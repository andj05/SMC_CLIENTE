using System;
namespace SMC_CLIENTE.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
