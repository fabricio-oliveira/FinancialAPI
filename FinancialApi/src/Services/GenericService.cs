using System;
using FinancialApi.Models.DTO.Request;
using FinancialApi.Models.DTO.Response;
using FinancialApi.src.Utils;

namespace FinancialApi.Services 
{
    public class GenericService<T> where T : EntryDTO
    {

        protected virtual ErrorsDTO Validate(T entry){
            var errors = new ErrorsDTO();

            if (entry.Date < DateTime.Today)
                errors.Add(entry.GetJSonFieldName("Date"), "Date can\'t be less that current date");
            
            return errors;
        }

    }

}