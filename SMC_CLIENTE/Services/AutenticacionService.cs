using MySql.Data.MySqlClient;
using SMC_CLIENTE.Data;
using SMC_CLIENTE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SMC_CLIENTE.Services
{
    public class AutenticacionService
    {
        public static Usuario UsuarioActual { get; private set; }

        public static bool Login(string username, string password)
        {
            try
            {
                string hashedPassword = HashPassword(password);

                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT id_usuario, username, nombre_completo, rol, email, telefono, activo 
                                   FROM usuarios 
                                   WHERE username = @username AND password = @password AND activo = 1";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", hashedPassword);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UsuarioActual = new Usuario
                                {
                                    IdUsuario = reader.GetInt32("id_usuario"),
                                    Username = reader.GetString("username"),
                                    NombreCompleto = reader.GetString("nombre_completo"),
                                    Rol = reader.GetString("rol"),
                                    Email = reader.GetString("email"),
                                    Telefono = reader.IsDBNull("telefono") ? "" : reader.GetString("telefono"),
                                    Activo = reader.GetBoolean("activo")
                                };
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al autenticar usuario: {ex.Message}");
            }

            return false;
        }

        public static void Logout()
        {
            UsuarioActual = null;
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}