using Microsoft.AspNetCore.Mvc;

namespace JMCarsWeb.Controllers
{
    public class VehiculoController : Controller
    {
        public IActionResult Listar()
        {
            return View();
        }
    }
}
