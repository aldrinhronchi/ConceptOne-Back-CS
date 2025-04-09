﻿using Coopersam_WebAPI_CS.Models.ViewModels;

namespace Coopersam_WebAPI_CS.Services.Usuarios.Interfaces
{
    public interface IPermissoesService
    {
        public List<MenuViewModel> Listar(String IDCargo);

        public Boolean Salvar(String IDCargo, MenuViewModel[] NovasPermissoesMenu);
    }
}