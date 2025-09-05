namespace net.Models;

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
    public int UsuarioId { get; set; }

    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

    public decimal Precio { get; set; }
    public int Estado { get; set; }

    public EstadoContrato Tipo => (EstadoContrato)Estado;

    //Trazabilidad
    public int? AnuladorId { get; set; }
    public DateTime? FechaAnulacion { get; set; }

    //Relaciones
    public Inquilino? Inquilino { get; set; }
    public Inmueble? Inmueble { get; set; }
    //public Usuario? Creador { get; set; }
    //public Usuario? Anulador { get; set; }

    //Relacion con clase Pago -> 1:N
    //public List<Pago> Pagos { get; set; }

    public override string ToString()
    {
        return $"ContratoId: {ContratoId}, InquilinoId: {InquilinoId}, InmuebleId: {InmuebleId}, " +
            $"UsuarioId: {UsuarioId}, FechaInicio: {FechaInicio}, FechaFin: {FechaFin}, Precio: {Precio}, Estado: {Estado}";
    }
}