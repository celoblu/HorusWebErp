using Dapper;
using HorusWebErp.Modelos;
using HorusWebErp.Utilitarios;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HorusWebErp.Models.Tabelas
{
    public class Grupos
    {
        public List<GruposModel> ListagemGrupos(string NomeGrupos)
        {
            string sql = "";
            string Filtro = "";                       

            if (!string.IsNullOrEmpty(NomeGrupos))
            {
                Filtro = "%" + NomeGrupos.ToUpper().Trim() + "%";
                Filtro = $" Where descricao LIKE '{Filtro}'";
            }

            sql = "select bm_grupo_fiscal.*" +
                  " from bm_grupo_fiscal" +
                  $"{Filtro}" + " order by descricao";

            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            List<GruposModel> lst = daldapper.Query<GruposModel>(sql).ToList();
            daldapper.Close();

            return lst;
        }
        
    }
}
