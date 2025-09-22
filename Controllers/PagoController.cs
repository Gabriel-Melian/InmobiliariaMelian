using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using net.Models;

namespace net.Controllers;

[Authorize]
public class PagoController : Controller
{

    private readonly ILogger<PagoController> _logger;
    private RepositorioPago repo = new RepositorioPago();
    //private readonly RepositorioPago repoInquilino = new RepositorioPago();
    //private RepositorioPago repoInmueble = new RepositorioPago();

    public PagoController(ILogger<PagoController> logger)
    {
        _logger = logger;
    }

    [Authorize(Roles = "Empleado")]
    public IActionResult Edicion(int id)
    {
        if (id == 0)
            return View(new Pago());
        else
        {
            var pago = repo.ObtenerUno(id);
            return View(pago);//Vista Edicion.cshtml
        }
    }

    [Authorize(Roles = "Empleado")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Guardar(Pago pago)
    {
        if (!ModelState.IsValid)
        {
            return View("Edicion", pago);
        }

        var idUsuarioCrea = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        if (pago.PagoId == 0)
        {
            pago.UsuarioAltaId = idUsuarioCrea;
            repo.Agregar(pago);
            TempData["Mensaje"] = "Pago registrado correctamente.";
        }
        else
        {
            repo.Modificar(pago);
            TempData["Mensaje"] = "Detalles del pago actualizados.";
        }

        return RedirectToAction("Detalle", "Contrato", new { id = pago.IdContrato });
    }

    [Authorize(Roles = "Empleado")]
    public IActionResult Anular(int id)
    {
        var pago = repo.ObtenerUno(id);
        if (pago == null) return RedirectToAction("Index", "Contrato");

        var usuarioBajaId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        repo.Anulado(id, usuarioBajaId);
        TempData["Mensaje"] = "Pago anulado correctamente.";
        return RedirectToAction("Detalle", "Contrato", new { id = pago.IdContrato });
    }

    [Authorize(Roles = "Empleado")]
    public IActionResult Pendiente(int id)
    {
        var pago = repo.ObtenerUno(id);
        if (pago == null) return RedirectToAction("Index", "Contrato");

        repo.Pendiente(id);
        TempData["Mensaje"] = "El pago fue marcado como pendiente.";
        return RedirectToAction("Detalle", "Contrato", new { id = pago.IdContrato });
    }

    [Authorize(Roles = "Empleado")]
    public IActionResult Pagado(int id)
    {
        var pago = repo.ObtenerUno(id);
        if (pago == null) return RedirectToAction("Index", "Contrato");

        repo.Pagado(id);
        TempData["Mensaje"] = "El pago fue marcado como pagado.";
        return RedirectToAction("Detalle", "Contrato", new { id = pago.IdContrato });
    }

}