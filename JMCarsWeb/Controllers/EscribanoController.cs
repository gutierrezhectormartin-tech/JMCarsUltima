using Microsoft.AspNetCore.Mvc;
using JMCarsWeb.DTOs;
using JMCarsWeb.Services;

namespace WebApi.Controllers
{
    public class EscribanoController : Controller
    {
        private EscribanoService _escribanoService;

        public EscribanoController(EscribanoService escribanoService)
        {
            _escribanoService = escribanoService;
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(EscribanoDTO escribanoPasado, bool aceptaTerminos)
        {

            if (!ModelState.IsValid)
            {
                return View(escribanoPasado);
            }

            if(!aceptaTerminos)
            {
                ViewBag.Error = "Debe aceptar los terminos y condiciones para registrarse";
                return View(escribanoPasado);
            }
            try
            {
                bool exito =  await _escribanoService.Registrar(escribanoPasado, aceptaTerminos);

                if(!exito)
                {
                    ViewBag.Error = "No se puede completar el registro. Verifique sus datos";
                    return View(escribanoPasado);
                }
                TempData["Mensaje"] = "Registro recibido. Tu cuenta será activada cuando un administrador la apruebe.";//le agregue aca el tempdata porque nunca iba a funcionarte con viewbag, luego de un redirect el viewbag se pierde te acordas martin?
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: No se pudo completar el registro. Verifica tus datos";
                return View(escribanoPasado);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            // chequeo de autorizacion: tiene que estar logueado y ser escribano (rol = 2)
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");
            if (idUsuario == null || idRol != 2)
            {
                return RedirectToAction("Index", "Login");
            }


            // cargo los datos actuales del escribano desde la base
            EscribanoDTO? escribano = await _escribanoService.ObtenerPorId(idUsuario.Value);

            if (escribano == null)
            {
                return RedirectToAction("Index", "Login");
            }

            HttpContext.Session.SetString("EmailEscribano", escribano.Email);
            HttpContext.Session.SetString("NumeroCajaEscribano", escribano.NumeroCaja);

            return View(escribano);
        }

        [HttpPost]
        public async Task<IActionResult> Perfil(EscribanoDTO escribanoPasado)
        {
            // chequeo de autorizacion: tiene que estar logueado y ser escribano (rol = 2)
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");
            if (idUsuario == null || idRol != 2)
            {
                return RedirectToAction("Index", "Login");
            }


            ModelState.Remove("Email");
            ModelState.Remove("NumeroCaja");

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewBag.Error = string.Join(" | ", errores);
                return View(escribanoPasado);
            }

            try
            {
                escribanoPasado.IdUsuario = idUsuario.Value;
                escribanoPasado.Email = HttpContext.Session.GetString("EmailEscribano");
                escribanoPasado.NumeroCaja = HttpContext.Session.GetString("NumeroCajaEscribano");
                await _escribanoService.ActualizarPerfil(escribanoPasado);
                HttpContext.Session.SetString("NombreCompleto", escribanoPasado.NombreCompleto);
                ViewBag.Mensaje = "Perfil actualizado correectamente";
                return View(escribanoPasado);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "No se pudo actualizar el perfil.";
                return View(escribanoPasado);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Inactivar()
        {
            // chequeo de autorizacion: tiene que estar logueado y ser escribano (rol = 2)
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");
            if (idUsuario == null || idRol != 2)
            {
                return RedirectToAction("Index", "Login");
            }


            await _escribanoService.Inactivar(idUsuario.Value);
            HttpContext.Session.Clear();
            return RedirectToAction("CuentaInactivada", "Home");
        }
    }
}
