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
                c.idUsuario AS UsuarioId,
                c.desde AS FechaInicio,
                c.hasta AS FechaFin,
                c.precio AS Precio,
                c.estado AS Estado,
                
                i.id AS InquilinoId,
                i.nombre AS Nombre,
                i.apellido AS Apellido,
                i.dni AS Dni,
                
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
                        UsuarioId = reader.GetInt32("UsuarioId"),
                        FechaInicio = reader.GetDateTime("FechaInicio"),
                        FechaFin = reader.GetDateTime("FechaFin"),
                        Precio = reader.GetDecimal("Precio"),
                        Estado = reader.GetInt32("Estado"),

                        Inquilino = new Inquilino
                        {
                            InquilinoId = reader.GetInt32("InquilinoId"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Dni = reader.GetString("Dni")
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

}