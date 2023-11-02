using Models.Common;
using SgkAssistant.Forms.Common;
using SgkAssistant.Properties;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

/*
 * Check for update example.
 * Copyright: mech
 * http://themech.net/2008/05/adding-check-for-update-option-in-csharp/
 * http://themech.net/2008/09/check-for-updates-how-to-download-and-install-a-new-version-of-your-csharp-application/
 */

namespace SgkAssistant.Helpers
{
    // this struct will contain the info from the xml file
    public struct DownloadedVersionInfo
    {
        public bool Error;
        public Version LatestVersion;
        public string InstallerUrl;
        public string Cguid;
        public string Vd;
    }

    // this will contain info about the downloaded installer
    public struct DownloadInstallerInfo
    {
        public bool Error;
        public string Path;
    }

    // delegates (will forward the request to our Form1)
    // this of course could be done in a better (more flexible) way
    public delegate bool DelegateCheckForUpdateFinished(DownloadedVersionInfo versionInfo);
    public delegate void DelegateDownloadInstallerFinished(DownloadInstallerInfo info);
    public delegate void DelegateDownloadedBytes(long br, int cl);

    class CheckForUpdate
    {
        private readonly FAbout mainApp;
        private readonly FSplash splashApp;
        DownloadedVersionInfo dvInfo;
        bool download;
        Thread mWorkerThread;
        readonly ManualResetEvent mEventStopThread;
        readonly ManualResetEvent mEventThreadStopped;

        private int contentLength = 0;
        private int bytesRead = 0;

        public CheckForUpdate(FAbout mainApp)
        {
            this.mainApp = mainApp;
            mEventStopThread = new ManualResetEvent(false);
            mEventThreadStopped = new ManualResetEvent(false);
        }
        public CheckForUpdate(FSplash splashApp)
        {
            this.splashApp = splashApp;
            mEventStopThread = new ManualResetEvent(false);
            mEventThreadStopped = new ManualResetEvent(false);
        }

        // start the check for update process (if it is not already running)
        public void OnCheckForUpdate()
        {
            if ((this.mWorkerThread != null) && (this.mWorkerThread.IsAlive)) return;
            mWorkerThread = new Thread(this.CheckForUpdateFunction);
            mEventStopThread.Reset();
            mEventThreadStopped.Reset();
            mWorkerThread.Start();
        }

        public void OnGetNewVersion()
        {
            if ((this.mWorkerThread != null) && (this.mWorkerThread.IsAlive)) return;
            mWorkerThread = new Thread(this.GetNewVersionFunction);
            mEventStopThread.Reset();
            mEventThreadStopped.Reset();
            mWorkerThread.Start();
        }

        // when the worker thread is running - let it know it should stop
        public void StopThread()
        {
            if ((this.mWorkerThread != null) && this.mWorkerThread.IsAlive)
            {
                mEventStopThread.Set();
                while (mWorkerThread.IsAlive)
                {
                    if (WaitHandle.WaitAll(
                        (new ManualResetEvent[] { mEventThreadStopped }),
                        100,
                        true))
                    {
                        break;
                    }
                    Application.DoEvents();
                }
            }
        }

        // internal method - return true when the thread is supposed to stop
        public bool StopWorkerThread()
        {
            if (mEventStopThread.WaitOne(0, true))
            {
                mEventThreadStopped.Set();
                return true;
            }
            return false;
        }

        // this is run in a thread. do the whole updating process:
        // - check for the new version (downloading the xml file)
        // - download the installer
        // the communication with the Form is done with the delegates

