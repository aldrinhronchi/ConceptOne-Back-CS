namespace Coopersam_WebAPI_CS.Models.ViewModels
{
    /// <summary>
    /// View Model (Sem Registros no DB) para trazer as permissões de acesso das telas
    /// </summary>
    public class AutorizacaoViewModel
    {
        /// <summary>
        /// Pode Criar Novos Items
        /// </summary>
        public Boolean Criar { get; set; } = false;

        /// <summary>
        /// Pode Visualizar porem não pode editar os Items Existentes
        /// </summary>
        public Boolean Revisar { get; set; } = false;

        /// <summary>
        /// Pode Editar os Items já existentes e seus campos
        /// </summary>
        public Boolean Editar { get; set; } = false;

        /// <summary>
        /// Pode Deletar os Items já existentes
        /// </summary>
        public Boolean Deletar { get; set; } = false;
    }
}