using System;
using FinancialApi.Models.Entity;
using FinancialApi.Models.DTO;
using FinancialApi.src.Utils;

namespace FinancialApi.Services 
{
    public class GenericService<T> where T : Entry
    {

        protected ErrorsDTO Validate(T value){
            var errors = new ErrorsDTO();

            if (value.Date < DateTime.Today)
                errors.Add(value.GetJSonFieldName("Date"), "Date can\'t be less that current date");
            
            return errors;
        }

    }

}