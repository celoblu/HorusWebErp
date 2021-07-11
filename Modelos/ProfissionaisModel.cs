using System;

namespace HorusWebErp.Modelos
{
    public class ProfissionaisModel
    {
        public int Id { get; set; }
        public int Id_Entidade { get; set; }
        public string nome { get; set; }
        public string apelido { get; set; }
        public int ativo { get; set; }
        public int tpprofissional { get; set; }
        public string dscfuncao { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        public string admin { get; set; }
        public int id_grupo_usuarios { get; set; }
        public string printer { get; set; }
        public DateTime dtcadastro { get; set; }
        public DateTime dtatualizacao { get; set; }
        public int id_profissional_cad { get; set; }
        public int id_profissional_alt { get; set; }
        public int lb_execpedido { get; set; }
        public int lb_bloqueiocliente { get; set; }
        public int lb_usoagenda { get; set; }
        public DateTime dtultacesso { get; set; }
        public string token_web { get; set; }

    }
}
