using Microsoft.AspNetCore.Http.Timeouts;
using MySql.Data.MySqlClient;

namespace net.Models;

public class RepositorioUso : RepositorioBase
{
    public List<UsoInmueble> ObtenerTodos()
    {
        List<UsoInmueble> usos = new List<UsoInmueble>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"SELECT 
           id AS UsoId,
           valor AS UsoValor
           FROM usoinmueble";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    usos.Add(new UsoInmueble
                    {
                        UsoId = reader.GetInt32(nameof(UsoInmueble.UsoId)),
                        UsoValor = reader.GetString(nameof(UsoInmueble.UsoValor))
                    });
                }
            }
        }
        return usos;
    }

    public UsoInmueble? ObtenerUno(int id)
    {
        UsoInmueble? uso = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"SELECT 
           id AS UsoId,
           valor AS UsoValor
           FROM usoinmueble
           WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    uso = new UsoInmueble
                    {
                        UsoId = reader.GetInt32(nameof(UsoInmueble.UsoId)),
                        UsoValor = reader.GetString(nameof(UsoInmueble.UsoValor))
                    };
                }
                connection.Close();
            }
        }
        return uso;
    }

    public int Alta(UsoInmueble uso)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"INSERT INTO usoinmueble
           (valor)
            VALUES(@valor);
            SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@valor", uso.UsoValor);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            return res;
        }
    }

    public int Modificar(UsoInmueble uso)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"UPDATE usoinmueble
           SET valor = @valor
           WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", uso.UsoId);
                command.Parameters.AddWithValue("@valor", uso.UsoValor);
                connection.Open();
                res = command.ExecuteNonQuery();
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
            var query = $@"DELETE FROM usoinmueble
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