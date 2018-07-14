using System;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Models.Entity;
using FinancialApi.Utils;

namespace FinancialApi.Services
{
    public abstract class EntryService
    {

        protected virtual ErrorsDTO Validate(Entry entry)
        {
            var errors = new ErrorsDTO();

            return errors;
        }

    }

}