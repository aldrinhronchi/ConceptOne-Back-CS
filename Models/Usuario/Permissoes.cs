namespace Coopersam_WebAPI_CS.Models.Usuario
{
    public class Permissoes
    {
        public Int32 ID { get; set; }
        public Int32 IDCargo { get; set; }
        public Int32 IDModulo { get; set; }
        public Int32 IDPagina { get; set; }
        public Boolean Criar { get; set; }
        public Boolean Revisar { get; set; }
        public Boolean Editar { get; set; }
        public Boolean Deletar { get; set; }
    }
}