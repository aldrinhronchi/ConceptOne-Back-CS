using TMODELOBASET_WebAPI_CS.Connections.Database;
using TMODELOBASET_WebAPI_CS.Models.Usuario;
using TMODELOBASET_WebAPI_CS.Models.ViewModels;
using TMODELOBASET_WebAPI_CS.Services.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace TMODELOBASET_WebAPI_CS.Services.Core
{
    public class CoreService :ICoreService
    {
        public List<MenuViewModel> ExibirMenu(String IDCargo)
        {
            if (!Int32.TryParse(IDCargo, out Int32 CargoID))
            {
                throw new ValidationException("ID invalido!");
            }
            List<MenuViewModel> Menu = new List<MenuViewModel>();

            using (TMODELOBASETContext db = new TMODELOBASETContext())
            {
                Cargo? Cargo = db.Cargo.Find(CargoID);
                if (Cargo == null || !Cargo.Ativo)
                {
                    throw new ValidationException("Cargo desativado ou não encontrado, entre em contato com o suporte.");
                }

                List<Permissoes> Permissoes = db.Permissoes.Where(x => x.IDCargo == Cargo.ID).ToList();
                List<Int32> IDModulos = Permissoes.Select(x => x.IDModulo).Distinct().ToList();
                List<Modulo> Modulos = db.Modulo.Where(x => x.Ativo && IDModulos.Contains(x.ID)).ToList();
                for (Int32 index = 0; index < Modulos.Count; index++)
                {
                    Modulo Modulo = Modulos[index];
                    MenuViewModel ModuloMenu = new MenuViewModel()
                    {
                        Nome = Modulo.Nome,
                        Ordem = index,
                        Icone = Modulo.Icone
                    };

                    List<Permissoes> PermissoesModulo = Permissoes.Where(x => x.IDModulo == Modulo.ID).ToList();
                    List<Int32> IDPaginas = PermissoesModulo.Select(x => x.IDPagina).ToList();
                    List<Pagina> Paginas = db.Pagina.Where(x => x.IDModulo == Modulo.ID && IDPaginas.Contains(x.ID) && x.Ativo).OrderBy(x => x.Ordem).ToList();

                    List<PaginaViewModel> PaginasModulo = new List<PaginaViewModel>();
                    foreach (Pagina? Pagina in Paginas)
                    {
                        Permissoes? PermissaoPagina = PermissoesModulo.FirstOrDefault(x => x.IDPagina == Pagina.ID);

                        if (PermissaoPagina == null)
                        {
                            continue;
                        }

                        PaginaViewModel Pag = new PaginaViewModel()
                        {
                            Nome = Pagina.Nome,
                            Url = Pagina.Url ?? Pagina.Nome.ToLower(),
                            Autorizacao = new AutorizacaoViewModel()
                            {
                                Criar = PermissaoPagina.Criar,
                                Revisar = PermissaoPagina.Revisar,
                                Editar = PermissaoPagina.Editar,
                                Deletar = PermissaoPagina.Deletar
                            }
                        };
                        PaginasModulo.Add(Pag);
                    }
                    ModuloMenu.Paginas = PaginasModulo;
                    Menu.Add(ModuloMenu);
                }

                return Menu;
            }
        }
    }
}
