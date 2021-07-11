using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace HorusWebErp.Utilitarios
{
    public class Procedures
    {
        public static string Server { get; set; }
        public static string Porta { get; set; }
        public static string Database { get; set; }
        public static string User { get; set; }
        public static string Password { get; set; }
        //
        public List<Procedures> confiJSON { get; set; }

        public Procedures()

        {
            string arquivo = @".\config\config.json";
            if (File.Exists(arquivo))
            {
                try
                {
                    string UpLoad = File.ReadAllText(arquivo);
                    dynamic tmp = JsonConvert.DeserializeObject(UpLoad);

                    Server = tmp.Server;
                    Porta = tmp.Porta;
                   // Database = tmp.Database;
                    User = tmp.User;
                    Password = tmp.Password;
                }

                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                // (" O arquivo " + arquivo + "não foi localizado !");
            }

            return;

        }


        public string VerificaCombo(string Str)
        {
            if (Str == "0")
            {
                return ("null");
            }
            else
            {
                return (Str);
            }
        }

        public string VerificarNum(string Str)
        {
            if (Str == string.Empty | Str == null)
            {
                return ("null");
            }
            else
            {
                return Str.Replace(",", ".");
            }

        }

        public string Mysqlstr(string Str)
        {
            if (Str != null)
            {
                Str = "'" + Str.Trim() + "'";
            }
            else
            {
                return ("null");
            }
            return (Str);
        }
        /// <summary>
        /// Converte string com Aspas antes de enviar ao Banco usando concatenação de strings
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public string VerificarStr(string Str)
        {
            if (Str != null)
            {
                Str = Str.Trim();
            }
            else
            {
                return ("null");
            }
            return (Str);
        }

        /// <summary>
        ///  Converte Data em Ano-Mes-Dia ex: "9999-99-99"
        ///  ou em Null
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string DataStr(string data)
        {
            if (data != null)
            {
                return (Mysqlstr(Convert.ToDateTime(data).ToString("yyyy/MM/dd")));
            }
            else
            {
                return (data = "null");
            }
        }

        /// <summary>
        ///  deve ser passado como parametros id Usario logado + Codigo Evento
        ///  Tabela So_evento_tipo
        ///  Alimenta Tabela So_evento
        /// </summary>
        /// <param name="UsuarioLogado"></param>
        /// <param name="CodigoEvento"></param>
        /// <returns></returns>
        public string RegistraEvento(string UsuarioLogado, string CodigoEvento)
        {

            // string dtEvento = DateTime.Now.ToString("yyyy/mm/dd");


            DataTable dt = new DataTable();
            DAL obj = new DAL();

            string sql = "insert into So_evento(" +
                         "id_profissional," +
                         "id_evento_tipo)" +
                         "Value(" +
                         $"'{UsuarioLogado}'," +
                         $"'{CodigoEvento}')";

            obj.ExecutarCommandoSQL(sql);

            // retorna ID So_evento
            sql = "SELECT So_evento.id From So_evento Where id = LAST_INSERT_ID()";
            dt = obj.RetDataTable(sql);

            string idRet = dt.Rows[0]["id"].ToString();

            obj.FechaConexao();
            return idRet;


        }

    }
}
