using HorusWebErp.Modelos;
using HorusWebErp.Models.Servicos;
using HorusWebErp.Models.Tabelas;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using LocalReport = Microsoft.Reporting.NETCore.LocalReport;

namespace HorusWebErp.ModelosControllers
{
    public class ServicosController : Controller
    {
        public decimal VlrTotalProdutos { get; set; }
        // private readonly IHostingEnvironment _webHostEnvironment;  (core 2.0)   
        private readonly IWebHostEnvironment _webHostEnvironment;
        public static string Pacote { get; set; }

        public ServicosController(IWebHostEnvironment webHostEnvironment)
        //public ServicosController(IHostingEnvironment webHostEnvironment) // (core 2.0)
        {
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        //******************************
        // Impressão da O.S Selecionada
        //******************************
        public IActionResult Print(string id)
        {
             OrdemServicoModel DadosOS = new Servicos().ConsultaOS(id);


            // Filtra Materiais OS
            // _= new DataTable();

            //DataTable dt = Materiais(); 
            DataTable dt = MateriaisOS(id);

            string RenderFormat = "PDF";
          //  string extension    = "pdf";
            string mimetype     = "application/pdf";
            string DeviceInf    = "<DeviceInfo>" +
            "  <OutputFormat>EMF</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.25in</MarginTop>" +
            "  <MarginLeft>0.25in</MarginLeft>" +
            "  <MarginRight>0.25in</MarginRight>" +
            "  <MarginBottom>0.25in</MarginBottom>" +
            "</DeviceInfo>";

            LocalReport report = new LocalReport();
            // using var rep = new LocalReport();             

            // var path = $"{this._webHostEnvironment.WebRootPath}\\Relatorios\\Rpt.rdlc";
            // Dictionary<string, string> parametros = new Dictionary<string, string>();
            // parametros.Add("param1", "Ordem de Serviço Cliente");
            // parametros.Add("Cliente", DadosOS.NomeCliente);

            try
            {
                report.DataSources.Add(new ReportDataSource("rptOS", dt));


                //ReportParameter[] parameters = new[] { new ReportParameter("Titulo", "Ordem de Serviço Cliente") };                 
                //report.ReportPath = $"{this._webHostEnvironment.WebRootPath}\\Relatorios\\rptOrdemServico.rdlc";
                //report.SetParameters(parameters);

                //ReportParameter[] Parametros = new ReportParameter[2];                
                //p1[0] = new ReportParameter("Titulo", "Ordem de Serviço Cliente");
                //p2[1] = new ReportParameter("Cliente", "Ordem de Serviço Cliente");
                decimal VlrMO = DadosOS.vlrmaoobra;
                decimal VlrMaterial = VlrTotalProdutos;
                decimal VlrTotal = VlrMO + VlrMaterial;

                //string imagePath = new Uri($"{this._webHostEnvironment.WebRootPath}\\images\\logo.jpg").AbsoluteUri;                
                string logo = string.Format("http://htsinformatica.com.br/logos/{0}.jpg", HttpContext.Session.GetString("NomeDatabase"));

                ReportParameter p1 = new ReportParameter("Titulo", "Ordem de Serviço Cliente");
                ReportParameter p2 = new ReportParameter("Cliente", DadosOS.NomeCliente);
                ReportParameter p3 = new ReportParameter("Empresa", HttpContext.Session.GetString("NomeEmpresa"));
                ReportParameter p4 = new ReportParameter("Equipamento", DadosOS.DescricaoEquipamento);
                ReportParameter p5 = new ReportParameter("NumeroOS", DadosOS.Id.ToString());
                ReportParameter p6 = new ReportParameter("VlrMO", string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", VlrMO));
                ReportParameter p7 = new ReportParameter("VlrMaterial", string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", VlrMaterial));
                ReportParameter p8 = new ReportParameter("VlrTotal", string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", VlrTotal));
                ReportParameter p9 = new ReportParameter("CaminhoImagem", logo);
                ReportParameter p10 = new ReportParameter("Memo", DadosOS.conclusao);

                report.EnableExternalImages = true;
                report.ReportPath = $"{this._webHostEnvironment.WebRootPath}\\Relatorios\\rptOrdemServico.rdlc";
                report.SetParameters(new ReportParameter[] {p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });               

                var pdf = report.Render(RenderFormat);
                var pdf2 = report.Render(RenderFormat, DeviceInf);

                return File(pdf2, mimetype);
                
                //var retorno = localRelatorio.Execute(RenderType.Pdf, extension, parametros, mimtype);
                // return File(retorno.MainStream, "application/pdf");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message.ToString();
            }

            return RedirectToAction("ErroSite", "Servicos");

        }
        // Rotina de testes de alimentação de um DataTable 
        private DataTable Materiais()
        {
            var dt = new DataTable();           
            dt.Columns.Add("NomeProduto");         

            DataRow row;
            for (int i = 1; i < 11; i++)
            {
                row = dt.NewRow();                
                row["NomeProduto"] = "Arroz" + i;
                dt.Rows.Add(row);
            }

            return dt;
        }

        [HttpGet]
        public IActionResult ErroSite()
        {
            ViewBag.Msg = TempData["erro"].ToString();

            return View();
        }

        #region    

        #endregion

        public DataTable HorasTrabalhasOS(string id)
        {
            var dtHrs = new DataTable();
            dtHrs.Columns.Add("Tecnico");
            dtHrs.Columns.Add("Qtd");
            dtHrs.Columns.Add("VlrHora");
            dtHrs.Columns.Add("VlrTotalHora");

            DataRow row;

            double TotalMO = 0;
            ViewBag.HrsTrabalhadas = new Servicos().ConsultaOSHoras(id);

            foreach (var item in (List<OsHorasModel>)ViewBag.HrsTrabalhadas)
            {
                TotalMO += item.qtdhoras * item.valorhora;
                var Vlrs = item.qtdhoras * item.valorhora;

                row = dtHrs.NewRow();
                row["Tecnico"] = item.nome;
                row["Qtd"] = item.qtdhoras;
                row["VlrHora"] = item.valorhora.ToString("C2", CultureInfo.CurrentCulture);
                row["VlrTotalHora"] = Vlrs.ToString("C2", CultureInfo.CurrentCulture);
                dtHrs.Rows.Add(row);

            }

            TempData["TotalHoras"] = TotalMO;
            return dtHrs;

        }

        public DataTable MateriaisOS(string id)
        {
            ViewBag.ListagemMaterias = new Servicos().ListagemMateriaisBmOs(id);
            VlrTotalProdutos = 0;
            decimal Vlrtotal = 0;

            var dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("NomeProduto");
            dt.Columns.Add("Quantidade");
            dt.Columns.Add("VlrUnitario");
            dt.Columns.Add("VlrTotal");

            DataRow row;
            foreach (var item in (List<ItensOrdemServicoModel>)ViewBag.ListagemMaterias)
            {

                Vlrtotal = item.quantidade * item.valor_unitario;
                VlrTotalProdutos += Vlrtotal;

                row = dt.NewRow();
                 row["Id"] = item.id;
                 row["NomeProduto"] = item.NomeProduto;
                 row["Quantidade"] = item.quantidade;
                 row["VlrUnitario"] = item.valor_unitario;
                 row["VlrTotal"]    = Vlrtotal.ToString("C2", CultureInfo.CurrentCulture);

                dt.Rows.Add(row);                
            }

            return dt;
        }

        [HttpGet]
        public IActionResult OrdensDeServico()
        {
            string Filtro = "";
            string TipoFiltro = "";

            if (TempData["Filtro"] != null && TempData["Tipo"] != null)
            {
                Filtro = TempData["Filtro"].ToString();
                TempData.Keep("Filtro");
                TipoFiltro = TempData["Tipo"].ToString();
                TempData.Keep("Tipo");
            }

            ViewBag.ListaOrdemServicos = new Servicos().ListagemServicos(Filtro, TipoFiltro);
            return View();
        }

        public IActionResult PesquisaOS(string id)
        {
            string[] Retorno = id.Split("|");
            TempData["Filtro"] = Retorno[0];
            TempData["Tipo"] = Retorno[1];

            return RedirectToAction("OrdensDeServico", "Servicos");
        }

        [HttpGet]
        public IActionResult InclusaoOS()
        {
            //ViewBag.Dados = new Servicos().ConsultaOS(Pacote);            
            ViewBag.Dados = new OrdemServicoModel();
            return View();
        }

        [HttpPost]
        public IActionResult InclusaoOS(OrdemServicoModel DadosForm)
        {
            Boolean Retorno = new Servicos().InserirOS(DadosForm);

            if (!Retorno)
            {
                // TODO - Temos que construir uma rotina de tratamento de erros
            }

            return RedirectToAction("OrdensDeServico", "Servicos");
        }

        [HttpGet]
        public IActionResult InclusaoOS2(string id)
        {
            // Pacote = id;
            return RedirectToAction("InclusaoOS", "Servicos"); //, new { @id = Pacote });
        }


        [HttpGet]
        public IActionResult DetalhesOS()
        {
            ViewBag.DetalhesOS = new Servicos().ConsultaOS(Pacote);
            ViewBag.ListagemMaterias = new Servicos().ListagemMateriaisBmOs(Pacote);

            decimal TotalMateriais = 0;
            foreach (var item in (List<ItensOrdemServicoModel>)ViewBag.ListagemMaterias)
            {
                TotalMateriais += item.quantidade * item.valor_unitario;
            }

            double TotalMO = 0;
            ViewBag.HrsTrabalhadas = new Servicos().ConsultaOSHoras(Pacote);
            foreach (var item in (List<OsHorasModel>)ViewBag.HrsTrabalhadas)
            {
                TotalMO += item.qtdhoras * item.valorhora;
            }

            Pacote = "";

            ViewBag.ValorMateriais = TotalMateriais;
            ViewBag.ValorMO = TotalMO;
            ViewBag.ValorTotal = TotalMateriais + Convert.ToDecimal(TotalMO);

            return View();
        }

        [HttpGet]
        public IActionResult DetalhesOS2(string id)
        {
            Pacote = id;
            return RedirectToAction("DetalhesOS", "Servicos");
        }

        /// <summary>
        /// Equipamentos
        /// </summary>
        /// <returns></returns>
        #region        
        [HttpGet]
        public IActionResult Equipamentos()
        {
            string Filtro = "";

            if (TempData["FiltroEquipamento"] != null)
            {
                Filtro = TempData["FiltroEquipamento"].ToString();
                TempData.Keep("FiltroEquipamento");
            }

            ViewBag.ListaEquipamentos = new Servicos().ListagemEquipamentos(Filtro);

            return View();
        }

        public IActionResult PesquisaEquipamento(string id)
        {
            TempData["FiltroEquipamento"] = id;
            return RedirectToAction("Equipamentos", "Servicos");
        }

        [HttpGet]
        public IActionResult IncluirEquipamento()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IncluirEquipamento(EquipamenotosModel Dados)
        {
            ViewBag.ListaEquipamentos = new Servicos().IncluirEquipamentos(Dados);
            return View();
        }

        [HttpPost]
        public JsonResult ExcluirEquipamento(string id)
        {
            new Servicos().ExcluirEquipamento(id);

            ViewBag.ListaEquipamentos = new Servicos().ListagemEquipamentos("");

            string Tabela = "";

            foreach (var item in (List<EquipamenotosModel>)ViewBag.ListaEquipamentos)
            {
                Tabela += "<tr>" +
                        "<td class='col-lg-1' style='display:none'>" + item.id + "</td>" +
                        "<td class='col-lg-5'>" + item.Descricao + "</td>" +
                        "<td  class='col-lg-1'><button type = button class='btn btn-info' onclick='Detalhes();'>Detalhes    </button></td>" +
                        "<td  class='col-lg-1'><button type = button class='btn btn-danger' onclick='Excluir();'>Excluir    </button></td>" +
                        "</td>" +
                        "</tr>";
            }

            return Json(Tabela);
        }

        [HttpGet]
        public IActionResult DetalhesEquipamento2(string id)
        {
            TempData["IdEquip"] = id;
            return RedirectToAction("DetalhesEquipamento", "Servicos");
        }

        [HttpGet]
        public IActionResult DetalhesEquipamento()
        {
            string id = TempData["IdEquip"].ToString();
            ViewBag.Dados = new Servicos().ConsultaEquipamento(id);
            return View();
        }

        [HttpPost]
        public IActionResult DetalhesEquipamento(EquipamenotosModel Dados)
        {
            new Servicos().AlterarEquipamentos(Dados);
            return RedirectToAction("Equipamentos", "Servicos");
        }

        #endregion

        /// <summary>
        /// Incluir Materias na O.S
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult IncluirMateriais2(string id)
        {
            TempData["IDOS"] = id;
            return RedirectToAction("IncluirMateriais", "Servicos");
        }

        [HttpGet]
        public IActionResult IncluirMateriais()
        {
            string id = TempData["IDOS"].ToString();
            ViewBag.ListagemMaterias = new Servicos().ListagemMateriaisBmOs(id);
            ViewBag.IdOS = id;

            return View();
        }

        [HttpPost]
        public JsonResult LocalizarPlaca(string id)
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

            OrdemServicoModel dados = new Servicos().PesquisaPlaca(id);
            ViewBag.Dados = dados;
            TempData["Placa"] = id;

            return Json(dados);
        }


        // Rotina Tabelas  
        public IActionResult TabelaVeiculos(string id)
        {
            string FiltroParteNome = id;
            ViewBag.ListaVeiculos = new Veiculos().ListagemVeiculos(FiltroParteNome);
            return PartialView();
        }

        [HttpPost]
        public JsonResult ListagemVeiculos(string id)
        {
            string FiltroParteNome = id;
            ViewBag.FiltroPublico = FiltroParteNome;
            ViewBag.ListaVeiculos = new Veiculos().ListagemVeiculos(FiltroParteNome);

            var Tabela = string.Empty;
            foreach (var item in (List<VeiculosModel>)ViewBag.ListaVeiculos)
            {
                string IdVeiculo = string.Format("{0,5: 000000}", item.Id).Trim();

                Tabela += "<tr>" +
                "<td class='col-lg-1'>" + IdVeiculo + "</td>" +
                "<td class='col-lg-6'>" + item.Descricao + "</td>" +
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
        public JsonResult FiltrarVeiculo(string id) //, string IdCliente
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

            OrdemServicoModel dados = new Servicos().PesquisaPlaca(id);
            ViewBag.Dados = dados;
            TempData["Placa"] = id;

            return Json(dados);
        }

        // Clientes
        public IActionResult TabelaClientes(string id)
        {
            string FiltroParteNome = id;
            ViewBag.ListaClientes = new Clientes().ListagemClientes(FiltroParteNome);
            return PartialView();
        }

        // Relação dos Materiais e Serviço Utilizados
        public IActionResult ItensOS(string id)
        {
            var ids = id.Split("|");
            decimal VlrMo = new Servicos().ConsultaOS(ids[0]).vlrmaoobra;
            if (VlrMo < 0)
            {
                VlrMo = 0;
            }

            ViewBag.TotalMaoDeObra = VlrMo;
            // ViewBag.ListagemMaterias = new Servicos().ListagemMateriaisOs(ids[1]);
            ViewBag.ListagemMaterias = new Servicos().ListagemMateriaisBmOs(ids[0]);

            return PartialView();
        }

        // Relação Horas Trabalhadas
        public IActionResult HorasTrabalhadas(string id)
        {
            var ids = id.Split("|");
            ViewBag.id_os = ids[0];
            ViewBag.HrsTrabalhadas = new Servicos().ConsultaOSHoras(ids[0]);
            ViewBag.Profissionais = new Profissionais().ListagemProfissionais("");
            ViewBag.TotalMo = 0;
            return PartialView();
        }

        [HttpPost]
        public JsonResult ListagemClientes(string id)
        {
            string FiltroParteNome = id;
            ViewBag.FiltroPublico = FiltroParteNome;
            ViewBag.ListaClientes = new Clientes().ListagemClientes(FiltroParteNome);

            var Tabela = string.Empty;
            foreach (var item in (List<ClienteModel>)ViewBag.ListaClientes)
            {
                string IdCli = string.Format("{0,5: 000000}", item.id).Trim();

                Tabela += "<tr>" +
                "<td class='col-lg-1'>" + IdCli + "</td>" +
                "<td class='col-lg-6'>" + item.nome + "</td>" +
                "<td class='col-lg-5'>" + item.logradouro + "</td>" +
                 "<td class='col-lg-1'><button type='button' class='btn btn-danger' onclick='Selecionar()'>Selecionar</button>" +
                "</td>" +
                "</tr>";

                //< img src = "~/images/accept.png" /> 
            }
            return Json(Tabela);
        }

        [HttpPost]
        public JsonResult IncluirProdutosOS(string IdOs, string idp, string qtd, string vlru)
        {
            new Servicos().InserirMateriaisOS(IdOs, idp, qtd, vlru);

            // ViewBag.ListagemMaterias = new Servicos().ListagemMateriaisBmOs(IdOs);          

            string Tabela = MontaTabelaHtmlMat(IdOs);
            return Json(Tabela);
        }

        /// <summary>
        /// Excluir Determinado Material da O.S
        /// </summary>
        /// <param name="id"> ID Da Tabela  Materiais Utilizados </param>
        /// <param name="IdOs">ID Da O.S</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExcluirProdutosOS(string id, string IdOs)
        {
            new Servicos().ExcluirMateriaisOS(id);

            string Tabela = MontaTabelaHtmlMat(IdOs);
            return Json(Tabela);
        }

        public string MontaTabelaHtmlMat(string Id)
        {

            ViewBag.ListagemMaterias = new Servicos().ListagemMateriaisBmOs(Id);
            ViewBag.IdOS = Id;

            string Tabela = string.Empty;
            decimal Valor = 0;
            decimal Total = 0;

            foreach (var item in (List<ItensOrdemServicoModel>)ViewBag.ListagemMaterias)
            {
                string DescricaoProduto = "";

                if (item.NomeProdutoX == null)
                {
                    DescricaoProduto = item.NomeProduto.Trim();
                }
                else
                {
                    DescricaoProduto = item.NomeProdutoX.Trim();
                }

                Valor = item.valor_unitario * item.quantidade;
                Total += Valor;

                Tabela += "<tr>" +
                "<td class='col-lg-1' style='display:none'>" + item.id + "</td>" +
                "<td class='col-lg-5'>" + DescricaoProduto + "</td>" +
                "<td class='col-lg-1'>" + item.quantidade + "</td>" +
                "<td class='col-lg-1 text-right' id='vlr'>" + string.Format("R$  {0:N2}", item.valor_unitario) + "</td>" +
                "<td class='col-lg-1 text-right' id='vlr'>" + string.Format("R$  {0:N2}", Valor) + "</td>" +
                "<td class='col-lg-1'><button type='button' class='btn btn-danger' onclick='Excluir()'>Excluir</button>" +
                "</td>" +
                "</tr>";
            }

            return Tabela;

        }

        /// <summary>
        /// Horas Trabalhadas por Técnico(Profissional) na O.S
        /// </summary>
        /// <param name="id"> ID O.S </param>
        /// <param name="idp"> ID do Profissional </param>
        /// <param name="qtdH"> Qtd. Hrs trabalhadas </param>
        /// <param name="vlru"> Vlr Unitário Do Serviço </param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InserirHoras(string id, string idprof, string qtdH, string vlru)
        {
            string Tabela = MontaTabelaHtml(id);

            if (string.IsNullOrEmpty(idprof) || Int32.Parse(idprof) <1)
            {
                return Json("");
            }   

            new Servicos().InserirHoras(id, idprof, qtdH, vlru);
            //ViewBag.HrsTrabalhadas = new Servicos().ConsultaOSHoras(id);

            Tabela = MontaTabelaHtml(id);
            object Totalizador = TempData["Valor"];
            double Total = Convert.ToDouble(Totalizador);

            // Totaliza Horas na Tabela de O.P
            new Servicos().AtualizaTotalMO(id, Total);

            return Json(Tabela);
        }

        /// <summary>
        /// Excluir Determinada Horas Aplicadas na O.S
        /// </summary>
        /// <param name="id"> ID Da Tabela Horas Trab. por Profissional</param>
        /// <param name="IdOs">ID Da O.S</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExcluirHoras(string id, string IdOs)
        {
            new Servicos().ExcluirHoras(id);

            // ViewBag.HrsTrabalhadas = new Servicos().ConsultaOSHoras(IdOs);

            string Tabela = MontaTabelaHtml(IdOs);
            object Totalizador = TempData["Valor"];
            double Total = Convert.ToDouble(Totalizador);

            // Totaliza Horas na Tabela de O.P ---------
            new Servicos().AtualizaTotalMO(IdOs, Total);
            //------------------------------------------

            return Json(Tabela);

        }

        public string MontaTabelaHtml(string Id)
        {
            ViewBag.HrsTrabalhadas = new Servicos().ConsultaOSHoras(Id);

            double Valor = 0;
            double Total = 0;
            string Tabela = string.Empty;

            foreach (var item in (List<OsHorasModel>)ViewBag.HrsTrabalhadas)
            {
                // string IdTec = string.Format("{0,3: 0000}", item.id).Trim();
                Valor = item.valorhora * item.qtdhoras;
                Total += Valor;

                Tabela += "<tr>" +
                "<td class='col-lg-1' style='display:none'>" + item.id + "</td>" +
                "<td class='col-lg-6'>" + item.nome + "</td>" +
                "<td class='col-lg-2'>" + item.qtdhoras + "</td>" +
                "<td class='col-lg-2 text-right' id='vlr'>" + string.Format("R$  {0:N2}", Valor) + "</td>" +
                "<td class='col-lg-1'><button type='button' class='btn btn-info' onclick='Excluir()'>Excluir</button>" +
                "</td>" +
                "</tr>";
            }

            TempData["Valor"] = Total;

            return Tabela;
        }

        [HttpPost]
        public JsonResult DadosCliente(string id) //, string IdCliente
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

            ClienteModel dados = new Clientes().PesquisaCliente(id);
            ViewBag.Dados = dados;


            return Json(dados);
        }

    }
}