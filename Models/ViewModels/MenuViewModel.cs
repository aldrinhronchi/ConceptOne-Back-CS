namespace TMODELOBASET_WebAPI_CS.Models.ViewModels
{
    /// <summary>
    /// View Model (Sem Registros no DB) para montar o Menu de Navegação no Front-End
    /// </summary>
    public class MenuViewModel
    {
        /// <summary>
        /// Nome mostrado no Menu
        /// </summary>
        public String Nome { get; set; }

        /// <summary>
        /// Icone utilizado no Menu
        /// </summary>
        public String Icone { get; set; }

        /// <summary>
        /// Ordem na exibição
        /// </summary>
        public Int32 Ordem { get; set; }

        /// <summary>
        /// View Model (Sem Registros no DB) das Paginas a serem exibidas
        /// </summary>
        public List<PaginaViewModel> Paginas { get; set; }
    }
}