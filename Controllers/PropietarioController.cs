using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net.Models;

namespace net.Controllers;
//[Authorize]
public class PropietarioController : Controller
{
    private readonly ILogger<PropietarioController> _logger;
    private RepositorioPropietario repo = new RepositorioPropietario();

    private RepositorioInmueble repoInmueble = new RepositorioInmueble();

    public PropietarioController(ILogger<PropietarioController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(int? estado)
    {
        List<Propietario> lista;
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
            else
            {
                lista = repo.ObtenerTodos();
            }
        }
        else
        {
            lista = repo.ObtenerTodos();
        }
        return View(lista);
    }

    public IActionResult Detalle(int id)
    {
        if (id == 0)
            return View();
        else
        {
            var propietario = repo.ObtenerUno(id);
            //ViewBag es "la bolsa de la vista" que me permite pasar informacion a la vista,
            //son emcapsulamientos de una misma estructura
            //ViewData es un String que me permite pasar informacion a la vista

            var inmuebles = repoInmueble.ObtenerPorPropietario(id);
            propietario.Inmuebles = inmuebles;
            
            //ViewBag.Inmuebles = repoInmueble.ObtenerPorPropietario(id);
            return View(propietario);
        }
    }

    [HttpPost]
    public IActionResult Guardar(int id, Propietario propietario)
    {
        if (!ModelState.IsValid)
        {
            return View("Edicion", propietario);
        }
        id = propietario.PropietarioId;
        if (repo.VerificarPropietario(propietario.Nombre, propietario.Apellido, propietario.Dni) && id == 0)
        {
            TempData["Error"] = "Ya existe un propietario con el mismo nombre, apellido y DNI.";
            return View("Edicion", propietario);
        }
        if (id == 0)
        {
            repo.Alta(propietario);
            TempData["Mensaje"] = "Propietario guardado";
        }
        else
        {
            repo.Modificar(propietario);
            TempData["Mensaje"] = "Cambios guardados";
        }
        return RedirectToAction("Index");
    }

    public IActionResult Edicion(int id)
    {
        if (id == 0)
            return View();
        else
        {
            var propietario = repo.ObtenerUno(id);
            return View(propietario);
        }
    }

    public IActionResult Eliminar(int id)
    {
        //if (!User.IsInRole("Administrador"))
        //{
        //TempData["Error"] = "Acceso denegado";
        //return Redirect("/Home/Index");
        //}


        //List<Inmueble> inmuebles = repoInmueble.ObtenerPorPropietario(id);//Para que cuando de de baja un propietario sus inm queden inactivos
        //foreach (var item in inmuebles)
        //{
        //repoInmueble.Baja(item.InmuebleId);
        //}
        int res = repo.Baja(id);
        if (res == -1)
            TempData["Error"] = "No se pudo eliminar el propietario";
        else
            //TempData["Mensaje"] = "El propietario se elimino y sus inmuebles también";
            TempData["Mensaje"] = "El propietario se elimino.";
        return RedirectToAction("Index");

    }
    
    public IActionResult Activar(int id)
    {
        //if (!User.IsInRole("Administrador"))
        //{
            //TempData["Error"] = "Acceso denegado";
            //return Redirect("/Home/Index");
        //}
        //primero me fijo que exista
        if (repo.ObtenerUno(id) == null)
        {
            TempData["Error"] = "No se encontro el propietario";
            return RedirectToAction("Index");
        }
        //y despues lo restaura
        int res = repo.Restore(id);
        if (res == -1)
            TempData["Error"] = "No se pudo activar el propietario";
        else
            TempData["Mensaje"] = "El propietario se activo";
        return RedirectToAction("Index");

    }
    
}