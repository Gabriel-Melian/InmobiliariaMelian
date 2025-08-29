using MySql.Data.MySqlClient;

namespace net.Models;

public class RepositorioInquilino : RepositorioBase
{
    public List<Inquilino> ObtenerTodos()
    {
        List<Inquilino> inquilinos = new List<Inquilino>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"SELECT
            id AS InquilinoId,
            nombre AS Nombre,
            apellido AS Apellido,
            dni AS Dni,
            email AS Email,
            telefono AS Telefono,
            estado AS Estado
           FROM inquilino";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inquilinos.Add(new Inquilino
                    {
                        InquilinoId = reader.GetInt32(nameof(Inquilino.InquilinoId)),
                        Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                        Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                        Dni = reader.GetString(nameof(Inquilino.Dni)),
                        Email = reader.GetString(nameof(Inquilino.Email)),
                        Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                        Estado = reader.GetInt32(nameof(Inquilino.Estado))
                    });
                }
            }
        }
        return inquilinos;
    }

    public Inquilino? ObtenerUno(int id)
    {
        Inquilino? inquilino = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"SELECT
            id AS InquilinoId,
            nombre AS Nombre,
            apellido AS Apellido,
            dni AS Dni,
            email AS Email,
            telefono AS Telefono,
            estado AS Estado
           FROM inquilino
           WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    inquilino = new Inquilino
                    {
                        InquilinoId = reader.GetInt32(nameof(Inquilino.InquilinoId)),
                        Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                        Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                        Dni = reader.GetString(nameof(Inquilino.Dni)),
                        Email = reader.GetString(nameof(Inquilino.Email)),
                        Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                        Estado = reader.GetInt32(nameof(Inquilino.Estado))
                    };
                }
            }
        }
        return inquilino;
    }

    public List<Inquilino> ObtenerActivos()
    {
        List<Inquilino> inquilinos = new List<Inquilino>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"SELECT
            id AS InquilinoId,
            nombre AS Nombre,
            apellido AS Apellido,
            dni AS Dni,
            email AS Email,
            telefono AS Telefono,
            estado AS Estado
           FROM inquilino
           WHERE estado = 1";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inquilinos.Add(new Inquilino
                    {
                        InquilinoId = reader.GetInt32(nameof(Inquilino.InquilinoId)),
                        Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                        Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                        Dni = reader.GetString(nameof(Inquilino.Dni)),
                        Email = reader.GetString(nameof(Inquilino.Email)),
                        Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                        Estado = reader.GetInt32(nameof(Inquilino.Estado))
                    });
                }
            }
        }
        return inquilinos;
    }

    public List<Inquilino> ObtenerInactivos()
    {
        List<Inquilino> inquilinos = new List<Inquilino>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"SELECT
            id AS InquilinoId,
            nombre AS Nombre,
            apellido AS Apellido,
            dni AS Dni,
            email AS Email,
            telefono AS Telefono,
            estado AS Estado
           FROM inquilino
           WHERE estado = 0";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inquilinos.Add(new Inquilino
                    {
                        InquilinoId = reader.GetInt32(nameof(Inquilino.InquilinoId)),
                        Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                        Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                        Dni = reader.GetString(nameof(Inquilino.Dni)),
                        Email = reader.GetString(nameof(Inquilino.Email)),
                        Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                        Estado = reader.GetInt32(nameof(Inquilino.Estado))
                    });
                }
            }
        }
        return inquilinos;
    }

    public int Modificar(Inquilino inquilino)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"UPDATE inquilino SET 
                nombre = @nombre,
                apellido = @apellido,
                dni = @dni,
                email = @email,
                telefono = @telefono
            WHERE id = @Id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", inquilino.InquilinoId);
                command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
                command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
                command.Parameters.AddWithValue("@dni", inquilino.Dni);
                command.Parameters.AddWithValue("@email", inquilino.Email);
                command.Parameters.AddWithValue("@telefono", inquilino.Telefono);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
                return res;
            }
        }
    }

    public bool VerificarInquilino(string dni, string email)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"SELECT COUNT(*) 
            FROM inquilino 
            WHERE dni = @dni 
            AND email = @email";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@dni", dni);
                command.Parameters.AddWithValue("@email", email);
                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                return count > 0;
            }
        }
    }

    public int Alta(Inquilino inquilino)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"INSERT INTO inquilino 
            (nombre, 
            apellido, 
            dni, 
            email, 
            telefono)
            VALUES (@nombre, @apellido, @dni, @email, @telefono)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
                command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
                command.Parameters.AddWithValue("@dni", inquilino.Dni);
                command.Parameters.AddWithValue("@email", inquilino.Email);
                command.Parameters.AddWithValue("@telefono", inquilino.Telefono);
                connection.Open();
                //Esto devuelve null//Aca podria fallar.
                res = Convert.ToInt32(command.ExecuteScalar());
                //res = command.ExecuteNonQuery();//Devuelve cantidad de filas afectadas.
                connection.Close();
            }
        }
        return res;
    }

    public int Baja(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"UPDATE inquilino
            SET estado = 0
            WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public int Restore(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"UPDATE inquilino
            SET estado = 1
            WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    
}