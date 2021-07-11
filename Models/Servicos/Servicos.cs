using Dapper;
using HorusWebErp.Modelos;
using HorusWebErp.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HorusWebErp.Models.Servicos
{
    public class Servicos
    {
        OrdemServicoModel DadosOS = new OrdemServicoModel();

        // Equipamentos
        #region
        public bool IncluirEquipamentos(EquipamenotosModel Dados)
        {
            string sql = $"insert into bm_os_equipamentos(Descricao) Value('{Dados.Descricao.ToUpper()}')";

            DALDPR db = new DALDPR();
            var daldapper = db.DALCON();
            daldapper.Execute(sql, new EquipamenotosModel());
            daldapper.Close();

            return true;
        }

        public EquipamenotosModel ConsultaEquipamento(string Id)
        {
            string sql = $"Select * From bm_os_equipamentos where id = '{Id}'";

            DALDPR db = new DALDPR();
            var daldapper = db.DALCON();
            EquipamenotosModel dados = daldapper.Query<EquipamenotosModel>(sql).FirstOrDefault();
            daldapper.Close();

            return dados;
        }

        // Alterar Equipamentos
        public bool ExcluirEquipamento(string id)
        {
            string sql = $"delete from bm_os_equipamentos where id = '{id}'";

            DALDPR db = new DALDPR();
            var daldapper = db.DALCON();
            daldapper.Execute(sql, new EquipamenotosModel());
            daldapper.Close();

            return true;
        }

        // excluir Equipamentos
        public bool AlterarEquipamentos(EquipamenotosModel dados)
        {
            string Id = dados.id.ToString();
            string sql = $"update bm_os_equipamentos set descricao = '{dados.Descricao.ToUpper()}' where id = '{Id}'";

            DALDPR db = new DALDPR();
            var daldapper = db.DALCON();
            daldapper.Execute(sql, new EquipamenotosModel());
            daldapper.Close();

            return true;
        }


        public List<EquipamenotosModel> ListagemEquipamentos(string Filtrargem)
        {
            string sql = "";
            string Filtro = "";

            if (!string.IsNullOrWhiteSpace(Filtrargem))
            {
                // Filtro = "Nome LIKE " + "%" + Filtrargem.ToUpper().Trim() + "%";

                Filtro = "%" + Filtrargem.ToUpper().Trim() + "%";

                sql = "Select * from bm_os_equipamentos" +
                     $" where Descricao LIKE '{Filtro}' order by Descricao";
            }
            else
            {
                sql = "Select * from bm_os_equipamentos" +
                      " order by Descricao limit 15";
            }

            DALDPR db = new DALDPR();
            var daldapper = db.DALCON();
            List<EquipamenotosModel> lst = daldapper.Query<EquipamenotosModel>(sql).ToList();
            daldapper.Close();
            return lst;
        }
        #endregion


        public List<OrdemServicoModel> ListagemServicos(string Filtrargem, string TipoFiltro)
        {
            List<OrdemServicoModel> lst = new List<OrdemServicoModel>();

            string sql = "";
            string Filtro = "";

            if (!string.IsNullOrWhiteSpace(Filtrargem))
            {
                // Filtro = "Nome LIKE " + "%" + Filtrargem.ToUpper().Trim() + "%";

                Filtro = "%" + Filtrargem.ToUpper().Trim() + "%";

                sql = "Select bm_os.*," +
                    " bm_entidade.nome as NomeCliente, bm_entidade.Logradouro, bm_entidade.Cidade, bm_entidade.Bairro, bm_entidade.Fone1, bm_entidade.Fone2," +
                    " bm_os_equipamentos.descricao as DescricaoEquipamento" +
                    " from bm_os" +
                    " left outer join bm_os_equipamentos" +
                    " on bm_os.id_os_equipamentos = bm_os_equipamentos.id" +
                    " left outer join bm_entidade" +
                    " on bm_os.id_entidade = bm_entidade.id";

                if (int.Parse(TipoFiltro) == 0)
                {
                    sql += $" Where Nome LIKE '{Filtro}'";
                }
                else
                {
                    sql += $" Where Placa LIKE '{Filtro}'";
                }

                sql += " order by bm_os.id desc";
            }
            else
            {
                sql = "Select bm_os.*," +
                    " bm_entidade.nome as NomeCliente, bm_entidade.Logradouro, bm_entidade.Cidade, bm_entidade.Bairro, bm_entidade.Fone1, bm_entidade.Fone2," +
                    " bm_os_equipamentos.descricao as DescricaoEquipamento" +
                    " from bm_os" +
                    " left outer join bm_os_equipamentos" +
                    " on bm_os.id_os_equipamentos = bm_os_equipamentos.id" +
                    " left outer join bm_entidade" +
                    " on bm_os.id_entidade = bm_entidade.id" +
                    " order by bm_os.id desc limit 15";
            }

            DALDPR db = new DALDPR();
            var daldapper = db.DALCON();
            lst = daldapper.Query<OrdemServicoModel>(sql).ToList();
            daldapper.Close();
            return lst;

            #region
            /*
            DAL db = new DAL();
            DataTable dt = new DataTable();
            OrdemServicoModel dados;
            dt = db.RetDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dados = new OrdemServicoModel
                {
                    NomeCliente = dt.Rows[i]["NomeCliente"].ToString(),
                    Endereco = dt.Rows[i]["Logradouro"].ToString(),
                    Cidade = dt.Rows[i]["Cidade"].ToString(),
                    Bairro = dt.Rows[i]["Bairro"].ToString(),
                    Fone1 = dt.Rows[i]["Fone1"].ToString(),
                    Fone2 = dt.Rows[i]["Fone2"].ToString(),
                    DescricaoEquipamento = dt.Rows[i]["DescricaoEquipamento"].ToString(),
                    id_entidade = int.Parse(dt.Rows[i]["id_cliente_gcfox"].ToString()),
                    id_os_gcfox = int.Parse(dt.Rows[i]["id_os_gcfox"].ToString()),
                    id_os_equipamentos = int.Parse(dt.Rows[i]["id_os_equipamentos"].ToString()),
                    placa = dt.Rows[i]["placa"].ToString(),
                    dataentrada = dt.Rows[i]["dataentrada"].ToString(),
                    Id = int.Parse(dt.Rows[i]["id"].ToString())
                };
                lst.Add(dados);
            }

            db.FechaConexao();

            return lst;
            */
            #endregion
        }

        // Inserção de Materiais
        public void InserirMateriaisOS(string idos, string idMaterial, string qtd, string vlrUnitario)
        {
            Procedures Util = new Procedures();

            string Vlr = Util.VerificarNum(vlrUnitario);
            string Qtd = Util.VerificarNum(qtd);

            string sql = "insert into bm_os_itens(id_os, Id_produto, Quantidade, Valor_unitario) value(" +
                         $"'{idos}', '{idMaterial}',  '{Qtd}', '{Vlr}')";

            DALDPR db = new DALDPR();
            var dalpper = db.DALCON();
            dalpper.Execute(sql, new ItensOrdemServicoModel());
            dalpper.Close();

            return;
        }

        // Materiais lancados na OS
        public List<ItensOrdemServicoModel> ListagemMateriaisBmOs(string id)
        {
            List<ItensOrdemServicoModel> lst = new List<ItensOrdemServicoModel>();

            string sql = "Select  bm_os_itens.id, bm_os_itens.id_produto, bm_os_itens.Descricao as NomeProdutoX," +
                " bm_os_itens.quantidade," +
                " bm_os_itens.valor_unitario," +
                " bm_produto.Descricao as NomeProduto" +
                " from bm_os_itens" +
                " left outer join bm_produto" +
                " on bm_os_itens.id_produto = bm_produto.id" +
               $" Where id_os =  '{id}'";

            DALDPR db = new DALDPR();
            var daldapper = db.DALCON();
            lst = daldapper.Query<ItensOrdemServicoModel>(sql).ToList();
            daldapper.Close();
            return lst;

        }

        //-------------------------
        // Excluir Materiais da O.S
        //-------------------------
        public void ExcluirMateriaisOS(string Id)
        {
            string sql = $"Delete from bm_os_itens where id = '{Id}'";

            DALDPR db = new DALDPR();
            MySql.Data.MySqlClient.MySqlConnection daldapper = db.DALCON();
            daldapper.Execute(sql, new OsHorasModel());
            daldapper.Close();

            return;
        }

        // Materiais lancados na OS
        public List<ItensOrdemServicoModel> ListagemMateriaisOs(string id)
        {
            List<ItensOrdemServicoModel> lst = new List<ItensOrdemServicoModel>();

            string sql = "Select bm_os_itens.id_produto, bm_os_itens.Descricao as NomeProdutoX, bm_os_itens.quantidade," +
                " bm_os_itens.valor_unitario," +
                " bm_produto.Descricao as NomeProduto" +
                " from bm_os_itens" +
                " left outer join bm_produto" +
                " on bm_os_itens.id_produto = bm_produto.id" +
               $" Where id_os_gcfox =  '{id}'";

            DALDPR db = new DALDPR();
            var daldapper = db.DALCON();
            lst = daldapper.Query<ItensOrdemServicoModel>(sql).ToList();
            daldapper.Close();
            return lst;

        }

        // Consulta Individual
        public OrdemServicoModel ConsultaOS(string Id)
        {
            string sql = "";

            sql = "Select bm_os.*," +
                  " bm_entidade.nome as NomeCliente, bm_entidade.Logradouro, bm_entidade.Cidade, bm_entidade.Bairro, bm_entidade.Fone1, bm_entidade.Fone2," +
                  " bm_os_equipamentos.descricao as DescricaoEquipamento" +
                  " from bm_os" +
                  " left outer join bm_os_equipamentos" +
                  " on bm_os.id_os_equipamentos = bm_os_equipamentos.id_equipamento_gcfox" +
                  " left outer join bm_entidade" +
                  " on bm_os.id_entidade = bm_entidade.id" +
                 $" where bm_os.id = '{Id}' ";

            DALDPR db = new DALDPR();
            var daldapper = db.DALCON();
            OrdemServicoModel Registro = daldapper.Query<OrdemServicoModel>(sql).FirstOrDefault();
            daldapper.Close();

            return Registro;

            #region
            /*
            DAL db = new DAL();
            DataTable dt = new DataTable();
            OrdemServicoModel Dados = new OrdemServicoModel();

            dt = db.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dados = new OrdemServicoModel
                {
                    NomeCliente = dt.Rows[i]["NomeCliente"].ToString(),
                    Endereco = dt.Rows[i]["Logradouro"].ToString(),
                    Cidade = dt.Rows[i]["Cidade"].ToString(),
                    Bairro = dt.Rows[i]["Bairro"].ToString(),
                    Fone1 = dt.Rows[i]["Fone1"].ToString(),
                    Fone2 = dt.Rows[i]["Fone2"].ToString(),
                    DescricaoEquipamento = dt.Rows[i]["DescricaoEquipamento"].ToString(),
                    id_entidade = int.Parse(dt.Rows[i]["id_cliente_gcfox"].ToString()),
                    id_os_gcfox = int.Parse(dt.Rows[i]["id_os_gcfox"].ToString()),
                    id_os_equipamentos = int.Parse(dt.Rows[i]["id_os_equipamentos"].ToString()),
                    placa = dt.Rows[i]["placa"].ToString(),
                    dataentrada = dt.Rows[i]["dataentrada"].ToString(),
                    km = dt.Rows[i]["km"].ToString(),
                    modelo = dt.Rows[i]["modelo"].ToString(),
                    ano = dt.Rows[i]["ano"].ToString(),
                    conclusao = dt.Rows[i]["conclusao"].ToString(),
                    Id = int.Parse(dt.Rows[i]["id"].ToString())
                };
            }

            db.FechaConexao();
            return Dados;
            */
            #endregion
        }


        // Consulta Individual
        public List<OsHorasModel> ConsultaOSHoras(string Id)
        {
            string sql = "";

            sql = " Select bm_os_tecnicos.id, bm_os_tecnicos.qtdhoras, bm_os_tecnicos.valorhora, bm_os_tecnicos.hrsini, bm_os_tecnicos.hrsfim, bm_os_tecnicos.id_os," +
                  " bm_profissional.nome, bm_profissional.apelido" +
                  " from bm_os_tecnicos" +
                  " left outer join bm_profissional" +
                  " on bm_os_tecnicos.id_profissional = bm_profissional.id" +
                 $" where bm_os_tecnicos.id_os = '{Id}' ";

            DALDPR db = new DALDPR();
            var daldapper = db.DALCON();
            List<OsHorasModel> Registros = daldapper.Query<OsHorasModel>(sql).ToList();
            daldapper.Close();

            return Registros;
        }

        // Inserção de Horas trabalhadas
        public void InserirHoras(string id, string idp, string qtdH, string vlru)
        {
            string sql = "Insert into bm_os_tecnicos(bm_os_tecnicos.qtdhoras, bm_os_tecnicos.valorhora, bm_os_tecnicos.id_os, bm_os_tecnicos.id_profissional)" +
                  $" Value('{qtdH}','{vlru}','{id}','{idp}')";

            // var daldapper = db.DALCON();     
            DALDPR db = new DALDPR();
            MySql.Data.MySqlClient.MySqlConnection daldapper = db.DALCON();
            daldapper.Execute(sql, new OsHorasModel());
            daldapper.Close();

            return;
        }

        // Excluir Horas trabalhadas
        public void ExcluirHoras(string IdHoras)
        {
            string sql = $"Delete from bm_os_tecnicos where id = '{IdHoras}'";

            // var daldapper = db.DALCON();     
            DALDPR db = new DALDPR();
            MySql.Data.MySqlClient.MySqlConnection daldapper = db.DALCON();
            daldapper.Execute(sql, new OsHorasModel());
            daldapper.Close();

            return;
        }

        public void AtualizaTotalMO(string id_os, double TotalMO)
        {
            Procedures Util = new Procedures();

            string VlrMO = Util.VerificarNum(Convert.ToDecimal(TotalMO).ToString("0.00"));
            string sql = $"Update bm_os set vlrmaoobra = '{VlrMO}' where id = '{id_os}' ";

            DALDPR db = new DALDPR();
            var daldapper = db.DALCON();

            try
            {
                daldapper.Execute(sql, new OrdemServicoModel());
                daldapper.Close();
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
            }

            return;
        }


        // Filtrar pela Placa
        public OrdemServicoModel PesquisaPlaca(string NumerodaPlaca)
        {
            string sql = "";

            sql = "Select bm_os.*," +
                  " bm_entidade.nome as NomeCliente, bm_entidade.Logradouro, bm_entidade.Cidade, bm_entidade.Bairro, bm_entidade.Fone1, bm_entidade.Fone2, bm_entidade.id as id_entidade," +
                  " bm_os_equipamentos.descricao as DescricaoEquipamento" +
                  " from bm_os" +
                  " left outer join bm_os_equipamentos" +
                  " on bm_os.id_os_equipamentos = bm_os_equipamentos.id_equipamento_gcfox" +
                  " left outer join bm_entidade" +
                  " on bm_os.id_entidade = bm_entidade.id" +
                 $" where bm_os.placa = '{NumerodaPlaca}' order by id desc LIMIT 1";


            DALDPR db = new DALDPR();
            var daldapper = db.DALCON();
            OrdemServicoModel Registro = daldapper.Query<OrdemServicoModel>(sql).FirstOrDefault();
            daldapper.Close();

            return Registro;

            #region
            /*
            DAL db = new DAL();
            DataTable dt = new DataTable();
            OrdemServicoModel Dados = new OrdemServicoModel();

            dt = db.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dados = new OrdemServicoModel
                {                    
                    NomeCliente = dt.Rows[i]["NomeCliente"].ToString(),
                    Endereco = dt.Rows[i]["Logradouro"].ToString(),
                    Cidade = dt.Rows[i]["Cidade"].ToString(),
                    Bairro = dt.Rows[i]["Bairro"].ToString(),
                    Fone1 = dt.Rows[i]["Fone1"].ToString(),
                    Fone2 = dt.Rows[i]["Fone2"].ToString(),
                    DescricaoEquipamento = dt.Rows[i]["DescricaoEquipamento"].ToString(),
                    id_entidade = int.Parse(dt.Rows[i]["id_cliente_gcfox"].ToString()),
                    id_os_gcfox = int.Parse(dt.Rows[i]["id_os_gcfox"].ToString()),
                    id_os_equipamentos = int.Parse(dt.Rows[i]["id_os_equipamentos"].ToString()),
                    placa = dt.Rows[i]["placa"].ToString(),
                    dataentrada = dt.Rows[i]["dataentrada"].ToString(),
                    km = dt.Rows[i]["km"].ToString(),
                    modelo = dt.Rows[i]["modelo"].ToString(),
                    ano = dt.Rows[i]["ano"].ToString(),
                    conclusao = dt.Rows[i]["conclusao"].ToString(),
                    Id = int.Parse(dt.Rows[i]["id"].ToString())
                };
            }

            db.FechaConexao();
            return Dados;
            */
            #endregion
        }

        // Inserção Ordem de Serviços
        public Boolean InserirOS(OrdemServicoModel DadosFormulario)
        {
            DadosFormulario.dataentrada = DateTime.Now;  //.ToString("yyyy/MM/dd");           

            if (DadosFormulario.placa != null)
            {
                DadosFormulario.placa = DadosFormulario.placa.ToUpper();
            }
            
            string sql = "Insert Into bm_os(" +
                        "dataentrada," +
                        "id_entidade," +
                        "id_os_equipamentos," +
                        "placa," +
                        "ano," +
                        "km," +
                        "conclusao," +
                        "id_os_gcfox," +
                        "id_cliente_gcfox," +
                        "modelo)" +
                        "Value(" +
                      "@dataentrada," +
                      "@id_entidade," +
                      "@id_os_equipamentos," +
                      "@placa," +
                      "@ano," +
                      "@km," +
                      "@conclusao," +
                      "0," +                  // id o.s fox erp
                      "@id_cliente_gcfox," +
                      "@modelo)";                      

            DALDPR db = new DALDPR();
            var daldapper = db.DALCON();
            int idos = daldapper.ExecuteScalar<int>(sql + "; select LAST_INSERT_ID();", DadosFormulario);
            daldapper.Close();

            return true;
            #region
            /*
            DAL dal = new DAL();
            dal.ExecutarCommandoSQL(sql);

            // Busca ID
            DataTable dt = new DataTable();

            sql = "SELECT bm_os.id From bm_os Where id = LAST_INSERT_ID()";

            dt = dal.RetDataTable(sql);

            if (dt.Rows.Count == 1)  // Encontrou
            {
                string idos = dt.Rows[0]["id"].ToString();
            }

            dal.FechaConexao();
            */
            #endregion


        }

    }
}
