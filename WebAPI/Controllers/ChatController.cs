using Microsoft.AspNetCore.Mvc;
using Logica;
using Modelo;
using Logica.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ILogicaChat _logicaChat;

        public ChatController()
        {
            _logicaChat = FabricaLogica.GetInstancia().GetLogicaChat();
        }

        [HttpGet("listar/[idUsuario")]
        public IActionResult ListarChatsPorUsuario(int idUsuario)
        {
            try
            {
                List<Chat> chats = _logicaChat.ListarChatsPorUsuario(idUsuario);

                if (chats == null || !chats.Any())
                {
                    return NotFound(new { mensaje = "No existen chats para este usuario" });
                }
                return Ok(chats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
                throw;
            }
        }

        [HttpGet("{idChat}/mensajes/{idUsuario}")]
        public IActionResult ObtenerMensajes (int idChat, int idUsuario)
        {
            try
            {
                List<Mensaje> mensajes = _logicaChat.ObtenerMensajes(idChat, idUsuario);

                if(mensajes == null || !mensajes.Any())
                {
                    return NotFound(new { mensaje = "No existen mensajes en este chat" });
                }
                return Ok(mensajes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("obtener-o-crear")]
        public IActionResult ObtenerOCrearChat([FromBody] ObtenerOCrearChatRequest request)
        {
            try
            {
                int idChat = _logicaChat.ObtenerOCrearChat(request.IdVehiculo, request.IdComprador, request.IdVendedor);
                return Ok(new { idChat });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("{idChat}/mensajes")]
        public IActionResult EnviarMensaje (int idChat, [FromBody] EnviarMensajeRquest request)
        {
            try
            {
                _logicaChat.EnviarMensaje(idChat, request.idEmisor, request.Contenido);
                return Ok(new { mensaje = "El mensaje ha sido enviado correctamente al chat" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
                throw;
            }
        }
    }

    public class ObtenerOCrearChatRequest
    {
        public int IdVehiculo { get; set; }
        public int IdComprador { get; set; }
        public int IdVendedor { get; set; }

    }

    public class EnviarMensajeRquest
    {
        public int idEmisor { get; set; }
        public string Contenido { get; set; }
    }
}
