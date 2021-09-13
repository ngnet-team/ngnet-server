using System;

namespace Ngnet.Data.DbModels
{
    public class BaseModel<IdType> : IBaseModel
    {
        public IdType Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
