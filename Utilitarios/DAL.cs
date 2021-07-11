using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace HorusWebErp.Utilitarios
{
    public class DALDPR : Procedures
    {
        public MySqlConnection DALCON()
        {
            MySqlConnection ConMsql;
            string strConexao = $"Server=" + Server + ";Port=" + Porta + ";Database=" + Database + ";Uid=" + User + ";password=" + Password;
            ConMsql = new MySqlConnection(strConexao);
            return ConMsql;
        }

        public MySqlConnection DALCONPARAM()
        {
            MySqlConnection ConMsql;
            string strConexao = $"Server=" + Server + ";Port=" + Porta + ";Database=dbparametros" + ";Uid=" + User + ";password=" + Password;
            ConMsql = new MySqlConnection(strConexao);
            return ConMsql;
        }
    }


    public class DAL : Procedures
    {

        //Atributos
        //public MySqlConnection con = new MySqlConnection();
        protected MySqlConnection Con;
        protected MySqlCommand Cmd;
        protected MySqlDataReader Dr;

        private static string ConnectionString = $"Server=" + Server + ";Port=" + Porta + ";Database=" + Database + ";Uid=" + User + ";password=" + Password;

        protected MySqlConnection Connection;

        public DAL()
        {

            try
            {
                //poderia ser assim !!  
                //MySqlConnection con = new MySqlConnection(caminho);             
                //con.ConnectionString = caminho;                            

                //MySqlConnection Connection = new MySqlConnection(ConnectionString);  
                Connection = new MySqlConnection(ConnectionString);
                Connection.Open();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void FechaConexao()
        {
            try
            {
                Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Esta função espera um paramentro do tipo string contendo um comando SQL coma instrução SELECT de busca no Banco de Dados
        public DataTable RetDataTable(string sql)
        {
            try
            {
                DataTable data = new DataTable();  // cursor vazio fox tabela
                MySqlCommand Command = new MySqlCommand(sql, Connection);
                MySqlDataAdapter da = new MySqlDataAdapter(Command);
                da.Fill(data);  // alimentando o cursor de memoria com os dados do SELECT
                return (data);

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Retornar Dados : " + ex.Message);
            }

            /*finally
            {
                FechaConexao();
            }
            */
        }

        // Executa comandos sql do tipo
        // insert , delete, update
        public void ExecutarCommandoSQL(string sql)
        {
            try
            {
                MySqlCommand Command = new MySqlCommand(sql, Connection);
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Executar Coamando SQL : " + ex.Message);
            }

        }

    }
}
