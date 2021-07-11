using Dapper;
using HorusWebErp.Modelos;
using HorusWebErp.Utilitarios;
using System.Collections.Generic;
using System.Linq;

namespace HorusWebErp.Models.Tabelas
{
    public class Veiculos
    {

        public List<VeiculosModel> ListagemVeiculos(string Filtro)
        {
            string sql;
            if (Filtro != null)
            {
                Filtro = "%" + Filtro + "%";
                sql = $"select * from bm_os_equipamentos where descricao like '{Filtro}' order by descricao";
            }
            else
            {
                sql = "select * from bm_os_equipamentos order by descricao";
            }


            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            List<VeiculosModel> lst = daldapper.Query<VeiculosModel>(sql).ToList();
            daldapper.Close();


            #region
            /*
            DAL dal = new DAL();
            DataTable dt = new DataTable();
            dt = dal.RetDataTable(sql);

            VeiculosModel Dados;
            List<VeiculosModel> lst = new List<VeiculosModel>();
           
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                Dados = new VeiculosModel
                {
                    Id = int.Parse(dt.Rows[i]["id"].ToString()),
                    id_equipamento_gcfox = int.Parse(dt.Rows[i]["id_equipamento_gcfox"].ToString()),
                    Descricao = dt.Rows[i]["descricao"].ToString()
                };

                lst.Add(Dados);

            }
                       
            dal.FechaConexao();
            */
            #endregion

            return lst;
        }
    }
}
