namespace net.Models;

public class UsoInmueble
{
    public int UsoId { get; set; }

    public string? UsoValor { get; set; }

    public override string ToString()
		{
			var res = $"{UsoValor}";
			return res;
		}
}