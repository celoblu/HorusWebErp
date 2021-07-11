using System;
using System.ComponentModel.DataAnnotations;

namespace HorusWebErp.Modelos
{
    public class ClienteModel
    {
        public int id { get; set; }
        public int id_Erp { get; set; }  // id da base de dados desktop
        public int id_cliente_gcfox { get; set; }  // id da entidade hts003 gcfox
        public int id_profissional_vnd { get; set; }
        public int id_entidade_regiao { get; set; }
        public int id_entidade_grupo { get; set; }
        public int id_entidade_vnd { get; set; }
        public string nome { get; set; }
        public string fantasia { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string cep { get; set; }
        public string uf { get; set; }
        public string ptoreferencia { get; set; }
        public string codibge { get; set; }
        public string CodMunFed { get; set; }
        public string codpais { get; set; }
        public string cpf { get; set; }
        public string rg { get; set; }
        [Required]
        [Display(Name = "CNPJ"),
         DisplayFormat(DataFormatString = "{0:##.###.###/####-##}", ApplyFormatInEditMode = true),
         StringLength(14, MinimumLength = 14, ErrorMessage = "CNPJ não pode ser maior que 14 digitos.")]
        public string cnpj { get; set; }
        public string inscestadual { get; set; }
        public int dstindied { get; set; }
        public string inscmunicipal { get; set; }
        public string CodRNTC { get; set; }
        public string email { get; set; }
        public string nfemail { get; set; }
        public string email2 { get; set; }
        public string ddd1 { get; set; }
        public string fone1 { get; set; }
        public string ddd2 { get; set; }
        public string fone2 { get; set; }
        public string ddd3 { get; set; }
        public string fone3 { get; set; }
        public decimal ativo { get; set; }
        public decimal fisicajuridica { get; set; }
        public decimal chkcliente { get; set; }
        public decimal chkfornecedor { get; set; }
        public decimal chktransportador { get; set; }
        public decimal chkparceiro { get; set; }
        public decimal chkoutros { get; set; }
        public decimal bloqueado { get; set; }
        public decimal partes_vnd { get; set; }
        public int id_profissional_cad { get; set; }
        public int id_profissional_alt { get; set; }
        public DateTime dtcadastro { get; set; }
        public DateTime dtatualizacao { get; set; }
        public DateTime dtanascimento { get; set; }

        // Variaveis de suporte
        public string ClientePesquisar { get; set; }
    }
}
