using System.Security.Authentication;
using MySql.Data.MySqlClient;

namespace net.Models;

public class RepositorioContrato : RepositorioBase
{
    public List<Contrato> ObtenerTodos()
    {
        List<Contrato> contratos = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
            SELECT 
                c.id AS ContratoId,
                c.idInquilino AS InquilinoId,
                c.idInmueble AS InmuebleId,
                c.idUsuarioCreador AS UsuarioCreadorId,
                c.fechaCreacion AS FechaCreacion,
                c.desde AS FechaInicio,
                c.hasta AS FechaFin,
                c.precio AS Precio,
                c.estado AS Estado,
                c.idUsuarioAnulador AS UsuarioAnuladorId,
                c.fechaAnulacion AS FechaAnulacion,
                
                i.id AS InquilinoId,
                i.nombre AS NombreInq,
                i.apellido AS ApellidoInq,
                i.dni AS DniInq,
                
                inm.id AS InmuebleId,
                inm.latitud AS Latitud,
                inm.longitud AS Longitud,
                inm.precio AS PrecioInmueble,
                inm.idPropietario AS PropietarioId,
                
                p.id AS PropietarioId,
                p.nombre AS NombreP,
                p.apellido AS ApellidoP,
                p.dni AS DniP
            FROM contrato c
            INNER JOIN inquilino i ON i.id = c.idInquilino
            INNER JOIN inmueble inm ON inm.id = c.idInmueble
            INNER JOIN propietario p ON p.id = inm.idPropietario;";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    contratos.Add(new Contrato
                    {
                        ContratoId = reader.GetInt32("ContratoId"),
                        InquilinoId = reader.GetInt32("InquilinoId"),
                        InmuebleId = reader.GetInt32("InmuebleId"),
                        UsuarioCreadorId = reader.GetInt32("UsuarioCreadorId"),
                        FechaCreacion = reader.GetDateTime("FechaCreacion"),
                        FechaInicio = reader.GetDateTime("FechaInicio"),
                        FechaFin = reader.GetDateTime("FechaFin"),
                        Precio = reader.GetDecimal("Precio"),
                        Estado = reader.GetInt32("Estado"),
                        UsuarioAnuladorId = reader.IsDBNull(reader.GetOrdinal("UsuarioAnuladorId"))
                            ? (int?)null : reader.GetInt32("UsuarioAnuladorId"),
                        FechaAnulacion = reader.IsDBNull(reader.GetOrdinal("FechaAnulacion"))
                            ? (DateTime?)null : reader.GetDateTime("FechaAnulacion"),

                        Inquilino = new Inquilino
                        {
                            InquilinoId = reader.GetInt32("InquilinoId"),
                            Nombre = reader.GetString("NombreInq"),
                            Apellido = reader.GetString("ApellidoInq"),
                            Dni = reader.GetString("DniInq")
                        },

                        Inmueble = new Inmueble
                        {
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            Latitud = reader.GetString("Latitud"),
                            Longitud = reader.GetString("Longitud"),
                            Precio = reader.GetDecimal("PrecioInmueble"),
                            IdPropietario = reader.GetInt32("PropietarioId"),
                            Propietario = new Propietario
                            {
                                PropietarioId = reader.GetInt32("PropietarioId"),
                                Nombre = reader.GetString("NombreP"),
                                Apellido = reader.GetString("ApellidoP"),
                                Dni = reader.GetString("DniP")
                            }
                        }
                    });
                }
            }
        }
        return contratos;
    }

    public Contrato? ObtenerUno(int id)
    {
        Contrato? contrato = null;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    c.id AS ContratoId,
                    c.idInquilino AS InquilinoId,
                    c.idInmueble AS InmuebleId,
                    c.idUsuarioCreador AS UsuarioCreadorId,
                    c.fechaCreacion AS FechaCreacion,
                    c.desde AS FechaInicio,
                    c.hasta AS FechaFin,
                    c.precio AS Precio,
                    c.estado AS Estado,
                    c.idUsuarioAnulador AS UsuarioAnuladorId,
                    c.fechaAnulacion AS FechaAnulacion,

                    i.id AS InquilinoId,
                    i.nombre AS NombreInquilino,
                    i.apellido AS ApellidoInquilino,
                    i.dni AS DniInquilino,

                    inm.id AS InmuebleId,
                    inm.latitud AS Latitud,
                    inm.longitud AS Longitud,
                    inm.precio AS PrecioInmueble,
                    inm.idPropietario AS PropietarioId,

                    p.id AS PropietarioId,
                    p.nombre AS NombrePropietario,
                    p.apellido AS ApellidoPropietario,
                    p.dni AS DniPropietario
                FROM contrato c
                INNER JOIN inquilino i ON i.id = c.idInquilino
                INNER JOIN inmueble inm ON inm.id = c.idInmueble
                INNER JOIN propietario p ON p.id = inm.idPropietario
                WHERE c.id = @id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    contrato = new Contrato
                    {
                        ContratoId = reader.GetInt32("ContratoId"),
                        InquilinoId = reader.GetInt32("InquilinoId"),
                        InmuebleId = reader.GetInt32("InmuebleId"),
                        UsuarioCreadorId = reader.GetInt32("UsuarioCreadorId"),
                        FechaCreacion = reader.GetDateTime("FechaCreacion"),
                        FechaInicio = reader.GetDateTime("FechaInicio"),
                        FechaFin = reader.GetDateTime("FechaFin"),
                        Precio = reader.GetDecimal("Precio"),
                        Estado = reader.GetInt32("Estado"),
                        UsuarioAnuladorId = reader.IsDBNull(reader.GetOrdinal("UsuarioAnuladorId")) ? null : reader.GetInt32("UsuarioAnuladorId"),
                        FechaAnulacion = reader.IsDBNull(reader.GetOrdinal("FechaAnulacion")) ? null : reader.GetDateTime("FechaAnulacion"),

                        Inquilino = new Inquilino
                        {
                            InquilinoId = reader.GetInt32("InquilinoId"),
                            Nombre = reader.GetString("NombreInquilino"),
                            Apellido = reader.GetString("ApellidoInquilino"),
                            Dni = reader.GetString("DniInquilino")
                        },
                        Inmueble = new Inmueble
                        {
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            Latitud = reader.GetString("Latitud"),
                            Longitud = reader.GetString("Longitud"),
                            Precio = reader.GetDecimal("PrecioInmueble"),
                            IdPropietario = reader.GetInt32("PropietarioId"),
                            Propietario = new Propietario
                            {
                                PropietarioId = reader.GetInt32("PropietarioId"),
                                Nombre = reader.GetString("NombrePropietario"),
                                Apellido = reader.GetString("ApellidoPropietario"),
                                Dni = reader.GetString("DniPropietario")
                            }
                        }
                    };
                }
            }
        }
        return contrato;
    }

    public List<Contrato> ObtenerActivos()
    {
        var contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    c.id AS ContratoId,
                    c.idInquilino AS InquilinoId,
                    c.idInmueble AS InmuebleId,
                    c.idUsuarioCreador AS UsuarioCreadorId,
                    c.fechaCreacion AS FechaCreacion,
                    c.desde AS FechaInicio,
                    c.hasta AS FechaFin,
                    c.precio AS Precio,
                    c.estado AS Estado,
                    c.idUsuarioAnulador AS UsuarioAnuladorId,
                    c.fechaAnulacion AS FechaAnulacion,

                    i.id AS InquilinoId,
                    i.nombre AS NombreInquilino,
                    i.apellido AS ApellidoInquilino,
                    i.dni AS DniInquilino,

                    inm.id AS InmuebleId,
                    inm.latitud AS Latitud,
                    inm.longitud AS Longitud,
                    inm.precio AS PrecioInmueble,
                    inm.idPropietario AS PropietarioId,

                    p.id AS PropietarioId,
                    p.nombre AS NombrePropietario,
                    p.apellido AS ApellidoPropietario,
                    p.dni AS DniPropietario
                FROM contrato c
                INNER JOIN inquilino i ON i.id = c.idInquilino
                INNER JOIN inmueble inm ON inm.id = c.idInmueble
                INNER JOIN propietario p ON p.id = inm.idPropietario
                WHERE c.estado = 1";

            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var contrato = new Contrato
                    {
                        ContratoId = reader.GetInt32("ContratoId"),
                        InquilinoId = reader.GetInt32("InquilinoId"),
                        InmuebleId = reader.GetInt32("InmuebleId"),
                        UsuarioCreadorId = reader.GetInt32("UsuarioCreadorId"),
                        FechaCreacion = reader.GetDateTime("FechaCreacion"),
                        FechaInicio = reader.GetDateTime("FechaInicio"),
                        FechaFin = reader.GetDateTime("FechaFin"),
                        Precio = reader.GetDecimal("Precio"),
                        Estado = reader.GetInt32("Estado"),
                        UsuarioAnuladorId = reader.IsDBNull(reader.GetOrdinal("UsuarioAnuladorId")) ? null : reader.GetInt32("UsuarioAnuladorId"),
                        FechaAnulacion = reader.IsDBNull(reader.GetOrdinal("FechaAnulacion")) ? null : reader.GetDateTime("FechaAnulacion"),

                        Inquilino = new Inquilino
                        {
                            InquilinoId = reader.GetInt32("InquilinoId"),
                            Nombre = reader.GetString("NombreInquilino"),
                            Apellido = reader.GetString("ApellidoInquilino"),
                            Dni = reader.GetString("DniInquilino")
                        },

                        Inmueble = new Inmueble
                        {
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            Latitud = reader.GetString("Latitud"),
                            Longitud = reader.GetString("Longitud"),
                            Precio = reader.GetDecimal("PrecioInmueble"),
                            IdPropietario = reader.GetInt32("PropietarioId"),
                            Propietario = new Propietario
                            {
                                PropietarioId = reader.GetInt32("PropietarioId"),
                                Nombre = reader.GetString("NombrePropietario"),
                                Apellido = reader.GetString("ApellidoPropietario"),
                                Dni = reader.GetString("DniPropietario")
                            }
                        }
                    };
                    contratos.Add(contrato);
                }
            }
        }
        return contratos;
    }

    public List<Contrato> ObtenerInactivos()
    {
        var contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    c.id AS ContratoId,
                    c.idInquilino AS InquilinoId,
                    c.idInmueble AS InmuebleId,
                    c.idUsuarioCreador AS UsuarioCreadorId,
                    c.fechaCreacion AS FechaCreacion,
                    c.desde AS FechaInicio,
                    c.hasta AS FechaFin,
                    c.precio AS Precio,
                    c.estado AS Estado,
                    c.idUsuarioAnulador AS UsuarioAnuladorId,
                    c.fechaAnulacion AS FechaAnulacion,

                    i.id AS InquilinoId,
                    i.nombre AS NombreInquilino,
                    i.apellido AS ApellidoInquilino,
                    i.dni AS DniInquilino,

                    inm.id AS InmuebleId,
                    inm.latitud AS Latitud,
                    inm.longitud AS Longitud,
                    inm.precio AS PrecioInmueble,
                    inm.idPropietario AS PropietarioId,

                    p.id AS PropietarioId,
                    p.nombre AS NombrePropietario,
                    p.apellido AS ApellidoPropietario,
                    p.dni AS DniPropietario
                FROM contrato c
                INNER JOIN inquilino i ON i.id = c.idInquilino
                INNER JOIN inmueble inm ON inm.id = c.idInmueble
                INNER JOIN propietario p ON p.id = inm.idPropietario
                WHERE c.estado = 0";

            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var contrato = new Contrato
                    {
                        ContratoId = reader.GetInt32("ContratoId"),
                        InquilinoId = reader.GetInt32("InquilinoId"),
                        InmuebleId = reader.GetInt32("InmuebleId"),
                        UsuarioCreadorId = reader.GetInt32("UsuarioCreadorId"),
                        FechaCreacion = reader.GetDateTime("FechaCreacion"),
                        FechaInicio = reader.GetDateTime("FechaInicio"),
                        FechaFin = reader.GetDateTime("FechaFin"),
                        Precio = reader.GetDecimal("Precio"),
                        Estado = reader.GetInt32("Estado"),
                        UsuarioAnuladorId = reader.IsDBNull(reader.GetOrdinal("UsuarioAnuladorId")) ? null : reader.GetInt32("UsuarioAnuladorId"),
                        FechaAnulacion = reader.IsDBNull(reader.GetOrdinal("FechaAnulacion")) ? null : reader.GetDateTime("FechaAnulacion"),

                        Inquilino = new Inquilino
                        {
                            InquilinoId = reader.GetInt32("InquilinoId"),
                            Nombre = reader.GetString("NombreInquilino"),
                            Apellido = reader.GetString("ApellidoInquilino"),
                            Dni = reader.GetString("DniInquilino")
                        },

                        Inmueble = new Inmueble
                        {
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            Latitud = reader.GetString("Latitud"),
                            Longitud = reader.GetString("Longitud"),
                            Precio = reader.GetDecimal("PrecioInmueble"),
                            IdPropietario = reader.GetInt32("PropietarioId"),
                            Propietario = new Propietario
                            {
                                PropietarioId = reader.GetInt32("PropietarioId"),
                                Nombre = reader.GetString("NombrePropietario"),
                                Apellido = reader.GetString("ApellidoPropietario"),
                                Dni = reader.GetString("DniPropietario")
                            }
                        }
                    };
                    contratos.Add(contrato);
                }
            }
        }
        return contratos;
    }

    public bool EstaOcupado(int idInmueble, DateTime desde, DateTime hasta)
    {
        bool ocupado = false;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT COUNT(*)
                FROM contrato c
                WHERE c.idInmueble = @idInmueble
                AND (c.desde <= @hasta AND c.hasta >= @desde)
                AND c.estado = 1";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idInmueble", idInmueble);
                command.Parameters.AddWithValue("@desde", desde);
                command.Parameters.AddWithValue("@hasta", hasta);

                connection.Open();
                var result = Convert.ToInt32(command.ExecuteScalar());
                ocupado = result > 0;//Si result es mayor a 0, entonces esta ocupado y devuelve true
            }
        }
        return ocupado;
    }

    public List<Contrato> ObtenerVigentes()
    {
        var contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    c.id AS ContratoId, 
                    c.idInquilino AS InquilinoId, 
                    c.idInmueble AS InmuebleId,
                    c.idUsuarioCreador AS UsuarioCreadorId,
                    c.fechaCreacion AS FechaCreacion,
                    c.desde AS FechaInicio, 
                    c.hasta AS FechaFin, 
                    c.precio AS Precio,
                    c.estado AS Estado,
                    c.idUsuarioAnulador AS UsuarioAnuladorId,
                    c.fechaAnulacion AS FechaAnulacion,

                    i.id AS InquilinoId,
                    i.nombre AS NombreInquilino,
                    i.apellido AS ApellidoInquilino,
                    i.dni AS DniInquilino,

                    inm.id AS InmuebleId,
                    inm.latitud AS Latitud,
                    inm.longitud AS Longitud,
                    inm.idPropietario AS PropietarioId,
                    inm.precio AS PrecioInmueble,

                    p.id AS PropietarioId,
                    p.nombre AS NombreP,
                    p.apellido AS ApellidoP,
                    p.dni AS DniP
                FROM contrato c
                INNER JOIN inquilino i ON i.id = c.idInquilino
                INNER JOIN inmueble inm ON inm.id = c.idInmueble
                INNER JOIN propietario p ON p.id = inm.idPropietario
                WHERE CURDATE() BETWEEN c.desde AND c.hasta
                AND c.estado = 1";

            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    contratos.Add(MapearContrato(reader));
                }
            }
        }
        return contratos;
    }

    public List<Contrato> ObtenerPorFecha(DateTime inicio, DateTime fin)
    {
        var contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    c.id AS ContratoId, 
                    c.idInquilino AS InquilinoId, 
                    c.idInmueble AS InmuebleId,
                    c.idUsuarioCreador AS UsuarioCreadorId,
                    c.fechaCreacion AS FechaCreacion,
                    c.desde AS FechaInicio, 
                    c.hasta AS FechaFin, 
                    c.precio AS Precio,
                    c.estado AS Estado,
                    c.idUsuarioAnulador AS UsuarioAnuladorId,
                    c.fechaAnulacion AS FechaAnulacion,

                    i.id AS InquilinoId,
                    i.nombre AS NombreInquilino,
                    i.apellido AS ApellidoInquilino,
                    i.dni AS DniInquilino,

                    inm.id AS InmuebleId,
                    inm.latitud AS Latitud,
                    inm.longitud AS Longitud,
                    inm.idPropietario AS PropietarioId,
                    inm.precio AS PrecioInmueble,

                    p.id AS PropietarioId,
                    p.nombre AS NombreP,
                    p.apellido AS ApellidoP,
                    p.dni AS DniP
                FROM contrato c
                INNER JOIN inquilino i ON i.id = c.idInquilino
                INNER JOIN inmueble inm ON inm.id = c.idInmueble
                INNER JOIN propietario p ON p.id = inm.idPropietario
                WHERE c.desde >= @inicio AND c.hasta <= @fin";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@inicio", inicio);
                command.Parameters.AddWithValue("@fin", fin);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    contratos.Add(MapearContrato(reader));
                }
            }
        }
        return contratos;
    }

    public List<Contrato> ObtenerPorInmueble(int id)
    {
        var contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    c.id AS ContratoId, 
                    c.idInquilino AS InquilinoId, 
                    c.idInmueble AS InmuebleId,
                    c.idUsuarioCreador AS UsuarioCreadorId,
                    c.fechaCreacion AS FechaCreacion,
                    c.desde AS FechaInicio, 
                    c.hasta AS FechaFin, 
                    c.precio AS Precio,
                    c.estado AS Estado,
                    c.idUsuarioAnulador AS UsuarioAnuladorId,
                    c.fechaAnulacion AS FechaAnulacion,

                    i.id AS InquilinoId,
                    i.nombre AS NombreInquilino,
                    i.apellido AS ApellidoInquilino,
                    i.dni AS DniInquilino,

                    inm.id AS InmuebleId,
                    inm.latitud AS Latitud,
                    inm.longitud AS Longitud,
                    inm.idPropietario AS PropietarioId,
                    inm.precio AS PrecioInmueble,

                    p.id AS PropietarioId,
                    p.nombre AS NombreP,
                    p.apellido AS ApellidoP,
                    p.dni AS DniP
                FROM contrato c
                INNER JOIN inquilino i ON i.id = c.idInquilino
                INNER JOIN inmueble inm ON inm.id = c.idInmueble
                INNER JOIN propietario p ON p.id = inm.idPropietario
                WHERE c.idInmueble = @id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    contratos.Add(MapearContrato(reader));
                }
            }
        }
        return contratos;
    }

    public List<Contrato> ObtenerPorInquilino(int id)
    {
        var contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    c.id AS ContratoId, 
                    c.idInquilino AS InquilinoId, 
                    c.idInmueble AS InmuebleId,
                    c.idUsuarioCreador AS UsuarioCreadorId,
                    c.fechaCreacion AS FechaCreacion,
                    c.desde AS FechaInicio, 
                    c.hasta AS FechaFin, 
                    c.precio AS Precio,
                    c.estado AS Estado,
                    c.idUsuarioAnulador AS UsuarioAnuladorId,
                    c.fechaAnulacion AS FechaAnulacion,

                    i.id AS InquilinoId,
                    i.nombre AS NombreInquilino,
                    i.apellido AS ApellidoInquilino,
                    i.dni AS DniInquilino,

                    inm.id AS InmuebleId,
                    inm.latitud AS Latitud,
                    inm.longitud AS Longitud,
                    inm.idPropietario AS PropietarioId,
                    inm.precio AS PrecioInmueble,

                    p.id AS PropietarioId,
                    p.nombre AS NombreP,
                    p.apellido AS ApellidoP,
                    p.dni AS DniP
                FROM contrato c
                INNER JOIN inquilino i ON i.id = c.idInquilino
                INNER JOIN inmueble inm ON inm.id = c.idInmueble
                INNER JOIN propietario p ON p.id = inm.idPropietario
                WHERE c.idInquilino = @id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    contratos.Add(MapearContrato(reader));
                }
            }
        }
        return contratos;
    }

    public int Alta(Contrato contrato)
    {
        int res = -1;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                INSERT INTO contrato
                (idInquilino, idInmueble, idUsuarioCreador, fechaCreacion, desde, hasta, precio)
                VALUES (@idInquilino, @idInmueble, @idUsuarioCreador, @fechaCreacion, @desde, @hasta, @precio);
                SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idInquilino", contrato.InquilinoId);
                command.Parameters.AddWithValue("@idInmueble", contrato.InmuebleId);
                command.Parameters.AddWithValue("@idUsuarioCreador", contrato.UsuarioCreadorId);
                command.Parameters.AddWithValue("@fechaCreacion", contrato.FechaCreacion);
                command.Parameters.AddWithValue("@desde", contrato.FechaInicio);
                command.Parameters.AddWithValue("@hasta", contrato.FechaFin);
                command.Parameters.AddWithValue("@precio", contrato.Precio);

                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
            }
        }
        return res;
    }

    public int Modificar(Contrato contrato)
    {
        int res = -1;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                UPDATE contrato SET
                    idInquilino = @idInquilino,
                    idInmueble = @idInmueble,
                    desde = @desde,
                    hasta = @hasta,
                    precio = @precio
                WHERE id = @id;";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", contrato.ContratoId);
                command.Parameters.AddWithValue("@idInquilino", contrato.InquilinoId);
                command.Parameters.AddWithValue("@idInmueble", contrato.InmuebleId);
                command.Parameters.AddWithValue("@desde", contrato.FechaInicio);
                command.Parameters.AddWithValue("@hasta", contrato.FechaFin);
                command.Parameters.AddWithValue("@precio", contrato.Precio);

                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }
        return res;
    }

    public int Baja(int id, int idUsuarioAnulador, DateTime fechaAnulacion)
    {
        int res = -1;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                UPDATE contrato
                SET estado = 0,
                    idUsuarioAnulador = @idUsuarioAnulador,
                    fechaAnulacion = @fechaAnulacion
                WHERE id = @id;";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@idUsuarioAnulador", idUsuarioAnulador);
                command.Parameters.AddWithValue("@fechaAnulacion", fechaAnulacion);

                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }
        return res;
    }

    public int Restore(int id)
    {
        int res = -1;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                UPDATE contrato
                SET estado = 1,
                    idUsuarioAnulador = NULL,
                    fechaAnulacion = NULL
                WHERE id = @id;";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }
        return res;
    }
    
    //Este metodo contruye el objeto Contrato, asi reutilizo el codigo
    private Contrato MapearContrato(MySqlDataReader reader)
    {
        return new Contrato
        {
            ContratoId = reader.GetInt32("ContratoId"),
            InquilinoId = reader.GetInt32("InquilinoId"),
            InmuebleId = reader.GetInt32("InmuebleId"),
            UsuarioCreadorId = reader.GetInt32("UsuarioCreadorId"),
            FechaCreacion = reader.GetDateTime("FechaCreacion"),
            FechaInicio = reader.GetDateTime("FechaInicio"),
            FechaFin = reader.GetDateTime("FechaFin"),
            Precio = reader.GetDecimal("Precio"),
            Estado = reader.GetInt32("Estado"),
            UsuarioAnuladorId = reader.IsDBNull(reader.GetOrdinal("UsuarioAnuladorId"))
                ? (int?)null : reader.GetInt32("UsuarioAnuladorId"),
            FechaAnulacion = reader.IsDBNull(reader.GetOrdinal("FechaAnulacion"))
                ? (DateTime?)null : reader.GetDateTime("FechaAnulacion"),

            Inquilino = new Inquilino
            {
                InquilinoId = reader.GetInt32("InquilinoId"),
                Nombre = reader.GetString("NombreInquilino"),
                Apellido = reader.GetString("ApellidoInquilino"),
                Dni = reader.GetString("DniInquilino")
            },
            Inmueble = new Inmueble
            {
                InmuebleId = reader.GetInt32("InmuebleId"),
                Latitud = reader.GetString("Latitud"),
                Longitud = reader.GetString("Longitud"),
                Precio = reader.GetDecimal("PrecioInmueble"),
                IdPropietario = reader.GetInt32("PropietarioId"),
                Propietario = new Propietario
                {
                    PropietarioId = reader.GetInt32("PropietarioId"),
                    Nombre = reader.GetString("NombreP"),
                    Apellido = reader.GetString("ApellidoP"),
                    Dni = reader.GetString("DniP")
                }
            }
        };
    }


}