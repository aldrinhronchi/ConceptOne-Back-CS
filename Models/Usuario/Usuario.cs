using TMODELOBASET_WebAPI_CS.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMODELOBASET_WebAPI_CS.Models.Usuario
{
    [Table("Usuarios")]
    public class Usuario : Entity
    {
        public String Nome { get; set; }
        public String Login { get; set; }
        public String Senha { get; set; }
        public Int32 IDCargo { get; set; }

        [ForeignKey(nameof(IDCargo))]
        public Cargo? Cargo { get; set; }

        public String Email { get; set; }

        [NotMapped]
        public String? Token { get; set; }
    }
}