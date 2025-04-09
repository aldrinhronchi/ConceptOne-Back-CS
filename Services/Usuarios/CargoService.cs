using Coopersam_WebAPI_CS.Connections.Database;
using Coopersam_WebAPI_CS.Connections.Database.Repositories;
using Coopersam_WebAPI_CS.Connections.Database.Repositories.Interfaces;
using Coopersam_WebAPI_CS.Extensions.Helpers;
using Coopersam_WebAPI_CS.Models.Usuario;
using Coopersam_WebAPI_CS.Models.ViewModels;
using Coopersam_WebAPI_CS.Services.Usuarios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Coopersam_WebAPI_CS.Services.Usuarios
{
    public class CargoService : ICargoService
    {
        public CargoService()
        {
        }

        public RequisicaoViewModel<Cargo> Listar(Int32 Pagina, Int32 RegistrosPorPagina,
            String CamposQuery = "", String ValoresQuery = "", String Ordenacao = "", Boolean Ordem = false)
        {
            RequisicaoViewModel<Cargo> requisicao;
            using (CoopersamContext db = new CoopersamContext())
            {
                IQueryable<Cargo> _Cargos = db.Cargo.AsQueryable();
                if (!String.IsNullOrWhiteSpace(CamposQuery))
                {
                    String[] CamposArray = CamposQuery.Split(";|;");
                    String[] ValoresArray = ValoresQuery.Split(";|;");
                    if (CamposArray.Length == ValoresArray.Length)
                    {
                        Dictionary<String, String> Filtros = new Dictionary<String, String>();
                        for (Int32 index = 0; index < CamposArray.Length; index++)
                        {
                            String? Campo = CamposArray[index];
                            String? Valor = ValoresArray[index];
                            if (!(String.IsNullOrWhiteSpace(Campo) && String.IsNullOrWhiteSpace(Valor)))
                            {
                                Filtros.Add(Campo, Valor);
                            }
                        }
                        IQueryable<Cargo> CargoFiltrado = _Cargos;
                        foreach (KeyValuePair<String, String> Filtro in Filtros)
                        {
                            switch (Filtro.Key)
                            {
                                default:
                                    CargoFiltrado = TipografiaHelper.Filtrar(CargoFiltrado, Filtro.Key, Filtro.Value);
                                    break;
                            }
                        }
                        _Cargos = CargoFiltrado;
                    }
                    else
                    {
                        throw new ValidationException("Não foi possivel filtrar!");
                    }
                }
                if (!String.IsNullOrWhiteSpace(Ordenacao))
                {
                    _Cargos = TipografiaHelper.Ordenar(_Cargos, Ordenacao, Ordem);
                }
                else
                {
                    _Cargos = TipografiaHelper.Ordenar(_Cargos, "ID", Ordem);
                }
                requisicao = TipografiaHelper.FormatarRequisicao(_Cargos, Pagina, RegistrosPorPagina);
            }

            return requisicao;
        }

        public Boolean Salvar(Cargo CargoViewModel)
        {
            Validator.ValidateObject(CargoViewModel, new ValidationContext(CargoViewModel), true);
            using (CoopersamContext db = new CoopersamContext())
            {
                IRepository<Cargo> CargoRepo = new Repository<Cargo>(db);
                Cargo? _Cargo = db.Cargo.AsNoTracking().FirstOrDefault(x => x.ID == CargoViewModel.ID);
                if (_Cargo == null)
                {
                    if (CargoViewModel.ID != 0)
                    {
                        throw new ValidationException("ID deve ser vazio!");
                    }
                    CargoViewModel.DataCriado = TimeZoneManager.GetTimeNow();
                    CargoRepo.Create(CargoViewModel);
                }
                else
                {
                    CargoViewModel.DataAlterado = TimeZoneManager.GetTimeNow();
                    CargoRepo.Update(CargoViewModel);
                }
            }
            return true;
        }

        public Boolean Excluir(String ID)
        {
            if (!Int32.TryParse(ID, out Int32 CargoID))
            {
                throw new ValidationException("ID invalido!");
            }
            if (CargoID == 1)
            {
                throw new ValidationException("O cargo Administrador não pode ser exluido");
            }
            using (CoopersamContext db = new CoopersamContext())
            {
                IRepository<Cargo> CargoRepo = new Repository<Cargo>(db);
                Cargo? _Cargo = CargoRepo.Find(CargoID);
                if (_Cargo == null)
                {
                    throw new ValidationException("Cargo não encontrado");
                }
                return CargoRepo.Delete(_Cargo);
            }
        }
    }
}