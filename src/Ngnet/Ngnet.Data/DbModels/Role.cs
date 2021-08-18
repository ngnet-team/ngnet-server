﻿using Microsoft.AspNetCore.Identity;
using System;

namespace Ngnet.Data.DbModels
{
    public class Role : IdentityRole
    {
        public Role()
            : this(null)
        {
        }

        public Role(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
