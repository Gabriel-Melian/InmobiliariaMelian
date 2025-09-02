using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net.Models;
using Microsoft.AspNetCore.Mvc.Rendering;//Para trabajar con listas desplegables (SelectList)

namespace net.Controllers;
//[Authorize]
public class InmuebleController : Controller
{
    private readonly ILogger<InmuebleController> _logger;
    private RepositorioInmueble repo = new RepositorioInmueble();
    private RepositorioPropietario repoPropietario = new RepositorioPropietario();
    private RepositorioTipo repoTipo = new RepositorioTipo();
    private RepositorioUso repoUso = new RepositorioUso();
    //private RepositorioContrato repoContrato = new RepositorioContrato();

    public InmuebleController(ILogger<InmuebleController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(int estado = -1, DateTime? desde = null, DateTime? hasta = null)
    {
        List<Inmueble> lista;
        if (desde.HasValue && hasta.HasValue)
        {
            lista = repo.ObtenerTodos();
            //Los disponibles se obtienen una vez tenga las fechas de inicio y fin, de momento muestro todos.
            //lista = repo.ObtenerDisponibles(desde.Value.ToString("yyyy-MM-dd"), hasta.Value.ToString("yyyy-MM-dd"));
        }
        else
        {
            lista = repo.ObtenerTodos();
        }
        if (estado != -1)
        {
            if (estado == 1)
            {
                lista = repo.ObtenerActivos();
            }
            else if (estado == 0)
            {
                lista = repo.ObtenerInactivos();
            }
        }
        return View(lista);
    }

    public IActionResult Detalle(int id)
    {
        if (id == 0)
            return View();
        else
        {
            var inmueble = repo.ObtenerUno(id);
            //var propietario = repoPropietario.ObtenerUno(inmueble.IdPropietario);
            //var uso = repoUso.ObtenerUno(inmueble.IdUso);
            //var tipo = repoTipo.ObtenerUno(inmueble.IdTipo);
            //ViewBag.Contratos = repoContrato.ObtenerPorInmueble(id);
            //inmueble.Propietario = propietario;
            //inmueble.UsoInmueble = uso;
            //inmueble.TipoInmueble = tipo;
            return View(inmueble);
        }
    }

    public IActionResult Edicion(int id)
    {
        var inmueble = id == 0 ? new Inmueble() : repo.ObtenerUno(id);

        //Propietarios activos para evitar inconsistencias
        //Armamos las propiedades del SelectList para mostrar nombre, apellido y dni.
        var propietarios = repoPropietario.ObtenerActivos()
            .Select(p => new
            {
                p.PropietarioId,
                Display = $"{p.Nombre} {p.Apellido} - DNI: {p.Dni}"
            }).ToList();
        //Aca le pasamos el Display
        ViewBag.Propietarios = new SelectList(propietarios, "PropietarioId", "Display", inmueble?.IdPropietario);

        //Usos
        var usos = repoUso.ObtenerTodos();
        ViewBag.Usos = new SelectList(usos, "UsoId", "UsoValor", inmueble?.IdUso);

        //Tipos
        var tipos = repoTipo.ObtenerTodos();
        ViewBag.Tipos = new SelectList(tipos, "TipoId", "Valor", inmueble?.IdTipo);

        return View(inmueble);
    }

    [HttpPost]
    public IActionResult Guardar(Inmueble inmueble)
    {
        //repo.Alta(inmueble);
        //return RedirectToAction("Index");

        if (!ModelState.IsValid)
        {
            return View("Edicion", inmueble);
        }
        else if (inmueble.InmuebleId == 0)
        {
            repo.Alta(inmueble);
            TempData["Mensaje"] = "Inmueble guardado";
        }
        else
        {
            repo.Modificar(inmueble);
            TempData["Mensaje"] = "Cambios guardados";
        }
        return RedirectToAction("Index");
    }

    public IActionResult Eliminar(int id)//Esto solo lo haria el admin
    {
        //if(!User.IsInRole("Administrador")){//Falta autenticacion
        //TempData["Error"] = "Acceso denegado";
        //return Redirect("/Home/Index");
        //}
        int res = repo.Baja(id);
        if (res == -1)
            TempData["Error"] = "No se pudo eliminar el inmueble";
        else if (res == 0)
            TempData["Error"] = "No se encontró el inmueble";
        else
            TempData["Mensaje"] = "El inmueble se elimino";
        return RedirectToAction("Index");
    }

    public IActionResult Activar(int id)//Esto solo lo haria el admin
    {
        //if(!User.IsInRole("Administrador")){//Falta autenticacion
        //TempData["Error"] = "Acceso denegado";
        //return Redirect("/Home/Index");
        //}
        if(!Verificaciones(id))
            return RedirectToAction("Index");

        int res = repo.Restore(id);
        if (res == -1)
            TempData["Error"] = "No se pudo activar el inmueble";
        else
            TempData["Mensaje"] = "El inmueble se activo";
        return RedirectToAction("Index");
    }
    
    public bool Verificaciones(int id){
        Inmueble inmueble = repo.ObtenerUno(id);
        if(inmueble==null){
            TempData["Error"] = "No se encontró el inmueble";
            return false;
        }

        Propietario propietario = repoPropietario.ObtenerUno(inmueble.IdPropietario);
        if(propietario == null || propietario.Estado == 0){
            TempData["Error"] = "El propietario no se encuentra activo";
            return false;
        }

        return true;
    }
}