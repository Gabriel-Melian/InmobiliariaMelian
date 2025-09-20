using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net.Models;

namespace net.Controllers;

//[Authorize]
public class MultaController : Controller
{
    private readonly ILogger<MultaController> _logger;
    private RepositorioMulta repo = new RepositorioMulta();

    public MultaController(ILogger<MultaController> logger)
    {
        _logger = logger;
    }

    public IActionResult Edicion(int id)
    {
        if (id == 0)
            return View(new Multa());
        else
        {
            var multa = repo.ObtenerUno(id);
            return View(multa);
        }
    }

    public IActionResult Generar(int contratoId, DateTime fechaTerminacion)
    {
        var repoContrato = new RepositorioContrato();
        var contrato = repoContrato.ObtenerUno(contratoId);

        if (contrato == null)
        {
            TempData["Error"] = "No se encontró el contrato.";
            return RedirectToAction("Index", "Contrato");
        }

        var repoPago = new RepositorioPago();

        //Calculo de multa
        var monto = repo.CalcularMulta(contrato, fechaTerminacion);

        //Revisar si tiene pagos pendientes
        var pagosPendientes = repoPago.ObtenerPorContrato(contratoId)
                                    .Count(p => p.Estado == (int)Pago.EstadoPago.Pendiente);

        var multa = new Multa
        {
            IdContrato = contratoId,
            FechaAviso = DateTime.Now,
            FechaTerminacion = fechaTerminacion,
            Monto = monto,
            Estado = (int)Multa.EstadoMulta.Activa
        };

        repo.Agregar(multa);

        TempData["Mensaje"] = $"Multa generada por {monto:C0}. El inquilino tiene {pagosPendientes} pagos pendientes.";
        return RedirectToAction("Detalle", "Contrato", new { id = contratoId });
    }
}