using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using net.Models;

namespace net.Controllers;

//[Authorize]
public class ContratoController : Controller
{
    private readonly ILogger<ContratoController> _logger;
    private RepositorioContrato repo = new RepositorioContrato();

    //private RepositorioPago repoPago = new RepositorioPago();
    private readonly RepositorioInquilino repoInquilino = new RepositorioInquilino();
    private RepositorioInmueble repoInmueble = new RepositorioInmueble();

    public ContratoController(ILogger<ContratoController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(DateTime? desde, DateTime? hasta, int estado = -1)
    {
        var lista = repo.ObtenerTodos();

        //Filtro por fechas
        if (desde.HasValue && hasta.HasValue)
        {
            lista = lista
                .Where(c => c.FechaInicio >= desde.Value && c.FechaFin <= hasta.Value)
                .ToList();
        }

        // Filtro por estado
        if (estado != -1)
        {
            lista = lista.Where(c => c.Estado == estado).ToList();
        }

        return View(lista);
    }
}