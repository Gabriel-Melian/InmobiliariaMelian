using MySql.Data.MySqlClient;

namespace net.Models;

public class RepositorioInmueble : RepositorioBase
{
    public List<Inmueble> ObtenerTodos()
    {
        List<Inmueble> inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
            SELECT 
                i.id AS InmuebleId,
                i.direccion AS Direccion,
                i.latitud AS Latitud,
                i.longitud AS Longitud,
                i.idPropietario AS IdPropietario,
                i.idUsoInmueble AS IdUso,
                i.idTipoInmueble AS IdTipo,
                i.ambientes AS Ambientes,
                i.precio AS Precio,
                i.estado AS Estado,

                p.id AS PropietarioId,
                p.nombre AS Nombre,
                p.apellido AS Apellido,
                p.dni AS Dni,

                t.id AS TipoId,
                t.valor AS Valor,

                u.id AS UsoId,
                u.valor AS UsoValor

            FROM inmueble i
            INNER JOIN propietario p ON i.idPropietario = p.id
            INNER JOIN tipoinmueble t ON i.idTipoInmueble = t.id
            INNER JOIN usoinmueble u ON i.idUsoInmueble = u.id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inmuebles.Add(new Inmueble
                    {
                        InmuebleId = reader.GetInt32(nameof(Inmueble.InmuebleId)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                        IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                        IdUso = reader.GetInt32(nameof(Inmueble.IdUso)),
                        IdTipo = reader.GetInt32(nameof(Inmueble.IdTipo)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Estado = reader.GetInt32(nameof(Inmueble.Estado)),

                        Propietario = new Propietario
                        {
                            PropietarioId = reader.GetInt32(nameof(Propietario.PropietarioId)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Dni = reader.GetString(nameof(Propietario.Dni))
                        },
                        UsoInmueble = new UsoInmueble
                        {
                            UsoId = reader.GetInt32(nameof(UsoInmueble.UsoId)),
                            UsoValor = reader.GetString(nameof(UsoInmueble.UsoValor))
                        },
                        TipoInmueble = new TipoInmueble
                        {
                            TipoId = reader.GetInt32(nameof(TipoInmueble.TipoId)),
                            Valor = reader.GetString(nameof(TipoInmueble.Valor))
                        }
                    });
                }
            }
        }
        return inmuebles;
    }

    public List<Inmueble> ObtenerActivos()
    {
        List<Inmueble> inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    i.id            AS InmuebleId,
                    i.direccion     AS Direccion,
                    i.latitud       AS Latitud,
                    i.longitud      AS Longitud,
                    i.idPropietario AS IdPropietario,
                    i.idUsoInmueble AS IdUso,
                    i.idTipoInmueble AS IdTipo,
                    i.ambientes     AS Ambientes,
                    i.precio        AS Precio,
                    i.estado        AS Estado,

                    p.id            AS PropietarioId,
                    p.nombre        AS Nombre,
                    p.apellido      AS Apellido,
                    p.dni           AS Dni,
                    p.estado        AS PropietarioEstado,

                    t.id            AS TipoId,
                    t.valor         AS Valor,

                    u.id            AS UsoId,
                    u.valor         AS UsoValor
                FROM inmueble i
                INNER JOIN propietario p   ON i.idPropietario   = p.id
                INNER JOIN tipoinmueble t  ON i.idTipoInmueble  = t.id
                INNER JOIN usoinmueble u   ON i.idUsoInmueble   = u.id
                WHERE i.estado = 1 AND p.estado = 1";//Devuelve inmuebles activos con propietarios activos

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inmuebles.Add(new Inmueble
                    {
                        InmuebleId = reader.GetInt32(nameof(Inmueble.InmuebleId)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                        IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                        IdUso = reader.GetInt32(nameof(Inmueble.IdUso)),
                        IdTipo = reader.GetInt32(nameof(Inmueble.IdTipo)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Estado = reader.GetInt32(nameof(Inmueble.Estado)),

                        Propietario = new Propietario
                        {
                            PropietarioId = reader.GetInt32(nameof(Propietario.PropietarioId)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Dni = reader.GetString(nameof(Propietario.Dni)),
                            Estado = reader.GetInt32(nameof(Propietario.Estado))
                        },
                        UsoInmueble = new UsoInmueble
                        {
                            UsoId = reader.GetInt32(nameof(UsoInmueble.UsoId)),
                            UsoValor = reader.GetString(nameof(UsoInmueble.UsoValor))
                        },
                        TipoInmueble = new TipoInmueble
                        {
                            TipoId = reader.GetInt32(nameof(TipoInmueble.TipoId)),
                            Valor = reader.GetString(nameof(TipoInmueble.Valor))
                        }
                    });
                }
            }
        }
        return inmuebles;
    }

