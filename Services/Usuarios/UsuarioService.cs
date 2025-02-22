using Coopersam_WebAPI_CS.Connections.Database;
using Coopersam_WebAPI_CS.Connections.Database.Repositories;
using Coopersam_WebAPI_CS.Connections.Database.Repositories.Interfaces;
using Coopersam_WebAPI_CS.Extensions.Helpers;
using Coopersam_WebAPI_CS.Models.Usuario;
using Coopersam_WebAPI_CS.Models.ViewModels;
using Coopersam_WebAPI_CS.Services.Usuarios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Coopersam_WebAPI_CS.Services.Usuarios
{
    public class UsuarioService : IUsuarioService
    {
        public RequisicaoViewModel<Usuario> Listar(Int32 Pagina, Int32 RegistrosPorPagina,
            String CamposQuery = "", String ValoresQuery = "", String Ordenacao = "", Boolean Ordem = false)
        {
            RequisicaoViewModel<Usuario> Requisicao;
            using (CoopersamContext db = new CoopersamContext())
            {
                IQueryable<Usuario> _Usuarios = db.Usuario.Include(x => x.Cargo);
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
                        IQueryable<Usuario> UsuarioFiltrado = _Usuarios;
                        foreach (KeyValuePair<String, String> Filtro in Filtros)
                        {
                            switch (Filtro.Key)
                            {
                                case "CargoNome":
                                    UsuarioFiltrado = UsuarioFiltrado.Where(x => x.Cargo.Nome.ToUpper().Contains(Filtro.Value.ToUpper()));
                                    break;

                                default:
                                    UsuarioFiltrado = TipografiaHelper.Filtrar(UsuarioFiltrado, Filtro.Key, Filtro.Value);
                                    break;
                            }
                        }
                        _Usuarios = UsuarioFiltrado;
                    }
                    else
                    {
                        throw new ValidationException("Não foi possivel filtrar!");
                    }
                }
                if (!String.IsNullOrWhiteSpace(Ordenacao))
                {
                    switch (Ordenacao)
                    {
                        case "CargoNome":
                            if (Ordem)
                            {
                                _Usuarios = _Usuarios.OrderBy(x => x.Cargo.Nome);
                            }
                            else
                            {
                                _Usuarios = _Usuarios.OrderByDescending(x => x.Cargo.Nome);
                            }
                            break;

                        default:
                            _Usuarios = TipografiaHelper.Ordenar(_Usuarios, Ordenacao, Ordem);
                            break;
                    }
                }
                else
                {
                    _Usuarios = TipografiaHelper.Ordenar(_Usuarios, "ID", Ordem);
                }
                Requisicao = TipografiaHelper.FormatarRequisicao(_Usuarios, Pagina, RegistrosPorPagina);
            }
            return Requisicao;
        }

        public Boolean Salvar(Usuario UsuarioViewModel)
        {
            Validator.ValidateObject(UsuarioViewModel, new ValidationContext(UsuarioViewModel), true);
            using (CoopersamContext db = new CoopersamContext())
            {
                //Cargo? _Cargo = db.Cargo.Find(UsuarioViewModel.IDCargo);
                //if (_Cargo == null)
                //{
                //    throw new ValidationException("Cargo invalido!");
                //}
                //else
                //{
                //    if (!_Cargo.Ativo)
                //    {
                //        throw new ValidationException("Cargo invalido!");
                //    }
                //}
                IRepository<Usuario> UsuarioRepo = new Repository<Usuario>(db);
                Usuario? _Usuario = db.Usuario.AsNoTracking().FirstOrDefault(x => x.ID == UsuarioViewModel.ID);
                if (_Usuario == null)
                {
                    if (UsuarioViewModel.ID != 0)
                    {
                        throw new ValidationException("ID deve ser vazio!");
                    }
                    UsuarioViewModel.Senha = EncriptarSenha(UsuarioViewModel.Senha);
                    _Usuario = db.Usuario.AsNoTracking().FirstOrDefault(x => x.Login == UsuarioViewModel.Login || x.Email == UsuarioViewModel.Email);
                    if (_Usuario == null)
                    {
                        UsuarioViewModel.DataCriado = TimeZoneManager.GetTimeNow();
                        UsuarioRepo.Create(UsuarioViewModel);
                    }
                    else
                    {
                        throw new ValidationException("Email/Login já cadastrado");
                    }
                }
                else
                {
                    UsuarioViewModel.DataAlterado = TimeZoneManager.GetTimeNow();
                    if (UsuarioViewModel.Senha == String.Empty)
                    {
                        UsuarioViewModel.Senha = _Usuario.Senha;
                    }
                    else
                    {
                        UsuarioViewModel.Senha = EncriptarSenha(UsuarioViewModel.Senha);
                    }
                    UsuarioRepo.Update(UsuarioViewModel);
                }
            }

            return true;
        }

        public Boolean Excluir(String ID)
        {
            if (!Int32.TryParse(ID, out Int32 UsuarioID))
            {
                throw new ValidationException("ID invalido!");
            }
            using (CoopersamContext db = new CoopersamContext())
            {
                IRepository<Usuario> UsuarioRepo = new Repository<Usuario>(db);

                Usuario? _Usuario = UsuarioRepo.Find(UsuarioID);
                if (_Usuario == null)
                {
                    throw new ValidationException("Usuario não encontrado");
                }
                return UsuarioRepo.Delete(_Usuario);
            }
        }

        public RequisicaoViewModel<Usuario> Autenticar(LoginViewModel Requisicao)
        {
            if (String.IsNullOrWhiteSpace(Requisicao.Login) || String.IsNullOrWhiteSpace(Requisicao.Senha))
            {
                throw new ValidationException("Login/Senha obrigatorios.");
            }

            Requisicao.Senha = EncriptarSenha(Requisicao.Senha);
            List<Usuario>? Usuarios = null;
            using (CoopersamContext db = new CoopersamContext())
            {
                Usuarios = db.Usuario.Where(x => (x.Email.ToUpper() == Requisicao.Login.ToUpper() || x.Login.ToUpper() == Requisicao.Login.ToUpper())
                                                 && x.Ativo).ToList();
            }
            if (Usuarios == null || Usuarios.Count() == 0)
            {
                throw new ValidationException("Usuario não encontrado");
            }

            Usuario? usuario = Usuarios.FirstOrDefault(x => x.Senha == Requisicao.Senha);
            if (usuario == null)
            {
                throw new ValidationException("Senha Incorreta");
            }

            usuario.Token = TokenHelper.GerarToken(usuario);
            RequisicaoViewModel<Usuario> requisicao = new RequisicaoViewModel<Usuario>()
            {
                Data = new List<Usuario>() { usuario },
                Page = 1,
                PageSize = 10,
                Type = nameof(Usuario)
            };
            return requisicao;
        }

        private String EncriptarSenha(String Senha)
        {
            HashAlgorithm sha = SHA1.Create();

            Byte[] encryptedPassword = sha.ComputeHash(Encoding.UTF8.GetBytes(Senha));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (Byte caracter in encryptedPassword)
            {
                stringBuilder.Append(caracter.ToString("X2"));
            }

            return stringBuilder.ToString();
        }
    }
}