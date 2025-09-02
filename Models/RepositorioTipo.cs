using Microsoft.AspNetCore.Http.Timeouts;
using MySql.Data.MySqlClient;

namespace net.Models;

public class RepositorioTipo : RepositorioBase
{
    public List<TipoInmueble> ObtenerTodos()
    {
        List<TipoInmueble> tipos = new List<TipoInmueble>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"SELECT 
           id AS TipoId,
           valor AS Valor
           FROM tipoinmueble";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tipos.Add(new TipoInmueble
                    {
                        TipoId = reader.GetInt32(nameof(TipoInmueble.TipoId)),
                        Valor = reader.GetString(nameof(TipoInmueble.Valor))
                    });
                }
            }
        }
        return tipos;
    }

    public TipoInmueble? ObtenerUno(int id)
    {
        TipoInmueble? tipo = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"SELECT 
           id AS TipoId,
           valor AS Valor
           FROM tipoinmueble
           WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    tipo = new TipoInmueble
                    {
                        TipoId = reader.GetInt32(nameof(TipoInmueble.TipoId)),
                        Valor = reader.GetString(nameof(TipoInmueble.Valor))
                    };
                }
                connection.Close();
            }
        }
        return tipo;
    }

    public int Alta(TipoInmueble tipo)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"INSERT INTO tipoinmueble
           (valor)
            VALUES(@valor);
            SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@valor", tipo.Valor);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            return res;
        }
    }

    public int Modificar(TipoInmueble tipo)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"UPDATE tipoinmueble
           SET valor = @valor
           WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", tipo.TipoId);
                command.Parameters.AddWithValue("@valor", tipo.Valor);
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
            var query = $@"DELETE FROM tipoinmueble
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