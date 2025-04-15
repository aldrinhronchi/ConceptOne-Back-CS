using TMODELOBASET_WebAPI_CS.Models.Usuario;
using TMODELOBASET_WebAPI_CS.Models.ViewModels;
using TMODELOBASET_WebAPI_CS.Services.Usuarios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TMODELOBASET_WebAPI_CS.Controllers
{
    //[Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService UsuarioService;
        private readonly ICargoService CargoService;
        private readonly IPermissoesService PermissoesService;

        public UsuarioController(IUsuarioService UsuarioService, ICargoService cargoService, IPermissoesService permissoesService)
        {
            this.UsuarioService = UsuarioService;
            this.CargoService = cargoService;
            this.PermissoesService = permissoesService;
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

        #region Cargo

        [HttpGet]
        public IActionResult ListarCargos(Int32 Pagina = 1, Int32 RegistroPorPagina = 10, String Campos = "", String Valores = "", String Ordenacao = "", Boolean Ordem = false)
        {
            return Ok(this.CargoService.Listar(Pagina, RegistroPorPagina, Campos, Valores, Ordenacao, Ordem));
        }

        [HttpPost("Cargo")]
        public IActionResult SalvarCargo(Cargo CargoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return this.CargoService.Salvar(CargoViewModel) ? Ok() : BadRequest();
        }

        [HttpDelete("{_CargoID}")]
        public IActionResult ExcluirCargo(String _CargoID)
        {
            return Ok(this.CargoService.Excluir(_CargoID));
        }

        #endregion Cargo

        #region Permissoes

        [HttpGet]
        public IActionResult ListarPermissoes(String IDCargo)
        {
            return Ok(this.PermissoesService.Listar(IDCargo));
        }

        [HttpPost]
        public IActionResult SalvarPermissoes(String IDCargo, MenuViewModel[] NovasPermissoesMenu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return this.PermissoesService.Salvar(IDCargo, NovasPermissoesMenu) ? Ok() : BadRequest();
        }

        #endregion Permissoes
    }
}