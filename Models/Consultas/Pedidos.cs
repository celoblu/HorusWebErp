using HorusWebErp.Modelos;
using HorusWebErp.Utilitarios;
using System;
using System.Collections.Generic;
using System.Data;

namespace HorusWebErp.Models.Consultas
{
    public class Pedidos
    {
        public List<PedidosModel> ListagemPedidos(int? Id)
        {
            string sql = "";
            PedidosModel Dados;
            List<PedidosModel> lst = new List<PedidosModel>();

            if (Id > 0)
            {
                sql = "select bm_pedido.ID, bm_pedido.id_entidade, bm_pedido.valor, bm_pedido.DtCadastro, bm_entidade.nome" +
                      " from bm_pedido" +
                      " left outer join bm_entidade" +
                     $" on bm_pedido.id_entidade = bm_entidade.id" +
                     $" where bm_pedido.id_entidade='{Id}' order by data";
            }
            else
            {
                return lst;
            }

            DAL ObjDal = new DAL();
            DataTable dt = ObjDal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ///
                DateTime dta = DateTime.Parse(dt.Rows[i]["DtCadastro"].ToString());
                ///

                Dados = new PedidosModel
                {
                    ID = dt.Rows[i]["id"].ToString(),
                    NomeCliente = dt.Rows[i]["Nome"].ToString(),
                    valor = double.Parse(dt.Rows[i]["Valor"].ToString()),
                    dtcadastro = DateTime.Parse(dt.Rows[i]["DtCadastro"].ToString())
                };

                lst.Add(Dados);
            }

            ObjDal.FechaConexao();

            return lst;
        }


        public List<ItensPedidos> ConsultaItensPedidos(int? Id)
        {
            string sql = "";
            ItensPedidos Dados;
            List<ItensPedidos> lst = new List<ItensPedidos>();

            if (Id > 0)
            {
                sql = "select bm_pedido_item.ID, bm_pedido_item.id_pedido, bm_pedido_item.id_produto, bm_pedido_item.vlrunitario," +
                    " bm_pedido_item.quantidade,bm_Produto.descricao," +
                    " bm_Entidade.id as IdCliente, bm_Entidade.nome as NomeCliente, bm_Entidade.Logradouro, bm_Entidade.numero, bm_Entidade.bairro,  bm_Entidade.Cidade" +
                    " from bm_pedido_item" +
                    " left outer join bm_Produto" +
                    " on bm_pedido_item.id_produto = bm_Produto.id" +
                    " left outer join bm_pedido" +
                    " on bm_pedido_item.id_pedido = bm_pedido.id" +
                    " left outer join bm_entidade" +
                    " on bm_pedido.id_entidade = bm_entidade.id" +
                   $" where bm_pedido_item.id_pedido ='{Id}' order by descricao";
            }

            DAL ObjDal = new DAL();
            DataTable dt = ObjDal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dados = new ItensPedidos
                {
                    IdCliente = dt.Rows[i]["IdCliente"].ToString(),
                    id_pedido = int.Parse(dt.Rows[i]["id_pedido"].ToString()),
                    id_produto = int.Parse(dt.Rows[i]["Id_produto"].ToString()),
                    NomeProduto = dt.Rows[i]["Descricao"].ToString(),
                    vlrunitario = double.Parse(dt.Rows[i]["Vlrunitario"].ToString()),
                    quantidade = double.Parse(dt.Rows[i]["Quantidade"].ToString()),
                    NomeCliente = dt.Rows[i]["NomeCliente"].ToString(),
                    Logradouro = dt.Rows[i]["Logradouro"].ToString(),
                    Numero = dt.Rows[i]["Numero"].ToString(),
                    Cidade = dt.Rows[i]["Cidade"].ToString(),
                    Bairro = dt.Rows[i]["Bairro"].ToString(),
                    vlrtotal = double.Parse(dt.Rows[i]["Vlrunitario"].ToString()) * double.Parse(dt.Rows[i]["Quantidade"].ToString())

                };

                lst.Add(Dados);
            }

            ObjDal.FechaConexao();

            return lst;
        }


        public List<ClienteModel> ListarClientes(string Filtro)
        {

            string sql;
            List<ClienteModel> lst = new List<ClienteModel>();

            if (Filtro != null)
            {
                Filtro = "%" + Filtro.ToUpper().Trim() + "%";
                sql = "select bm_entidade.id, bm_entidade.Nome, bm_entidade.cnpj, bm_entidade.cpf" +
                      " from bm_entidade" +
                     $" Where nome LIKE '{Filtro}'" +
                      " order by nome";
            }
            else
            {
                sql = "select bm_entidade.id, bm_entidade.Nome, bm_entidade.cnpj, bm_entidade.cpf " +
                         "from bm_entidade order by nome";
            }

            DAL objDal = new DAL();
            DataTable dt = objDal.RetDataTable(sql);
            ClienteModel Dados;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dados = new ClienteModel
                {
                    id = int.Parse(dt.Rows[i]["id"].ToString()),
                    nome = dt.Rows[i]["nome"].ToString(),
                    cpf = dt.Rows[i]["cpf"].ToString(),
                    cnpj = dt.Rows[i]["cnpj"].ToString()
                };

                lst.Add(Dados);

            }

            objDal.FechaConexao();

            return lst;
        }
    }
}
