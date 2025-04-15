using System.ComponentModel.DataAnnotations.Schema;

namespace TMODELOBASET_WebAPI_CS.Models.Usuario
{
    [Table("Paginas")]
    public class Pagina
    {
        public Int32 ID { get; set; }
        public Int32 IDModulo { get; set; }
        public String Nome { get; set; }
        public String? Url { get; set; }

        //public String Icone { get; set; }
        public Int32 Ordem { get; set; }

        public Boolean Ativo { get; set; }
    }
}