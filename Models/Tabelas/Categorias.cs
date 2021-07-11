using Dapper;
using HorusWebErp.Modelos;
using HorusWebErp.Utilitarios;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HorusWebErp.Models.Tabelas
{
    public class Categorias
    {

        public List<CategoriasModel> ListagemCategorias(string NomeCategoria)
        {
            string sql = "";
            string Filtro = "";

            CategoriasModel Dados;
            List<CategoriasModel> lst = new List<CategoriasModel>();

            if (!string.IsNullOrEmpty(NomeCategoria))
            {
                Filtro = "%" + NomeCategoria.ToUpper().Trim() + "%";
                Filtro = $" Where descricao LIKE '{Filtro}'";
            }

            sql = "select bm_produto_categoria.*" +
                  " from bm_produto_categoria" +
                  $"{Filtro}" + " order by descricao";

            DAL ObjDal = new DAL();
            DataTable dt = ObjDal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dados = new CategoriasModel
                {
                    id = dt.Rows[i]["id"].ToString().ToString(),
                    descricao = dt.Rows[i]["Descricao"].ToString()
                };

                lst.Add(Dados); // Adiciona registro a Lista
            }

            ObjDal.FechaConexao();

            return lst;
        }

        public List<SubCategoriaModel> ListagemSubCategorias(string NomeCategoria)
        {
            string sql = "";
            string Filtro = "";

            if (!string.IsNullOrEmpty(NomeCategoria))
            {
                Filtro = "%" + NomeCategoria.ToUpper().Trim() + "%";
                Filtro = $" Where descricao LIKE '{Filtro}'";
            }

            sql = "select bm_produto_subcategoria.*" +
                  " from bm_produto_subcategoria" +
                  $"{Filtro}" + " order by descricao";

            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            List<SubCategoriaModel> lista = daldapper.Query<SubCategoriaModel>(sql).ToList();
            daldapper.Close();

            return lista;

            #region
            /*
            DAL ObjDal = new DAL();
            DataTable dt = ObjDal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dados = new CategoriasModel
                {
                    id = dt.Rows[i]["id"].ToString().ToString(),
                    descricao = dt.Rows[i]["Descricao"].ToString()
                };

                lst.Add(Dados); // Adiciona registro a Lista
            }

            ObjDal.FechaConexao();
            */
            #endregion

        }
    }
}
