using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net.Models;

namespace net.Controllers;
//[Authorize]
public class InquilinoController : Controller
{
    private readonly ILogger<InquilinoController> _logger;
    private RepositorioInquilino repo = new RepositorioInquilino();

    public InquilinoController(ILogger<InquilinoController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(int? estado)
    {
        List<Inquilino> lista;
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
            var inquilino = repo.ObtenerUno(id);
            return View(inquilino);
        }
    }

    public IActionResult Edicion(int id)
    {
        if (id == 0)
            return View();
        else
        {
            var inquilino = repo.ObtenerUno(id);
            return View(inquilino);
        }
    }

    [HttpPost]
    public IActionResult Guardar(int id, Inquilino inquilino)
    {
        if (!ModelState.IsValid)//ModelState es para las verificaciones. IsValid devuelve true o false.
        {
            return View("Edicion", inquilino);
        }
        id = inquilino.InquilinoId;
        if (repo.VerificarInquilino(inquilino.Dni, inquilino.Email) && id == 0)
        {
            TempData["Error"] = "Ya existe un inquilino con el mismo DNI y email.";
            return View("Edicion", inquilino);
        }
        if (id == 0)
        {
            repo.Alta(inquilino);
            //TempData envia datos temporales.
            TempData["Mensaje"] = "Inquilino guardado";
        }
        else
        {
            repo.Modificar(inquilino);
            TempData["Mensaje"] = "Cambios guardados";
        }
        return RedirectToAction("Index");
    }

    public IActionResult Eliminar(int id)
    {
        //if (!User.IsInRole("Administrador"))
        //{
        //    TempData["Error"] = "Acceso denegado";
        //    return Redirect("/Home/Index");
        //}
        int res = repo.Baja(id);
        if (res == -1)
            TempData["Error"] = "No se pudo eliminar el inquilino";
        else
            TempData["Mensaje"] = "El inquilino se elimino";
        return RedirectToAction("Index");
    }
    
    public IActionResult Activar(int id)
    {
        //if (!User.IsInRole("Administrador"))
        //{
        //    TempData["Error"] = "Acceso denegado";
        //    return Redirect("/Home/Index");
        //}
        //Si Existe
        if (repo.ObtenerUno(id) == null)
        {
            TempData["Error"] = "No se encontro el inquilino";
            return RedirectToAction("Index");
        }
        //Entonces lo restaura
        int res = repo.Restore(id);
        if (res == -1)
            TempData["Error"] = "No se pudo activar el inquilino";
        else
            TempData["Mensaje"] = "El inquilino se activo";
        return RedirectToAction("Index");
    }

}