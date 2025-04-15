namespace TMODELOBASET_WebAPI_CS.Models.ViewModels
{
    /// <summary>
    /// View Model (Sem Registros no DB) da Página para ser usada no Front-End
    /// </summary>
    public class PaginaViewModel
    {
        /// <summary>
        /// Nome da Página
        /// </summary>
        public String Nome { get; set; }

        /// <summary>
        /// Url da Página, tendo valor padrão de enviar para a Tela de Login
        /// </summary>
        public String Url { get; set; } = "/";

        public AutorizacaoViewModel Autorizacao { get; set; }
    }
}