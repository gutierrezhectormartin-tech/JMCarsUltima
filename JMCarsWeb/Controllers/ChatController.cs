using Microsoft.AspNetCore.Mvc;
using JMCarsWeb.DTOs;
using JMCarsWeb.Services;
using JMCarsWeb.ViewModels;
using X.PagedList;
using X.PagedList.Extensions;
using static JMCarsWeb.Helpers.Paginado;

namespace JMCarsWeb.Controllers
{
    public class ChatController : Controller
    {
        private readonly ChatService _chatService;
        private readonly VehiculoService _vehiculoService;
        public ChatController(ChatService chatService, VehiculoService vehiculoService)
        {
            _chatService = chatService;
            _vehiculoService = vehiculoService;
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
                int idChat = await _chatService.ObtenerOCrearChat(idVehiculo, idUsuario.Value, idVendedor);
                return RedirectToAction("Conversacion", new { idChat, idVehiculo });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Detalle", "Vehiculo", new { id = idVehiculo });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Conversacion(int idChat, int idVehiculo = 0)
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
                List<MensajeDTO> mensajes = await _chatService.ObtenerMensajes(idChat, idUsuario.Value);
                
                ViewBag.IdChat = idChat;
                ViewBag.IdUsuario = idUsuario.Value;
                ViewBag.IdVehiculo = idVehiculo;
                return View(mensajes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Historial");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EnviarMensaje(int idChat, int idVehiculo,string contenido)
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
                await _chatService.EnviarMensaje(idChat, idUsuario.Value, contenido);
                return RedirectToAction("Conversacion", new { idChat, idVehiculo });


            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Conversacion", new { idChat, idVehiculo });
            }

        }

        [HttpGet]
        public async Task<IActionResult> Historial(string? buscar, int pagina = 1)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if (idUsuario == null || idRol != 3)
            {
                TempData["Error"] = "Debe ingresar para poder ver sus chats.";
                return RedirectToAction("Index", "Login");
            }

            const int tamanoPagina = 10;

            ChatsViewModel modelo = new ChatsViewModel
            {
                Buscar = buscar
            };

            try
            {
                List<ChatDTO> todos = await _chatService.ListarChatsPorUsuario(idUsuario.Value);
                IEnumerable<ChatDTO> filtrados = todos;
                if(!string.IsNullOrWhiteSpace(buscar))
                {
                    string texto = buscar.Trim();
                    filtrados = filtrados.Where(c => (c.Vehiculo?.Modelo?.Marca?.NombreMarca ?? "").Contains(texto, StringComparison.OrdinalIgnoreCase)
                    || (c.Vehiculo?.Modelo?.Modelo ?? "").Contains(texto, StringComparison.OrdinalIgnoreCase)
                    || (c.NombreOtroUsuario ?? "").Contains(texto, StringComparison.OrdinalIgnoreCase));
                }

                List<ChatDTO> lista = filtrados.OrderByDescending(c => c.FechaUltimoMensaje).ToList();
                modelo.TotalFiltrados = lista.Count;
                modelo.Chats = lista.ToPagedList(CorregirPagina(pagina, lista.Count, tamanoPagina), tamanoPagina);
                return View(modelo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(modelo);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConsultasVehiculo(int idVehiculo, string? buscar, int pagina = 1)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idUsuario == null || idRol != 3)
            {
                TempData["Error"] = "Debes iniciar sesion para ver tus consultas";
                return RedirectToAction("Index", "Login");
            }

            const int tamanoPagina = 10;
            ViewBag.IdVehiculo = idVehiculo;

            ChatsViewModel modelo = new ChatsViewModel
            {
                Buscar = buscar
            };
            try
            {
                VehiculoDTO? vehiculo = await _vehiculoService.DetalleVehiculo(idVehiculo);
                if(vehiculo == null || vehiculo.Vendedor?.IdUsuario != idUsuario.Value)
                {
                    TempData["Error"] = "Solo podes ver las consultas de tus propios vehiculos";
                    return RedirectToAction("MisVehiculos", "Vehiculo");
                }
                List<ChatDTO> todos = await _chatService.ListarChatsPorVehiculo(idVehiculo);
                IEnumerable<ChatDTO> filtrados = todos;

                if(!string.IsNullOrWhiteSpace(buscar))
                {
                    string texto = buscar.Trim();
                    filtrados = filtrados.Where(c => (c.NombreOtroUsuario ?? "").Contains(texto, StringComparison.OrdinalIgnoreCase)
                    || (c.UltimoMensaje ?? "").Contains(texto, StringComparison.OrdinalIgnoreCase));
                }
                List<ChatDTO> lista = filtrados.OrderByDescending(c => c.FechaUltimoMensaje).ToList();
                modelo.TotalFiltrados = lista.Count;
                modelo.Chats = lista.ToPagedList(CorregirPagina(pagina, lista.Count, tamanoPagina), tamanoPagina);
                return View(modelo);
            }
            catch (Exception ex)
            {
                TempData["Error"] =  ex.Message;
                return RedirectToAction("MisVehiculos", "Vehiculo");
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
                List<MensajeDTO> mensajes = await _chatService.ObtenerMensajes(idChat, idUsuario.Value);

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
                await _chatService.EnviarMensaje(idChat, idUsuario.Value, contenido);
                return RedirectToAction("ConversacionesVendedor", new { idChat, idVehiculo });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("ConversacionesVendedor", new { idChat, idVehiculo });
            }
        }
      
    }
}
