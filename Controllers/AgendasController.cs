using Microsoft.AspNetCore.Mvc;

namespace HorusWebErp.ModelosControllers
{
    public class AgendasController : Controller
    {
        [HttpGet]
        public IActionResult Agenda()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Carteirinhas()
        {

            return View();
        }


    }
}