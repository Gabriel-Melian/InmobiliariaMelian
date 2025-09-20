using System.Reflection.Metadata.Ecma335;
using System.Security.Authentication;
using MySql.Data.MySqlClient;

namespace net.Models;

public class RepositorioMulta : RepositorioBase
{
    public List<Multa> ObtenerTodos()//No se usa
    {
        var lista = new List<Multa>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    m.id AS MultaId,
                    m.idContrato AS IdContrato,
                    m.fechaAviso AS FechaAviso,
                    m.fechaTerminacion AS FechaTerminacion,
                    m.monto AS Monto,
                    m.estado AS Estado,

                    c.id AS ContratoId,
                    c.desde AS FechaInicio,
                    c.hasta AS FechaFin,
                    c.precio AS PrecioContrato,

                    inm.id AS InmuebleId,
                    inm.direccion AS Direccion,

                    i.id AS InquilinoId,
                    i.nombre AS NombreInquilino,
                    i.apellido AS ApellidoInquilino,

                    pr.id AS PropietarioId,
                    pr.nombre AS NombrePropietario,
                    pr.apellido AS ApellidoPropietario
                FROM multa m
                INNER JOIN contrato c ON m.idContrato = c.id
                INNER JOIN inmueble inm ON c.idInmueble = inm.id
                INNER JOIN inquilino i ON c.idInquilino = i.id
                INNER JOIN propietario pr ON inm.idPropietario = pr.id
                ORDER BY m.fechaAviso DESC";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Multa
                    {
                        MultaId = reader.GetInt32("MultaId"),
                        IdContrato = reader.GetInt32("IdContrato"),
                        FechaAviso = reader.GetDateTime("FechaAviso"),
                        FechaTerminacion = reader.GetDateTime("FechaTerminacion"),
                        Monto = reader.GetDecimal("Monto"),
                        Estado = reader.GetInt32("Estado"),

                        Contrato = new Contrato
                        {
                            ContratoId = reader.GetInt32("ContratoId"),
                            FechaInicio = reader.GetDateTime("FechaInicio"),
                            FechaFin = reader.GetDateTime("FechaFin"),
                            Precio = reader.GetDecimal("PrecioContrato"),

                            Inquilino = new Inquilino
                            {
                                InquilinoId = reader.GetInt32("InquilinoId"),
                                Nombre = reader.GetString("NombreInquilino"),
                                Apellido = reader.GetString("ApellidoInquilino")
                            },
                            Inmueble = new Inmueble
                            {
                                InmuebleId = reader.GetInt32("InmuebleId"),
                                Direccion = reader.GetString("Direccion"),
                                Propietario = new Propietario
                                {
                                    PropietarioId = reader.GetInt32("PropietarioId"),
                                    Nombre = reader.GetString("NombrePropietario"),
                                    Apellido = reader.GetString("ApellidoPropietario")
                                }
                            }
                        }
                    });
                }
            }
        }
        return lista;
    }

    public Multa? ObtenerUno(int id)
    {
        var multa = new Multa();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    m.id AS MultaId,
                    m.idContrato AS IdContrato,
                    m.fechaAviso AS FechaAviso,
                    m.fechaTerminacion AS FechaTerminacion,
                    m.monto AS Monto,
                    m.estado AS Estado,

                    c.id AS ContratoId,
                    c.desde AS FechaInicio,
                    c.hasta AS FechaFin,
                    c.precio AS PrecioContrato,

                    inm.id AS InmuebleId,
                    inm.direccion AS Direccion,

                    i.id AS InquilinoId,
                    i.nombre AS NombreInquilino,
                    i.apellido AS ApellidoInquilino,

                    pr.id AS PropietarioId,
                    pr.nombre AS NombrePropietario,
                    pr.apellido AS ApellidoPropietario
                FROM multa m
                INNER JOIN contrato c ON m.idContrato = c.id
                INNER JOIN inmueble inm ON c.idInmueble = inm.id
                INNER JOIN inquilino i ON c.idInquilino = i.id
                INNER JOIN propietario pr ON inm.idPropietario = pr.id
                WHERE m.id = @id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    multa = new Multa
                    {
                        MultaId = reader.GetInt32("MultaId"),
                        IdContrato = reader.GetInt32("IdContrato"),
                        FechaAviso = reader.GetDateTime("FechaAviso"),
                        FechaTerminacion = reader.GetDateTime("FechaTerminacion"),
                        Monto = reader.GetDecimal("Monto"),
                        Estado = reader.GetInt32("Estado"),

                        Contrato = new Contrato
                        {
                            ContratoId = reader.GetInt32("ContratoId"),
                            FechaInicio = reader.GetDateTime("FechaInicio"),
                            FechaFin = reader.GetDateTime("FechaFin"),
                            Precio = reader.GetDecimal("PrecioContrato"),

                            Inquilino = new Inquilino
                            {
                                InquilinoId = reader.GetInt32("InquilinoId"),
                                Nombre = reader.GetString("NombreInquilino"),
                                Apellido = reader.GetString("ApellidoInquilino")
                            },
                            Inmueble = new Inmueble
                            {
                                InmuebleId = reader.GetInt32("InmuebleId"),
                                Direccion = reader.GetString("Direccion"),
                                Propietario = new Propietario
                                {
                                    PropietarioId = reader.GetInt32("PropietarioId"),
                                    Nombre = reader.GetString("NombrePropietario"),
                                    Apellido = reader.GetString("ApellidoPropietario")
                                }
                            }
                        }
                    };
                }
            }
        }
        return multa;
    }

    public List<Multa> ObtenerPorContrato(int id)
    {
        List<Multa> multas = new List<Multa>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    m.id AS MultaId,
                    m.idContrato AS IdContrato,
                    m.fechaAviso AS FechaAviso,
                    m.fechaTerminacion AS FechaTerminacion,
                    m.monto AS Monto,
                    m.estado AS Estado,

                    c.id AS ContratoId,
                    c.desde AS FechaInicio,
                    c.hasta AS FechaFin,
                    c.precio AS PrecioContrato,

                    inm.id AS InmuebleId,
                    inm.direccion AS Direccion,

                    i.id AS InquilinoId,
                    i.nombre AS NombreInquilino,
                    i.apellido AS ApellidoInquilino,

                    pr.id AS PropietarioId,
                    pr.nombre AS NombrePropietario,
                    pr.apellido AS ApellidoPropietario
                FROM multa m
                INNER JOIN contrato c ON m.idContrato = c.id
                INNER JOIN inmueble inm ON c.idInmueble = inm.id
                INNER JOIN inquilino i ON c.idInquilino = i.id
                INNER JOIN propietario pr ON inm.idPropietario = pr.id
                WHERE c.id = @id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    multas.Add(new Multa
                    {
                        MultaId = reader.GetInt32("MultaId"),
                        IdContrato = reader.GetInt32("IdContrato"),
                        FechaAviso = reader.GetDateTime("FechaAviso"),
                        FechaTerminacion = reader.GetDateTime("FechaTerminacion"),
                        Monto = reader.GetDecimal("Monto"),
                        Estado = reader.GetInt32("Estado"),

                        Contrato = new Contrato
                        {
                            ContratoId = reader.GetInt32("ContratoId"),
                            FechaInicio = reader.GetDateTime("FechaInicio"),
                            FechaFin = reader.GetDateTime("FechaFin"),
                            Precio = reader.GetDecimal("PrecioContrato"),

                            Inquilino = new Inquilino
                            {
                                InquilinoId = reader.GetInt32("InquilinoId"),
                                Nombre = reader.GetString("NombreInquilino"),
                                Apellido = reader.GetString("ApellidoInquilino")
                            },
                            Inmueble = new Inmueble
                            {
                                InmuebleId = reader.GetInt32("InmuebleId"),
                                Direccion = reader.GetString("Direccion"),
                                Propietario = new Propietario
                                {
                                    PropietarioId = reader.GetInt32("PropietarioId"),
                                    Nombre = reader.GetString("NombrePropietario"),
                                    Apellido = reader.GetString("ApellidoPropietario")
                                }
                            }
                        }
                    });
                }
            }
        }
        return multas;
    }

    public int Agregar(Multa multa)
    {
        var res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                INSERT INTO multa 
                (idContrato, 
                fechaAviso, 
                fechaTerminacion, 
                monto)
                VALUES (@idContrato, @fechaAviso, @fechaTerminacion, @monto);
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idContrato", multa.IdContrato);
                command.Parameters.AddWithValue("@fechaAviso", multa.FechaAviso);
                command.Parameters.AddWithValue("@fechaTerminacion", multa.FechaTerminacion);
                command.Parameters.AddWithValue("@monto", multa.Monto);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res;
    }

    public int Modificar(Multa multa)
    {
        var res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                UPDATE multa SET 
                fechaAviso = @fechaAviso, 
                fechaTerminacion = @fechaTerminacion, 
                monto = @monto
                WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", multa.MultaId);
                command.Parameters.AddWithValue("@fechaAviso", multa.FechaAviso);
                command.Parameters.AddWithValue("@fechaTerminacion", multa.FechaTerminacion);
                command.Parameters.AddWithValue("@monto", multa.Monto);
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
            var query = "UPDATE multa SET estado = 0 WHERE id = @id";
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

    public decimal CalcularMulta(Contrato contrato, DateTime fechaTerminacion)
    {
        var duracionTotal = (contrato.FechaFin - contrato.FechaInicio).TotalDays;
        var transcurrido = (fechaTerminacion - contrato.FechaInicio).TotalDays;

        if (transcurrido < duracionTotal / 2)
            return contrato.Precio * 2;
        else
            return contrato.Precio * 1;
    }
}