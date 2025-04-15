namespace TMODELOBASET_WebAPI_CS.Models.Entities
{
    /// <summary>
    /// View Model (Sem Registros no DB) para padronizar a comunicação do Back-End com o Front-End, trazendo a exibição dos erros.
    /// </summary>
    public class ErrorInfo
    {
        /// <summary>
        /// O Codigo do Erro
        /// </summary>
        public Int32 StatusCode { get; set; } = 0;

        /// <summary>
        /// A mensagem do Erro
        /// </summary>
        public String Message { get; set; } = String.Empty;
    }
}