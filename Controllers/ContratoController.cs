using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using net.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace net.Controllers;

[Authorize]
public class ContratoController : Controller
{
    private readonly ILogger<ContratoController> _logger;
    private RepositorioContrato repo = new RepositorioContrato();

    private RepositorioPago repoPago = new RepositorioPago();

    private RepositorioMulta repoMulta = new RepositorioMulta();
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

        //Filtro por estado
        if (estado != -1)
        {
            lista = lista.Where(c => c.Estado == estado).ToList();
        }

        return View(lista);
    }

    [Authorize]
    public IActionResult Detalle(int id)
    {
        var contrato = repo.ObtenerUno(id);
        if (contrato == null)
            return RedirectToAction("Index");

        contrato.Inquilino = repoInquilino.ObtenerUno(contrato.InquilinoId);
        contrato.Inmueble = repoInmueble.ObtenerUno(contrato.InmuebleId);
        contrato.Pagos = repoPago.ObtenerPorContrato(id);//Estuve testeando y funciona
        contrato.Multas = repoMulta.ObtenerPorContrato(id);//Falta testear

        return View(contrato);
    }

    [Authorize(Roles = "Empleado")]
    public IActionResult Edicion(int id)
    {
        //ViewBad para cargar inquilinos e inmuebles activos en los SelectList
        var inquilinos = repoInquilino.ObtenerActivos()
            .Select(i => new
            {
                i.InquilinoId,
                Display = $"{i.Nombre} {i.Apellido} - DNI: {i.Dni}"
            }).ToList();

        var inmuebles = repoInmueble.ObtenerActivos()
            .Select(i => new
            {
                i.InmuebleId,
                Display = $"{i.Direccion} ({i.TipoInmueble.Valor}, {i.UsoInmueble.UsoValor})"
            }).ToList();
        ViewBag.Inquilinos = new SelectList(inquilinos, "InquilinoId", "Display", inquilinos?.First().InquilinoId);//Eso hay que chequearlo
        ViewBag.Inmuebles = new SelectList(inmuebles, "InmuebleId", "Display", inmuebles?.First().InmuebleId);//inquilinos?.First().InquilinoId

        if (id == 0)
            return View(new Contrato());

        var contrato = repo.ObtenerUno(id);
        return View(contrato);
    }

    [Authorize(Roles = "Empleado")]
    [HttpPost]
    public IActionResult Guardar(Contrato contrato)
    {
        if (VerificarDisponibilidad(contrato.FechaInicio, contrato.FechaFin, contrato.InmuebleId, contrato.ContratoId))
        {
            TempData["Error"] = "El inmueble ya se encuentra ocupado entre esas fechas";
            return RedirectToAction("Edicion", new { id = contrato.ContratoId });
        }

        if (contrato.ContratoId == 0)
        {
            contrato.UsuarioCreadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            contrato.FechaCreacion = DateTime.Now;
            repo.Alta(contrato);
            TempData["Mensaje"] = "Contrato generado correctamente";
        }
        else
        {
            repo.Modificar(contrato);
            TempData["Mensaje"] = "Cambios guardados";
        }

        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Empleado")]
    [HttpPost]
    public IActionResult Renovar(Contrato contrato)
    {
        if (VerificarDisponibilidad(contrato.FechaInicio, contrato.FechaFin, contrato.InmuebleId, contrato.ContratoId))
        {
            TempData["Error"] = "El inmueble ya se encuentra ocupado entre esas fechas";
            return RedirectToAction("Edicion", new { id = contrato.ContratoId });
        }

        //repo.Modificar(contrato);
        //TempData["Mensaje"] = "Cambios guardados";
        //return RedirectToAction("Index");

        contrato.ContratoId = 0;//Porque seria un Nuevo Contrato
        contrato.UsuarioCreadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        contrato.FechaCreacion = DateTime.Now;
        contrato.Estado = 1;
        
        repo.Alta(contrato);

        TempData["Mensaje"] = "Contrato renovado correctamente";
        return RedirectToAction("Index");
    }

    private bool VerificarDisponibilidad(DateTime inicio, DateTime fin, int idInmueble, int? idContrato = null)
    {
        return repo.EstaOcupado(idInmueble, inicio, fin, idContrato);
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        if (!User.IsInRole("Administrador"))
        {
            TempData["Error"] = "Acceso denegado";
            return Redirect("/Home/Index");
        }

        int anuladorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        int res = repo.Baja(id, anuladorId, DateTime.Now);

        TempData[res == -1 ? "Error" : "Mensaje"] =
            res == -1 ? "No se pudo dar de baja el contrato" : "El contrato fue anulado correctamente";

        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Activar(int id)
    {
        if (!User.IsInRole("Administrador"))
        {
            TempData["Error"] = "Acceso denegado";
            return Redirect("/Home/Index");
        }

        var contrato = repo.ObtenerUno(id);
        if (contrato == null)
        {
            TempData["Error"] = "No se encontró el contrato";
            return RedirectToAction("Index");
        }

        int res = repo.Restore(id);
        TempData[res == -1 ? "Error" : "Mensaje"] =
            res == -1 ? "No se pudo activar el contrato" : "El contrato fue reactivado";

        return RedirectToAction("Index");
    }

    // Filtrar contratos que expiran en X dias
    public IActionResult FiltrarPorPlazo(int? plazo)//Falta revisar
    {
        var lista = repo.ObtenerTodos();
        var hoy = DateTime.Now;

        if (plazo.HasValue && plazo != 0)
        {
            var fechaLimite = hoy.AddDays(plazo.Value);
            lista = lista.Where(c => c.FechaFin <= fechaLimite && c.FechaFin >= hoy).ToList();
        }

        ViewBag.PlazoSeleccionado = plazo;
        return View("Index", lista);
    }

    /*[HttpPost]
    public IActionResult RegistrarPago(Pago pago)//Falta revisar
    {
        if (ModelState.IsValid)
        {
            var contrato = repo.ObtenerUno(pago.IdContrato);
            if (contrato == null)
            {
                ModelState.AddModelError("", "El contrato no existe.");
                return View("Detalle", repo.ObtenerUno(pago.IdContrato));
            }

            pago.CreadorId = int.Parse(User.Claims.First().Value);
            pago.Fecha = DateTime.Now;
            pago.AnuladorId = null;
            pago.IdContrato = contrato.ContratoId;

            repoPago.Agregar(pago);
            return RedirectToAction("Detalle", new { id = pago.IdContrato });
        }

        return View("Detalle", repo.ObtenerUno(pago.IdContrato));
    }*/

}