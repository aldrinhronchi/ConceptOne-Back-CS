using TMODELOBASET_WebAPI_CS.Connections.Database;
using TMODELOBASET_WebAPI_CS.Connections.Database.Repositories;
using TMODELOBASET_WebAPI_CS.Models.Usuario;
using TMODELOBASET_WebAPI_CS.Models.ViewModels;
using TMODELOBASET_WebAPI_CS.Services.Usuarios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TMODELOBASET_WebAPI_CS.Services.Usuarios
{
    public class PermissoesService : IPermissoesService
    {
        public List<MenuViewModel> Listar(string IDCargo)
        {
            if (!int.TryParse(IDCargo, out int CargoID))
            {
                throw new ValidationException("ID invalido!");
            }
            List<MenuViewModel> Menu = new List<MenuViewModel>();

            using (TMODELOBASETContext db = new TMODELOBASETContext())
            {
                Cargo? Cargo = db.Cargo.Find(CargoID);
                if (Cargo == null)
                {
                    throw new ValidationException("Cargo desativado ou não encontrado, entre em contato com o suporte.");
                }

                List<Permissoes> Permissoes = db.Permissoes.Where(x => x.IDCargo == Cargo.ID).ToList();
                List<Modulo> Modulos = db.Modulo.Where(x => x.Ativo).ToList();
                for (int index = 0; index < Modulos.Count; index++)
                {
                    Modulo Modulo = Modulos[index];
                    MenuViewModel ModuloMenu = new MenuViewModel()
                    {
                        Nome = Modulo.Nome,
                        Ordem = index,
                        Icone = Modulo.Icone
                    };

                    List<Permissoes> PermissoesModulo = Permissoes.Where(x => x.IDModulo == Modulo.ID).ToList();
                    List<Pagina> Paginas = db.Pagina.Where(x => x.IDModulo == Modulo.ID && x.Ativo).OrderBy(x => x.Ordem).ToList();

                    List<PaginaViewModel> PaginasModulo = new List<PaginaViewModel>();
                    foreach (Pagina? Pagina in Paginas)
                    {
                        Permissoes? PermissaoPagina = PermissoesModulo.FirstOrDefault(x => x.IDPagina == Pagina.ID);

                        if (PermissaoPagina == null)
                        {
                            PermissaoPagina = new Permissoes()
                            {
                                Criar = false,
                                Revisar = false,
                                Editar = false,
                                Deletar = false
                            };
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

        public bool Salvar(string IDCargo, MenuViewModel[] NovasPermissoesMenu)
        {
            if (!int.TryParse(IDCargo, out int CargoID))
            {
                throw new ValidationException("ID invalido!");
            }
            if (CargoID == 1)
            {
                throw new ValidationException("Não é possivel alterar o cargo Administrador!");
            }

            using (TMODELOBASETContext db = new TMODELOBASETContext())
            {
                Cargo? Cargo = db.Cargo.Find(CargoID);
                if (Cargo == null || !Cargo.Ativo)
                {
                    throw new ValidationException("Cargo desativado ou não encontrado, entre em contato com o suporte.");
                }

                List<Permissoes> NovasPermissoes = new List<Permissoes>();
                foreach (MenuViewModel ModuloMenu in NovasPermissoesMenu)
                {
                    Modulo? Modulo = db.Modulo.FirstOrDefault(x => x.Nome == ModuloMenu.Nome);
                    if (Modulo == null)
                    {
                        continue;
                    }
                    foreach (PaginaViewModel PaginaMenu in ModuloMenu.Paginas)
                    {
                        Pagina? Pagina = db.Pagina.FirstOrDefault(x => x.Nome == PaginaMenu.Nome);
                        if (Pagina == null)
                        {
                            continue;
                        }
                        if (!PaginaMenu.Autorizacao.Criar && !PaginaMenu.Autorizacao.Revisar
                            && !PaginaMenu.Autorizacao.Editar && !PaginaMenu.Autorizacao.Deletar)
                        {
                            continue;
                        }
                        NovasPermissoes.Add(new Permissoes()
                        {
                            IDCargo = CargoID,
                            IDModulo = Modulo!.ID,
                            IDPagina = Pagina!.ID,
                            Criar = PaginaMenu.Autorizacao.Criar,
                            Revisar = PaginaMenu.Autorizacao.Revisar,
                            Editar = PaginaMenu.Autorizacao.Editar,
                            Deletar = PaginaMenu.Autorizacao.Deletar,
                        });
                    }
                }
                List<Permissoes> PermissoesAtuais = db.Permissoes.AsNoTracking().Where(x => x.IDCargo == Cargo.ID).ToList();
                Repository<Permissoes> PermissoesRepo = new Repository<Permissoes>(db);
                foreach (Permissoes PermissaoNova in NovasPermissoes)
                {
                    Permissoes? PermissaoAtual = PermissoesAtuais.FirstOrDefault(x => x.IDModulo == PermissaoNova.IDModulo && x.IDPagina == PermissaoNova.IDPagina);
                    if (PermissaoAtual != null)
                    {
                        PermissaoNova.ID = PermissaoAtual.ID;
                        PermissoesRepo.Update(PermissaoNova);
                        PermissoesAtuais.Remove(PermissaoAtual);
                    }
                    else
                    {
                        PermissoesRepo.Create(PermissaoNova);
                    }
                }
                if (PermissoesAtuais.Count() != 0)
                {
                    foreach (Permissoes PermissaoAtual in PermissoesAtuais)
                    {
                        PermissoesRepo.Delete(PermissaoAtual);
                    }
                }
            }
            return true;
        }
    }
}