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

    public PropietarioController(ILogger<PropietarioController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<Propietario> lista;
        lista = repo.ObtenerTodos();
        return View(lista);
    }

    public IActionResult Detalle(int id)
    {
        if (id == 0)
            return View();
        else
        {
            var propietario = repo.ObtenerUno(id);
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
    
}