using System;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Models.Entity;
using FinancialApi.Utils;

namespace FinancialApi.Services 
{
    public abstract class EntryService<T> where T : Entry
    {

        protected virtual ErrorsDTO Validate(T entry){
            var errors = new ErrorsDTO();

            if (entry.DateToPay < DateTime.Today)
                errors.Add(entry.GetJSonFieldName("Date"), "Date can\'t be less that current date");
            
            return errors;
        }

    }

}