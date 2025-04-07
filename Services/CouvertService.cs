using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TerracoDaCida.DTO;
using TerracoDaCida.Enums;
using TerracoDaCida.Models;
using TerracoDaCida.Repositories;
using TerracoDaCida.Repositories.Interfaces;
using TerracoDaCida.Services.Interfaces;
using TerracoDaCida.Util;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TerracoDaCida.Services
{
    public class CouvertService : ICouvertService
    {
        private readonly ILogger<CouvertService> _logger;
        private readonly ICouvertRepository _couvertRepository;

        public CouvertService(ILogger<CouvertService> logger, ICouvertRepository couvertRepository)
        {
            _logger = logger;
            _couvertRepository = couvertRepository;
        }

        public async Task<ApiResponse<List<CouvertDTO>>> BuscaCouverts()
        {
            List<Couvert> couverts = await _couvertRepository.BuscarCouverts();

            List<CouvertDTO> listaCouverts = couverts.Select(c => new CouvertDTO()
            {
                ValorCouvert = c.VrCouvert,
                IsAtivo = c.IsAtivo
            }).ToList();

            return ApiResponse<List<CouvertDTO>>.SuccessOk(listaCouverts);
        }

        public async Task<ApiResponse<bool>> CriaCouvert(CriaCouvertDTO criaCouvertDTO)
        {
            Couvert? couvertAtivo = await _couvertRepository.BuscaCouvertAtivo();
            if (couvertAtivo != null)
            {
                if (await _couvertRepository.DesativarCouvertAtivo(couvertAtivo.CoCouvert))
                {
                    _logger.LogInformation($"Couvert desativado com sucesso - {couvertAtivo.CoCouvert}");
                }
                else
                {
                    return ApiResponse<bool>.Error($"Erro ao desativar Couvert. coCouvert: {couvertAtivo.CoCouvert}");
                }
            }

            Couvert couvert = new Couvert
            {
                VrCouvert = criaCouvertDTO.ValorCouvert,
                IsAtivo = true
            };

            if (await _couvertRepository.CriarCouvert(couvert))
            {
                _logger.LogInformation($"Couvert criado com sucesso no valor de {criaCouvertDTO.ValorCouvert}");
                return ApiResponse<bool>.SuccessCreated(true);
            }
            else
            {
                return ApiResponse<bool>.Error($"Erro criar Couvert. DTO de entrada: {criaCouvertDTO.ToJson()}");
            }
        }

    }
}
