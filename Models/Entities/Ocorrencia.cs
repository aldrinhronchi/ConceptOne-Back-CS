using System.ComponentModel.DataAnnotations.Schema;

namespace TMODELOBASET_WebAPI_CS.Models.Entities
{
    [Table("OcorrenciaLog")]
    public class Ocorrencia
    {
        public Int32 ID { get; set; }
        public Int32 IDErro { get; set; }
        public String Aplicacao { get; set; }
        public DateTime Data { get; set; }
    }
}