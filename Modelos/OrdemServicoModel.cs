using System;
using System.ComponentModel.DataAnnotations;

namespace HorusWebErp.Modelos
{
    public class OrdemServicoModel
    {
        public int Id { get; set; }
        public int id_os_gcfox { get; set; }
        public int id_os_equipamentos { get; set; }
        public int id_entidade { get; set; }           // id do cliente
        public int id_cliente_gcfox { get; set; }
        public int id_ct_receber { get; set; }
        public int id_pedido { get; set; }
        public int id_finan_condicao { get; set; }
        public int id_profissional { get; set; }       // ID Tecnico
        public int id_registro_fiscal { get; set; }    // ID HTS148
        public int id_os_status { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime dataentrada { get; set; }
        // public string dataprevisao { get; set; }       // CAMPO EXCLUIDO
        public DateTime dataencerramento { get; set; }
        public string dataalteracao { get; set; }
        public string km { get; set; }
        public string placa { get; set; }
        public string patrimonio { get; set; }
        public string motorista { get; set; }
        public string equipamento { get; set; }
        public decimal vlrmateriais { get; set; }
        public decimal vlrmaoobra { get; set; }
        public string vlrterceiro { get; set; }
        public string vlrdesconto { get; set; }
        public string impresso { get; set; }
        public string encerrada { get; set; }
        public string observacao { get; set; }
        public string conclusao { get; set; }
        public string ano { get; set; }
        public string modelo { get; set; }

        // Campo de Apoio Não Pertencem a Tabela de Ordem de Serviço

        // Cliente
        public string NomeCliente { get; set; }
        public string Endereco { get; set; }
        public string logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Fone1 { get; set; }
        public string Fone2 { get; set; }

        // Equipamento/Carro
        public string DescricaoEquipamento { get; set; }

    }

    public class ItensOrdemServicoModel
    {
        public int id { get; set; }
        public int id_os { get; set; }
        public int id_produto { get; set; }
        public string NomeProduto { get; set; }
        public string NomeProdutoX { get; set; }
        public decimal quantidade { get; set; }
        public decimal valor_unitario { get; set; }
        public decimal desconto { get; set; }
    }
}
