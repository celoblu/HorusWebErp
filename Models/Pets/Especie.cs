using Dapper;
using HorusWebErp.Modelos;
using HorusWebErp.Utilitarios;
using System.Collections.Generic;
using System.Linq;


namespace HorusWebErp.Models.Pets
{
    public class Especie
    {
        public List<EspecieModel> ListagemEspecies(string Filtro)
        {
            string sql;

            if (string.IsNullOrEmpty(Filtro))
            {
                sql = "select * from pt_especies order by nome";
            }
            else
            {
                Filtro = "%" + Filtro + "%";
                sql = $"select * from pt_especies where nome like '{Filtro}' order by nome";
            }

            DALDPR dal = new DALDPR();
            var dalper = dal.DALCON();
            List<EspecieModel> lst = dalper.Query<EspecieModel>(sql).ToList();

            return lst;

        }


        public EspecieModel DetalhesEspecie(string id)
        {
            string sql;
            sql = $"select * from pt_especies where id = '{id}' order by nome";
            DALDPR dal = new DALDPR();
            var dalper = dal.DALCON();
            EspecieModel Dados = dalper.Query<EspecieModel>(sql).FirstOrDefault();

            return Dados;
        }


        public bool ExcluirEspecie(string id)
        {
            string sql;

            sql = $"delete from pt_especies where id = '{id}'";
            DALDPR dal = new DALDPR();
            var dalper = dal.DALCON();
            int ret = dalper.Execute(sql);

            return true;
        }

        public bool AlterarEspecie(EspecieModel dados)
        {

            string sql = $"update pt_especies set nome='{dados.nome.ToUpper()}' where id = '{dados.id}'";
            DALDPR dal = new DALDPR();
            var dalper = dal.DALCON();
            int ret = dalper.Execute(sql);

            return true;
        }


        public bool IncluirEspecie(EspecieModel dados)
        {
            string sql = $"Insert into pt_especies set nome='{dados.nome.ToUpper()}'";
            DALDPR dal = new DALDPR();
            var dalper = dal.DALCON();
            int ret = dalper.Execute(sql);

            return true;
        }



    }
}
