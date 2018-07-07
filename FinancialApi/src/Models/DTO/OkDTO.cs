using System;
namespace FinancialApi.Models.DTO
{
    public class OkDTO : IBaseDTO
    {
        public string UUID { get; set; }

        public OkDTO(){}

        public OkDTO(string UUID)
        {
            this.UUID = UUID;
        }
    }
}
