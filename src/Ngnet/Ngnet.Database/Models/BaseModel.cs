﻿using System;

namespace Ngnet.Database.Models
{
    public class BaseModel<IdType> : IBaseModel
    {
        public IdType Id { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
