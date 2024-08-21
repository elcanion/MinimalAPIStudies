using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class CountryService : ICountryService
    {
        public CountryDTO Retrieve(int id)
        {
            return new CountryDTO();
        }

        public List<CountryDTO> GetAll()
        {
            List<CountryDTO> countryDTOs = new List<CountryDTO>();
            return countryDTOs;
        }
        public int CreateOrUpdate(CountryDTO country)
        {
            return 0;
        }
        public bool UpdateDescription(int id, string description)
        {
            return true;
        }
        public bool Delete(int id)
        {
            return true;
        }
    }
}
