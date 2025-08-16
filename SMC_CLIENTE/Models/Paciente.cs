
namespace SMC_CLIENTE.Models
{
    public class Paciente
    {
        public int IdPaciente { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Cedula { get; set; }
        public string Genero { get; set; }
        public string EstadoCivil { get; set; }
        public string Ocupacion { get; set; }
        public string ContactoEmergencia { get; set; }
        public string TelefonoEmergencia { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

        public int Edad
        {
            get
            {
                return DateTime.Now.Year - FechaNacimiento.Year;
            }
        }
    }
}
