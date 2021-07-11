using Dapper;
using HorusWebErp.Modelos;
using HorusWebErp.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HorusWebErp.Models.Tabelas
{
    public class Profissionais
    {
        public List<ProfissionaisModel> ListagemProfissionais(string Filtro)
        {

            string sql;

            if (string.IsNullOrEmpty(Filtro))
            {
                sql = "select * from bm_profissional order by nome";
            }
            else
            {
                Filtro = "%" + Filtro + "%";
                sql = $"select * from bm_profissional where nome like '{Filtro}' order by nome";
            }

            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            List<ProfissionaisModel> lst = daldapper.Query<ProfissionaisModel>(sql).ToList();

            return lst;
        }

        public ProfissionaisModel DetalhesProfissionais(string Filtro)
        {
            string sql = $"select * from bm_profissional where id = '{Filtro}'";

            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            ProfissionaisModel Dados = daldapper.Query<ProfissionaisModel>(sql).FirstOrDefault();

            return Dados;

        }


        public bool ExcluirProfissional(ProfissionaisModel dados)
        {
            try
            {
                string sql = $"delete from bm_profissional where id = '{dados.Id}'";

                DALDPR dal = new DALDPR();
                var daldapper = dal.DALCON();
                int ret = daldapper.Execute(sql);

                return true;

            }
            catch (Exception e)
            {
                string erro = e.Message;
                return false;

            }
        }

        public bool GravaDetalhes(ProfissionaisModel dados)
        {
            try
            {
                string sql = "update bm_profissional set " +
                              $"nome = '{dados.nome.ToUpper()}', " +
                              $"apelido= '{dados.apelido.ToUpper()}', " +
                              $"dscfuncao = '{dados.dscfuncao.ToUpper()}'" +
                              $" where id= '{dados.Id}'";

                DALDPR dal = new DALDPR();
                var daldapper = dal.DALCON();
                int ret = daldapper.Execute(sql);

                return true;
            }
            catch (Exception e)
            {
                string erro = e.Message;
                return false;
            }

        }

        public bool InserirProfissional(ProfissionaisModel dados)
        {
            try
            {
                string sql = "insert into  bm_profissional(" +
                              "nome," +
                              "apelido," +
                              "dscfuncao) value(" +
                              $"'{dados.nome.ToUpper()}'," +
                              $"'{dados.apelido.ToUpper()}'," +
                              $"'{dados.dscfuncao.ToUpper()}')";

                DALDPR dal = new DALDPR();
                var daldapper = dal.DALCON();
                int ret = daldapper.Execute(sql);

                return true;
            }
            catch (Exception e)
            {
                string erro = e.Message;
                return false;
            }

        }


    }
}
