using TMODELOBASET_WebAPI_CS.Models.ViewModels;

namespace TMODELOBASET_WebAPI_CS.Services.Core.Interfaces
{
    public interface ICoreService
    {
        List<MenuViewModel> ExibirMenu(String IDCargo);

    }
}
