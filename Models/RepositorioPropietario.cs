using MySql.Data.MySqlClient;

namespace net.Models;

public class RepositorioPropietario : RepositorioBase
{
    public List<Propietario> ObtenerTodos()
    {
        List<Propietario> propietarios = new List<Propietario>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"SELECT
            id AS PropietarioId,
            nombre AS Nombre,
            apellido AS Apellido,
            dni AS Dni,
            email AS Email,
            telefono AS Telefono,
            direccion AS Direccion,
            estado AS Estado
           FROM propietario";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    propietarios.Add(new Propietario
                    {
                        PropietarioId = reader.GetInt32(nameof(Propietario.PropietarioId)),
                        Nombre = reader.GetString(nameof(Propietario.Nombre)),
                        Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        Dni = reader.GetString(nameof(Propietario.Dni)),
                        Email = reader.GetString(nameof(Propietario.Email)),
                        Telefono = reader.GetString(nameof(Propietario.Telefono)),
                        Direccion = reader.GetString(nameof(Propietario.Direccion)),
                        Estado = reader.GetInt32(nameof(Propietario.Estado))
                    });
                }
            }
        }
        return propietarios;
    }

    public Propietario? ObtenerUno(int id)
    {
        Propietario? propietario = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"SELECT  
           id AS PropietarioId,
            nombre AS Nombre,
            apellido AS Apellido,
            dni AS Dni,
            email AS Email,
            telefono AS Telefono,
            direccion AS Direccion,
            estado AS Estado
           FROM propietario 
           WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    propietario = new Propietario
                    {
                        PropietarioId = reader.GetInt32(nameof(Propietario.PropietarioId)),
                        Nombre = reader.GetString(nameof(Propietario.Nombre)),
                        Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        Dni = reader.GetString(nameof(Propietario.Dni)),
                        Email = reader.GetString(nameof(Propietario.Email)),
                        Telefono = reader.GetString(nameof(Propietario.Telefono)),
                        Direccion = reader.GetString(nameof(Propietario.Direccion)),
                        Estado = reader.GetInt32(nameof(Propietario.Estado))
                    };
                }
                connection.Close();
            }
        }
        return propietario;
    }

    public int Modificar(Propietario propietario)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"UPDATE propietario SET 
            nombre = @nombre,
            apellido = @apellido,
            dni = @dni,
            email = @email,
            telefono = @telefono,
            direccion = @direccion
            WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", propietario.PropietarioId);
                command.Parameters.AddWithValue("@nombre", propietario.Nombre);
                command.Parameters.AddWithValue("@apellido", propietario.Apellido);
                command.Parameters.AddWithValue("@dni", propietario.Dni);
                command.Parameters.AddWithValue("@email", propietario.Email);
                command.Parameters.AddWithValue("@telefono", propietario.Telefono);
                command.Parameters.AddWithValue("@direccion", propietario.Direccion);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
                return res;
            }
        }
    }

    public bool VerificarPropietario(string nombre, string apellido, string dni)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"SELECT COUNT(*) 
            FROM propietario 
            WHERE LOWER(nombre) = LOWER(@nombre) 
            AND LOWER(apellido) = LOWER(@apellido) 
            AND dni = @dni";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nombre", nombre);
                command.Parameters.AddWithValue("@apellido", apellido);
                command.Parameters.AddWithValue("@dni", dni);
                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                return count > 0;
            }
        }
    }

    public int Alta(Propietario propietario)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"INSERT INTO propietario
           (nombre,
            apellido,
            dni,
            email,
            telefono,
            direccion)
           VALUES(@nombre,@apellido,@dni,@email,@telefono,@direccion);
           SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nombre", propietario.Nombre);
                command.Parameters.AddWithValue("@apellido", propietario.Apellido);
                command.Parameters.AddWithValue("@dni", propietario.Dni);
                command.Parameters.AddWithValue("@email", propietario.Email);               
                command.Parameters.AddWithValue("@telefono", propietario.Telefono);
                command.Parameters.AddWithValue("@direccion", propietario.Direccion);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res;
    }

}