namespace TMODELOBASET_WebAPI_CS.Models.ViewModels
{
    /// <summary>
    /// View Model (Sem Registros no DB) para trazer o Login e a Senha
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Login, podendo ser o campo "Login" ou "Email" do Usuario
        /// </summary>
        public String Login { get; set; }

        /// <summary>
        /// Senha do Usuario, será criptografada para validar no DB
        /// </summary>
        public String Senha { get; set; }
    }
}