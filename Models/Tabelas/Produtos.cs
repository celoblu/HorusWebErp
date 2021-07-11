using Dapper;
using HorusWebErp.Modelos;
using HorusWebErp.Utilitarios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HorusWebErp.Models.Tabelas
{
    public class Produtos
    {
        public List<ProdutosModel> ListagemProdutos(string NomeProduto, string TipoFiltro)
        {
            string TipoAtivo = "1";  // 1= Ativo, 2= Produtos Eliminados

            string sql = "";
            int Tpfiltro = 1;

            string Filtro = "";

            if (int.Parse(TipoFiltro) == 0)
            {
                Tpfiltro = 0;
            }

            Filtro = "%" + NomeProduto.ToUpper().Trim() + "%";

            sql = "select bm_produto.id, bm_produto.descricao, bm_produto.referencia, ifnull(bm_produto.qtEstoque,'0') as qtEstoque, ifnull(bm_produto_valor.vltabela1,'0') as VlTabela1" +
                  " from bm_produto" +
                  " left outer join bm_produto_valor" +
                  " on bm_produto.id = bm_produto_valor.id_produto";

            if (Tpfiltro == 0)
            {
                if (!string.IsNullOrEmpty(NomeProduto))
                {
                    // Filtro = $" Where descricao LIKE '{Filtro}'";
                    sql += $" Where bm_produto.Descricao LIKE '{Filtro}'";
                    sql += $" And Ativo = '{TipoAtivo}'";
                    sql += " order by bm_produto.Descricao asc";
                }
                else
                {
                    sql += $" Where Ativo = '{TipoAtivo}'";
                    sql += " order by bm_produto.Descricao asc limit 12";
                }
            }
            else
            {
                sql += $" Where bm_produto.Referencia LIKE '{Filtro}'";
                sql += $" And Ativo = '{TipoAtivo}'";
                sql += " order by bm_produto.Referencia asc";
            }

            // $"{Filtro}" + " order by descricao";           

            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            List<ProdutosModel> lst = daldapper.Query<ProdutosModel>(sql).ToList();
            daldapper.Close();

            #region
            /*
            DAL ObjDal = new DAL();
            DataTable dt = ObjDal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ///
                //DateTime dta = DateTime.Parse(dt.Rows[i]["DtCadastro"].ToString());
                ///

                Dados = new ProdutosModel
                {
                    id = dt.Rows[i]["id"].ToString().ToString(),
                    descricao = dt.Rows[i]["Descricao"].ToString(),
                    referencia = dt.Rows[i]["referencia"].ToString(),
                    qtEstoque = decimal.Parse(dt.Rows[i]["estq"].ToString()),
                    VlTabela1 = double.Parse(dt.Rows[i]["Valor"].ToString())
                };

                lst.Add(Dados); // Adiciona registro a Lista
            }

            ObjDal.FechaConexao();
            */
            #endregion

            return lst;
        }

        public ProdutosModel ConsultaProduto(string idProd)
        {
            string sql = "select bm_produto.*, ifnull(bm_produto_valor.vltabela1,'0') as vltabela1," +
                  " bm_produto_fiscal.ncm, bm_produto_fiscal.Cest, bm_produto_fiscal.CFOP," +
                  " bm_produto_fiscal.AliqICMS, bm_produto_fiscal.AliqIPI, bm_produto_fiscal.AliqPIS," +
                  " bm_produto_fiscal.AliqCofins, bm_produto_fiscal.CSTIPI, bm_produto_fiscal.CSTPISCOF," +
                  " bm_produto_fiscal.Cenq, bm_produto_fiscal.SitTrib, bm_produto_fiscal.ModBC," +
                  " bm_produto_fiscal.ModBCST, bm_produto_fiscal.Origem, bm_produto_fiscal.PredBCST," +
                  " bm_produto_fiscal.codservicoiss, bm_produto_fiscal.aliqisqn, bm_produto_fiscal.aliqibpt," +
                  " bm_produto_fiscal.Id_nfs_tabnacserv, bm_produto_fiscal.Id_grupo_fiscal, bm_produto_fiscal.id_prmfiscal_tpproduto," +
                  " bm_produto_fiscal.PredBC, bm_produto_fiscal.vBCSTRet, bm_produto_fiscal.vICMSSTRet," +
                  " ifnull(bm_produto_categoria.descricao,' ') as DescricaoCategoria," +
                  " ifnull(bm_produto_subcategoria.Descricao,' ') as DescricaoSubCategoria," +
                  " ifnull(Compra.Descricao,' ') as DescricaoUnidCompra," +
                  " ifnull(Venda.Descricao,' ') as DescricaoUnidVenda," +
                  " ifnull(Tributado.Descricao,' ') as DescricaoUnidTributada" +
                  " from bm_produto" +
                  " left outer join bm_produto_valor" +
                  " on bm_produto.id = bm_produto_valor.id_produto" +
                  " left outer join bm_produto_fiscal" +
                  " on bm_produto.id = bm_produto_fiscal.id_produto" +
                  " left outer join bm_produto_categoria" +
                  " on bm_produto.id_produto_categoria = bm_produto_categoria.id" +
                  " left outer join bm_produto_subcategoria" +
                  " on bm_produto.id_produto_subcategoria = bm_produto_subcategoria.id" +
                  " left outer join bm_produto_unidade Compra" +
                  " on bm_produto.id_produto_unidcompra = Compra.id" +
                  " left outer join bm_produto_unidade Venda" +
                  " on bm_produto.id_produto_unidvenda = Venda.id" +
                  " left outer join bm_produto_unidade Tributado" +
                  " on bm_produto.id_produto_unidtributada = Tributado.id" +
                  $" Where bm_produto.id = '{idProd}'";

            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            ProdutosModel Dados = daldapper.Query<ProdutosModel>(sql).FirstOrDefault(); // retorna ID gerado                        
            daldapper.Close();

            return Dados;
            #region
            /*
            DAL ObjDal = new DAL();
            DataTable dt = ObjDal.RetDataTable(sql);
            ProdutosModel Dados= new ProdutosModel();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var EstoqueTotal = dt.Rows[i]["qtEstoque"].ToString();

                if (string.IsNullOrEmpty(EstoqueTotal))
                {
                    EstoqueTotal = "0";
                }

                Dados = new ProdutosModel
                {
                    id = dt.Rows[i]["id"].ToString().ToString(),
                    descricao = dt.Rows[i]["Descricao"].ToString(),
                    referencia = dt.Rows[i]["referencia"].ToString(),
                    id_produto_categoria = dt.Rows[i]["id_produto_categoria"].ToString(),
                    id_produto_subcategoria = dt.Rows[i]["id_produto_subcategoria"].ToString(),
                    id_produto_unidcompra = dt.Rows[i]["id_produto_unidcompra"].ToString(),
                    id_produto_unidvenda = dt.Rows[i]["id_produto_unidvenda"].ToString(),
                    id_produto_unidtributada = dt.Rows[i]["id_produto_unidtributada"].ToString(),
                    id_produto_marca = dt.Rows[i]["id_produto_marca"].ToString(),
                    id_plc_gerencial = dt.Rows[i]["id_plc_gerencial"].ToString(),
                    infadicional = dt.Rows[i]["infadicional"].ToString(),
                    referenciacompra = dt.Rows[i]["referenciacompra"].ToString(),
                    gtin = dt.Rows[i]["gtin"].ToString(),
                    ippt = dt.Rows[i]["ippt"].ToString(),
                    codigoanp = dt.Rows[i]["codigoanp"].ToString(),
                    qtEstoquemin = dt.Rows[i]["qtEstoquemin"].ToString(),
                    qtEntradas = dt.Rows[i]["qtEntradas"].ToString(),
                    qtSaidas = dt.Rows[i]["qtSaidas"].ToString(),
                    vlUltCompra = dt.Rows[i]["vlUltCompra"].ToString(),
                    vlCustoMedio = dt.Rows[i]["vlCustoMedio"].ToString(),
                    ProdutoServico = dt.Rows[i]["ProdutoServico"].ToString(),
                    Localiza1 = dt.Rows[i]["Localiza1"].ToString(),
                    Localiza2 = dt.Rows[i]["Localiza2"].ToString(),
                    Localiza3 = dt.Rows[i]["Localiza3"].ToString(),
                    ativo = dt.Rows[i]["ativo"].ToString(),
                    dtcadastro = dt.Rows[i]["dtcadastro"].ToString(),
                    dtatualizacao = dt.Rows[i]["dtatualizacao"].ToString(),
                    qtEstoque = decimal.Parse(EstoqueTotal),
                    VlTabela1 = double.Parse(dt.Rows[i]["Valor"].ToString()),
                    //
                    DescricaoCategoria = dt.Rows[i]["DescricaoCategoria"].ToString(),
                    DescricaoSubCategoria = dt.Rows[i]["DescricaoSubCategoria"].ToString(),                    
                    DescricaoUnidCompra = dt.Rows[i]["DescricaoUnidCompra"].ToString(),
                    DescricaoUnidVenda = dt.Rows[i]["DescricaoUnidVenda"].ToString(),
                    DescricaoUnidTributada = dt.Rows[i]["DescricaoUnidTributada"].ToString(),                    //
                    //
                    ncm = dt.Rows[i]["ncm"].ToString(),
                    Id_nfs_tabnacserv = dt.Rows[i]["Id_nfs_tabnacserv"].ToString(),
                    Id_grupo_fiscal = dt.Rows[i]["Id_grupo_fiscal"].ToString(),
                    id_prmfiscal_tpproduto = dt.Rows[i]["id_prmfiscal_tpproduto"].ToString(),
                    codservicoiss = dt.Rows[i]["codservicoiss"].ToString(),
                    aliqisqn = dt.Rows[i]["aliqisqn"].ToString(),
                    aliqibpt = dt.Rows[i]["aliqibpt"].ToString(),
                    Cest = dt.Rows[i]["Cest"].ToString(),
                    AliqICMS = dt.Rows[i]["AliqICMS"].ToString(),
                    AliqIPI = dt.Rows[i]["AliqIPI"].ToString(),
                    AliqPIS = dt.Rows[i]["AliqPIS"].ToString(),
                    AliqCofins = dt.Rows[i]["AliqCofins"].ToString(),
                    CSTIPI = dt.Rows[i]["CSTIPI"].ToString(),
                    CSTPISCOF = dt.Rows[i]["CSTPISCOF"].ToString(),
                    Cenq = dt.Rows[i]["Cenq"].ToString(),
                    SitTrib = dt.Rows[i]["SitTrib"].ToString(),
                    ModBC = dt.Rows[i]["ModBC"].ToString(),
                    ModBCST = dt.Rows[i]["ModBCST"].ToString(),
                    CFOP = dt.Rows[i]["CFOP"].ToString(),
                    Origem = dt.Rows[i]["Origem"].ToString(),
                    PredBCST = dt.Rows[i]["PredBCST"].ToString(),
                    PredBC = dt.Rows[i]["PredBC"].ToString(),
                    vBCSTRet = dt.Rows[i]["vBCSTRet"].ToString(),
                    vICMSSTRet = dt.Rows[i]["vICMSSTRet"].ToString()
                };
            }

            ObjDal.FechaConexao();
            */
            #endregion
        }

        public bool ExcluirProduto(string idProd)
        {
            bool ret = true;

            string sql =$"select id from bm_os_itens where id_produto = '{idProd}' ";

            DALDPR db = new DALDPR();
            var dalpper = db.DALCON();          
            List<ProdutosModel> lst = dalpper.Query<ProdutosModel>(sql).ToList();

            if(lst.Count == 0)  // Nenhuma OS Possui este Produto Lançado
            {
                sql = $"delete from bm_produto Where bm_produto.id = '{idProd}'";
                dalpper.Execute(sql, new ProdutosModel());
            }
            else
            {
                ret = false;
            }

            dalpper.Close();
            return ret;          
        }

        public bool AlterarProduto(ProdutosModel Produtos)
        {
            // TODO - Alteraçao Produtos

            string Adicional = Produtos.infadicional != null ? Produtos.infadicional.ToUpper() : "";

            string sql = "update bm_produto set " +
                        $"descricao = '{Produtos.descricao.ToUpper()}'," +
                        $"infadicional = '{Adicional}'," +
                        $"gtin = '{Produtos.gtin}'," +
                        $"referencia = '{Produtos.referencia}'," +
                        $"referenciacompra = '{Produtos.referenciacompra}'," +
                        $"id_produto_categoria = '{Produtos.id_produto_categoria}'," +
                        $"id_produto_subcategoria = '{Produtos.id_produto_subcategoria}'," +
                        $"id_produto_unidvenda = '{Produtos.id_produto_unidvenda}'" +
                        $" Where id = '{Produtos.id}'";

            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            daldapper.Execute(sql, new ProdutosModel());

            // Atualiza Valor de Venda
            var VlrVenda = Produtos.VlTabela1.ToString().Replace(',', '.'); ;

            if (decimal.Parse(VlrVenda) / 100 > 0)
            {
                // Atualização Tabela de Preco de Venda              
                sql = $"Select * From  bm_produto_valor" +
                     $" where  bm_produto_valor.id_produto = '{Produtos.id}'";

                List<ProdutosValorModel> lst = daldapper.Query<ProdutosValorModel>(sql).ToList();
                if (lst.Count > 0)
                {
                    sql = $"UpDate bm_produto_valor set vltabela1 = '{VlrVenda}'" +
                          $" where  bm_produto_valor.id_produto = '{Produtos.id}'";
                }
                else
                {
                    sql = $"Insert into bm_produto_valor(id_produto, vlTabela1) Value('{Produtos.id}', '{VlrVenda}')";
                }

                daldapper.Execute(sql, new ProdutosValorModel());
            }

            daldapper.Close();

            return true;
        }

        public bool InserirProduto(ProdutosModel Produto)
        {
            string Dat1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string Dat2 = Produto.dtatualizacao.ToString("yyyy-MM-dd HH:mm:ss");
            string Ativo = "1";  //Flag indicando que Produto está em atividade          
            string InformacaoAdicional = !string.IsNullOrEmpty(Produto.infadicional)? Produto.infadicional.ToUpper():null;
          
            Procedures Util = new Procedures(); // Biblioteca utilitarios (Tratamento de Variaveis neste caso)

            string sql = "Insert into bm_produto (id_produto_categoria," +
                    "id_produto_subcategoria," +
                    "id_produto_unidcompra," +
                    "id_produto_unidvenda," +
                    "id_produto_unidtributada," +
                    " id_produto_marca," +
                    "id_plc_gerencial," +
                    "id_profissional_cad," +
                    " id_profissional_alt," +
                    "descricao," +
                    "infadicional," +
                    "referencia," +
                    "referenciacompra," +
                    "gtin," +
                    "ippt," +
                    "codigoanp," +
                    "qtEstoque," +
                    "qtEstoquemin," +
                    "qtEntradas," +
                    "qtSaidas," +
                    "vlUltCompra," +
                    "vlCustoMedio," +
                    "ProdutoServico," +
                    "Localiza1," +
                    "Localiza2," +
                    "Localiza3," +
                    "ativo," +
                    "dtcadastro," +
                    "dtatualizacao)" +
                    $"Value('{Produto.id_produto_categoria}'," +
                    $"'{Produto.id_produto_subcategoria}'," +
                    $"'{Produto.id_produto_unidcompra}'," +
                    $"'{Produto.id_produto_unidvenda}'," +
                    $"'{Produto.id_produto_unidtributada}'," +
                    $"'{Produto.id_produto_marca}'," +
                    $"'{Produto.id_plc_gerencial}'," +
                    $"'{Produto.id_profissional_cad}'," +
                    $"'{Produto.id_profissional_alt}'," +
                    $"'{Produto.descricao.ToUpper()}'," +
                    $"'{InformacaoAdicional}'," +
                    $"'{Produto.referencia}'," +
                    $"'{Produto.referenciacompra}'," +
                    $"'{Produto.gtin}'," +
                    $"'{Produto.ippt}'," +
                    $"'{Produto.codigoanp}'," +
                    $"'{Util.VerificarNum(Produto.qtEstoque.ToString())}'," +
                    $"'{Util.VerificarNum(Produto.qtEstoquemin.ToString())}'," +
                    $"'{Util.VerificarNum(Produto.qtEntradas.ToString())}'," +
                    $"'{Util.VerificarNum(Produto.qtSaidas.ToString())}'," +
                    $"'{Util.VerificarNum(Produto.vlUltCompra.ToString())}'," +
                    $"'{Util.VerificarNum(Produto.vlCustoMedio.ToString())}'," +
                    $"'{Produto.ProdutoServico}'," +
                    $"'{Produto.Localiza1}'," +
                    $"'{Produto.Localiza2}'," +
                    $"'{Produto.Localiza3}'," +
                    $"'{Ativo}'," +
                    $"'{Dat1}'," +
                    $"'{Dat2}')";

            DALDPR dal = new DALDPR();
            var daldapper = dal.DALCON();
            var IdProduto = daldapper.ExecuteScalar(sql + "; select LAST_INSERT_ID();", new ProdutosModel());

            // Registro Novo Tabela de Preços


            if (Produto.VlTabela1 > 0)
            {
                string VlrVenda = Util.VerificarNum(Produto.VlTabela1.ToString());
                sql = $"Insert into bm_produto_valor(id_produto, vlTabela1) Value('{IdProduto}', '{VlrVenda}')";
                daldapper.Execute(sql, new ProdutosValorModel());
            }

            daldapper.Close();
            return true;
        }

        public bool GravarEstoque(ProdutosModel Produtos)
        {
            string sql = "";

            List<ProdutosModel> Lista_Entradas = JsonConvert.DeserializeObject<List<ProdutosModel>>(Produtos.ListaEntradas);
            Procedures util = new Procedures();

            for (int i = 0; i < Lista_Entradas.Count; i++)
            {
                var idProd = Lista_Entradas[i].id.ToString();

                string Qtd = util.VerificarNum(Lista_Entradas[i].qtEstoque.ToString("0.00"));

                //decimal VlrCompra = Lista_Entradas[i].vlUltCompra;
                var VlrCompra = Lista_Entradas[i].vlUltCompra.ToString().Replace(',', '.');
                //decimal VlrVenda = Lista_Entradas[i].VlTabela1;
                var VlrVenda = Lista_Entradas[i].VlTabela1.ToString().Replace(',', '.');

                // Verifica historico do Produto Antes das atualizaçõs
                ProdutosModel produto = ConsultaProduto(idProd);
                if (decimal.Parse(VlrCompra) == 0)
                {
                    VlrCompra = produto.vlUltCompra.ToString().Replace(',', '.');
                }

                sql = $"UpDate bm_produto set qtEstoque = coalesce(qtEstoque,0) + '{Qtd}'," +
                      $" vlUltCompra = '{VlrCompra}'" +
                      $" where id = '{idProd}'";

                // Atualização de Estoque Produtos           
                DALDPR dal = new DALDPR();
                var daldapper = dal.DALCON();
                daldapper.Execute(sql, new ProdutosModel());

                if (decimal.Parse(VlrVenda) / 100 > 0)
                {
                    // Atualização Tabela de Preco de Venda              
                    sql = $"Select * From  bm_produto_valor" +
                         $" where  bm_produto_valor.id_produto = '{idProd}'";

                    List<ProdutosValorModel> lst = daldapper.Query<ProdutosValorModel>(sql).ToList();
                    if (lst.Count > 0)
                    {
                        sql = $"UpDate bm_produto_valor set vltabela1 = '{VlrVenda}'" +
                              $" where  bm_produto_valor.id_produto = '{idProd}'";
                    }
                    else
                    {
                        sql = $"Insert into bm_produto_valor(id_produto, vlTabela1) Value('{idProd}', '{VlrVenda}')";
                    }

                    daldapper.Execute(sql, new ProdutosValorModel());
                }

                //
                // Atualização Tabela Kardex             
                // TODO - Colocar id do usuario logado
                int IdProfissional = 0;
                string documento = "0000";
                string data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                decimal EstoqueAnterior = produto.qtEstoque;
                decimal Qtdade = decimal.Parse(Qtd) / 100;
                decimal EstoqueApos = EstoqueAnterior + Qtdade;

                sql = "Insert into bm_produto_kardex (Id_Profissional ," +
                                  " Documento ," +
                                  " Quantidade ," +
                                  " EstoqueAnterior," +
                                  " EstoquePosterior," +
                                  " Data," +
                                  " Id_Produto)" +
                                  "Value(" +
                                  $" '{IdProfissional}'," +
                                  $" '{documento}'," +
                                  $" '{Qtd}'," +
                                  $" '{EstoqueAnterior.ToString().Replace(",", ".")}'," +
                                  $" '{EstoqueApos.ToString().Replace(",", ".")}'," +
                                  $" '{data}'," +
                                  $" '{idProd}' )";

                daldapper.Execute(sql, new KardexModel());
                daldapper.Close();
            }
            return true;
        }

        public List<ProdutosModel> Entradas(string ID, string Descricao, decimal Qtd)
        {
            List<ProdutosModel> Entradas = new List<ProdutosModel>();
            ProdutosModel Itens;
            Itens = new ProdutosModel
            {
                id = ID,
                descricao = Descricao,
                qtEstoque = Qtd
            };

            Entradas.Add(Itens);

            return Entradas;
        }
    }
}
