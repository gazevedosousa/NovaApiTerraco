﻿using System.Threading.Tasks;
using TerracoDaCida.DTO;
using TerracoDaCida.Models;
using TerracoDaCida.Util;

namespace TerracoDaCida.Services.Interfaces
{
    public interface IComandaService
    {
        Task<ApiResponse<List<ComandaDTO>>> BuscaComandas();
        Task<ApiResponse<bool>> AbreComanda(AbreComandaDTO abreComandaDTO);
        Task<ApiResponse<DetalhaComandaDTO>> DetalhaComanda(int coComanda);
        Task<bool> ExisteComanda(int coComanda);
        Task<bool> ExisteComandaAbertaParaNomeInformado(string noComanda);
    }
}
