namespace net.Models;
using System.ComponentModel.DataAnnotations;

public class Usuario
{
    public enum Roles
    {
        Administrador = 1,
        Empleado = 2,
    }

    public int UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Rol { get; set; }
    public string Dni { get; set; }
    public string? Avatar { get; set; }

    //Esto es para verificaciones en codigo if(usuario.RolEnum == Usuario.Roles.Administrador){}
    public Roles RolEnum => (Roles)Rol;

    public override string ToString()
    {
        return $"{Nombre} {Apellido} {Dni}";
    }
}