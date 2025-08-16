using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC_CLIENTE.Models
{
    public class Cita
    {
        public int IdCita { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public DateTime FechaCita { get; set; }
        public TimeSpan HoraCita { get; set; }
        public string MotivoConsulta { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int RegistradoPor { get; set; }

        // Propiedades adicionales para mostrar información completa
        public string NombrePaciente { get; set; }
        public string NombreMedico { get; set; }
        public string TelefonoPaciente { get; set; }
    }
}
