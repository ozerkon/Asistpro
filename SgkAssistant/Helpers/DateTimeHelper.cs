using Models.Common;
using System.ComponentModel;

namespace SgkAssistant.Helpers
{
    public static class DateTimeHelper
    {
        private static BackgroundWorker _bgwSearch;
        public static string Msg = "";
        static DateTimeHelper()
        {
            _bgwSearch = new BackgroundWorker();
            _bgwSearch.DoWork += new DoWorkEventHandler(BgwSearchDoWork);
            _bgwSearch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgwSearchComplated);
            if (!_bgwSearch.IsBusy)
            {
                _bgwSearch.RunWorkerAsync();
            }
        }

        private static void BgwSearchComplated(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Msg = $"Hata: {e.Error.Message}";
            }
            else
            {
                Msg = "";
            }
        }

        private static void BgwSearchDoWork(object sender, DoWorkEventArgs e)
        {
            GetDateTime();
        }

        public static void GetDateTime()
        {
            GlobalVars.Ct = NtpClient.GetNetworkTime(out Msg);
        }

    }
}
