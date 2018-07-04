using System;
namespace FinancialApi.Models.Response
{
    public class OK : Base
    {
        public string UUID { get; set; }

        public OK(string UUID)
        {
            this.UUID = UUID;
        }
    }
}
