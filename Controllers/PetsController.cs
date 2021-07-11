using HorusWebErp.Modelos;
using HorusWebErp.Models.Pets;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using X.PagedList;

namespace HorusWebErp.ModelosControllers
{
    public class PetsController : Controller
    {
        /// <summary>
        ///  Cadastro de Pets
        /// </summary>        
        #region
        [HttpGet]
        public IActionResult Pets(string id)
        {

            // ViewBag.LstRacas = new Racas().ListagemRacas(id);


            string StringPesquisa = "";

            int page = 1;
            if (TempData["FiltroPet"] != null)
            {
                StringPesquisa = TempData["FiltroPet"].ToString();
                TempData.Keep("FiltroPet");
            }
            else
            {
                TempData.Remove("FiltroPet");
            }

            if (TempData["Pagina"] != null)
            {
                page = Convert.ToInt32(TempData["Pagina"].ToString());
                TempData.Keep("PaginaSoc");
            }

            ViewBag.FiltroPetPubPet = StringPesquisa;
            ViewBag.PageList = PageNomePet(page, StringPesquisa);

            //ViewBag.lst = new Pets().ListagemPets(id);

            return View();
        }

        //
        [HttpGet]
        public IActionResult CadastroPets()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastroPets(PetsModel dados)
        {
            bool ret = new Pets().InserirPet(dados);
            return RedirectToAction("CadastroPets", "Pets");
        }

        //
        [HttpGet]
        public IActionResult DetalhesPets()
        {
            string IdPet = TempData["IdPet"].ToString();
            TempData.Keep("IdPet");

            ViewBag.DadosPet = new Pets().ConsultaPet(IdPet);
            return View();
        }

        [HttpPost]
        public IActionResult DetalhesPets(PetsModel dados)
        {
            bool ret = new Pets().AlterarPet(dados);
            return RedirectToAction("Pets", "Pets");
        }

        public IActionResult DetalhesPets2(string id)
        {
            TempData["IdPet"] = id;
            return RedirectToAction("DetalhesPets", "Pets");
        }
        //




        public IActionResult FiltrarPet(string id)
        {
            TempData["FiltroPet"] = id;
            return RedirectToAction("Pets", "Pets");    //, new { @id = FiltroPet });
        }


        // Contador de Paginas Pets
        #region
        public IActionResult PesqPet(string id, int? page = 1)
        {
            TempData["FiltroPet"] = id;
            TempData["Pagina"] = page;

            return RedirectToAction("Pets", "Pets");
        }

        protected IPagedList<PetsModel> PageNomePet(int? page, string currentFilter)
        {
            if (page.HasValue && page < 1)
                return null;

            //pesquisando no bano de dados              
            List<PetsModel> listUnpaged = new Pets().ListagemPets(currentFilter);

            // Registros por Página
            const int pageSize = 9;
            var listPaged = listUnpaged.ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }
        #endregion




        #endregion


        /// <summary>
        /// Especies
        /// </summary>                
        #region
        [HttpGet]
        public IActionResult Especies(string id)
        {
            ViewBag.lst = new Especie().ListagemEspecies(id);
            return View();
        }

        [HttpGet]
        public IActionResult FiltrarEspcie(string id)
        {
            string FiltroPet = id;
            return RedirectToAction("Especies", "Pets", new { @id = FiltroPet });
        }

        // Inclusão ----------------------------------------------
        [HttpGet]
        public IActionResult InclusaoEspecie()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InclusaoEspecie(EspecieModel Dados)
        {
            bool Ret = new Especie().IncluirEspecie(Dados);
            return RedirectToAction("Especies", "Pets");
        }
        //

        // Exclusão ----------------------------------------------
        public IActionResult ExclusaoDaEspecie()
        {
            string id = TempData["IdEspecie"].ToString();
            TempData.Keep("IdEspecie");

            ViewBag.Dados = new Especie().DetalhesEspecie(id);
            return View();
        }

        public IActionResult ExclusaoEspecie(string id)
        {
            TempData["IdEspecie"] = id;
            return RedirectToAction("ExclusaoDaEspecie", "Pets");
        }


        public IActionResult ExcluirAEspecie(string id)
        {
            bool retorno = new Especie().ExcluirEspecie(id);
            return RedirectToAction("Especies", "Pets");
        }

        // Detalhes ------------------------------------------
        [HttpGet]
        public IActionResult DetalhesEspecie()
        {
            string id = TempData["IdRaca"].ToString();
            TempData.Keep("IdRaca");
            ViewBag.Dados = new Especie().DetalhesEspecie(id);
            return View();
        }

        [HttpPost]
        public IActionResult DetalhesEspecie(EspecieModel dados)
        {
            bool ret = new Especie().AlterarEspecie(dados);
            return RedirectToAction("Especies", "Pets");
        }

        public IActionResult DetalhesEspecie2(string id)
        {
            List<PetsModel> lst = new List<PetsModel>();
            TempData["Tmp"] = lst;

            TempData["IdRaca"] = id;
            return RedirectToAction("DetalhesEspecie", "Pets");
        }
        //-----------------------------------------------------        
        #endregion

        /// <summary>
        ///  Raça de Pets
        /// </summary>       
        #region

