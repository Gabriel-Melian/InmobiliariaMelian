using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net.Models;

namespace net.Controllers;
[Authorize]
public class UsoInmuebleController : Controller
{
    private readonly ILogger<UsoInmuebleController> _logger;
    private RepositorioUso repo = new RepositorioUso();

    public UsoInmuebleController(ILogger<UsoInmuebleController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var lista = repo.ObtenerTodos();
        return View(lista);
    }

    [HttpPost]
    public IActionResult Guardar(UsoInmueble uso)
    {

        repo.Alta(uso);
        TempData["Mensaje"] = "Uso de inmueble guardado";
        return RedirectToAction("Index", "UsoInmueble");
    }

    public IActionResult Baja(int id)
    {
        repo.Baja(id);
        TempData["Mensaje"] = "Uso eliminado";
        return RedirectToAction("Index", "UsoInmueble");
    }
    
    public IActionResult Modificar(int id)
    {
        var uso = repo.ObtenerUno(id);
        return View(uso);
    }
    

}