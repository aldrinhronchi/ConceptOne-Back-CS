namespace Coopersam_WebAPI_CS.Models.Entities
{
    /// <summary>
    /// Classe Pai contendo o basico para boa parte dos cadastros <br/>
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Identificador Unico, de Um em Um
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// Registro Ativo ou Inativo
        /// </summary>
        public Boolean Ativo { get; set; }

        /// <summary>
        /// Data de Criação, tendo valor padrão no banco de GetDate()
        /// </summary>
        public DateTime DataCriado { get; set; }

        /// <summary>
        /// Data da Última Alteração
        /// </summary>
        public DateTime? DataAlterado { get; set; }

        /// <summary>
        /// Identificador de quem gerou o registro sendo inserido no padrão: "IDUsuario - NomeUsuario", vindo formatado do Front-End
        /// </summary>
        public String? UsuarioCriado { get; set; }

        /// <summary>
        /// Identificador de quem fez a ultima alteração do registro, sendo inserido no padrão: "IDUsuario - NomeUsuario", vindo formatado do Front-End
        /// </summary>
        public String? UsuarioAlterado { get; set; }
    }
}