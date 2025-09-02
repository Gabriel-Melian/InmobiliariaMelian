namespace net.Models;

public class TipoInmueble
{
	public int TipoId { get; set; }

	public string? Valor { get; set; }

	public override string ToString()
	{
		var res = $"{Valor}";
		return res;
	}

	//Tipo: Residencial, Comercial, Industrial, Terreno, Otro.
	//Uso: Alquiler, Venta, Arriendo, Otros.
}