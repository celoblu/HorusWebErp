using Dapper;
using HorusWebErp.Modelos;
using HorusWebErp.Utilitarios;
using System.Collections.Generic;
using System.Linq;

namespace HorusWebErp.Models.Pets
{
    public class Racas
    {

        public List<RacasModel> ListagemRacas(string Filtro)
        {
            string sql;

            if (string.IsNullOrEmpty(Filtro))
            {
                sql = "select * from pt_racas order by nome";
            }
            else
            {
                Filtro = "%" + Filtro + "%";
                sql = $"select * from pt_racas where nome like '{Filtro}' order by nome";
            }

            DALDPR dal = new DALDPR();
            var dalper = dal.DALCON();
            List<RacasModel> lst = dalper.Query<RacasModel>(sql).ToList();

            return lst;

        }

        public RacasModel DetalhesRaca(string id)
        {
            string sql;
            sql = $"select * from pt_racas where id = '{id}' order by nome";
            DALDPR dal = new DALDPR();
            var dalper = dal.DALCON();
            RacasModel Dados = dalper.Query<RacasModel>(sql).FirstOrDefault();

            return Dados;
        }


        public bool ExcluirRaca(string id)
        {
            string sql;

            sql = $"delete from pt_racas where id = '{id}'";
            DALDPR dal = new DALDPR();
            var dalper = dal.DALCON();
            int ret = dalper.Execute(sql);

            return true;
        }

        public bool AlterarRaca(RacasModel dados)
        {
            string porte = string.IsNullOrEmpty(dados.porte) ? dados.porte : dados.porte.ToUpper();

            string sql = $"update pt_racas set nome='{dados.nome.ToUpper()}', porte='{porte}', foto='{dados.foto}'  where id = '{dados.id}'";
            DALDPR dal = new DALDPR();
            var dalper = dal.DALCON();
            int ret = dalper.Execute(sql);

            return true;
        }


        public bool IncluirRaca(RacasModel dados)
        {

            string porte = string.IsNullOrEmpty(dados.porte) ? dados.porte : dados.porte.ToUpper();

            string sql = $"Insert into pt_racas set nome='{dados.nome.ToUpper()}', porte='{porte}', foto='{dados.foto}' ";
            DALDPR dal = new DALDPR();
            var dalper = dal.DALCON();
            int ret = dalper.Execute(sql);

            return true;
        }

    }

}
