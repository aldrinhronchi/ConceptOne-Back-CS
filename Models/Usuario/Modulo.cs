using System.ComponentModel.DataAnnotations.Schema;

namespace TMODELOBASET_WebAPI_CS.Models.Usuario
{
    [Table("Modulos")]
    public class Modulo
    {
        public Int32 ID { get; set; }
        public String Nome { get; set; }
        public String Icone { get; set; }
        public Boolean Ativo { get; set; }
    }
}