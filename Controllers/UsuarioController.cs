using Coopersam_WebAPI_CS.Models.Usuario;
using Coopersam_WebAPI_CS.Models.ViewModels;
using Coopersam_WebAPI_CS.Services.Usuarios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coopersam_WebAPI_CS.Controllers
{
    //[Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService UsuarioService;

        public UsuarioController(IUsuarioService UsuarioService)
        {
            this.UsuarioService = UsuarioService;
        }

        #region Usuario

        [HttpGet]
        public IActionResult ListarUsuarios(Int32 Pagina = 1, Int32 RegistroPorPagina = 10,
            String Campos = "", String Valores = "", String Ordenacao = "", Boolean Ordem = false)
        {
            return Ok(this.UsuarioService.Listar(Pagina, RegistroPorPagina, Campos, Valores, Ordenacao, Ordem));
        }

        [HttpPost]
        public IActionResult SalvarUsuario(Usuario userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return this.UsuarioService.Salvar(userViewModel) ? Ok() : BadRequest();
        }

        [HttpDelete("{_userID}")]
        public IActionResult ExcluirUsuario(String _userID)
        {
            return Ok(this.UsuarioService.Excluir(_userID));
        }

        #endregion Usuario

        #region Token

        [HttpPost, AllowAnonymous]
        public IActionResult Autenticar(LoginViewModel login)
        {
            return Ok(this.UsuarioService.Autenticar(login));
        }

        #endregion Token
    }
}