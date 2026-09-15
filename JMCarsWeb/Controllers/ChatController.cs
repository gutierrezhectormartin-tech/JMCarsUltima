using Microsoft.AspNetCore.Mvc;
using JMCarsWeb.DTOs;
using JMCarsWeb.Services;

namespace JMCarsWeb.Controllers
{
    public class ChatController : Controller
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> IniciarChat(int idVehiculo, int idVendedor)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idUsuario == null || idRol != 3)
            {
                TempData["Error"] = "Solo podrá contactarse con un vendedor, siendo usuario del sistema.";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                int? idChat = await _chatService.ObtenerOCrearChat(idVehiculo, idUsuario.Value, idVendedor);
                if(idChat == null)
                {
                    TempData["Error"] = "Ha ocurrido un error al iniciar el chat. Intente nuevamente";
                    return RedirectToAction("Detalle", "Vehiculo", new { id = idVehiculo });
                }

                return RedirectToAction("Conversacion", new { idChat = idChat.Value });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ha ocurrido un error" + ex.Message;
                return RedirectToAction("Detalle", "Vehiculo", new { id = idVehiculo });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Conversacion(int idChat)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");


            if(idUsuario == null || idRol != 3)
            {
                TempData["Error"] = "Solo podrá contactarse con un vendedor, siendo usuario del sistema.";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                List<MensajeDTO>? mensajes = await _chatService.ObtenerMensajes(idChat, idUsuario.Value);
                
                if(mensajes == null)
                {
                    TempData["Error"] = "Ha ocurrido un error al cargar los mensajes";
                    return RedirectToAction("Historial");
                }

                ViewBag.IdChat = idChat;
                ViewBag.IdUsuario = idUsuario.Value;
                return View(mensajes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ha ocurrido un error" + ex.Message;
                return RedirectToAction("Historial");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EnviarMensaje(int idChat, string contenido)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if (idUsuario == null || idRol != 3)
            {
                TempData["Error"] = "Debe ser un usuario para enviar mensajes.";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                bool exito = await _chatService.EnviarMensaje(idChat, idUsuario.Value, contenido);
                if(!exito)
                {
                    TempData["Error"] = "No se pudo enviar le mensaje. Intente nuevamente";
                }
                return RedirectToAction("Conversacion", new { idChat });


            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ha ocurrido un error" + ex.Message;
                return RedirectToAction("Historial");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Historial()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if (idUsuario == null || idRol != 3)
            {
                TempData["Error"] = "Debe ingresar para poder ver sus chats.";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                List<ChatDTO>? chats = await _chatService.ListarChatsPorUsuario(idUsuario.Value);

                if(chats == null)
                {
                    TempData["Error"] = "Ha ocurrido un error al cargar los chats";
                    return View(new List<ChatDTO>());
                }

                return View(chats);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ha ocurrido un error" + ex.Message;
                return RedirectToAction("Historial");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConsultasVehiculo(int idVehiculo)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idUsuario == null || idRol != 3)
            {
                TempData["Error"] = "Debes iniciar sesion para ver tus consultas";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                List<ChatDTO>? chats = await _chatService.ListarChatsPorVehiculo(idVehiculo);

                if(chats == null)
                {
                    TempData["Error"] = "Ha ocurrido un error al intentar cargar las consultas";
                    return View(new List<ChatDTO>());
                }
                if(!chats.Any())
                {
                    TempData["Mensaje"] = "El vehiculo no ha tenido consultas aun";
                }
                ViewBag.IdVehiculo = idVehiculo;
                return View(chats);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ha ocurrido un error" + ex.Message;
                return View(new List<ChatDTO>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConversacionesVendedor(int idChat, int idVehiculo)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if (idUsuario == null || idRol != 3)
            {
                TempData["Error"] = "Debes iniciar sesion para ver tus consultas";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                List<MensajeDTO>? mensajes = await _chatService.ObtenerMensajes(idChat, idUsuario.Value);

                if(mensajes == null)
                {
                    TempData["Error"] = "Ha ocurrido un error al cargar los mensajes de esta conversacion";
                    return RedirectToAction("ConsultasVehiculo", new { idVehiculo });
                }

                ViewBag.IdChat = idChat;
                ViewBag.IdUsuario = idUsuario.Value;
                ViewBag.IdVehiculo = idVehiculo;
                return View(mensajes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ha ocurrido un error:" + ex.Message;
                return RedirectToAction("ConsultasVehiculo", new { idVehiculo });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EnviarMensajeVendedor(int idChat, int idVehiculo, string contenido)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if (idUsuario == null || idRol != 3)
            {
                TempData["Error"] = "Debes iniciar sesion para enviar mensajes";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                bool exito = await _chatService.EnviarMensaje(idChat, idUsuario.Value, contenido);

                if(!exito)
                {
                    TempData["Error"] = "No se pudo enviar el mensaje";
                }
                return RedirectToAction("ConversacionesVendedor", new { idChat, idVehiculo });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ha ocurrido un error" + ex.Message;
                return RedirectToAction("ConversacionesVendedor", new { idChat, idVehiculo });
            }
        }
    }
}
