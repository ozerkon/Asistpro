using System;

namespace Models.Common
{
    public class PublicHolidays
    {
        public DateTime Day { get; set; }
        public bool Ft { get; set; } // full time or part time
        public string Desc { get; set; }
    }
}
