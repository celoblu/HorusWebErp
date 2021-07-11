using Dapper;
using HorusWebErp.Modelos;
using HorusWebErp.Uteis;
using HorusWebErp.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HorusWebErp.Models.Tabelas
{
    public class Clientes
    {
        public Boolean IncluirEntidade(ClienteModel dados)
        {

            Limpa lmp = new Limpa();

            dados.cep = lmp.RestaNumero(dados.cep);
            dados.cpf = lmp.RestaNumero(dados.cpf);
            dados.cnpj = lmp.RestaNumero(dados.cnpj);
            dados.fone1 = lmp.RestaNumero(dados.fone1);
            dados.fone2 = lmp.RestaNumero(dados.fone2);

            // TODO -  ESTE CAMPO NÃO PODE SER INFORMADO ASSIM O USUARIO DEVE SELECIONAR
            dados.chkcliente = 1;

            string dta = DateTime.Now.ToString("yyyy/MM/dd");
            string dtaNasc = dados.dtanascimento.ToString("yyyy/MM/dd");

            string sql = "insert into bm_entidade(" +
                        "id_profissional_vnd," +
                        "id_entidade_regiao," +
                        "id_entidade_grupo," +
                        "id_entidade_vnd," +
                        "nome," +
                        "fantasia," +
                        "logradouro," +
                        "numero," +
                        "complemento," +
                        "bairro," +
                        "cidade," +
                        "cep," +
                        "uf," +
                        "ptoreferencia," +
                        "codibge," +
                        "CodMunFed," +
                        "codpais," +
                        "cpf," +
                        "rg," +
                        "cnpj," +
                        "inscestadual," +
                        "dstindied," +
                        "inscmunicipal," +
                        "CodRNTC," +
                        "email," +
                        "nfemail," +
                        "email2," +
                        "ddd1," +
                        "fone1," +
                        "ddd2," +
                        "fone2," +
                        "ddd3," +
                        "fone3," +
                        "ativo," +
                        "fisicajuridica," +
                        "chkcliente," +
                        "chkfornecedor," +
                        "chktransportador," +
                        "chkparceiro," +
                        "chkoutros," +
                        "bloqueado," +
                        "partes_vnd," +
                        "id_profissional_cad," +
                        "id_profissional_alt," +
                        "dtanascimento," +
                        "dtcadastro," +
                        "dtatualizacao)" +
                       "Value(" +
                       $"'{dados.id_profissional_vnd}'," +
                       $"'{dados.id_entidade_regiao}'," +
                       $"'{dados.id_entidade_grupo}'," +
                       $"'{dados.id_entidade_vnd}'," +
                       $"'{dados.nome.ToUpper()}'," +
                       $"'{dados.fantasia}'," +
                       $"'{dados.logradouro}'," +
                       $"'{dados.numero}'," +
                       $"'{dados.complemento}'," +
                       $"'{dados.bairro}'," +
                       $"'{dados.cidade}'," +
                       $"'{dados.cep}'," +
                       $"'{dados.uf}'," +
                       $"'{dados.ptoreferencia}'," +
                       $"'{dados.codibge}'," +
                       $"'{dados.CodMunFed}'," +
                       $"'{dados.codpais}'," +
                       $"'{dados.cpf}'," +
                       $"'{dados.rg}'," +
                       $"'{dados.cnpj}'," +
                       $"'{dados.inscestadual}'," +
                       $"'{dados.dstindied}'," +
                       $"'{dados.inscmunicipal}'," +
                       $"'{dados.CodRNTC}'," +
                       $"'{dados.email}'," +
                       $"'{dados.nfemail}'," +
                       $"'{dados.email2}'," +
                       $"'{dados.ddd1}'," +
                       $"'{dados.fone1}'," +
                       $"'{dados.ddd2}'," +
                       $"'{dados.fone2}'," +
                       $"'{dados.ddd3}'," +
                       $"'{dados.fone3}'," +
                       $"'{dados.ativo}'," +
                       $"'{dados.fisicajuridica}'," +
                       $"'{dados.chkcliente}'," +
                       $"'{dados.chkfornecedor}'," +
                       $"'{dados.chktransportador}'," +
                       $"'{dados.chkparceiro}'," +
                       $"'{dados.chkoutros}'," +
                       $"'{dados.bloqueado}'," +
                       $"'{dados.partes_vnd}'," +
                       $"'{dados.id_profissional_cad}'," +
                       $"'{dados.id_profissional_alt}'," +
                       $"'{dtaNasc}'," +
                       $"'{dta}'," +
                       $"'{dta}')";

            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            daldapper.Execute(sql, dados);
            daldapper.Close();

            /*
            DataTable DT = new DataTable();
            DAL dal = new DAL();
            dal.ExecutarCommandoSQL(sql);
            */

            return true;
        }

        public List<ClienteModel> ListagemClientes(string Filtro)
        {
            string sql;
            if (!string.IsNullOrEmpty(Filtro))
            {
                Filtro = "%" + Filtro + "%";
                sql = $"select id, nome, cidade, logradouro, numero, bairro, fone1 from bm_entidade where nome like '{Filtro}' order by nome";
            }
            else
            {
                sql = $"select id, nome, cidade, logradouro, numero, bairro, fone1 from bm_entidade  order by nome limit 10";
            }

            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            List<ClienteModel> lst = daldapper.Query<ClienteModel>(sql).ToList();
            daldapper.Close();

            return lst;

            #region

            /*
            DAL dal = new DAL();
            DataTable dt = new DataTable();
            dt = dal.RetDataTable(sql);

            ClienteModel Dados;
            List<ClienteModel> lst = new List<ClienteModel>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dados = new ClienteModel
                {
                    id  = int.Parse(dt.Rows[i]["id"].ToString()),                    
                    nome = dt.Rows[i]["nome"].ToString(),
                    cidade = dt.Rows[i]["cidade"].ToString(),
                    logradouro = dt.Rows[i]["logradouro"].ToString(),
                    numero = dt.Rows[i]["numero"].ToString(),
                    fone1 = dt.Rows[i]["fone1"].ToString(),
                    bairro = dt.Rows[i]["bairro"].ToString()
                };

                lst.Add(Dados);
            }

            dal.FechaConexao();

            return lst;
            */
            #endregion

        }

        public ClienteModel PesquisaCliente(string Codigo)
        {
            string sql = "";
            int id = int.Parse(Codigo);

            sql = "Select *" +
                  " from bm_entidade" +
                 $" where bm_entidade.id = '{id}'";

            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            ClienteModel dadoscli = daldapper.Query<ClienteModel>(sql).FirstOrDefault(); // retorna ID gerado                        
            daldapper.Close();
            return dadoscli;
        }
        public bool ExcluirCliente(ClienteModel Dados)
        {     
            int _id = Dados.id;

            string sql = $"delete from bm_entidade where bm_entidade.id = '{_id}'";           

            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            daldapper.Execute(sql, Dados);
            daldapper.Close();

            /*
            DAL db = new DAL();
            db.ExecutarCommandoSQL(sql);
            db.FechaConexao();
            */

            return true;
        }


        public void AlterarCliente(ClienteModel Dados)
        {
            Limpa lmp = new Limpa();
            Dados.cep = lmp.RestaNumero(Dados.cep);
            Dados.cpf = lmp.RestaNumero(Dados.cpf);
            Dados.cnpj = lmp.RestaNumero(Dados.cnpj);
            Dados.fone1 = lmp.RestaNumero(Dados.fone1);
            Dados.fone2 = lmp.RestaNumero(Dados.fone2);
            string dta = DateTime.Now.ToString("yyyy/MM/dd");
            string dtaNasc = Dados.dtanascimento.ToString("yyyy/MM/dd");
            int _id = Dados.id;

            string sql = "UPDATE bm_entidade Set" +
                  $" nome='{Dados.nome}'," +
                  $" logradouro='{Dados.logradouro}'," +
                  $" numero='{Dados.numero}'," +
                  $" complemento='{Dados.complemento}'," +
                  $" bairro='{Dados.bairro}'," +
                  $" cidade='{Dados.cidade}'," +
                  $" cep='{Dados.cep}'," +
                  $" uf='{Dados.uf}'," +
                  $" cpf='{Dados.cpf}'," +
                  $" rg='{Dados.rg}'," +
                  $" cnpj='{Dados.cnpj}'," +
                  $" email='{Dados.email}'," +
                  $" fone1='{Dados.fone1}'," +
                  $" fone2='{Dados.fone2}'," +
                  $" dtatualizacao='{dta}'," +
                  $" dtanascimento='{dtaNasc}'" +
                 $" where bm_entidade.id = '{_id}'";

            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            daldapper.Execute(sql, Dados);
            daldapper.Close();

            /*
            DAL db = new DAL();
            db.ExecutarCommandoSQL(sql);
            db.FechaConexao();
            */

            return;
        }

    }
}