        [HttpGet]
        public IActionResult Racas(string id)
        {

            // ViewBag.LstRacas = new Racas().ListagemRacas(id);
            string StringPesquisa = "";

            //--------------------------------------------------

            int page = 1;
            if (TempData["FiltroPet"] != null)
            {
                StringPesquisa = TempData["FiltroPet"].ToString();
                TempData.Keep("FiltroPet");
            }
            else
            {
                TempData.Remove("FiltroPet");
            }

            if (TempData["Pagina"] != null)
            {
                page = Convert.ToInt32(TempData["Pagina"].ToString());
                TempData.Keep("PaginaSoc");
            }

            ViewBag.FiltroPetPubeRaca = StringPesquisa;
            ViewBag.PageListeRaca = GetPagedNames(page, StringPesquisa);

            return View();
        }

        public IActionResult FiltrarRaca(string id)
        {
            TempData["FiltroPet"] = id;
            return RedirectToAction("Racas", "Pets");    //, new { @id = FiltroPet });
        }
        //
        [HttpGet]
        public IActionResult InclusaoRaca()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InclusaoRaca(RacasModel Dados)
        {
            bool Ret = new Racas().IncluirRaca(Dados);
            return RedirectToAction("Racas", "Pets");
        }

        // 
        [HttpGet]
        public IActionResult DetalhesRaca()
        {
            string id = TempData["IdRaca"].ToString();
            TempData.Keep("IdRaca");
            ViewBag.Dados = new Racas().DetalhesRaca(id);
            return View();
        }

        [HttpPost]
        public IActionResult DetalhesRaca(RacasModel dados)
        {

            bool ret = new Racas().AlterarRaca(dados);
            return RedirectToAction("Racas", "Pets");
        }

        public IActionResult DetalhesRaca2(string id)
        {

            TempData["IdRaca"] = id;
            return RedirectToAction("DetalhesRaca", "Pets");
        }

        public IActionResult ExclusaoDaRaca()
        {
            string id = TempData["IdRaca"].ToString();
            TempData.Keep("IdRaca");

            ViewBag.Dados = new Racas().DetalhesRaca(id);
            return View();
        }

        public IActionResult ExclusaoRaca(string id)
        {
            TempData["IdRaca"] = id;
            return RedirectToAction("ExclusaoDaRaca", "Pets");
        }

        public IActionResult ExcluirARaca(string id)
        {
            bool retorno = new Racas().ExcluirRaca(id);
            return RedirectToAction("Racas", "Pets");
        }


        // Contador de Paginas RACAS
        #region
        public IActionResult SearchData(string id, int? page = 1)
        {
            TempData["FiltroPet"] = id;
            TempData["Pagina"] = page;

            return RedirectToAction("Racas", "Pets");
        }

        protected IPagedList<RacasModel> GetPagedNames(int? page, string currentFilter)
        {
            if (page.HasValue && page < 1)
                return null;

            // monta lista pesquisando no bano de dados              
            List<RacasModel> listUnpaged = new Racas().ListagemRacas(currentFilter);

            // Registros por Página
            const int pageSize = 9;
            var listPaged = listUnpaged.ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }
        #endregion



        #endregion


        // Modais de Pesquisa
        #region          
        public IActionResult TabelaEspecies(string id)
        {
            ViewBag.ListaEspecie = new Especie().ListagemEspecies(id);
            return PartialView();
        }

        [HttpPost]
        public JsonResult ListagemEspecies(string id)
        {
            string FiltroPetParteNome = id;
            // ViewBag.FiltroPetPublico = FiltroPetParteNome;
            ViewBag.ListaEspecie = new Especie().ListagemEspecies(id);

            var Tabela = string.Empty;
            foreach (var item in (List<EspecieModel>)ViewBag.ListaEspecie)
            {
                string idEsp = string.Format("{0,4: 0000}", item.id).Trim();

                Tabela += "<tr>" +
                "<td class='col-lg-1'>" + idEsp + "</td>" +
                "<td class='col-lg-6'>" + item.nome + "</td>" +
                "<td class='col-lg-1'>" +
                "<button type='button' onclick='Selecionar()'>" +
                "Selecionar" +
                "</button>" +
                "</td>" +
                "</tr>";

            }
            return Json(Tabela);
        }



        public IActionResult TabelaRacas(string id)
        {
            ViewBag.Listagem = new Racas().ListagemRacas(id);
            return PartialView();
        }

        [HttpPost]
        public JsonResult ListagemRacas(string id)
        {
            string FiltroPetParteNome = id;

            ViewBag.ListaRacas = new Racas().ListagemRacas(id);

            var Tabela = string.Empty;
            foreach (var item in (List<RacasModel>)ViewBag.ListaRacas)
            {
                string idRaca = string.Format("{0,4: 0000}", item.id).Trim();

                Tabela += "<tr>" +
                "<td class='col-lg-1'>" + idRaca + "</td>" +
                "<td class='col-lg-6'>" + item.nome + "</td>" +
                "<td class='col-lg-1'>" +
                "<button type='button' onclick='Selecionar()'>" +
                "Selecionar" +
                "</button>" +
                "</td>" +
                "</tr>";

            }
            return Json(Tabela);
        }

        #endregion

    }

}