    public List<Inmueble> ObtenerDisponibles(String fechaDesde, String fechaHasta)//Que yo sepa quedo funcionando
    {
        List<Inmueble> inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    i.id            AS InmuebleId,
                    i.direccion     AS Direccion,
                    i.latitud       AS Latitud,
                    i.longitud      AS Longitud,
                    i.idPropietario AS IdPropietario,
                    i.idUsoInmueble AS IdUso,
                    i.idTipoInmueble AS IdTipo,
                    i.ambientes     AS Ambientes,
                    i.precio        AS Precio,
                    i.estado        AS Estado,

                    p.id            AS PropietarioId,
                    p.nombre        AS Nombre,
                    p.apellido      AS Apellido,
                    p.dni           AS Dni,

                    t.id            AS TipoId,
                    t.valor         AS Valor,

                    u.id            AS UsoId,
                    u.valor         AS UsoValor
                FROM inmueble i
                INNER JOIN propietario p   ON i.idPropietario   = p.id
                INNER JOIN tipoinmueble t  ON i.idTipoInmueble  = t.id
                INNER JOIN usoinmueble u   ON i.idUsoInmueble   = u.id
                WHERE i.estado = 1 AND p.estado = 1 AND i.id NOT IN (
                    SELECT idInmueble
                    FROM contrato
                    WHERE desde <= @fechaDesde AND hasta >= @fechaHasta
                )";//Devuelve inmuebles activos con propietarios activos y que no esten reservados

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fechaDesde", fechaDesde);
                command.Parameters.AddWithValue("@fechaHasta", fechaHasta);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inmuebles.Add(new Inmueble
                    {
                        InmuebleId = reader.GetInt32(nameof(Inmueble.InmuebleId)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                        IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                        IdUso = reader.GetInt32(nameof(Inmueble.IdUso)),
                        IdTipo = reader.GetInt32(nameof(Inmueble.IdTipo)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Estado = reader.GetInt32(nameof(Inmueble.Estado)),

                        Propietario = new Propietario
                        {
                            PropietarioId = reader.GetInt32(nameof(Propietario.PropietarioId)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Dni = reader.GetString(nameof(Propietario.Dni)),
                            Estado = reader.GetInt32(nameof(Propietario.Estado))
                        },
                        UsoInmueble = new UsoInmueble
                        {
                            UsoId = reader.GetInt32(nameof(UsoInmueble.UsoId)),
                            UsoValor = reader.GetString(nameof(UsoInmueble.UsoValor))
                        },
                        TipoInmueble = new TipoInmueble
                        {
                            TipoId = reader.GetInt32(nameof(TipoInmueble.TipoId)),
                            Valor = reader.GetString(nameof(TipoInmueble.Valor))
                        }
                    });
                }
            }
        }
        return inmuebles;
    }

