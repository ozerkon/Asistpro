using Models.Common;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SgkAssistant.Helpers
{
    public class NtpClient
    {
        public static Random Rd  = new Random();
        public static DateTime GetNetworkTime(out string msg)
        {
            msg = "";
            try
            {
                return GetDateTime(out msg); //GetNetworkTime("time.windows.com"); // time-a.nist.gov
            }
            catch 
            {
                msg = "Bu programın çalışması için internet bağlantısı gereklidir";
                return DateTime.MinValue;
            }
        }
        public static bool GetTime(out string msg)
        {
            msg = "";
            try
            {
                DateTime dt = GetDateTime(out msg); //GetNetworkTime("time.windows.com") 
                if (dt.Date == DateTime.Now.Date) { return true; } else { msg = "Bilgisayarınızın tarih ayarları hatalı"; return false; } // time-a.nist.gov
            }
            catch 
            {
                msg = "Bu programın çalışması için internet bağlantısı gereklidir";
                return false;
            }
        }
        public static DateTime GetNetworkTime(string ntpServer)
        {
            IPAddress[] address = Dns.GetHostEntry(ntpServer).AddressList;

            if (address == null || address.Length == 0)
                throw new ArgumentException("Could not resolve ip address from '" + ntpServer + "'.", "ntpServer");

            IPEndPoint ep = new IPEndPoint(address[0], 123);

            return GetNetworkTime(ep);
        }
        public static DateTime GetNetworkTime(IPEndPoint ep)
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                try
                {
                    s.Connect(ep);

                    byte[] ntpData = new byte[48]; // RFC 2030 
                    ntpData[0] = 0x1B;
                    for (int i = 1; i < 48; i++)
                        ntpData[i] = 0;

                    s.Send(ntpData);
                    s.Receive(ntpData);

                    byte offsetTransmitTime = 40;
                    ulong intpart = 0;
                    ulong fractpart = 0;

                    for (int i = 0; i <= 3; i++)
                        intpart = 256 * intpart + ntpData[offsetTransmitTime + i];

                    for (int i = 4; i <= 7; i++)
                        fractpart = 256 * fractpart + ntpData[offsetTransmitTime + i];

                    ulong milliseconds = (intpart * 1000 + (fractpart * 1000) / 0x100000000L);
                    return (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)milliseconds)).ToLocalTime();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"NTP error: {ex.Message}");
                    throw;
                }
            }
        }
        public static string Complication(DateTime dt, out string msg)
        {
            StringBuilder builder = new StringBuilder();
            msg = ""; 
            try
            {
                string dtt = dt.ToString("yyyy");
                dtt += dt.ToString("MM");
                dtt += dt.ToString("dd");
                string rndt = (((Convert.ToInt64(dtt) * 11) + 17) * 19).ToString();
                int fc = Rd.Next(1, 9);

                builder.Append(fc.ToString());

                for (var i = 0; i < fc * 10 - 1; i++)
                {
                    builder.Append(Rd.Next(0, 9).ToString());
                }
                foreach (char c in rndt)
                {
                    builder.Append(c);
                }
                for (int i = 0; i < 100 - fc * 10; i++)
                {
                    builder.Append(Rd.Next(0, 9).ToString());
                }
                return builder.ToString();
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return String.Empty;
            }
            
        }
        public static DateTime DeComplicationSd(string ct)
        {
            try
            {
                int fc = Convert.ToInt32(ct.Substring(0,1));
                Int64 cd =  Convert.ToInt64( ct.Substring(fc * 10, 10) );
                Int64 v = (((cd / 19) - 17) / 11);
                string vy = v.ToString().Substring(0, 4);
                string vm = v.ToString().Substring(4, 2);
                string vd = v.ToString().Substring(6, 2);
                return DateTime.Parse($"{vy}-{vm}-{vd}");
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime GetDateTime(out string msg)
        {
            msg = "";
            DateTime dateTime = DateTime.MinValue;
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = GlobalVars.SetCulture();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.com/");
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //string todaysDates = response.Headers["Date"]; todaysDates = todaysDates.Substring(5, 11);
                    
                    //dateTime = Convert.ToDateTime(todaysDates);
                    //dateTime = dateTime.AddHours(3);
                    dateTime = response.LastModified;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return dateTime;
        }
    }
}
