using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ICountryService
    {
        CountryDTO Retrieve(int id);
        List<CountryDTO> GetAll();
        int CreateOrUpdate(CountryDTO country);
        bool UpdateDescription(int id, string description);
        bool Delete(int id);

    }
}
