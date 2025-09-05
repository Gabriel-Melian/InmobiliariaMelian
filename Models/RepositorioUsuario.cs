using MySql.Data.MySqlClient;

namespace net.Models;

public class RepositorioUsuario : RepositorioBase
{
    public Usuario? ObtenerPorEmail(string email)
    {
        Usuario? usuario = null;

        using (var connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    id AS UsuarioId,
                    nombre AS Nombre,
                    apellido AS Apellido,
                    email AS Email,
                    password AS Password,
                    rol AS Rol,
                    dni AS Dni,
                    avatar AS Avatar
                FROM usuario
                WHERE email = @email
                LIMIT 1;";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@email", email);
                connection.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        UsuarioId = reader.GetInt32(nameof(Usuario.UsuarioId)),
                        Nombre = reader.GetString(nameof(Usuario.Nombre)),
                        Apellido = reader.GetString(nameof(Usuario.Apellido)),
                        Email = reader.GetString(nameof(Usuario.Email)),
                        Password = reader.GetString(nameof(Usuario.Password)),
                        Rol = reader.GetInt32(nameof(Usuario.Rol)),
                        Dni = reader.GetString(nameof(Usuario.Dni)),
                        Avatar = reader.IsDBNull(reader.GetOrdinal(nameof(Usuario.Avatar)))
                                ? null
                                : reader.GetString(nameof(Usuario.Avatar))
                    };
                }
            }
        }

        return usuario;
    }

}