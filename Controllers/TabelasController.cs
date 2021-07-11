using HorusWebErp.Modelos;
using HorusWebErp.Models.Tabelas;
using HorusWebErp.Uteis;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace HorusWebErp.Controllers
{
    public class TabelasController : Controller
    {
        public static string Pacote { get; set; }
        public static ProdutosModel DetalheDoProdutos;

        // ************************************
        // Produtos       
        // ************************************
        #region
        [HttpGet]
        public IActionResult Produtos()
        {
            string Filtro = "";
            string TipoFiltro = "0";

            if (TempData["Filtro"] != null)
            {
                Filtro = TempData["Filtro"].ToString();
                TempData.Keep("Filtro");
                TipoFiltro = TempData["Tipo"].ToString();
                TempData.Keep("Tipo");
            }

            ViewBag.ListaProdutos = new Produtos().ListagemProdutos(Filtro, TipoFiltro);
            return View();
        }

        [HttpGet]
        public IActionResult ListaProdutosParteNome(string id)
        {            
            var Retorno = id.Split("|");
            TempData["Filtro"] = Retorno[0];
            TempData["Tipo"] = Retorno[1];           
            Pacote = id;
            return RedirectToAction("Produtos", "Tabelas"); //, new { @id = Pacote });
        }

        public JsonResult ExcluirProduto(string id)
        {
            bool Retorno =  new Produtos().ExcluirProduto(id);
            var Tabela = string.Empty;

            if (Retorno)
            {
                string Filtro = "";
                string TipoFiltro = "0";

                if (TempData["Filtro"] != null)
                {
                    Filtro = TempData["Filtro"].ToString();
                    TempData.Keep("Filtro");
                    TipoFiltro = TempData["Tipo"].ToString();
                    TempData.Keep("Tipo");
                }

                ViewBag.ListaProdutos = new Produtos().ListagemProdutos(Filtro, TipoFiltro);              

                foreach (var item in (List<ProdutosModel>)ViewBag.ListaProdutos)
                {
                    Tabela += "<tr>" +
                    "<td class='col-lg-1' style='display: none'>" + item.id + "</td>" +
                    "<td class='col-lg-7'>" + item.descricao + "</td>" +
                    "<td class='col-lg-1'>" + item.referencia + "</td>" +
                    "<td class='col-lg-1'>" + string.Format("{0:0.000}", item.qtEstoque) + "</td>" +
                    "<td class='col-lg-1'>" + string.Format("R$ {0:0.00}", item.VlTabela1) + "</td>" +

                    "<td class='col-lg-1'>" +
                        "<button type='button' class='btn btn-info' onclick='Detalhes();'>" +
                        "Detalhes" +
                        "</button>" +
                    "</td>" +

                    "<td class='col-lg-1'>" +
                        "<button type='button' class='btn btn-danger'  onclick='Excluir()'>" +
                        "Excluir" +
                        "</button>" +
                    "</td>" +

                    "</tr>";
                }
                
            }
            else
            {
                Tabela = "Erro";
            }

            return Json(Tabela);
        }


        // ************************************
        // Visualização e Alteração de Produtos
        // ************************************       
        [HttpGet]
        public IActionResult DetalhesProduto2(string id)
        {
            DetalheDoProdutos = null;
            Pacote = id;
            return RedirectToAction("DetalhesProduto", "Tabelas"); //, new { @id = Pacote });
        }

        [HttpGet]
        public IActionResult DetalhesProduto()
        {
            if (DetalheDoProdutos == null)
            {
                DetalheDoProdutos = new Produtos().ConsultaProduto(Pacote);
            }
            ViewBag.Produto = DetalheDoProdutos;

            return View();
        }

        [HttpPost]
        public IActionResult DetalhesProduto(ProdutosModel produtos)
        {
            bool Retorno = new Produtos().AlterarProduto(produtos);

            return RedirectToAction("Produtos", "Tabelas");
        }

        [HttpGet]
        public IActionResult NovoProduto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NovoProduto(ProdutosModel produto)
        {
            bool Retorno = new Produtos().InserirProduto(produto);
            return RedirectToAction("Produtos", "Tabelas");
        }

        [HttpGet]
        public IActionResult EntradaEstoque()
        {
            return View();
        }


        [HttpPost]
        public IActionResult EntradaEstoque(ProdutosModel produtos)
        {
            bool Retorno = new Produtos().GravarEstoque(produtos);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult PesquisarProduto(string id)
        {
            string StrFind = id;
            //ViewBag.FiltroPublico = FiltroParteNome;

            ViewBag.ListaProdutos = new Produtos().ListagemProdutos(StrFind, "");

            var Tabela = string.Empty;
            foreach (var item in (List<ProdutosModel>)ViewBag.ListaProdutos)
            {
                string idProd = string.Format("{0,5: 000000}", item.id).Trim();
                string Estq = string.Format("{0:0.000}", item.qtEstoque);
                string Vlr = string.Format("R$ {0:0.00}", item.VlTabela1);

                Tabela += "<tr>" +
                "<td class='col-lg-1'>" + id + "</td>" +
                "<td class='col-lg-7'>" + item.descricao + "</td>" +
                "<td class='col-lg-1'>" + item.referencia + "</td>" +
                "<td class='col-lg-1'>" + Estq + "</td>" +
                "<td class='col-lg-1'>" + Vlr + "</td>" +
                "<td class='col-lg-1'><button type='button' class='btn btn-info' onclick='ItensPedido(@item.idProd)'  data-dismiss='modal'    >Detalhes</button>" + "</td>" +
                "</tr>";

            }

            return Json(Tabela);

        }

        // Listagem da Tabela de Produtos
        public IActionResult TabelaProdutos(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "";
            }

            string StrFind = id;
            ViewBag.FiltroPublico = StrFind;
            ViewBag.ListaProdutos = new Produtos().ListagemProdutos(StrFind, "0");
            return PartialView();
        }

        [HttpPost]
        public JsonResult ListagemProdutos(string id)
        {
            var StrFind = id.Split("|");

            ViewBag.FiltroPublico = StrFind;
            ViewBag.ListaProdutos = new Produtos().ListagemProdutos(StrFind[0], StrFind[1]);

            var Tabela = string.Empty;
            foreach (var item in (List<ProdutosModel>)ViewBag.ListaProdutos)
            {
                string idPro = string.Format("{0,5: 000000}", item.id).Trim();

                Tabela += "<tr>" +
                "<td class='col-lg-1'>" + idPro + "</td>" +
                "<td class='col-lg-6'>" + item.descricao + "</td>" +
                "<td class='col-lg-1'>" + item.qtEstoque + "</td>" +
                "<td class='col-lg-1'>" + item.VlTabela1 + "</td>" +
                "<td class='col-lg-1'>" +
                "<button type='button' class='btn btn-primary'  onclick='Selecionar()'>" +
                "Selecionar" +
                "</button>" +
                "</td>" +
                "</tr>";

                //< img src = "~/images/accept.png" />     

            }
            return Json(Tabela);
        }


        #endregion


        // ***************************************************
        // Entidades (Clientes/Fornecedores/Transportadoras...      
        // ***************************************************
        #region       
        [HttpGet]
        public IActionResult Entidades()
        {
            string FiltroCliente = "";

            if (TempData["FiltroNomeEntidade"] != null)
            {
               FiltroCliente = TempData["FiltroNomeEntidade"].ToString();
                TempData.Keep("FiltroNomeEntidade");
            }
           
           ViewBag.ListaClientes = new Clientes().ListagemClientes(FiltroCliente);           
           return View();
        }

        [HttpGet]
        public IActionResult DetalhesEntidade2(string id)
        {
           // TempData["FiltroNomeEntidade"] = id;
            Pacote = id;
            return RedirectToAction("DetalhesEntidade", "Tabelas"); //, new { @id = Pacote });
        }

        [HttpGet]
        public IActionResult DetalhesEntidade()
        {
            ViewBag.entidades = new Clientes().PesquisaCliente(Pacote);
            return View();
        }

        [HttpPost]
        public IActionResult DetalhesEntidade(ClienteModel DadosForm)
        {
            new Clientes().AlterarCliente(DadosForm);
            return RedirectToAction("Entidades", "Tabelas");
        }

        /// <summary>
        /// Exclusão entidades
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ExcluirEntidade2(string id)
        {
            TempData["Id"] = id;
            return RedirectToAction("ExcluirEntidade", "Tabelas");
        }
        [HttpGet]
        public IActionResult ExcluirEntidade()
        {
            string id = TempData["Id"].ToString();
            ViewBag.entidades = new Clientes().PesquisaCliente(id);
            return View();

        }

        [HttpPost]
        public IActionResult ExcluirEntidade(ClienteModel dados)
        {
            bool retorno = new Clientes().ExcluirCliente(dados);
            Pacote = dados.nome.Trim();

            return RedirectToAction("Entidades", "Tabelas");
        }

        /// <summary>
        /// Pesuisar determinado Cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PesquisaEntidades(string id)
        {
            // Pacote = id;
            TempData["FiltroNomeEntidade"] = id;
            return RedirectToAction("Entidades", "Tabelas");    //, new { @id = Pacote });
        }

        /// <summary>
        /// Inclusão de Clientes/Fornecedores/Transportadoras/Colaboradores...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult IncluirEntidade()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IncluirEntidade(ClienteModel DadosForm)
        {
            Boolean Retorno = new Clientes().IncluirEntidade(DadosForm);
            return RedirectToAction("Entidades", "Tabelas");
        }



        /// <summary>
        /// Cadastro de Grupos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Grupos()
        {
            ViewBag.grupos = new Grupos().ListagemGrupos(Pacote);
            return View();
        }


        [HttpPost]
        public IActionResult PesquisaEntidades()
        {
            if (ModelState.IsValid)
            {
                return View();
            }

            return View();
        }


        [HttpPost]
        public JsonResult PesquisarEntidades(string id)
        {
            string StrFind = id;
            //ViewBag.FiltroPublico = FiltroParteNome;

            ViewBag.ListaClientes = new Clientes().ListagemClientes(StrFind);

            var Tabela = string.Empty;
            foreach (var item in (List<ClienteModel>)ViewBag.ListaClientes)
            {
                string idCli = string.Format("{0,5: 00000}", item.id).Trim();


                Tabela += "<tr>" +
                "<td class='col-lg-1' style='display: none'>" + id + "</td>" +
                "<td class='col-lg-7'>" + item.nome + "</td>" +
                "<td class='col-lg-1'>" + item.logradouro + "</td>" +
                "<td class='col-lg-1'>" + item.cidade + "</td>" +
                "<td class='col-lg-1'>" + item.fone1 + "</td>" +
                "<td class='col-lg-1'><button type='button' class='btn btn-info' onclick='Detalhes(@item.idCli)'  data-dismiss='modal'    >Detalhes</button>" + "</td>" +
                "</tr>";

            }
            return Json(Tabela);
        }

        #endregion

        // ***************************************************
        // Tecnicos Ordem de serviço PROFISSIONAIS
        // ***************************************************
        #region

        [HttpGet]
        public IActionResult Profissionais()
        {
            string FiltroProfissionais = "";
            if (TempData["Filtro"] != null)
            {
                FiltroProfissionais = TempData["Filtro"].ToString();
            }

            ViewBag.LstProfisasionais = new Profissionais().ListagemProfissionais(FiltroProfissionais);

            return View();
        }

        public IActionResult FiltroProfissional(string id)
        {
            TempData["Filtro"] = id;
            return RedirectToAction("Profissionais", "Tabelas");
        }

        [HttpGet]
        public IActionResult DetalhesProfissional(string id)
        {
            string FiltroProfissionais = TempData["Filtro"].ToString();
            TempData.Keep("Filtro");

            ViewBag.Dados = new Profissionais().DetalhesProfissionais(FiltroProfissionais);
            return View();
        }

        [HttpPost]
        public IActionResult DetalhesProfissional(ProfissionaisModel Dados)
        {
            bool ret = new Profissionais().GravaDetalhes(Dados);
            TempData.Remove("Filtro");
            return RedirectToAction("FiltroProfissional", "Tabelas");
        }

        public IActionResult DetalhesProfissional2(string id)
        {
            TempData["Filtro"] = id;
            return RedirectToAction("DetalhesProfissional", "Tabelas");
        }

        [HttpGet]
        public IActionResult InclusaoProfissional()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InclusaoProfissional(ProfissionaisModel dados)
        {
            bool ret = new Profissionais().InserirProfissional(dados);
            return RedirectToAction("FiltroProfissional", "Tabelas");
        }


        public IActionResult ExclusaoProfissional()
        {
            string FiltroProfissionais = TempData["Filtro"].ToString();
            TempData.Keep("Filtro");
            ViewBag.Dados = new Profissionais().DetalhesProfissionais(FiltroProfissionais);
            return View();
        }

        [HttpPost]
        public IActionResult ExclusaoProfissional(ProfissionaisModel dados)
        {
            TempData.Remove("Filtro");
            ViewBag.Dados = new Profissionais().ExcluirProfissional(dados);
            return RedirectToAction("FiltroProfissional", "Tabelas");
        }

        public IActionResult ExclusaoProfissional2(string id)
        {
            TempData["Filtro"] = id;
            return RedirectToAction("ExclusaoProfissional", "Tabelas");
        }

        #endregion
        // Rotina Tabela de Categorias    
        public IActionResult TabelaCategoria(string id)
        {
            string FiltroParteNome = id;
            ViewBag.FiltroPublico = FiltroParteNome;
            ViewBag.ListaCategorias = new Categorias().ListagemCategorias(FiltroParteNome);
            return PartialView();
        }


        public IActionResult CategoriaSelecionada(string id)
        {
            //DetalheDoProdutos.id_produto_categoria = "xx";
            DetalheDoProdutos.DescricaoCategoria = "Categoria Nova";
            return RedirectToAction("Tabelas", "Home");
        }

        [HttpPost]
        public JsonResult ListagemCategorias(string id)
        {
            string FiltroParteNome = id;
            ViewBag.FiltroPublico = FiltroParteNome;
            ViewBag.ListaClientes = new Categorias().ListagemCategorias(FiltroParteNome);

            var Tabela = string.Empty;
            foreach (var item in (List<CategoriasModel>)ViewBag.ListaClientes)
            {
                string idcat = string.Format("{0,5: 000000}", item.id).Trim();

                Tabela += "<tr>" +
                "<td class='col-lg-1'>" + idcat + "</td>" +
                "<td class='col-lg-6'>" + item.descricao + "</td>" +
                "<td class='col-lg-1'>" +
                "<button type='button' onclick='Selecionar()'>" +
                "Selecionar" +
                "</button>" +
                "</td>" +
                "</tr>";

                //< img src = "~/images/accept.png" />     

            }
            return Json(Tabela);
        }

        #region
        // Rotina Tabela de Unidade Medida
        public IActionResult TabelaUnidadesMedidas(string id)
        {
            string ParteNome = id;
            ViewBag.LstUnidadesMedida = new Unidades().ListagemUnidadesMedida(ParteNome);
            return PartialView();
        }

        [HttpPost]
        public JsonResult ListagemUnidadesMedidas(string id)
        {
            string FiltroParteNome = id;
            ViewBag.FiltroPublico = FiltroParteNome;
            ViewBag.ListaUnd = new Unidades().ListagemUnidadesMedida(FiltroParteNome);

            var Tabela = string.Empty;
            foreach (var item in (List<UnidadesMedidasModel>)ViewBag.ListaUnd)
            {
                Tabela += "<tr>" +
                "<td class='col-lg-1'>" + item.id + "</td>" +
                "<td class='col-lg-6'>" + item.descricao + "</td>" +
                "<td class='col-lg-1'>" + item.sigla + "</td>" +
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

        // Rotina Tabela de Sub Categorias    
        public IActionResult TabelaSubCategoria(string id)
        {
            string FiltroParteNome = id;
            // ViewBag.FiltroPublico   = FiltroParteNome;
            ViewBag.LstSubCategorias = new Categorias().ListagemSubCategorias(FiltroParteNome);
            return PartialView();
        }


        [HttpPost]
        public JsonResult ListagemSubCategorias(string id)
        {
            string FiltroParteNome = id;
            ViewBag.FiltroPublico = FiltroParteNome;
            ViewBag.ListaSCat = new Categorias().ListagemSubCategorias(FiltroParteNome);

            var Tabela = string.Empty;
            foreach (var item in (List<SubCategoriaModel>)ViewBag.ListaSCat)
            {
                //string idcat = string.Format("{0,5: 000000}", item.id).Trim();

                Tabela += "<tr>" +
                "<td class='col-lg-1'>" + item.id + "</td>" +
                "<td class='col-lg-6'>" + item.descricao + "</td>" +
                "<td class='col-lg-1'>" +
                "<button type='button' onclick='Selecionar()'>" +
                "Selecionar" +
                "</button>" +
                "</td>" +
                "</tr>";

                //< img src = "~/images/accept.png" />     

            }
            return Json(Tabela);
        }



        [HttpPost]
        public JsonResult LocalizarProduto(string id)
        {
            var sessionNome = new Byte[20];

            #region
            /*string Usuariologado = "";            
            try
            {
                HttpContext.Session.TryGetValue("IdUsuarioLogado", out sessionNome);
                Usuariologado = System.Text.Encoding.UTF8.GetString(sessionNome).ToString();                
            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException("Erro :", "", ex);
                throw argEx;
            }
            */
            #endregion

            ProdutosModel dados = new Produtos().ConsultaProduto(id);

            ViewBag.Dados = dados;
            TempData["IdProduto"] = id;

            return Json(dados);
        }

        // Utilitários
        public JsonResult PesquisarCnpj(string id)
        {
            string cnpj = new Suporte().RestaNumero(id);
            ClienteModel dados = new ClienteModel();

            if (ValidaCNPJ.IsCnpj(cnpj))
            {
                if (cnpj != null)
                {
                    CnpjWS cnpjws = new CnpjWS();
                    CnpjModel retorno = cnpjws.ConsultarCnpj(cnpj);

                    if (VerificarConteudo.VerificarPropriedadesNaoNulas(retorno))
                    {
                        //  "Verifique Sua Conexão, ou CNPJ Inexistente"
                        return Json(dados);
                    }

                    dados.cep = retorno.cep.Trim();
                    dados.logradouro = retorno.logradouro.Trim();
                    dados.numero = retorno.numero.Trim();
                    dados.cidade = retorno.municipio.Trim();
                    dados.bairro = retorno.bairro.Trim();
                    dados.uf = retorno.uf.Trim();
                    dados.email = retorno.email.Trim();
                    dados.fone1 = retorno.telefone.Trim();
                    dados.nome = retorno.nome.Trim();

                    //Ibge.Text = retorno.ib
                    foreach (var item in retorno.atividade_principal)
                    {
                        string Atividade = item.text.ToString();
                    }

                    return Json(dados);
                }
            }

            return Json(dados);
        }

        public JsonResult PesquisarCep(string id)
        {
            string cep;
            Endereco end = new Endereco();

            if (id != null)
            {
                cep = id.Trim().Replace("-", "").Replace(".", "");

                if (IsValidCEP(cep))
                {
                    end = (Endereco)CepWS.BuscarenderecoCEP(cep);
                }
            }

            return Json(end);

        }

        public bool IsValidCEP(string cep)
        {
            bool Valido = true;

            if (cep.Length != 8)
            {
                Valido = false;
            }

            // int NovoCEP = 0;

            if (!int.TryParse(cep, out int NovoCEP))
            {
                Valido = false;
            }

            return Valido;
        }

    }
}