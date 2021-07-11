using System;

namespace HorusWebErp.Modelos
{
    public class PedidosModel
    {
        public string ID { get; set; }
        public string NomeCliente { get; set; }
        public int id_entidade { get; set; }
        public int id_profissional_vnd { get; set; }
        public int id_entidade_vnd { get; set; }
        public int id_finan_condicao { get; set; }
        public int id_registro_Fiscal { get; set; }
        public int id_ct_receber { get; set; }
        public int Id_Plc_gerencial { get; set; }
        public int id_evento { get; set; }
        public int d_Pedido_Web { get; set; }
        public string Data { get; set; }
        public int tppedido { get; set; }
        public double valor { get; set; }
        public int id_profissional_cad { get; set; }
        public int id_profissional_alt { get; set; }
        public DateTime dtcadastro { get; set; }
        public DateTime dtatualizacao { get; set; }
        public DateTime dtcancelamento { get; set; }
        public int id_pedido_situacao { get; set; }
        public string observacao { get; set; }
    }

    public class ItensPedidos
    {
        public int id { get; set; }
        public int id_pedido { get; set; }
        public int id_produto { get; set; }
        public double vlrunitario { get; set; }
        public double vlrunitoriginal { get; set; }
        public double vlrtotal { get; set; }
        public double quantidade { get; set; }
        public string NomeProduto { get; set; }
        //
        public string IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Logradouro { get; set; }
    }
}