    public List<Inmueble> ObtenerInactivos()
    {
        List<Inmueble> inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    i.id            AS InmuebleId,
                    i.direccion     AS Direccion,
                    i.latitud       AS Latitud,
                    i.longitud      AS Longitud,
                    i.idPropietario AS IdPropietario,
                    i.idUsoInmueble AS IdUso,
                    i.idTipoInmueble AS IdTipo,
                    i.ambientes     AS Ambientes,
                    i.precio        AS Precio,
                    i.estado        AS Estado,

                    p.id            AS PropietarioId,
                    p.nombre        AS Nombre,
                    p.apellido      AS Apellido,
                    p.dni           AS Dni,

                    t.id            AS TipoId,
                    t.valor         AS Valor,

                    u.id            AS UsoId,
                    u.valor         AS UsoValor
                FROM inmueble i
                INNER JOIN propietario p   ON i.idPropietario   = p.id
                INNER JOIN tipoinmueble t  ON i.idTipoInmueble  = t.id
                INNER JOIN usoinmueble u   ON i.idUsoInmueble   = u.id
                WHERE i.estado = 0";//Devuelve inmuebles inactivos

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inmuebles.Add(new Inmueble
                    {
                        InmuebleId = reader.GetInt32(nameof(Inmueble.InmuebleId)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                        IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                        IdUso = reader.GetInt32(nameof(Inmueble.IdUso)),
                        IdTipo = reader.GetInt32(nameof(Inmueble.IdTipo)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Estado = reader.GetInt32(nameof(Inmueble.Estado)),

                        Propietario = new Propietario
                        {
                            PropietarioId = reader.GetInt32(nameof(Propietario.PropietarioId)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Dni = reader.GetString(nameof(Propietario.Dni)),
                            Estado = reader.GetInt32(nameof(Propietario.Estado))
                        },
                        UsoInmueble = new UsoInmueble
                        {
                            UsoId = reader.GetInt32(nameof(UsoInmueble.UsoId)),
                            UsoValor = reader.GetString(nameof(UsoInmueble.UsoValor))
                        },
                        TipoInmueble = new TipoInmueble
                        {
                            TipoId = reader.GetInt32(nameof(TipoInmueble.TipoId)),
                            Valor = reader.GetString(nameof(TipoInmueble.Valor))
                        }
                    });
                }
            }
        }
        return inmuebles;
    }

    public Inmueble? ObtenerUno(int id)
    {
        Inmueble? inmueble = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    i.id            AS InmuebleId,
                    i.direccion     AS Direccion,
                    i.latitud       AS Latitud,
                    i.longitud      AS Longitud,
                    i.idPropietario AS IdPropietario,
                    i.idUsoInmueble AS IdUso,
                    i.idTipoInmueble AS IdTipo,
                    i.ambientes     AS Ambientes,
                    i.precio        AS Precio,
                    i.estado        AS Estado,

                    p.id            AS PropietarioId,
                    p.nombre        AS Nombre,
                    p.apellido      AS Apellido,
                    p.dni           AS Dni,

                    t.id            AS TipoId,
                    t.valor         AS Valor,

                    u.id            AS UsoId,
                    u.valor         AS UsoValor
                FROM inmueble i
                INNER JOIN propietario p   ON i.idPropietario   = p.id
                INNER JOIN tipoinmueble t  ON i.idTipoInmueble  = t.id
                INNER JOIN usoinmueble u   ON i.idUsoInmueble   = u.id
                WHERE i.id = @id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    inmueble = new Inmueble
                    {
                        InmuebleId = reader.GetInt32(nameof(Inmueble.InmuebleId)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                        IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                        IdUso = reader.GetInt32(nameof(Inmueble.IdUso)),
                        IdTipo = reader.GetInt32(nameof(Inmueble.IdTipo)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Estado = reader.GetInt32(nameof(Inmueble.Estado)),

                        Propietario = new Propietario
                        {
                            PropietarioId = reader.GetInt32(nameof(Propietario.PropietarioId)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Dni = reader.GetString(nameof(Propietario.Dni)),
                            Estado = reader.GetInt32(nameof(Propietario.Estado))
                        },
                        UsoInmueble = new UsoInmueble
                        {
                            UsoId = reader.GetInt32(nameof(UsoInmueble.UsoId)),
                            UsoValor = reader.GetString(nameof(UsoInmueble.UsoValor))
                        },
                        TipoInmueble = new TipoInmueble
                        {
                            TipoId = reader.GetInt32(nameof(TipoInmueble.TipoId)),
                            Valor = reader.GetString(nameof(TipoInmueble.Valor))
                        }
                    };
                }
            }
        }
        return inmueble;
    }

