using System;

namespace Ngnet.Data.DbModels
{
    public class HealthServiceName : BaseModel<string>
    {
        public HealthServiceName()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; }
    }
}
