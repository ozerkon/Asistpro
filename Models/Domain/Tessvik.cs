namespace Models.Domain
{
    public class Incentive
    {
        public string Cn { get; set; } = string.Empty;
        public string Tcno { get; set; } = string.Empty;
        public string No { get; set; } = string.Empty; // kanun numarası
        public string Bbd { get; set; } = string.Empty; // başlangıç-bitiş dönemi
        public string Ts { get; set; } = string.Empty; // teşvik süresi
        public string Tk { get; set; } = string.Empty; // teşvik kazancı
        public int Ios { get; set; } = 0; // ilave olunacak sayı
        public bool Valid { get; set; } = true;
    }
    public class Tesvik
    {
        public string Tckn { get; set; } = string.Empty;
        public Ysys Ysys { get; set; } = new Ysys();
        public Abyob Aybob { get; set; } = new Abyob();
        public Oybyu Oybyu { get; set; } = new Oybyu();
        public Yybyu Yybyu { get; set; } = new Yybyu();
        public Yuoa Yuoa { get; set; } = new Yuoa();
        public Uidd Uidd { get; set; } = new Uidd();
        public bool Valid { get; set; } = true;

    }
    public class Kanunlar 
    {
        public string Ts { get; set; } = string.Empty;
        public string Ios { get; set; } = "0";
    }
    public class Ysys : Kanunlar { } // 2828
    public class Abyob : Kanunlar { } //6111
    public class Oybyu : Kanunlar { } // 17103
    public class Yybyu : Kanunlar { } // 27103
    public class Yuoa : Kanunlar { } // 7316
    public class Uidd : Kanunlar { } // 3294
}
