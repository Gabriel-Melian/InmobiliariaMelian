using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using net.Models;
using Microsoft.AspNetCore.Authorization;

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
        //Falta: Comparar password hasheado
        if (usuario == null || usuario.Password != model.Password)
        {
            ModelState.AddModelError(string.Empty, "Usuario o contraseña inválidos");
            return View(model);
        }

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),//aparece en el layout
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),//para obtener el Id luego
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
        //Aca quiero cargar los datos del usuario logueado y mostrarlos en una vista por ejemplo
        return View();
    }
}