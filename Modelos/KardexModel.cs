using System;
using System.ComponentModel.DataAnnotations;

namespace HorusWebErp.Modelos
{
    public class KardexModel
    {
        public int id { get; set; }
        public int id_produto { get; set; }
        public int Id_Profissional { get; set; }
        public string Documento { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }
        public decimal Quantidade { get; set; }
        public decimal EstoqueAnterior { get; set; }
        public decimal EstoquePosterior { get; set; }
    }
}
