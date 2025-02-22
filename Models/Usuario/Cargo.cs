using Coopersam_WebAPI_CS.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coopersam_WebAPI_CS.Models.Usuario
{
    [Table("Cargos")]
    public class Cargo : Entity
    {
        public String Nome { get; set; }
    }
}