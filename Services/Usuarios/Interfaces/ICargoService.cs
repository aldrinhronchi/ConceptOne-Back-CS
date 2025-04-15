using TMODELOBASET_WebAPI_CS.Models.Usuario;
using TMODELOBASET_WebAPI_CS.Models.ViewModels;

namespace TMODELOBASET_WebAPI_CS.Services.Usuarios.Interfaces
{
    public interface ICargoService
    {
        RequisicaoViewModel<Cargo> Listar(Int32 Pagina, Int32 RegistrosPorPagina, String CamposArray = "", String ValoresArray = "", String Ordenacao = "", Boolean Ordem = false);

        Boolean Salvar(Cargo CargoViewModel);

        Boolean Excluir(String ID);
    }
}