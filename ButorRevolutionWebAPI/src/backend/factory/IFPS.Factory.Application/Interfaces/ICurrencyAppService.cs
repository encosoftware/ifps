using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface ICurrencyAppService
    {
        Task<List<CurrencyDto>> GetCurrenciesListAsync();
    }
}
