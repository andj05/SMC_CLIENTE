using MySql.Data.MySqlClient;
using SMC_CLIENTE.Data;
using SMC_CLIENTE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC_CLIENTE.Services
{
    public class PacienteService
    {
        public static List<Paciente> ObtenerTodos()
        {
            var pacientes = new List<Paciente>();

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT * FROM pacientes WHERE activo = 1 ORDER BY nombre_completo";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pacientes.Add(CrearPacienteDesdeReader(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener pacientes: {ex.Message}");
            }

            return pacientes;
        }

        public static bool Insertar(Paciente paciente)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO pacientes 
                                   (nombre_completo, fecha_nacimiento, telefono, direccion, email, cedula, genero, estado_civil, ocupacion, contacto_emergencia, telefono_emergencia)
                                   VALUES (@nombre, @fecha_nac, @telefono, @direccion, @email, @cedula, @genero, @estado_civil, @ocupacion, @contacto_emerg, @tel_emerg)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        AgregarParametrosPaciente(command, paciente);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al insertar paciente: {ex.Message}");
            }
        }

        public static bool Actualizar(Paciente paciente)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"UPDATE pacientes SET 
                                   nombre_completo = @nombre, fecha_nacimiento = @fecha_nac, telefono = @telefono, 
                                   direccion = @direccion, email = @email, cedula = @cedula, genero = @genero,
                                   estado_civil = @estado_civil, ocupacion = @ocupacion, contacto_emergencia = @contacto_emerg,
                                   telefono_emergencia = @tel_emerg
                                   WHERE id_paciente = @id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        AgregarParametrosPaciente(command, paciente);
                        command.Parameters.AddWithValue("@id", paciente.IdPaciente);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar paciente: {ex.Message}");
            }
        }

        public static List<Paciente> Buscar(string criterio)
        {
            var pacientes = new List<Paciente>();

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT * FROM pacientes 
                                   WHERE activo = 1 AND 
                                   (nombre_completo LIKE @criterio OR cedula LIKE @criterio OR telefono LIKE @criterio)
                                   ORDER BY nombre_completo";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@criterio", $"%{criterio}%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pacientes.Add(CrearPacienteDesdeReader(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar pacientes: {ex.Message}");
            }

            return pacientes;
        }

        private static Paciente CrearPacienteDesdeReader(MySqlDataReader reader)
        {
            return new Paciente
            {
                IdPaciente = reader.GetInt32("id_paciente"),
                NombreCompleto = reader.GetString("nombre_completo"),
                FechaNacimiento = reader.GetDateTime("fecha_nacimiento"),
                Telefono = reader.IsDBNull("telefono") ? "" : reader.GetString("telefono"),
                Direccion = reader.IsDBNull("direccion") ? "" : reader.GetString("direccion"),
                Email = reader.IsDBNull("email") ? "" : reader.GetString("email"),
                Cedula = reader.IsDBNull("cedula") ? "" : reader.GetString("cedula"),
                Genero = reader.IsDBNull("genero") ? "" : reader.GetString("genero"),
                EstadoCivil = reader.IsDBNull("estado_civil") ? "" : reader.GetString("estado_civil"),
                Ocupacion = reader.IsDBNull("ocupacion") ? "" : reader.GetString("ocupacion"),
                ContactoEmergencia = reader.IsDBNull("contacto_emergencia") ? "" : reader.GetString("contacto_emergencia"),
                TelefonoEmergencia = reader.IsDBNull("telefono_emergencia") ? "" : reader.GetString("telefono_emergencia"),
                FechaRegistro = reader.GetDateTime("fecha_registro"),
                Activo = reader.GetBoolean("activo")
            };
        }

        private static void AgregarParametrosPaciente(MySqlCommand command, Paciente paciente)
        {
            command.Parameters.AddWithValue("@nombre", paciente.NombreCompleto);
            command.Parameters.AddWithValue("@fecha_nac", paciente.FechaNacimiento);
            command.Parameters.AddWithValue("@telefono", paciente.Telefono);
            command.Parameters.AddWithValue("@direccion", paciente.Direccion);
            command.Parameters.AddWithValue("@email", paciente.Email);
            command.Parameters.AddWithValue("@cedula", paciente.Cedula);
            command.Parameters.AddWithValue("@genero", paciente.Genero);
            command.Parameters.AddWithValue("@estado_civil", paciente.EstadoCivil);
            command.Parameters.AddWithValue("@ocupacion", paciente.Ocupacion);
            command.Parameters.AddWithValue("@contacto_emerg", paciente.ContactoEmergencia);
            command.Parameters.AddWithValue("@tel_emerg", paciente.TelefonoEmergencia);
        }
    }
}