    public List<Inmueble>? ObtenerPorPropietario(int id)
    {
        List<Inmueble> inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                SELECT 
                    i.id            AS InmuebleId,
                    i.direccion     AS Direccion,
                    i.latitud       AS Latitud,
                    i.longitud      AS Longitud,
                    i.idPropietario AS IdPropietario,
                    i.idUsoInmueble AS IdUso,
                    i.idTipoInmueble AS IdTipo,
                    i.ambientes     AS Ambientes,
                    i.precio        AS Precio,
                    i.estado        AS Estado,

                    p.id            AS PropietarioId,
                    p.nombre        AS Nombre,
                    p.apellido      AS Apellido,
                    p.dni           AS Dni,
                    p.estado        AS PropietarioEstado,

                    t.id            AS TipoId,
                    t.valor         AS Valor,

                    u.id            AS UsoId,
                    u.valor         AS UsoValor
                FROM inmueble i
                INNER JOIN propietario p   ON i.idPropietario   = p.id
                INNER JOIN tipoinmueble t  ON i.idTipoInmueble  = t.id
                INNER JOIN usoinmueble u   ON i.idUsoInmueble   = u.id
                WHERE i.idPropietario = @id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inmuebles.Add(new Inmueble
                    {
                        InmuebleId = reader.GetInt32(nameof(Inmueble.InmuebleId)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                        IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                        IdUso = reader.GetInt32(nameof(Inmueble.IdUso)),
                        IdTipo = reader.GetInt32(nameof(Inmueble.IdTipo)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Estado = reader.GetInt32(nameof(Inmueble.Estado)),

                        Propietario = new Propietario
                        {
                            PropietarioId = reader.GetInt32(nameof(Propietario.PropietarioId)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Dni = reader.GetString(nameof(Propietario.Dni)),
                            Estado = reader.GetInt32(nameof(Propietario.Estado))
                        },
                        UsoInmueble = new UsoInmueble
                        {
                            UsoId = reader.GetInt32(nameof(UsoInmueble.UsoId)),
                            UsoValor = reader.GetString(nameof(UsoInmueble.UsoValor))
                        },
                        TipoInmueble = new TipoInmueble
                        {
                            TipoId = reader.GetInt32(nameof(TipoInmueble.TipoId)),
                            Valor = reader.GetString(nameof(TipoInmueble.Valor))
                        }
                    });
                }
            }
        }
        return inmuebles;
    }

    public int Alta(Inmueble inmueble)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                INSERT INTO inmueble 
                (direccion,
                latitud, 
                longitud, 
                idPropietario, 
                idUsoInmueble, 
                idTipoInmueble, 
                ambientes, 
                precio)
                VALUES (@direccion, @latitud, @longitud, @idPropietario, @idUsoInmueble, @idTipoInmueble, @ambientes, @precio);
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
                command.Parameters.AddWithValue("@latitud", inmueble.Latitud);
                command.Parameters.AddWithValue("@longitud", inmueble.Longitud);
                command.Parameters.AddWithValue("@idPropietario", inmueble.IdPropietario);
                command.Parameters.AddWithValue("@idUsoInmueble", inmueble.IdUso);
                command.Parameters.AddWithValue("@idTipoInmueble", inmueble.IdTipo);
                command.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
                command.Parameters.AddWithValue("@precio", inmueble.Precio);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res;
    }

    public int Modificar(Inmueble inmueble)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
                UPDATE inmueble
                SET direccion = @direccion,
                    latitud = @latitud,
                    longitud = @longitud,
                    idPropietario = @idPropietario,
                    idUsoInmueble = @idUsoInmueble,
                    idTipoInmueble = @idTipoInmueble,
                    ambientes = @ambientes,
                    precio = @precio
                WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
                command.Parameters.AddWithValue("@latitud", inmueble.Latitud);
                command.Parameters.AddWithValue("@longitud", inmueble.Longitud);
                command.Parameters.AddWithValue("@idPropietario", inmueble.IdPropietario);
                command.Parameters.AddWithValue("@idUsoInmueble", inmueble.IdUso);
                command.Parameters.AddWithValue("@idTipoInmueble", inmueble.IdTipo);
                command.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
                command.Parameters.AddWithValue("@precio", inmueble.Precio);
                command.Parameters.AddWithValue("@id", inmueble.InmuebleId);
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
            var query = @"
                UPDATE inmueble
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
            var query = @"
                UPDATE inmueble
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

    //Falta ObtenerDisponibles(Que este filtra los disponibles entre dos fechas, inicio y fin)

}