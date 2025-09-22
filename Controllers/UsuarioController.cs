using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using net.Models;
using Microsoft.AspNetCore.Authorization;
using Google.Protobuf;

public class UsuarioController : Controller
{
    private readonly RepositorioUsuario repo = new RepositorioUsuario();

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var usuario = repo.ObtenerPorEmail(model.Email);
        //string hashed = BCrypt.Net.BCrypt.HashPassword("misipi");
        //string hashed2 = BCrypt.Net.BCrypt.HashPassword("judi");
        //Console.WriteLine($"Hashed password: {hashed}");
        //Console.WriteLine($"Hashed password 2: {hashed2}");

        //Aunque la BD tire un hash distinto cada vez (por el salt aleatorio), el Verify sabe validar y comparar
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuario.Password))
        {
            ModelState.AddModelError(string.Empty, "Usuario o contraseña inválidos");
            return View(model);
        }

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),//aparece en el layout
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),//para obtener el Id despues
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.RolEnum.ToString()),//"Administrador" / "Empleado"
                new Claim("Avatar", usuario.Avatar ?? string.Empty)
            };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var authProps = new AuthenticationProperties
        {
            IsPersistent = true,//cookie persiste
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public IActionResult Perfil()
    {
        var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var usuario = repo.ObtenerPorId(id);
        if (usuario == null)
        {
            TempData["Error"] = "No se pudo cargar el perfil.";
            return RedirectToAction("Index", "Home");
        }
        return View(usuario);
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Index()//El admin puede ver y modificar empleados.
    {
        var usuarios = repo.ObtenerEmpleados();
        return View(usuarios);
    }

    [HttpPost]
    [Authorize]
    public IActionResult ActualizarPerfil(Usuario model, IFormFile? AvatarFile, string? nuevaPassword)
    {
        var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        model.UsuarioId = id;

        //Para traer el avatar que tiene actualmente
        var usuarioActual = repo.ObtenerPorId(id);

        if (AvatarFile != null && AvatarFile.Length > 0)
        {
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/usuarios");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            var fileName = $"{id}_{Path.GetFileName(AvatarFile.FileName)}";
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                AvatarFile.CopyTo(stream);
            }

            model.Avatar = "/img/usuarios/" + fileName;
        }
        else
        {
            //Se lo paso nuevamente aca, sino le setea null.
            model.Avatar = usuarioActual?.Avatar;
        }

        bool cambiarPassword = !string.IsNullOrEmpty(nuevaPassword);
        if (cambiarPassword)
        {
            model.Password = nuevaPassword;
        }

        repo.ActualizarPerfil(model, cambiarPassword);

        //Faltaria refrescar claims y cargar de nuevo la sesion para reflejar cambios al momento, sino me equivoco.

        TempData["Mensaje"] = "Perfil actualizado correctamente.";
        return RedirectToAction("Perfil");
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Edicion(int id)
    {
        var usuario = repo.ObtenerPorId(id);
        if (usuario == null)
        {
            TempData["Error"] = "Usuario no encontrado.";
            return RedirectToAction("Index");
        }
        return View(usuario);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edicion(Usuario model)
    {
        if (!ModelState.IsValid)
            return View(model);

        repo.Modificacion(model);
        TempData["Mensaje"] = "Usuario actualizado correctamente.";
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Baja(int id)
    {
        repo.Baja(id);
        TempData["Mensaje"] = "Usuario desactivado correctamente.";
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Reactivar(int id)
    {
        repo.Reactivar(id);
        TempData["Mensaje"] = "Usuario activado correctamente.";
        return RedirectToAction("Index");
    }
}