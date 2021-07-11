using Dapper;
using HorusWebErp.Utilitarios;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HorusWebErp.Modelos
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Usuário é obrigatório")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; }

        // Sera nossa Session
        public string NomeUsuarioLogado { get; set; }
        public string IdUsuarioLogado { get; set; }
        public string Token { get; set; }

        public Login ValidarLogin()
        {
            string sql = "SELECT * FROM bm_login " +
                        $" WHERE login='{Usuario}' and senha='{Senha}'  ";

            DALDPR db = new DALDPR();
            var daldapper = db.DALCONPARAM();
            Login log = daldapper.Query<Login>(sql).FirstOrDefault();

            daldapper.Close();
            return log;

            /*
            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);
            if (dt.Rows.Count > 0)  // encontrou
            {
                // Linha Zero Campo ID do Vendedor
                IdUsuarioLogado = dt.Rows[0]["ID"].ToString();
                NomeUsuarioLogado = dt.Rows[0]["NOME"].ToString();
                Token = dt.Rows[0]["TOKEN_WEB"].ToString();
                // criar token de conexão

                objDAL.FechaConexao();
                return true;
            }
            else
            {
                objDAL.FechaConexao();
                return false;
            }            
           */

        }
    }
}
