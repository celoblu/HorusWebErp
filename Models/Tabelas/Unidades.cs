using Dapper;
using HorusWebErp.Modelos;
using HorusWebErp.Utilitarios;
using System.Collections.Generic;
using System.Linq;

namespace HorusWebErp.Models.Tabelas
{
    public class Unidades
    {

        public List<UnidadesMedidasModel> ListagemUnidadesMedida(string NomeCategoria)
        {
            string sql = "";
            string Filtro = "";

            if (!string.IsNullOrEmpty(NomeCategoria))
            {
                Filtro = "%" + NomeCategoria.ToUpper().Trim() + "%";
                Filtro = $" Where descricao LIKE '{Filtro}'";
            }

            sql = "select id , descricao, sigla" +
                  " from bm_produto_unidade" +
                  $"{Filtro}" + " order by descricao";

            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            List<UnidadesMedidasModel> lstUnidade = daldapper.Query<UnidadesMedidasModel>(sql).ToList();
            daldapper.Close();

            return lstUnidade;

        }


        public bool IncluirUnidade(UnidadesMedidasModel dados)
        {

            string sql = $"Insert Into bm_produto_unidade(descricao, sigla) Value('{dados.descricao.ToUpper()}', '{dados.sigla.ToUpper()}') ";

            DALDPR dal = new DALDPR();
            var dalpper = dal.DALCON();
            dalpper.Execute(sql);

            return true;
        }

        public UnidadesMedidasModel ConsultarUnidade(string Id)
        {
            string sql = $"Select * From bm_produto_unidade where id = '{Id}' ";

            DALDPR dal = new DALDPR();
            var dalpper = dal.DALCON();
            UnidadesMedidasModel dados = dalpper.Query<UnidadesMedidasModel>(sql).FirstOrDefault();

            return dados;
        }


        public bool AlterarUnidade(UnidadesMedidasModel dados)
        {
            string sql = $"Update bm_produto_unidade set descricao = '{dados.descricao.ToUpper()}', " +
                         $" sigla = '{dados.sigla.ToUpper()}'" +
                         $" where id = '{dados.id}'";

            DALDPR dal = new DALDPR();
            var dalpper = dal.DALCON();
            dalpper.Execute(sql);

            return true;
        }


        public List<UnidadesMedidasModel> ExcluirUnidade(string id)
        {
            string sql = $"Delete from bm_produto_unidade where id = '{id}'";

            DALDPR dal = new DALDPR();
            var dalpper = dal.DALCON();
            dalpper.Execute(sql);

            return ListagemUnidadesMedida("");
        }
    }
}
