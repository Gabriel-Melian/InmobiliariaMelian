using System.Reflection.Metadata.Ecma335;
using System.Security.Authentication;
using MySql.Data.MySqlClient;

namespace net.Models;

public class RepositorioPago : RepositorioBase
{
    public List<Pago> ObtenerTodos()//De momento no se usa, se obtienen pagos en base al contrato seleccionado en Detalles.
    {//Listar los pagos y hacer el update de los Detalles del pago unicamente.
        List<Pago> pagos = new List<Pago>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    p.id AS PagoId,
                    p.idContrato AS IdContrato, 
                    p.numero AS Numero, 
                    p.importe AS Importe, 
                    p.fecha AS Fecha, 
                    p.detalles AS Detalles,
                    p.estado AS Estado,

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
                FROM pago p
                INNER JOIN contrato c ON p.idContrato = c.id
                INNER JOIN inmueble inm ON c.idInmueble = inm.id
                INNER JOIN inquilino i ON c.idInquilino = i.id
                INNER JOIN propietario pr ON inm.idPropietario = pr.id
                ORDER BY p.fecha DESC";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    pagos.Add(new Pago
                    {
                        PagoId = reader.GetInt32("PagoId"),
                        IdContrato = reader.GetInt32("IdContrato"),
                        Numero = reader.GetInt32("Numero"),
                        Importe = reader.GetDecimal("Importe"),
                        Fecha = reader.GetDateTime("Fecha"),
                        Detalles = reader.IsDBNull(reader.GetOrdinal("Detalles")) ? null : reader.GetString("Detalles"),
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
                connection.Close();
            }
            return pagos;
        }
    }

    public Pago? ObtenerUno(int id)
    {
        Pago? pago = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    p.id AS PagoId,
                    p.idContrato AS IdContrato, 
                    p.numero AS Numero, 
                    p.importe AS Importe, 
                    p.fecha AS Fecha, 
                    p.detalles AS Detalles,
                    p.estado AS Estado,
                    p.idUsuarioCrea,
                    p.idUsuarioAnula,

                    uc.id AS UsuarioCreaId,
                    uc.nombre AS NombreUsuarioCrea,
                    uc.apellido AS ApellidoUsuarioCrea,

                    ua.id AS UsuarioAnulaId,
                    ua.nombre AS NombreUsuarioAnula,
                    ua.apellido AS ApellidoUsuarioAnula,

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
                FROM pago p
                INNER JOIN contrato c ON p.idContrato = c.id
                INNER JOIN inmueble inm ON c.idInmueble = inm.id
                INNER JOIN inquilino i ON c.idInquilino = i.id
                INNER JOIN propietario pr ON inm.idPropietario = pr.id
                LEFT JOIN usuario uc ON p.idUsuarioCrea = uc.id
                LEFT JOIN usuario ua ON p.idUsuarioAnula = ua.id
                WHERE p.id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    pago = new Pago
                    {
                        PagoId = reader.GetInt32("PagoId"),
                        IdContrato = reader.GetInt32("IdContrato"),
                        Numero = reader.GetInt32("Numero"),
                        Importe = reader.GetDecimal("Importe"),
                        Fecha = reader.GetDateTime("Fecha"),
                        Detalles = reader.IsDBNull(reader.GetOrdinal("Detalles")) ? null : reader.GetString("Detalles"),
                        Estado = reader.GetInt32("Estado"),
                        UsuarioAltaId = reader.IsDBNull(reader.GetOrdinal("idUsuarioCrea")) ? null : reader.GetInt32("idUsuarioCrea"),
                        UsuarioBajaId = reader.IsDBNull(reader.GetOrdinal("idUsuarioAnula")) ? null : reader.GetInt32("idUsuarioAnula"),

                        UsuarioAlta = reader.IsDBNull(reader.GetOrdinal("UsuarioCreaId")) ? null : new Usuario
                        {
                            UsuarioId = reader.GetInt32("UsuarioCreaId"),
                            Nombre = reader.GetString("NombreUsuarioCrea"),
                            Apellido = reader.GetString("ApellidoUsuarioCrea")
                        },
                        UsuarioBaja = reader.IsDBNull(reader.GetOrdinal("UsuarioAnulaId")) ? null : new Usuario
                        {
                            UsuarioId = reader.GetInt32("UsuarioAnulaId"),
                            Nombre = reader.GetString("NombreUsuarioAnula"),
                            Apellido = reader.GetString("ApellidoUsuarioAnula")
                        },

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
                connection.Close();
            }
            return pago;
        }
    }

    public List<Pago> ObtenerPorContrato(int id)
    {
        List<Pago> pagos = new List<Pago>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    p.id AS PagoId,
                    p.idContrato AS IdContrato, 
                    p.numero AS Numero, 
                    p.importe AS Importe, 
                    p.fecha AS Fecha, 
                    p.detalles AS Detalles,
                    p.estado AS Estado,
                    p.idUsuarioCrea,
                    p.idUsuarioAnula,

                    uc.id AS UsuarioCreaId,
                    uc.nombre AS NombreUsuarioCrea,
                    uc.apellido AS ApellidoUsuarioCrea,

                    ua.id AS UsuarioAnulaId,
                    ua.nombre AS NombreUsuarioAnula,
                    ua.apellido AS ApellidoUsuarioAnula,

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
                FROM pago p
                INNER JOIN contrato c ON p.idContrato = c.id
                INNER JOIN inmueble inm ON c.idInmueble = inm.id
                INNER JOIN inquilino i ON c.idInquilino = i.id
                INNER JOIN propietario pr ON inm.idPropietario = pr.id
                LEFT JOIN usuario uc ON p.idUsuarioCrea = uc.id
                LEFT JOIN usuario ua ON p.idUsuarioAnula = ua.id
                WHERE p.idContrato = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    pagos.Add(new Pago
                    {
                        PagoId = reader.GetInt32("PagoId"),
                        IdContrato = reader.GetInt32("IdContrato"),
                        Numero = reader.GetInt32("Numero"),
                        Importe = reader.GetDecimal("Importe"),
                        Fecha = reader.GetDateTime("Fecha"),
                        Detalles = reader.IsDBNull(reader.GetOrdinal("Detalles")) ? null : reader.GetString("Detalles"),
                        Estado = reader.GetInt32("Estado"),
                        UsuarioAltaId = reader.IsDBNull(reader.GetOrdinal("idUsuarioCrea")) ? null : reader.GetInt32("idUsuarioCrea"),
                        UsuarioBajaId = reader.IsDBNull(reader.GetOrdinal("idUsuarioAnula")) ? null : reader.GetInt32("idUsuarioAnula"),

                        UsuarioAlta = reader.IsDBNull(reader.GetOrdinal("UsuarioCreaId")) ? null : new Usuario
                        {
                            UsuarioId = reader.GetInt32("UsuarioCreaId"),
                            Nombre = reader.GetString("NombreUsuarioCrea"),
                            Apellido = reader.GetString("ApellidoUsuarioCrea")
                        },
                        UsuarioBaja = reader.IsDBNull(reader.GetOrdinal("UsuarioAnulaId")) ? null : new Usuario
                        {
                            UsuarioId = reader.GetInt32("UsuarioAnulaId"),
                            Nombre = reader.GetString("NombreUsuarioAnula"),
                            Apellido = reader.GetString("ApellidoUsuarioAnula")
                        },

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
                connection.Close();
            }
        }
        return pagos;
    }

    public int Agregar(Pago pago)
    {
        var res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"INSERT INTO pago
                (idcontrato,
                numero,
                importe, 
                fecha, 
                detalles,
                idUsuarioCrea)
                VALUES
                (@idcontrato, @numero, @importe, @fecha, @detalles, @idUsuarioCrea);
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idcontrato", pago.IdContrato);
                command.Parameters.AddWithValue("@numero", pago.Numero);
                command.Parameters.AddWithValue("@importe", pago.Importe);
                command.Parameters.AddWithValue("@fecha", pago.Fecha);
                command.Parameters.AddWithValue("@detalles", pago.Detalles);
                command.Parameters.AddWithValue("@idUsuarioCrea", pago.UsuarioAltaId);
                connection.Open();//Ojo aca, estaba ejecutando tambien command.ExecuteNonQuery(); y se duplicaba el pago
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res;
    }

    public int Modificar(Pago pago)
    {
        var res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"UPDATE pago SET 
            detalles = @detalles
            WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@detalles", pago.Detalles);
                command.Parameters.AddWithValue("@id", pago.PagoId);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public int Pendiente(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"UPDATE pago
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

    public int Anulado(int id, int idUsuarioAnula)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"UPDATE pago
            SET idUsuarioAnula = @idUsuarioAnula,
            estado = 2
            WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@idUsuarioAnula", idUsuarioAnula);
                connection.Open();

                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }
    
    public int Pagado(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = $@"UPDATE pago
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