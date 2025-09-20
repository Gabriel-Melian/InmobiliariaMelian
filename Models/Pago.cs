namespace net.Models;
using System.ComponentModel.DataAnnotations;

public class Pago
{
    public enum EstadoPago
    {
        Pendiente = 0,
        Pagado = 1,
        Anulado = 2
    }
    public int PagoId { get; set; }
    public int IdContrato { get; set; }
    public int Numero { get; set; }
    public decimal Importe { get; set; }
    public DateTime Fecha { get; set; }
    public string? Detalles { get; set; }
    public int Estado { get; set; }

    public EstadoPago Tipo => (EstadoPago)Estado;

    //Relacion con el contrato
    public Contrato? Contrato { get; set; }

    public override string ToString()
    {
        return $"Pago Nº {Numero} - {Importe:C} - {Fecha:dd/MM/yyyy}";
    }
}