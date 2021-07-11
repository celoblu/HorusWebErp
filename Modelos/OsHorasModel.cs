namespace HorusWebErp.Modelos
{

    public class OsHorasModel
    {
        // Tabela bm_os_tecnicos
        public int id { get; set; }
        public int id_profissional { get; set; }
        public int id_os { get; set; }
        public string nome { get; set; }
        public string apelido { get; set; }
        public double qtdhoras { get; set; }
        public double valorhora { get; set; }
        public double hrsini { get; set; }
        public double hrsfim { get; set; }
    }

}
