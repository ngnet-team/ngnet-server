using System.Collections.Generic;

namespace Ngnet.Common
{
    public static class Seeding
    {
        public static List<string> RoleNames = new List<string> 
        { 
            RoleType.Admin.ToString(), 
            RoleType.User.ToString() 
        };
    }
}