        private void CheckForUpdateFunction()
        {
            string msg = "";
            dvInfo = new DownloadedVersionInfo();
            dvInfo.Error = true;
            dvInfo.InstallerUrl = "";
            dvInfo.Cguid = "";
            dvInfo.Vd = "";
            try
            {
                if (StopWorkerThread()) return;
                Surum sr = IOC.PkcData.GetNewVersion(out msg);

                if (sr.V != "error")
                {
                    dvInfo.Error = false;
                    dvInfo.LatestVersion = new Version(sr.V); 
                    dvInfo.Cguid = sr.Cguid;
                    dvInfo.InstallerUrl = sr.Url;
                    dvInfo.Vd = sr.Vd;
                }
            }
            catch (Exception)
            {
            }
            if (StopWorkerThread()) return;
            if (this.mainApp != null)
            {
                download = (bool)this.mainApp.Invoke(new DelegateCheckForUpdateFinished(mainApp.OnCheckForUpdateFinished), new Object[] { dvInfo });
            }
            else if (this.splashApp != null)
            {
                Version curVer = Assembly.GetExecutingAssembly().GetName().Version;
                if ((dvInfo.Error) || (dvInfo.InstallerUrl.Length == 0) || (dvInfo.LatestVersion == null))
                {
                    GlobalVars.NewVersionfound = false; 
                }
                else if (curVer.CompareTo(dvInfo.LatestVersion) >= 0)
                {
                    GlobalVars.NewVersionfound = false;
                }
                else { GlobalVars.NewVersionfound = true; }
                
            }
            Settings.Default.lastUpdateCheckDate = DateTime.Now;
            Settings.Default.Save();
        }

        public void GetNewVersionFunction()
        {
            if (!download ) return;

            // download and let the main thread know
            DownloadInstallerInfo dii = new DownloadInstallerInfo();
            dii.Error = true;
            string filepath = "";
            try
            {
                WebRequest request = WebRequest.Create(dvInfo.InstallerUrl);
                WebResponse response = request.GetResponse(); 
                string filename = "";
                contentLength = 0;
                for (int a = 0; a < response.Headers.Count; a++)
                {
                    try
                    {
                        string val = response.Headers.Get(a);

                        switch (response.Headers.GetKey(a).ToLower())
                        {
                            case "content-length":
                                contentLength = Convert.ToInt32(val);
                                break;
                            case "content-disposition":
                                string[] v2 = val.Split(';');
                                foreach (string s2 in v2)
                                {
                                    string[] v3 = s2.Split('=');
                                    if (v3.Length == 2)
                                    {
                                        if (v3[0].Trim().ToLower() == "filename")
                                        {
                                            char[] sss = { ' ', '"' };
                                            filename = v3[1].Trim(sss);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    catch (Exception) { };
                }
                if (StopWorkerThread()) return;
                if (filename.Length == 0) filename = "SgkInstaller.msi";
                filepath = Path.Combine(Path.GetTempPath(), filename);

                if (File.Exists(filepath))
                {
                    try
                    {
                        File.Delete(filepath);
                    }
                    catch
                    {
                    }
                    if (File.Exists(filepath))
                    {
                        string rname = Path.GetRandomFileName();
                        rname.Replace('.', '_');
                        rname += ".msi";
                        filepath = Path.Combine(Path.GetTempPath(), rname);
                    }
                }
                Stream stream = response.GetResponseStream(); 
                int pos = 0;
                byte[] buf2 = new byte[8192];
                FileStream fs = new FileStream(filepath, FileMode.CreateNew);
                while ((0 == contentLength) || (pos < contentLength))
                {
                    int maxBytes = 8192;
                    if ((0 != contentLength) && ((pos + maxBytes) > contentLength)) maxBytes = contentLength - pos;
                    bytesRead = stream.Read(buf2, 0, maxBytes);
                    if (bytesRead <= 0) break;
                    fs.Write(buf2, 0, bytesRead); 
                    if (StopWorkerThread()) return;
                    pos += bytesRead;
                    this.mainApp.BeginInvoke(new DelegateDownloadedBytes(mainApp.DownloadedBytes), new object[] {fs.Length ,contentLength });
                }
                fs.Close();
                stream.Close();
                dii.Error = false;
                dii.Path = filepath;
            }
            catch
            {
                // when something goes wrong - at least do the cleanup :)
                if (filepath.Length > 0)
                {
                    try
                    {
                        File.Delete(filepath);
                    }
                    catch
                    {
                    }
                }
            }
            if (StopWorkerThread()) return;
            this.mainApp.BeginInvoke(new DelegateDownloadInstallerFinished(mainApp.OnDownloadInstallerinished), new Object[] { dii });
        }

    }
}
