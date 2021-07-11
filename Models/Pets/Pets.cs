using Dapper;
using HorusWebErp.Modelos;
using HorusWebErp.Utilitarios;
using System.Collections.Generic;
using System.Linq;

namespace HorusWebErp.Models.Pets
{
    public class Pets
    {
        public PetsModel ConsultaPet(string id)
        {

            string sql = "select pt_pets.id, id_racas, id_especies, id_entidades,  nomepet, sexo, dtnascimento," +
                          " idade, pelagem, cor, pt_pets.porte, numerochip, pedigree," +
                          " castrado, cuidados, obs, peso, status," +
                          " pt_especies.nome as NomeEspecie," +
                          " pt_racas.nome as NomeRaca," +
                          " bm_entidade.nome as NomeEntidade" +
                          " from pt_pets " +
                          " left outer join pt_especies" +
                          " on pt_pets.id_especies = pt_especies.id" +
                          " left outer join pt_racas" +
                          " on pt_pets.id_racas = pt_racas.id" +
                          " left outer join bm_entidade" +
                          " on pt_pets.id_entidades = bm_entidade.id" +
                         $" where pt_pets.id = '{id}'";


            DALDPR dal = new DALDPR();
            var dalper = dal.DALCON();
            PetsModel dados = dalper.Query<PetsModel>(sql).FirstOrDefault();

            return dados;
        }


        public bool AlterarPet(PetsModel dados)
        {
            string Porte = string.IsNullOrEmpty(dados.porte) ? dados.porte : dados.porte.ToUpper();

            string sql = $"update pt_pets set nomepet='{dados.nomepet.ToUpper()}'," +
                         $" id_racas='{dados.id_racas}'," +
                         $" id_especies='{dados.id_especies}'," +
                         $" id_entidades='{dados.id_entidades}'," +
                         $" sexo='{dados.sexo}'," +
                         $" idade='{dados.idade}'," +
                         $" pelagem='{dados.pelagem}'," +
                         $" cor='{dados.cor}'," +
                         $" porte='{Porte}'," +
                         $" numerochip='{dados.numerochip}'," +
                         $" pedigree='{dados.pedigree}'," +
                         $" castrado='{dados.castrado}'," +
                         $" cuidados='{dados.cuidados}'," +
                         $" obs='{dados.obs}'," +
                         $" peso='{dados.peso}'," +
                         $" dtnascimento ='{dados.dtnascimento.ToString("yyyy/MM/dd")}'," +
                         $" status='{dados.status}' where id = '{dados.id}'";

            DALDPR dal = new DALDPR();
            var dalper = dal.DALCON();
            int ret = dalper.Execute(sql);

            return true;
        }



        public List<PetsModel> ListagemPets(string Filtro)
        {
            string sql;

            if (string.IsNullOrEmpty(Filtro))
            {
                sql = "select pt_pets.id, pt_pets.nomepet, pt_pets.id_especies," +
                       "pt_pets.id_racas, pt_pets.id_entidades," +
                       "pt_especies.nome as NomeEspecie," +
                       "pt_racas.nome as NomeRaca," +
                       "bm_entidade.nome as NomeEntidade" +
                       " from pt_pets " +
                       " left outer join pt_especies" +
                       " on pt_pets.id_especies = pt_especies.id" +
                       " left outer join pt_racas" +
                       " on pt_pets.id_racas = pt_racas.id" +
                       " left outer join bm_entidade" +
                       " on pt_pets.id_entidades = bm_entidade.id" +
                       " order by nomepet limit 30";
            }
            else
            {
                Filtro = "%" + Filtro + "%";
                sql = "select pt_pets.id, pt_pets.nomepet, pt_pets.id_especies," +
                     "pt_pets.id_racas, pt_pets.id_entidades," +
                     "pt_especies.nome as NomeEspecie," +
                     "pt_racas.nome as NomeRaca," +
                     "bm_entidade.nome as NomeEntidade" +
                     " from pt_pets " +
                     " left outer join pt_especies" +
                     " on pt_pets.id_especies = pt_especies.id" +
                     " left outer join pt_racas" +
                     " on pt_pets.id_racas = pt_racas.id" +
                     " left outer join bm_entidade" +
                     " on pt_pets.id_entidades = bm_entidade.id" +
                    $" where nomepet like '{Filtro}' order by nomepet";
            }

            DALDPR dal = new DALDPR();
            var dalper = dal.DALCON();
            List<PetsModel> lst = dalper.Query<PetsModel>(sql).ToList();

            return lst;

        }

        public bool InserirPet(PetsModel dados)
        {
            char Ativo = dados.status == "true" ? '1' : '0';

            string sql = "insert into pt_pets(" +
                          " id_racas," +
                          " id_especies," +
                          " id_entidades," +
                          " nomepet," +
                          " sexo," +
                          " dtnascimento," +
                          " idade," +
                          " pelagem," +
                          " cor," +
                          " porte," +
                          " numerochip," +
                          " pedigree," +
                          " castrado," +
                          " cuidados," +
                          " obs," +
                          " peso," +
                          " status)" +
                          $" Value( '{dados.id_racas}'," +
                          $"'{dados.id_especies}'," +
                          $"'{dados.id_entidades}'," +
                          $"'{dados.nomepet.ToUpper()}'," +
                          $"'{dados.sexo}'," +
                          $"'{dados.dtnascimento.ToString("yyyy/MM/dd")}'," +
                          $"'{dados.idade}'," +
                          $"'{dados.pelagem}'," +
                          $"'{dados.cor}'," +
                          $"'{dados.porte}'," +
                          $"'{dados.numerochip}'," +
                          $"'{dados.pedigree}'," +
                          $"'{dados.castrado}'," +
                          $"'{dados.cuidados}'," +
                          $"'{dados.obs}'," +
                          $"'{dados.peso}'," +
                          $"'{Ativo}' )";


            DALDPR db = new DALDPR();
            var daldapper = db.DALCON();
            int idPet = daldapper.ExecuteScalar<int>(sql + "; select LAST_INSERT_ID();", dados);
            daldapper.Close();

            return true;

        }
    }
}
