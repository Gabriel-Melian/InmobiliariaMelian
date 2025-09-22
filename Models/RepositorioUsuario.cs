using MySql.Data.MySqlClient;

namespace net.Models;

public class RepositorioUsuario : RepositorioBase
{
    public List<Usuario> ObtenerEmpleados()
    {
        var lista = new List<Usuario>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @"SELECT id AS UsuarioId, nombre, apellido, email, rol, dni, avatar, estado
                        FROM usuario
                        WHERE rol = @rolEmpleado
                        ORDER BY apellido, nombre";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@rolEmpleado", (int)Usuario.Roles.Empleado);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Usuario
                    {
                        UsuarioId = reader.GetInt32("UsuarioId"),
                        Nombre = reader.GetString("nombre"),
                        Apellido = reader.GetString("apellido"),
                        Email = reader.GetString("email"),
                        Rol = reader.GetInt32("rol"),
                        Dni = reader.GetString("dni"),
                        Avatar = reader.IsDBNull(reader.GetOrdinal("avatar")) ? null : reader.GetString("avatar"),
                        Estado = reader.GetInt32("estado")
                    });
                }
            }
        }
        return lista;
    }

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
                    avatar AS Avatar,
                    estado AS Estado
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
                                : reader.GetString(nameof(Usuario.Avatar)),
                        Estado = reader.GetInt32(nameof(Usuario.Estado))
                    };
                }
            }
        }

        return usuario;
    }

    public Usuario? ObtenerPorId(int id)
    {
        Usuario? usuario = null;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @"SELECT id AS UsuarioId, nombre, apellido, email, dni, rol, avatar, estado
                        FROM usuario WHERE id=@id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        UsuarioId = reader.GetInt32("UsuarioId"),
                        Nombre = reader.GetString("nombre"),
                        Apellido = reader.GetString("apellido"),
                        Email = reader.GetString("email"),
                        Dni = reader.GetString("dni"),
                        Rol = reader.GetInt32("rol"),
                        Avatar = reader.IsDBNull(reader.GetOrdinal("avatar")) ? null : reader.GetString("avatar"),
                        Estado = reader.GetInt32("estado")
                    };
                }
            }
        }
        return usuario;
    }

    //Un admin modifica un empleado
    public int Modificacion(Usuario usuario)
    {
        int res = -1;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @"UPDATE usuario 
                        SET nombre=@nombre, apellido=@apellido, email=@email,
                            dni=@dni
                        WHERE id=@id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", usuario.UsuarioId);
                command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@apellido", usuario.Apellido);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@dni", usuario.Dni);
                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }
        return res;
    }

    //El empleado modifica solo su info personal
    public int ActualizarPerfil(Usuario usuario, bool cambiarPassword)
    {
        int res = -1;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql;
            if (cambiarPassword)
            {
                sql = @"UPDATE usuario 
                        SET nombre=@nombre, apellido=@apellido, email=@email,
                            dni=@dni, avatar=@avatar, password=@password
                        WHERE id=@id";
            }
            else
            {
                sql = @"UPDATE usuario 
                        SET nombre=@nombre, apellido=@apellido, email=@email,
                            dni=@dni, avatar=@avatar
                        WHERE id=@id";
            }

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", usuario.UsuarioId);
                command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@apellido", usuario.Apellido);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@dni", usuario.Dni);
                command.Parameters.AddWithValue("@avatar", usuario.Avatar);

                if (cambiarPassword)
                    command.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(usuario.Password));

                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }
        return res;
    }

    //Admin desactiva Usuario
    public int Baja(int id)
    {
        int res = -1;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @"UPDATE usuario SET estado = 0 WHERE id=@id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }
        return res;
    }

    //Admin reactiva Usuario
    public int Reactivar(int id)
    {
        int res = -1;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @"UPDATE usuario SET estado = 1 WHERE id=@id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }
        return res;
    }

}