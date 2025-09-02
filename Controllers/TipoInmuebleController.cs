using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net.Models;

namespace net.Controllers;
//[Authorize (Policy = "Administrador")]
public class TipoInmuebleController : Controller
{
    private readonly ILogger<TipoInmuebleController> _logger;
    private RepositorioTipo repo = new RepositorioTipo();

    public TipoInmuebleController(ILogger<TipoInmuebleController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var lista = repo.ObtenerTodos();
        return View(lista);
    }

    [HttpPost]
    public IActionResult Guardar(TipoInmueble tipo)
    {
        repo.Alta(tipo);
        TempData["Mensaje"] = "Tipo de inmueble guardado";
        return RedirectToAction("Index", "TipoInmueble");
    }

    public IActionResult Baja(int id)
    {
        repo.Baja(id);
        TempData["Mensaje"] = "Tipo eliminado";
        return RedirectToAction("Index", "TipoInmueble");
    }
    
    public IActionResult Modificar(int id)
    {
        var tipo = repo.ObtenerUno(id);
        return View(tipo);
    }
    

}