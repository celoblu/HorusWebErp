using HorusWebErp.Modelos;
using HorusWebErp.Models.Consultas;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HorusWebErp.ModelosControllers
{
    public class ConsultasController : Controller
    {

        public static int IdCliente { get; set; }

        [HttpGet]
        public IActionResult ConsultaPedidos(int id)
        {
            if (IdCliente > 0)
            {
                id = IdCliente;
            }

            ViewBag.ListaPedidos = new Pedidos().ListagemPedidos(id);

            if (id > 0)
            {
                foreach (var Daddos in (List<PedidosModel>)ViewBag.ListaPedidos)
                {
                    ViewBag.Cliente = Daddos.NomeCliente;
                    break;
                }
            }
            return View();
        }

        public IActionResult ConsultaPedidos2(int id)
        {
            IdCliente = id;
            return RedirectToAction("ConsultaPedidos");
        }

        [HttpGet]
        public IActionResult ItensPedido()
        {
            int id = IdCliente;
            double TotalPedido = 0;

            ViewBag.ListaItensPedido = new Pedidos().ConsultaItensPedidos(id);

            foreach (var item in (List<ItensPedidos>)ViewBag.ListaItensPedido)
            {
                ViewBag.IdCliente = item.IdCliente;
                ViewBag.NomeCliente = item.NomeCliente;
                ViewBag.Logradouro = item.Logradouro;
                ViewBag.Cidade = item.Cidade;
                ViewBag.Numero = item.Numero;
                ViewBag.BAirro = item.Bairro;
                TotalPedido += item.vlrtotal;
            }

            ViewBag.TotalPedido = string.Format("Total R$ {0:0.00}", TotalPedido);
            ViewBag.Id = id;
            return View();
        }

        [HttpGet]
        public IActionResult ItensPedido2(int id)
        {
            IdCliente = id;
            return RedirectToAction("ItensPedido");
        }

        // Recebe id Cliente Selecionado na Pesquisa
        [HttpGet]
        [HttpPost]
        public IActionResult ClientePesquisado(string id)
        {
            var Parametros = id.Split("|");
            return RedirectToAction(Parametros[1], "Consultas", new { @id = Parametros[0] });
        }

        // Rotina Tabela de Cliente Modal     
        public IActionResult TabelaClientes(string id)
        {
            string FiltroParteNome = id;
            ViewBag.FiltroPublico = FiltroParteNome;
            ViewBag.ListaClientes = new Pedidos().ListarClientes(FiltroParteNome);
            return PartialView();
        }

        [HttpPost]
        public JsonResult ListagemCliente(string id)
        {
            string FiltroParteNome = id;
            ViewBag.FiltroPublico = FiltroParteNome;
            ViewBag.ListaClientes = new Pedidos().ListarClientes(FiltroParteNome);

            var Tabela = string.Empty;
            foreach (var item in (List<ClienteModel>)ViewBag.ListaClientes)
            {
                string idcli = string.Format("{0,5: 000000}", item.id).Trim();

                Tabela += "<tr>" +
                "<td class='col-lg-1'>" + idcli + "</td>" +
                "<td class='col-lg-6'>" + item.nome + "</td>" +
                "<td class='col-lg-2'>" + item.cnpj + "</td>" +
                "<td class='col-lg-2'>" + item.cpf + "</td>" +
                "<td class='col-lg-1'><button type='button' class='btn btn-info' onclick='Selecionar()' data-dismiss='modal'>Selecionar</button>" + "</td>" +
                "</tr>";

                //"<td class='col-lg-1'><button type='button' class='btn btn-info' onclick='Selecionar(@item.id)' data-dismiss='modal'>Selecionar</button>" + "</td>" +
            }
            return Json(Tabela);
        }

    }
}