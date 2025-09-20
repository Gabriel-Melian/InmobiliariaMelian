namespace net.Models;
using System.ComponentModel.DataAnnotations;

public class Contrato
{
    public enum EstadoContrato
    {
        Inactivo = 0,
        Vigente = 1,
        Anulado = 2
    }

    public int ContratoId { get; set; }

    public int InquilinoId { get; set; }
    public int InmuebleId { get; set; }
    public int UsuarioCreadorId { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El precio debe ser un número entero positivo")]
    public decimal Precio { get; set; }
    public int Estado { get; set; }

    public EstadoContrato Tipo => (EstadoContrato)Estado;

    //Trazabilidad
    public int? UsuarioAnuladorId { get; set; }
    public DateTime? FechaAnulacion { get; set; }

    //Relaciones
    public Inquilino? Inquilino { get; set; }
    public Inmueble? Inmueble { get; set; }
    public Usuario? Creador { get; set; }
    public Usuario? Anulador { get; set; }

    //Relacion con clase Pago -> 1:N
    public List<Pago>? Pagos { get; set; }
    public List<Multa>? Multas { get; set; }

    public override string ToString()
    {
        return $"ContratoId: {ContratoId}, InquilinoId: {InquilinoId}, InmuebleId: {InmuebleId}, " +
               $"UsuarioCreadorId: {UsuarioCreadorId}, FechaInicio: {FechaInicio}, FechaFin: {FechaFin}, " +
               $"Precio: {Precio}, Estado: {Estado}, FechaCreacion: {FechaCreacion}, " +
               $"UsuarioAnuladorId: {UsuarioAnuladorId}, FechaAnulacion: {FechaAnulacion}";
    }
}