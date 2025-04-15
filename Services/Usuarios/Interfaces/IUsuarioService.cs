using TMODELOBASET_WebAPI_CS.Connections.Database;
using TMODELOBASET_WebAPI_CS.Extensions.Helpers;
using TMODELOBASET_WebAPI_CS.Models.Usuario;
using TMODELOBASET_WebAPI_CS.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace TMODELOBASET_WebAPI_CS.Services.Usuarios.Interfaces
{
    public interface IUsuarioService
    {
        RequisicaoViewModel<Usuario> Listar(Int32 Pagina, Int32 RegistrosPorPagina, String CamposQuery = "", String ValoresQuery = "", String Ordenacao = "", Boolean Ordem = false);

        Boolean Salvar(Usuario userViewModel);

        Boolean Excluir(String ID);

        RequisicaoViewModel<Usuario> Autenticar(LoginViewModel Requisicao);
    }
}