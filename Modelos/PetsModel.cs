using System;

namespace HorusWebErp.Modelos
{
    public class PetsModel
    {
        // Variaveis Banco de Dados
        public int id { get; set; }
        public int id_racas { get; set; }
        public int id_especies { get; set; }
        public int id_entidades { get; set; }
        public string nomepet { get; set; }
        public string sexo { get; set; }
        public DateTime dtnascimento { get; set; }
        public int idade { get; set; }
        public string pelagem { get; set; }
        public string cor { get; set; }
        public string porte { get; set; }
        public string numerochip { get; set; }
        public string pedigree { get; set; }
        public string castrado { get; set; }
        public string cuidados { get; set; }
        public string obs { get; set; }
        public string status { get; set; }
        public decimal peso { get; set; }

        // Variaveis de Suporte as telas
        public string NomeEspecie { get; set; }
        public string NomeRaca { get; set; }
        public string NomeEntidade { get; set; }


    }
}
