using System.Text.Json.Serialization;

namespace Coopersam_WebAPI_CS.Models.ViewModels
{
    /// <summary>
    /// View Model (Sem Registros no DB) para padronizar a comunicação do Back-End com o Front-End
    /// </summary>
    public class RequisicaoViewModel<T>
    {
        /// <summary>
        /// Os Dados a serem enviados
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// O tipo dos Dados
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// A Pagina atual de navegação, exceto quando conter "0", pois se conter ele nao tera paginação e deve trazer todos os registros.
        /// </summary>
        public Int32? Page { get; set; } = 1;

        /// <summary>
        /// Quantidade de Registros a serem mostrados por página
        /// </summary>
        public Int32? PageSize { get; set; } = 10;

        /// <summary>
        /// Total de Páginas
        /// </summary>
        public Int32? PageCount { get; set; } = 1;

        /// <summary>
        /// Status da Requisição
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public String? Status { get; set; }
    }
}