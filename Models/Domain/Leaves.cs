using System;

namespace Models.Domain
{
    public class Leaves 
    {
        public long Id { get; set; }
        public string   Trxtype { get; set; }// 1: ekle, 2: güncelle 
        public long   Pid { get; set; }
        public int Cid { get; set; }
        public DateTime Trxdate { get; set; } // değişiklik zamanı
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public bool Paid { get; set; } = true;
        public decimal Timeval { get; set; } 
        public int Timeunit { get; set; } = 1; // 1: gün, 2: hafta, 3: ay
        public DateTime Cd { get; set; } // oluşturma zamanı
        public string Docref { get; set; } = "";
        public string Notes { get; set; } = "";
        public bool Ph {get; set; } // resmi tatiller iş günü sayılsın mı?
        public bool Avans { get; set; } = false;
        public string Period { get; set; }
        public int Cu { get; set; }
    }

    public class LeavesForShow
    {
        public long Id { get; set; }
        public string Tcno { get; set; }
        public string Ads { get; set; }
        public string Comp { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public bool Paid { get; set; } = true;
        public decimal Timeval { get; set; }
        public bool Ph { get; set; }
        public string Notes { get; set; } = "";
        public string Docref { get; set; } = "";
    }
    public class ExcelLeaves
    {
        public string Tcno { get; set; }
        public string Ads { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public bool Paid { get; set; } = true;
        public string Notes { get; set; } = "";
        public bool Ph { get; set; }
    }
    public class LeavePeriod
    {
        public long Id { get; set; } = 0;
        public string Tcno { get; set; }
        public string Period { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public decimal His { get; set; }
        public decimal Ki { get; set; }
        public decimal Srk { get; set; }
    }
    public static class LeavesChanged
    {
        public static bool HasChanged { get; set; } = false;
        public static bool HasChangedForDialog { get; set; } = false;
    }
}
    