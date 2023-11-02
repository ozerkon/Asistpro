using Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGKServices.Captcha
{
    public interface ICaptchaSolver
    {
        string Solve(out string msg);
        void SetApiKey(string token);
    }
    public class CpiSolver : ICaptchaSolver
    {
        private string accessToken = GlobalVars.SolverKey; 
        public string ImagePath { get; set; }
        public CpiSolver(string token)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            if (token.Length > 0)
                this.accessToken = token;
        }
        public void SetApiKey(string token)
        {
            if (token.Length > 0)
                this.accessToken = token;
        }
        public string Solve(out string msg)
        {
            msg = "";
            try
            {
                ImageTypers.ImageTypersAPI i = new ImageTypers.ImageTypersAPI(accessToken);

                Dictionary<string, string> imageParams = new Dictionary<string, string>();
                imageParams.Add("iscase", "true");         // case sensitive captcha
                imageParams.Add("isphrase", "true");       // text contains at least one space (phrase)
                imageParams.Add("ismath", "true");         // instructs worker that a math captcha has to be solved
                imageParams.Add("alphanumeric", "1");      // 1 - digits only, 2 - letters only
                imageParams.Add("minlength", "2");         // captcha text length (minimum)
                imageParams.Add("maxlength", "5");         // captcha text length (maximum)

                string captchaText = i.solve_captcha(SearchReport.LastCaptchaPath);
                return captchaText;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
    }

    public class OcrApiSolver : ICaptchaSolver
    {
        private string accessToken = GlobalVars.SolverKey;
        public string ImagePath { get; set; }
        public OcrApiSolver(string token)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            if (token.Length > 0)
                this.accessToken = token;
        }
        public void SetApiKey(string token)
        {
            if (token.Length > 0)
                this.accessToken = token;
        }
        public string Solve(out string msg) 
        {
            
            msg = "";
            string res = SolveAsync().Result;
            if (res.Contains("hata: ") || res == null || res == "")
            {
                return null;
            }
            else
            {
                return res;
            }
        }
        public async Task<string> SolveAsync()
        {
            //GlobalVars.ocrEngine = GlobalVars.ocrEngine == 4 ? 1 : GlobalVars.ocrEngine;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 0, 10);
                MultipartFormDataContent form = new MultipartFormDataContent();
                byte[] imageData = File.ReadAllBytes(SearchReport.LastCaptchaPath);

                form.Add(new StringContent(accessToken), "apikey"); //Added api key in form data
                form.Add(new StringContent("eng"), "language");
                form.Add(new StringContent(GlobalVars.OcrEngine.ToString()), "ocrengine");
                form.Add(new StringContent("true"), "scale");
                form.Add(new StringContent("true"), "istable");
                form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", "image.jpg");

                HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);
                string strContent = await response.Content.ReadAsStringAsync();
                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(strContent);
                string captchaText = "";

                if (ocrResult.OcrExitCode == 1)
                {
                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        captchaText += ocrResult.ParsedResults[i].ParsedText;
                    }
                }
                else
                {
                    captchaText = $"Hata: {strContent}";
                }
                httpClient.Dispose();
                captchaText = captchaText.Replace("\t\r\n", "");
                return captchaText;
            }
            catch (Exception ex)
            {
                if (GlobalVars.OcrEngine <= 3)
                {
                    GlobalVars.OcrEngine++;
                    await SolveAsync();
                }
                return $"hata: {ex.Message}";
            }
        }
    }

    public class Rootobject
    {
        public Parsedresult[] ParsedResults { get; set; }
        public int OcrExitCode { get; set; }
        public bool IsErroredOnProcessing { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }

    public class Parsedresult
    {
        public object FileParseExitCode { get; set; }
        public string ParsedText { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }

    public static class SolverFactory
    {
        private static ICaptchaSolver _solver;
        public static ICaptchaSolver CreateSolver(byte solverType)
        {
            switch (solverType)
            {
                case 1:
                   _solver = new CpiSolver(GlobalVars.SolverKey);
                    return _solver;
                default:
                    _solver = new OcrApiSolver(GlobalVars.SolverKey);
                    return _solver;
                    
            }
        }
    }
}
