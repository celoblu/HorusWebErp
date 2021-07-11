using System;
using System.ComponentModel.DataAnnotations;

namespace HorusWebErp.Modelos
{
    public class ProdutosModel
    {
        public string id { get; set; }
        public int id_produto_categoria { get; set; }
        public int id_produto_subcategoria { get; set; }
        public int id_produto_unidcompra { get; set; }
        public int id_produto_unidvenda { get; set; }
        public int id_produto_unidtributada { get; set; }
        public int id_produto_marca { get; set; }
        public int id_plc_gerencial { get; set; }
        public int id_profissional_cad { get; set; }
        public int id_profissional_alt { get; set; }
        [Required(ErrorMessage = "Nome do Produto é Obrigatório")]
        public string descricao { get; set; }
        public string infadicional { get; set; }
        public string referencia { get; set; }
        public string referenciacompra { get; set; }
        public string gtin { get; set; }
        public int ippt { get; set; }
        public string codigoanp { get; set; }
        public decimal qtEstoque { get; set; }
        public decimal qtEstoquemin { get; set; }
        public decimal qtEntradas { get; set; }
        public decimal qtSaidas { get; set; }
        public decimal vlUltCompra { get; set; }
        public decimal vlCustoMedio { get; set; }
        public decimal ProdutoServico { get; set; }
        public string Localiza1 { get; set; }
        public string Localiza2 { get; set; }
        public string Localiza3 { get; set; }
        public decimal ativo { get; set; }
        public DateTime dtcadastro { get; set; }
        public DateTime dtatualizacao { get; set; }

        // Entradas
        public string ListaEntradas { get; set; }

        // Tabela de Preços
        public decimal VlTabela1 { get; set; }

        // Tributos
        public string ncm { get; set; }
        public string Id_nfs_tabnacserv { get; set; }
        public string Id_grupo_fiscal { get; set; }
        public string id_prmfiscal_tpproduto { get; set; }
        public string codservicoiss { get; set; }
        public string aliqisqn { get; set; }
        public string aliqibpt { get; set; }
        public string Cest { get; set; }
        public string AliqICMS { get; set; }
        public string AliqIPI { get; set; }
        public string AliqPIS { get; set; }
        public string AliqCofins { get; set; }
        public string CSTIPI { get; set; }
        public string CSTPISCOF { get; set; }
        public string Cenq { get; set; }
        public string SitTrib { get; set; }
        public string ModBC { get; set; }
        public string ModBCST { get; set; }
        public string CFOP { get; set; }
        public string Origem { get; set; }
        public string PredBCST { get; set; }
        public string PredBC { get; set; }
        public string vBCSTRet { get; set; }
        public string vICMSSTRet { get; set; }

        // Categoria
        public string DescricaoCategoria { get; set; }

        // Sub Categoria
        public string DescricaoSubCategoria { get; set; }

        // UNIDADES DE MEDIDAS
        public string DescricaoUnidCompra { get; set; }
        public string DescricaoUnidVenda { get; set; }
        public string DescricaoUnidTributada { get; set; }





    }
}
