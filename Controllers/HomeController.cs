using HorusWebErp.Modelos;
using HorusWebErp.Models;
using HorusWebErp.Utilitarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace HorusWebErp.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public IActionResult Login(int? id)
        {
            if (id != null) // passou parametro
            {
                // testa para ver se deve ser efetuado logout
                if (id == 0)
                {
                    HttpContext.Session.SetString("IdUsuarioLogado", string.Empty);
                    HttpContext.Session.SetString("NomeUsuarioLogado", string.Empty);
                    HttpContext.Session.SetString("IdConsultorio", string.Empty);
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {

                Login log = login.ValidarLogin();
                // criar um Tokem 
                //login.Token(login.Id.ToString());

                if (log == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    // Nome do Banco dados do Usuario Logado(Sempre será Cnpj Ou Cpf)
                    Procedures.Database = log.Cnpj;

                    HttpContext.Session.SetString("IdUsuarioLogado", log.id);
                    HttpContext.Session.SetString("NomeUsuarioLogado", log.login);
                    HttpContext.Session.SetString("Token", log.Token);
                    HttpContext.Session.SetString("NomeDatabase", log.Cnpj);
                    HttpContext.Session.SetString("NomeEmpresa", log.NomeEmpresa);
                    HttpContext.Session.SetInt32("Menu", log.menu);
                    Publicas.MenuPrincipal = log.menu;

                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public IActionResult Index()
        {
            var sessionNome = new Byte[20];

            string Usuariologado = "";
            string NomeDaEmpresaLogada = "";

            try
            {
                HttpContext.Session.TryGetValue("IdUsuarioLogado", out sessionNome);
                Usuariologado = System.Text.Encoding.UTF8.GetString(sessionNome).ToString();
                HttpContext.Session.TryGetValue("NomeEmpresa", out sessionNome);
                NomeDaEmpresaLogada = System.Text.Encoding.UTF8.GetString(sessionNome).ToString();
            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException("Erro :", "", ex);
               // throw argEx;
            }

            ViewBag.NomeDaEmpresaLogada = NomeDaEmpresaLogada;
            return View();
        }




        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
