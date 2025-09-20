namespace net.Models;
using System.ComponentModel.DataAnnotations;

public class Multa
{
    public enum EstadoMulta
    {
        Activa = 1,
        Resuelta = 0,
        Anulada = 2
    }
    public int MultaId { get; set; }
    [Required]
    public int IdContrato { get; set; }
    [Required]
    public DateTime FechaAviso { get; set; }
    [Required]
    public DateTime FechaTerminacion { get; set; }
    [Required]
    public decimal Monto { get; set; }
    public int Estado { get; set; }
    public EstadoMulta Tipo => (EstadoMulta)Estado;

    //Relacion con el contrato
    public Contrato? Contrato { get; set; }
}