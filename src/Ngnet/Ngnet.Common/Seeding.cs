using System.Collections.Generic;

namespace Ngnet.Common
{
    public static class Seeding
    {
        public static List<string> RoleNames = new List<string> { "Admin", "User" };

        public static List<string> CarServiceNames = new List<string> 
        { "Oil change", "Vignette", "Technical review", "Third-party liability insurance", "Мotor hull insurance" };

        public static List<string> HealthServiceNames = new List<string> 
        { "Dentist", "Eye doctor", "Allergists/Immunologists", "Cardiologists", "Dermatologists", "Gastroenterologists", "Nephrologists" };
    }
}
