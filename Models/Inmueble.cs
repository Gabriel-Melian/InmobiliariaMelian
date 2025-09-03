namespace net.Models;
using System.ComponentModel.DataAnnotations;

public class Inmueble
{
    public int InmuebleId { get; set; }

    [Required(ErrorMessage = "La latitud es obligatoria.")]
    public string? Latitud { get; set; }

    [Required(ErrorMessage = "La longitud es obligatoria.")]
    public string? Longitud { get; set; }

    [Required(ErrorMessage = "Seleccione un propietario.")]
    public int IdPropietario { get; set; }

    [Required(ErrorMessage = "Seleccione un uso.")]
    public int IdUso { get; set; }

    [Required(ErrorMessage = "Seleccione un tipo de inmueble.")]
    public int IdTipo { get; set; }

    [Required(ErrorMessage = "La cantidad de ambientes es obligatoria.")]
    public int? Ambientes { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
    public decimal? Precio { get; set; }

    public int? Estado { get; set; }
    
    public Propietario? Propietario { get; set; }//Propiedad de navegacion

    public TipoInmueble? TipoInmueble { get; set; }

    public UsoInmueble? UsoInmueble { get; set; }

    public override string ToString()
    {
        var res = $"{Latitud} {Longitud} {IdPropietario} {IdUso} {IdTipo} {Ambientes} {Precio} {Estado}";
        return res;
    }
}