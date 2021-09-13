using System;

namespace Ngnet.Data.DbModels
{
    public class CarServiceName : BaseModel<string>
    {
        public CarServiceName()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; }
    }
}
