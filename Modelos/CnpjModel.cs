using System.Collections.Generic;

namespace HorusWebErp.Modelos
{
    public class CnpjModel
    {
        public List<Atividade_principal> atividade_principal { get; set; }
        public string data_situacao { get; set; }
        public string complemento { get; set; }
        public string nome { get; set; }
        public string uf { get; set; }
        public string telefone { get; set; }
        public string situacao { get; set; }
        public string bairro { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string cep { get; set; }
        public string municipio { get; set; }
        public string porte { get; set; }
        public string abertura { get; set; }
        public string natureza_juridica { get; set; }
        public string cnpj { get; set; }
        public string fantasia { get; set; }
        public string email { get; set; }
        public string motivo_situacao { get; set; }
    }

    public class Atividade_principal
    {
        public string text { get; set; }
        public string code { get; set; }
    }
}
