using GridViewCustomCells;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using Models.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using SgkAssistant.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverX;

namespace SgkAssistant.Helpers
{
    public static class CheckBoxFactory
    {
        public static RadCheckBox Create(string text, int isChecked, int isActive)
        {

            RadCheckBox rcb = new RadCheckBox
            {
                Text = text,
                Checked = Convert.ToBoolean(isChecked),
                Enabled = Convert.ToBoolean(isActive)
            };
            return rcb;

        }
    }
    public class WinHelpers
    {
        private static string _driverLocation = "";
        public bool SetCompanyListRgv(RadGridView source, out string msg)
        {
            
            try
            {
                GlobalVars.Companies = IOC.CompanyDataService.GetCompanies(out msg);
                if (GlobalVars.Companies == null)
                {
                    return false;
                }
                string sgscEncDb = IOC.CompanyDataService.GetSgscEnc(out msg);
                string sgscEncCm = GetSgscEncFromCompanies();
                //if (sgscEncDb == null || sgscEncCm != sgscEncDb /* || sgscEncDB != GlobalVars.sgscEnc */ )
                //{
                //    msg = "Veritabanında manuel değişiklik algılandı, lütfen orijinal veritabınını yerine koyun";
                //    return false;
                //}
                source.DataSource = GlobalVars.Companies;
                int i = 0;
                source.Columns[i++].HeaderText = "Firma ID";
                source.Columns[i++].HeaderText = "Firma Adı";
                source.Columns[i++].HeaderText = "SGK Kullanıcı Adı";
                source.Columns[i++].HeaderText = "SGK Kullanıcı Kodu";
                source.Columns[i++].HeaderText = "SGK Sistem Şifresi";
                source.Columns[i++].HeaderText = "SGK İşyeri Şifresi";
                source.Columns[i++].HeaderText = "Firma Merkezi";
                source.Columns[i++].HeaderText = "GİB Kullanıcı Adı";
                source.Columns[i++].HeaderText = "GİB Parola";
                source.Columns[i++].HeaderText = "GİB Şifre";
                source.Columns[i++].HeaderText = "Firma Sicil No";
                source.Columns[i++].HeaderText = "Unvan";
                source.Columns[i++].HeaderText = "Adres";
                source.Columns[i++].HeaderText = "Bağlı Olduğu SGM";
                source.Columns[i++].HeaderText = "Kanun Kapsamına Alınış";
                source.Columns[i++].HeaderText = "Kanun Kapsamından Çıkış";
                source.Columns[i++].HeaderText = "Özel Kod 1";
                source.Columns[i++].HeaderText = "Özel Kod 2";
                source.Columns[i++].HeaderText = "Özel Kod 3";
                source.Columns[i++].HeaderText = "Özel Kod 4";
                source.Columns[i++].HeaderText = "Özel Kod 5";
                source.Columns[i++].HeaderText = "Ekleyen";
                source.Columns[i++].HeaderText = "Ekleme Tarihi";

                if (source.Columns[0].Name == "Id")
                {
                    GridViewDecimalColumn decimalColumn = new GridViewDecimalColumn();
                    decimalColumn.Name = "sira";
                    decimalColumn.HeaderText = "No";
                    decimalColumn.DecimalPlaces = 0;
                    source.Columns.Add(decimalColumn); // 23 
                    //source.Columns.RemoveAt(0);
                    //source.Columns.Move(22, 0);
                    source.Columns.Move(23, 0);
                    source.Columns.Move(1, 23);

                }

                for (int j = 0; j < source.RowCount; j++)
                {
                    source.Rows[j].Cells["Sira"].Value = j + 1;
                }
                HideColumns(source);
                source.Columns[0].MaxWidth = 60;
                source.Columns[0].MinWidth = 60;
                source.Columns[1].MinWidth = 200;
                source.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            return true;
        }
        public void SetUserListRgv(RadGridView source, out string msg)
        {
            msg = "";
            try
            {
                source.DataSource = IOC.UserDataService.GetAllUsers(out msg);
                int i = 0;
                source.Columns[i++].HeaderText = "Id";
                source.Columns[i++].HeaderText = "Adı";
                source.Columns[i++].HeaderText = "Soyadı";
                source.Columns[i++].HeaderText = "Kullanıcı Adı";
                source.Columns[i++].HeaderText = "Şifre";
                source.Columns[i++].HeaderText = "Şifre Tekrar";
                source.Columns[i++].HeaderText = "Yetki";
                source.Columns[i++].HeaderText = "Eklenme Tarihi";
                source.Columns[i++].HeaderText = "Özel Kod";
                source.Columns[i++].HeaderText = "Ekleyen";
                for (int j = 0; j < source.ColumnCount; j++)
                {
                    if (j == 1 || j == 2 || j == 3 || j == 6) { continue; }
                    source.Columns[j].IsVisible = false;

                }
                foreach (var item in source.Rows)
                {
                    string uy = item.Cells["Uy"].Value.ToString();
                    item.Cells["Uy"].Value = uy == "Y" ? "YÖNETİCİ" : "KULLANICI";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        public string GetSgscEncFromCompanies()
        {
            string sgscEnc = GlobalVars.CmpCheck;
            foreach (Company comp in GlobalVars.Companies)
            {
                sgscEnc += Encrypt.EncryptString(comp.Sgsc, Settings.Default.discid);
            }
            return sgscEnc;
        }
        public void ResetSgsc(out string msg)
        {
            GlobalVars.Companies = IOC.CompanyDataService.GetCompanies(out msg);
            GlobalVars.SgscEnc = GetSgscEncFromCompanies();
            IOC.CompanyDataService.UpdateSgscEnc(GlobalVars.SgscEnc, out msg);
            Settings.Default.sgscEnc = GlobalVars.SgscEnc;
            Settings.Default.Save();

        }
        public void AddSgscEnc(string sgsc, out string msg)
        {
            string sgscEnc = Encrypt.EncryptString(sgsc, Settings.Default.discid);
            IOC.CompanyDataService.AddSgscEnc(sgscEnc, out msg);
            Settings.Default.sgscEnc += sgscEnc; Settings.Default.Save();
            GlobalVars.SgscEnc += sgscEnc;
        }
        public Company EncryptCompany(Company c)
        {
            c.CompanyId = Encrypt.EncryptString(c.CompanyId, Settings.Default.passPhrase);
            c.CompanyId2 = Encrypt.EncryptString(c.CompanyId2, Settings.Default.passPhrase);
            c.SystemPassword = Encrypt.EncryptString(c.SystemPassword, Settings.Default.passPhrase);
            c.CompanyPassword = Encrypt.EncryptString(c.CompanyPassword, Settings.Default.passPhrase);
            c.Gun = Encrypt.EncryptString(c.Gun, Settings.Default.passPhrase);
            c.Gs = Encrypt.EncryptString(c.Gs, Settings.Default.passPhrase);
            c.Gp = Encrypt.EncryptString(c.Gp, Settings.Default.passPhrase);
            return c;
        }
        public Company DecryptCompany(Company c)
        {
            c.CompanyId = Encrypt.DecryptString(c.CompanyId, Settings.Default.passPhrase);
            c.CompanyId2 = Encrypt.DecryptString(c.CompanyId2, Settings.Default.passPhrase);
            c.SystemPassword = Encrypt.DecryptString(c.SystemPassword, Settings.Default.passPhrase);
            c.CompanyPassword = Encrypt.DecryptString(c.CompanyPassword, Settings.Default.passPhrase);
            c.Gun = Encrypt.DecryptString(c.Gun, Settings.Default.passPhrase);
            c.Gs = Encrypt.DecryptString(c.Gs, Settings.Default.passPhrase);
            c.Gp = Encrypt.DecryptString(c.Gp, Settings.Default.passPhrase);
            return c;
        }
        public void FillRgvWithLinks(RadGridView source)
        {
            string msg = "";
            try
            {
                int i = 0;
                if (LinkGlobals.LstLinks == null || LinkGlobals.LstLinks.Count == 0)
                {
                    LinkGlobals.LstLinks = IOC.LinksDataService.GetAllLinks(out msg);
                }
                if (LinkGlobals.LstLinks[0].Id != 1) IOC.LinksDataService.ResetIdColumn(out msg);
                source.DataSource = LinkGlobals.LstLinks;
                source.Columns[1].Name = "No"; source.Columns[i++].HeaderText = "Sıra No";             // 0
                source.Columns[1].Name = "Grp"; source.Columns[i++].HeaderText = "Ana Grup";            // 1
                source.Columns[2].Name = "Sgrp"; source.Columns[i++].HeaderText = "Alt Grup";            // 2
                source.Columns[i++].HeaderText = "Sürüm";               // 3 hide this
                source.Columns[4].Name = "Ad"; source.Columns[i++].HeaderText = "Başlık";              // 4
                source.Columns[i++].HeaderText = "Adres";               // 5 hide this
                source.Columns[i++].HeaderText = "Doğrulama Url";       // 6 hide this
                source.Columns[i++].HeaderText = "Açıklama";            // 7 hide this
                source.Columns[i++].HeaderText = "Anahtar Kelimeler";   // 8 hide this
                source.Columns[i++].HeaderText = "Komutlar";            // 9 hide this
                source.Columns[i++].HeaderText = "Sık Kullanılan";      // 10
                GridViewCheckBoxColumn cbCol = AddCheckBoxColumnToLinkRgv();
                cbCol.Name = "AddRemoveFavs";
                cbCol.HeaderText = "Favori";
                cbCol.MaxWidth = 100;
                cbCol.EnableHeaderCheckBox = false;
                source.Columns.Add(cbCol);

                foreach (var item in source.Columns)
                {
                    item.ReadOnly = true;
                    if (item.Index == 0 || item.Index == 1 || item.Index == 2 || item.Index == 4 || item.Index == 11) { continue; }
                    item.IsVisible = false;
                }
                source.Columns[11].ReadOnly = false;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }

        }
        public void HideColumns(RadGridView source)
        {
            int j = 0; UserPrm up = GlobalVars.UserPrm; bool au = Convert.ToBoolean(Users.ActiveUser.Yetki);
            if (up == null) { up = new UserPrm(); }
            source.Columns[j].VisibleInColumnChooser = true; source.Columns[j++].IsVisible = Convert.ToBoolean(Settings.Default.sira);
            source.Columns[j].VisibleInColumnChooser = false; source.Columns[j].AllowHide = false; source.Columns[j++].IsVisible = true;
            source.Columns[j].VisibleInColumnChooser = Convert.ToBoolean(up.Cid); source.Columns[j++].IsVisible = au ? Convert.ToBoolean(Settings.Default.cid) : false;
            source.Columns[j].VisibleInColumnChooser = Convert.ToBoolean(up.Cid2); source.Columns[j++].IsVisible = au ? Convert.ToBoolean(Settings.Default.cid2) : false;
            source.Columns[j].VisibleInColumnChooser = Convert.ToBoolean(up.Sp); source.Columns[j++].IsVisible = au ? Convert.ToBoolean(Settings.Default.sp) : false;
            source.Columns[j].VisibleInColumnChooser = Convert.ToBoolean(up.Cp); source.Columns[j++].IsVisible = au ? Convert.ToBoolean(Settings.Default.cp) : false;
            source.Columns[j].VisibleInColumnChooser = false; source.Columns[j++].IsVisible = false;
            source.Columns[j].VisibleInColumnChooser = Convert.ToBoolean(up.Gun); source.Columns[j++].IsVisible = au ? Convert.ToBoolean(Settings.Default.gun) : false;
            source.Columns[j].VisibleInColumnChooser = Convert.ToBoolean(up.Gp); source.Columns[j++].IsVisible = au ? Convert.ToBoolean(Settings.Default.gp) : false;
            source.Columns[j].VisibleInColumnChooser = Convert.ToBoolean(up.Gs); source.Columns[j++].IsVisible = au ? Convert.ToBoolean(Settings.Default.gs) : false;
            source.Columns[j].VisibleInColumnChooser = false; ; source.Columns[j++].IsVisible = false;
            source.Columns[j].VisibleInColumnChooser = false; ; source.Columns[j++].IsVisible = false;
            source.Columns[j].VisibleInColumnChooser = false; ; source.Columns[j++].IsVisible = false;
            source.Columns[j].VisibleInColumnChooser = false; ; source.Columns[j++].IsVisible = false;
            source.Columns[j].VisibleInColumnChooser = false; ; source.Columns[j++].IsVisible = false;
            source.Columns[j].VisibleInColumnChooser = false; ; source.Columns[j++].IsVisible = false;
            source.Columns[j].VisibleInColumnChooser = true; source.Columns[j++].IsVisible = Convert.ToBoolean(Settings.Default.scd1);
            source.Columns[j].VisibleInColumnChooser = true; source.Columns[j++].IsVisible = Convert.ToBoolean(Settings.Default.scd2);
            source.Columns[j].VisibleInColumnChooser = true; source.Columns[j++].IsVisible = Convert.ToBoolean(Settings.Default.scd3);
            source.Columns[j].VisibleInColumnChooser = true; source.Columns[j++].IsVisible = Convert.ToBoolean(Settings.Default.scd4);
            source.Columns[j].VisibleInColumnChooser = true; source.Columns[j++].IsVisible = Convert.ToBoolean(Settings.Default.scd5);
            source.Columns[j].VisibleInColumnChooser = false; ; source.Columns[j++].IsVisible = false;
            source.Columns[j].VisibleInColumnChooser = false; ; source.Columns[j++].IsVisible = false;
            source.Columns[j].VisibleInColumnChooser = false; ; source.Columns[j++].IsVisible = false;
        }
        public bool IsTcnoExist(RadGridView source, string tcno, out string msg)
        {
            msg = "";
            try
            {
                foreach (GridViewRowInfo row in source.Rows)
                {
                    if (tcno == row.Cells[0].Value.ToString())
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            return false;
        }
        public bool IsTcnoValid(string tcno, out string msg)
        {
            msg = "";
            SearchReport.KimlikNo = tcno;
            Regex rx = new Regex(@"[0-9]{1}[0-9]{9}[02468]{1}");
            if (tcno.Trim() == string.Empty)
            {
                msg = "Kimlik numarası giriniz"; return false;
            }
            else if (!rx.IsMatch(SearchReport.KimlikNo) || SearchReport.KimlikNo.Length > 11)
            {
                msg = "Geçersiz kimlik numarası";  return false;
            }
            return true;
        }
        public bool IsNameValid(string name, out string msg)
        {
            msg = "";
            Regex rx = new Regex(@"[a-zA-Z ğüşıöçĞÜŞİÖÇ]{5,50}$");
            if (!rx.IsMatch(name) )
            {
                msg = "Geçersiz ad soyad"; return false;
            }
            return true;
        }
        public Users GetUserFromRgv(RadGridView source, out string msg)
        {
            msg = "";
            Users u = new Users();
            try
            {
                u.Id = Convert.ToInt32(source.CurrentRow.Cells[0].Value);
                u.Unm = source.CurrentRow.Cells[1].Value.ToString();
                u.Uln = source.CurrentRow.Cells[2].Value.ToString();
                u.Un = source.CurrentRow.Cells[3].Value.ToString();
                u.Up = IOC.UserDataService.DecPass(source.CurrentRow.Cells[4].Value.ToString());
                u.Upp = IOC.UserDataService.DecPass(source.CurrentRow.Cells[5].Value.ToString());
                u.Uy = source.CurrentRow.Cells[6].Value.ToString();
                u.Cd = DateTime.Parse(source.CurrentRow.Cells[7].Value.ToString());
                u.Hc = source.CurrentRow.Cells[8].Value.ToString();
                u.Cu = Convert.ToInt32(source.CurrentRow.Cells[9].Value);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            return u;
        }

        public Company GetCompanyFromRgv(RadGridView source, out string msg)
        {
            msg = ""; int i = 1;

            Company comp = new Company();

            try
            {
                comp.CompanyName = source.CurrentRow.Cells[i++].Value.ToString();
                comp.CompanyId = source.CurrentRow.Cells[i++].Value.ToString();
                comp.CompanyId2 = source.CurrentRow.Cells[i++].Value.ToString();
                comp.SystemPassword = source.CurrentRow.Cells[i++].Value.ToString();
                comp.CompanyPassword = source.CurrentRow.Cells[i++].Value.ToString();
                comp.Fm = (source.CurrentRow.Cells[6].Value != null) ? Convert.ToInt32(source.CurrentRow.Cells[i++].Value) : 1;
                comp.Gun = (source.CurrentRow.Cells[7].Value != null) ? source.CurrentRow.Cells[i++].Value.ToString() : String.Empty;
                comp.Gp = (source.CurrentRow.Cells[8].Value != null) ? source.CurrentRow.Cells[i++].Value.ToString() : String.Empty;
                comp.Gs = (source.CurrentRow.Cells[9].Value != null) ? source.CurrentRow.Cells[i++].Value.ToString() : String.Empty;
                comp.Sgsc = (source.CurrentRow.Cells[10].Value != null) ? source.CurrentRow.Cells[i++].Value.ToString() : String.Empty;
                comp.Unvan = (source.CurrentRow.Cells[11].Value != null) ? source.CurrentRow.Cells[i++].Value.ToString() : String.Empty;
                comp.Adres = (source.CurrentRow.Cells[12].Value != null) ? source.CurrentRow.Cells[i++].Value.ToString() : String.Empty;
                comp.Sgm = (source.CurrentRow.Cells[13].Value != null) ? source.CurrentRow.Cells[i++].Value.ToString() : String.Empty;
                comp.Kka = DateTime.Parse(source.CurrentRow.Cells[i++].Value.ToString());
                comp.Kkc = DateTime.Parse(source.CurrentRow.Cells[i++].Value.ToString());
                comp.Sc1 = (source.CurrentRow.Cells[16].Value != null) ? source.CurrentRow.Cells[i++].Value.ToString() : String.Empty; i++;
                comp.Sc2 = (source.CurrentRow.Cells[17].Value != null) ? source.CurrentRow.Cells[i++].Value.ToString() : String.Empty; i++;
                comp.Sc3 = (source.CurrentRow.Cells[18].Value != null) ? source.CurrentRow.Cells[i++].Value.ToString() : String.Empty; i++;
                comp.Sc4 = (source.CurrentRow.Cells[19].Value != null) ? source.CurrentRow.Cells[i++].Value.ToString() : String.Empty; i++;
                comp.Sc5 = (source.CurrentRow.Cells[20].Value != null) ? source.CurrentRow.Cells[i++].Value.ToString() : String.Empty; i++;
                comp.Cu = Convert.ToInt32(source.CurrentRow.Cells[i++].Value);
                comp.Cd = DateTime.Parse(source.CurrentRow.Cells[i++].Value.ToString());
                comp.Id = Convert.ToInt32(source.CurrentRow.Cells[i++].Value);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            return comp;
        }

        public Company GetLoginFromReports(RadGridView source, out string msg)
        {
            Company l;
            string companyName, companyRegNo;
            msg = "";
            try
            {
                companyName = source.CurrentRow.Cells[1].Value.ToString();
                if (source.CurrentRow.Cells[15].Value.ToString() != string.Empty)
                {
                    companyRegNo = source.CurrentRow.Cells[15].Value.ToString();
                }
                else
                {
                    companyRegNo = string.Empty;
                }

                l = IOC.CompanyDataService.GetCompanyByNameAndRegNo(companyName, companyRegNo, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return l;
        }

        public List<Company> GetSelectedCompaniesFromRgv(RadGridView source, out string msg)
        {
            msg = "";
            List<Company> lst = new List<Company>();
            try
            {
                foreach (GridViewRowInfo row in source.SelectedRows)
                {
                    Company login = new Company(); int i = 1;

                    login.CompanyName = row.Cells[i++].Value.ToString();
                    login.CompanyId = row.Cells[i++].Value.ToString();
                    login.CompanyId2 = row.Cells[i++].Value.ToString();
                    login.SystemPassword = row.Cells[i++].Value.ToString();
                    login.CompanyPassword = row.Cells[i++].Value.ToString();
                    login.Fm = Convert.ToInt32(row.Cells[i++].Value);
                    var x = row.Cells[i++].Value;
                    login.Gun = (x == null) ? "" : x.ToString();
                    x = row.Cells[i++].Value;
                    login.Gp = (x == null) ? "" : x.ToString();
                    x = row.Cells[i++].Value;
                    login.Gs = (x == null) ? "" : x.ToString();
                    x = row.Cells[i++].Value;
                    login.Sgsc = (x == null) ? "" : x.ToString();
                    x = row.Cells[i++].Value;
                    login.Unvan = (x == null) ? "" : x.ToString();
                    x = row.Cells[i++].Value;
                    login.Adres = (x == null) ? "" : x.ToString();
                    x = row.Cells[i++].Value;
                    login.Sgm = (x == null) ? "" : x.ToString();
                    x = row.Cells[i++].Value;
                    login.Kka = (x == null) ? DateTime.MinValue : DateTime.Parse(x.ToString());
                    x = row.Cells[i++].Value;
                    login.Kkc = (x == null) ? DateTime.MinValue : DateTime.Parse(x.ToString());
                    x = row.Cells[i++].Value;
                    login.Sc1 = (x == null) ? "" : x.ToString();
                    x = row.Cells[i++].Value;
                    login.Sc2 = (x == null) ? "" : x.ToString();
                    x = row.Cells[i++].Value;
                    login.Sc3 = (x == null) ? "" : x.ToString();
                    x = row.Cells[i++].Value;
                    login.Sc4 = (x == null) ? "" : x.ToString();
                    x = row.Cells[i++].Value;
                    login.Sc5 = (x == null) ? "" : x.ToString();
                    x = row.Cells[i++].Value;
                    login.Cu = (x == null) ? 1 : Convert.ToInt32(x);
                    x = row.Cells[i++].Value;
                    login.Cd = (x == null) ? DateTime.MinValue : DateTime.Parse(x.ToString());
                    login.Id = Convert.ToInt32(row.Cells[i++].Value);
                    lst.Add(login);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return lst;
        }

        public List<int> GetSelectedCompaniesIDs(RadGridView source, out string msg)
        {
            msg = "";
            List<int> lst = new List<int>();
            try
            {
                foreach (GridViewRowInfo row in source.ChildRows)
                {
                    if (row.IsSelected) lst.Add(Convert.ToInt32(row.Cells[23].Value));
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return lst;
        }

        public List<Company> GetAllCompaniesFromRgv(RadGridView source, out string msg)
        {
            msg = "";
            List<Company> lst = new List<Company>();
            try
            {
                if (source.RowCount > 0)
                {

                    foreach (GridViewRowInfo row in source.Rows)
                    {
                        Company login = new Company(); int i = 1;

                        login.CompanyName = row.Cells[i++].Value.ToString();
                        login.CompanyId = row.Cells[i++].Value.ToString();
                        login.CompanyId2 = row.Cells[i++].Value.ToString();
                        login.SystemPassword = row.Cells[i++].Value.ToString();
                        login.CompanyPassword = row.Cells[i++].Value.ToString();
                        login.Fm = Convert.ToInt32(row.Cells[i++].Value);
                        var x = row.Cells[i++].Value;
                        login.Gun = (x == null) ? "" : x.ToString();
                        x = row.Cells[i++].Value;
                        login.Gp = (x == null) ? "" : x.ToString();
                        x = row.Cells[i++].Value;
                        login.Gs = (x == null) ? "" : x.ToString();
                        x = row.Cells[i++].Value;
                        login.Sgsc = (x == null) ? "" : x.ToString();
                        x = row.Cells[i++].Value;
                        login.Unvan = (x == null) ? "" : x.ToString();
                        x = row.Cells[i++].Value;
                        login.Adres = (x == null) ? "" : x.ToString();
                        x = row.Cells[i++].Value;
                        login.Sgm = (x == null) ? "" : x.ToString();
                        x = row.Cells[i++].Value;
                        login.Kka = (x == null) ? DateTime.MinValue : DateTime.Parse(x.ToString());
                        x = row.Cells[i++].Value;
                        login.Kkc = (x == null) ? DateTime.MinValue : DateTime.Parse(x.ToString());
                        x = row.Cells[i++].Value;
                        login.Sc1 = (x == null) ? "" : x.ToString();
                        x = row.Cells[i++].Value;
                        login.Sc2 = (x == null) ? "" : x.ToString();
                        x = row.Cells[i++].Value;
                        login.Sc3 = (x == null) ? "" : x.ToString();
                        x = row.Cells[i++].Value;
                        login.Sc4 = (x == null) ? "" : x.ToString();
                        x = row.Cells[i++].Value;
                        login.Sc5 = (x == null) ? "" : x.ToString();
                        x = row.Cells[i++].Value;
                        login.Cu = (x == null) ? 1 : Convert.ToInt32(x);
                        x = row.Cells[i++].Value;
                        login.Cd = (x == null) ? DateTime.MinValue : DateTime.Parse(x.ToString());
                        login.Id = Convert.ToInt32(row.Cells[i++].Value);
                        lst.Add(login);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return lst;
        }

        public List<ExcelComp> GetAllExcelCompaniesFromRgv(RadGridView source, out string msg)
        {
            msg = "";
            List<ExcelComp> lst = new List<ExcelComp>();
            try
            {
                if (source.RowCount > 0)
                {

                    foreach (GridViewRowInfo row in source.Rows)
                    {
                        ExcelComp ec = new ExcelComp(); int i = 0;

                        ec.CompanyName = row.Cells[i++].Value.ToString();
                        ec.CompanyId = row.Cells[i++].Value.ToString();
                        ec.CompanyId2 = row.Cells[i++].Value.ToString();
                        ec.SystemPassword = row.Cells[i++].Value.ToString();
                        ec.CompanyPassword = row.Cells[i++].Value.ToString();
                        ec.Gun = row.Cells[i++].Value.ToString();
                        ec.Gp = row.Cells[i++].Value.ToString();
                        ec.Gs = row.Cells[i++].Value.ToString();
                        ec.Sc1 = row.Cells[i++].Value.ToString();
                        ec.Sc2 = row.Cells[i++].Value.ToString();
                        ec.Sc3 = row.Cells[i++].Value.ToString();
                        ec.Sc4 = row.Cells[i++].Value.ToString();
                        ec.Sc5 = row.Cells[i++].Value.ToString();
                        lst.Add(ec);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return lst;
        }
        public List<ExcelPersonal> GetAllPersonalsFromRgv(RadGridView source, out string msg)
        {
            msg = "";
            List<ExcelPersonal> lst = new List<ExcelPersonal>();
            try
            {
                if (source.RowCount > 0)
                {
                    foreach (GridViewRowInfo row in source.Rows)
                    {
                        ExcelPersonal p = new ExcelPersonal(); int i = 0;

                        p.Tcno = row.Cells[i++].Value.ToString();
                        p.Ads = row.Cells[i++].Value.ToString();
                        p.Dtr = Convert.ToDateTime(row.Cells[i++].Value);
                        p.Igt = Convert.ToDateTime(row.Cells[i++].Value);
                        p.Tih = Convert.ToDecimal(row.Cells[i++].Value);
                        lst.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return lst;
        }
        public VisitsToBeProcessed GetVisitFromRgv(RadGridView source, out string msg)
        {
            msg = ""; string ws = "";
            VisitsToBeProcessed v = new VisitsToBeProcessed();
            try
            {
                v.Cnm = source.CurrentRow.Cells[1].Value.ToString();
                v.Tcno = source.CurrentRow.Cells[2].Value.ToString();
                v.AdSoyad = source.CurrentRow.Cells[3].Value.ToString();
                v.FirmaSicilNo = source.CurrentRow.Cells[15].Value.ToString();
                switch (SearchReport.ReportTypeAfterRgvLoad)
                {
                    case 1:
                    case 2:
                        ws = source.CurrentRow.Cells[22].Value.ToString();
                        v.WorkingStatus = (ws == "0") ? 0 : (ws == "1") ? 1 : 2;
                        v.Vaka = source.CurrentRow.Cells[4].Value.ToString();
                        v.ConfirmDate = source.CurrentRow.Cells[21].Value.ToString();
                        v.RaporTakipNo = source.CurrentRow.Cells[5].Value.ToString();
                        v.RaporSiraNo = Convert.ToInt32(source.CurrentRow.Cells[6].Value);
                        v.RaporBaslamaTarihi = DateTime.Parse(source.CurrentRow.Cells[7].Value.ToString());
                        v.RaporBitisTarihi = DateTime.Parse(source.CurrentRow.Cells[8].Value.ToString());
                        v.IsBasiKontrolTarihi = DateTime.Parse(source.CurrentRow.Cells[9].Value.ToString());
                        break;
                    case 3:
                        if (source.ActiveEditor is RadCheckBoxEditor)
                        {
                            v.CancelConfirm = source.ActiveEditor.Value.ToString(); //SearchReport.CheckBoxState;
                        }
                        v.PoliklinikTarihi = DateTime.Parse(source.CurrentRow.Cells[9].Value.ToString());
                        v.IsBasiKontrolTarihi = DateTime.Parse(source.CurrentRow.Cells[10].Value.ToString());
                        v.RaporTakipNo = source.CurrentRow.Cells[5].Value.ToString();
                        v.RaporSiraNo = Convert.ToInt32(source.CurrentRow.Cells[6].Value);
                        break;
                    case 4:
                        ws = source.CurrentRow.Cells[22].Value.ToString();
                        v.WorkingStatus = (ws == "0") ? 0 : (ws == "1") ? 1 : 2;
                        v.ConfirmDate = source.CurrentRow.Cells[21].Value.ToString();
                        v.IsBasiKontrolTarihi = DateTime.Parse(source.CurrentRow.Cells[9].Value.ToString());
                        v.RaporBaslamaTarihi = DateTime.Parse(source.CurrentRow.Cells[7].Value.ToString());
                        break;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return v;
        }
        public List<VisitsToBeProcessed> GetAllVisitsFromRgv(RadGridView source, out string msg)
        {
            msg = ""; string ws = "";
            List<VisitsToBeProcessed> lst = new List<VisitsToBeProcessed>();
            try
            {
                foreach (var row in source.Rows)
                {
                    VisitsToBeProcessed v = new VisitsToBeProcessed();
                    if (!row.Cells[0].ReadOnly) // şimdilik ReadOnly yok; onaylı raporlarda yapılabilir. 
                    {
                        v.Cnm = row.Cells[1].Value.ToString();
                        v.Tcno = row.Cells[2].Value.ToString();
                        v.AdSoyad = row.Cells[3].Value.ToString();
                        v.FirmaSicilNo = row.Cells[15].Value.ToString();
                        switch (SearchReport.ReportTypeAfterRgvLoad)
                        {
                            case 1:
                            case 2:
                                v.Vaka = row.Cells[4].Value.ToString();
                                v.RaporTakipNo = row.Cells[5].Value.ToString();
                                v.RaporSiraNo = Convert.ToInt32(row.Cells[6].Value.ToString());
                                v.RaporBaslamaTarihi = DateTime.Parse(row.Cells[7].Value.ToString());
                                v.RaporBitisTarihi = DateTime.Parse(row.Cells[8].Value.ToString());
                                v.IsBasiKontrolTarihi = DateTime.Parse(row.Cells[9].Value.ToString());
                                v.CezaDurumu = row.Cells[10].Value.ToString();
                                v.Aciklama = row.Cells[11].Value.ToString();
                                v.ConfirmDate = row.Cells[21].Value.ToString();
                                ws = row.Cells[22].Value.ToString();
                                v.WorkingStatus = (ws == "0") ? 0 : (ws == "1") ? 1 : 2;
                                break;
                            case 3:
                                if (source.ActiveEditor is RadCheckBoxEditor)
                                {
                                    v.CancelConfirm = source.ActiveEditor.Value.ToString(); //SearchReport.CheckBoxState;
                                }
                                v.Vaka = row.Cells[4].Value.ToString();
                                v.RaporTakipNo = row.Cells[5].Value.ToString();
                                v.RaporSiraNo = Convert.ToInt32(row.Cells[6].Value.ToString());
                                v.PoliklinikTarihi = DateTime.Parse(row.Cells[9].Value.ToString());
                                v.IsBasiKontrolTarihi = DateTime.Parse(row.Cells[10].Value.ToString());
                                break;
                            case 4:
                                ws = row.Cells[22].Value.ToString();
                                v.WorkingStatus = (ws == "0") ? 0 : (ws == "1") ? 1 : 2;
                                v.RaporBaslamaTarihi = DateTime.Parse(row.Cells[7].Value.ToString());
                                v.IsBasiKontrolTarihi = DateTime.Parse(row.Cells[9].Value.ToString());
                                v.ConfirmDate = row.Cells[21].Value.ToString();
                                v.Aciklama = row.Cells[11].Value.ToString(); ;
                                break;
                        }
                    }
                    lst.Add(v);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return lst;
        }
        public List<ConfirmReport> GetConfirmReportsFromRgv(RadGridView source, out string msg)
        {
            msg = "";
            List<ConfirmReport> lst = new List<ConfirmReport>();
            try
            {
                if (source.RowCount > 0)
                {

                    foreach (GridViewRowInfo row in source.Rows)
                    {
                        ConfirmReport cr = new ConfirmReport(); int i = 0;

                        cr.Tcid = row.Cells[i++].Value.ToString();
                        cr.Fullname = row.Cells[i++].Value.ToString();
                        cr.Vaka = row.Cells[i++].Value.ToString();
                        cr.Rbat = DateTime.Parse(row.Cells[i++].Value.ToString());
                        cr.Rbit = DateTime.Parse(row.Cells[i++].Value.ToString());
                        cr.Rtno = row.Cells[i++].Value.ToString();
                        cr.Rsno = Convert.ToInt32(row.Cells[i++].Value.ToString());
                        cr.Rslt = row.Cells[i++].Value.ToString();
                        cr.Onyt = DateTime.Parse(row.Cells[i++].Value.ToString());
                        cr.Pdffile = row.Cells[i++].Value.ToString();
                        lst.Add(cr);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return lst;
        }

        public static GridViewCheckBoxColumn AddCheckBoxColumnToRgv()
        {
            GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();
            checkBoxColumn.DataType = typeof(Boolean);
            checkBoxColumn.Name = "ColumnSelect";
            checkBoxColumn.FieldName = "select";
            checkBoxColumn.HeaderText = "Seç";
            checkBoxColumn.EnableHeaderCheckBox = true;
            checkBoxColumn.EditMode = EditMode.OnValidate;
            checkBoxColumn.ShouldCheckDataRows = true;
            checkBoxColumn.ThreeState = false;
            return checkBoxColumn;
        }
        public static GridViewCheckBoxColumn AddCheckBoxColumnToRgvForSettings()
        {
            GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();
            checkBoxColumn.DataType = typeof(Boolean);
            checkBoxColumn.Name = "ColumnSelect";
            checkBoxColumn.FieldName = "goster";
            checkBoxColumn.HeaderText = "Göster";
            checkBoxColumn.EditMode = EditMode.OnValidate;
            checkBoxColumn.ThreeState = false;
            return checkBoxColumn;
        }
        public static GridViewCheckBoxColumn AddCheckBoxColumnToLinkRgv()
        {
            GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();
            checkBoxColumn.DataType = typeof(Boolean);
            checkBoxColumn.Name = "ColumnSelect";
            checkBoxColumn.FieldName = "select";
            checkBoxColumn.HeaderText = "Seç";
            checkBoxColumn.EnableHeaderCheckBox = true;
            checkBoxColumn.EditMode = EditMode.OnValueChange;
            checkBoxColumn.ShouldCheckDataRows = true;
            checkBoxColumn.ThreeState = false;
            return checkBoxColumn;
        }
        public static GridViewDateTimeColumn AddDateColumnToRgv()
        {
            IndicatedDateTimeColumn dateColumn = new IndicatedDateTimeColumn();
            dateColumn.Name = "TarihSec";
            dateColumn.HeaderText = "Tarih Seç";
            dateColumn.Format = DateTimePickerFormat.Short;
            dateColumn.FormatInfo = new System.Globalization.CultureInfo("tr-TR");
            dateColumn.FormatString = "{0:dd.MM.yyyy}";
            return dateColumn;
        }
        public static GridViewComboBoxColumn AddComboBoxColumnToRgv()
        {
            Dictionary<string, string> cboxCalismaDurumu = new Dictionary<string, string>() { };
            cboxCalismaDurumu.Add("Çalışmamıştır", "0");
            cboxCalismaDurumu.Add("Çalışmıştır", "1");
            cboxCalismaDurumu.Add("Personelim Değil", "2");

            IndicatedComboBoxColumn columnCalismaDurumu = new IndicatedComboBoxColumn();
            columnCalismaDurumu.Name = "CalismaDurumu";
            columnCalismaDurumu.HeaderText = "Durum Seç";
            columnCalismaDurumu.DataSource = new BindingSource(cboxCalismaDurumu, null);
            columnCalismaDurumu.ValueMember = "Value";
            columnCalismaDurumu.DisplayMember = "Key";
            columnCalismaDurumu.Width = 200;
            columnCalismaDurumu.AllowFiltering = false;
            columnCalismaDurumu.AllowGroup = false;
            columnCalismaDurumu.AllowSort = false;
            columnCalismaDurumu.AllowSearching = false;
            columnCalismaDurumu.AllowReorder = false;
            columnCalismaDurumu.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            return columnCalismaDurumu;

        }
        public static GridViewCommandColumn AddCommandColumnToRgv(string header, string nm = "", string fn = "", string dt = "")
        {
            GridViewCommandColumn columnPdf = new GridViewCommandColumn();
            columnPdf.Name = nm == ""? "openPdf" : nm;
            columnPdf.UseDefaultText = true;
            columnPdf.HeaderText = header;
            columnPdf.FieldName = fn == ""? "Dosyayı Aç": fn;
            columnPdf.DefaultText = dt == ""? "Dosyayı Aç" : dt;
            columnPdf.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            columnPdf.Image = Resources.pdf;
            columnPdf.Width = 120;
            columnPdf.MinWidth = 120;
            columnPdf.MaxWidth = 120;
            columnPdf.AllowFiltering = false;
            columnPdf.AllowGroup = false;
            columnPdf.AllowSort = false;
            columnPdf.AllowSearching = false;
            columnPdf.AllowReorder = false;
            return columnPdf;
        }
        public static void ClearAll()
        {
            string msg;
            try
            {
                IOC.TrmBase.RemoveTempFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\SgkAsistan\Temp", out msg);
            }
            catch (Exception ex)
            {
                _ = ex.Message.ToString();
            }

        }

        #region regedit
        public static bool SetAccessTrustedLocation(int vers, out string msg)
        {
            msg = "";
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey($@"SOFTWARE\Microsoft\Office\{vers}.0\Access\Security\Trusted Locations\Location5");
                // 5 yerine 1'den başlayarak herhangi bir sayı yazılabilir, 0 numaralı konum ise varsayılan olarak "C:\Program Files (x86)\Microsoft Office\Root\Office16\ACCWIZ\" konumua ayarlanmış. Eğer varolan bir konum seçilirse üzerine yazıyor; bu yüzden başka programların da konum eklemiş olabileceğini varsayarak ne olur ne olmaz diye 5 yazıyoruz. 
                key.SetValue("oldFn", @"C:\ProgramData\SgkAsistan\Data\");
                key.SetValue("AllowSubfolders", "dword:00000001");
                key.SetValue("Description", "Asistpro Veritabanı konumu");
                key.SetValue("Date", $"\"{DateTime.Now.ToString("MM/dd/yyyy hh:mm")}\"");
                key.Close();
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }
        public static int GetAccessTrustedLocation(int vers, out string msg)
        {
            msg = "";
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey($@"SOFTWARE\Microsoft\Office\{vers}.0\Access\Security\Trusted Locations\Location5");
                if (key != null)
                {
                    return 1;
                }
                return vers;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public static int GetAccessVersion(out string msg)
        {
            msg = "";
            try
            {
                for (int i = 16; i >= 12; i--) // access 2007 = 12, access 2010 = 14, access 2013 = 15, access 2016, 2019, 365 = 16
                {
                    if (i == 13) continue;
                    RegistryKey key = Registry.CurrentUser.OpenSubKey($@"SOFTWARE\Microsoft\Office\{i}.0\Access");
                    if (key != null)
                        return i;
                }
                return 0; // office 2007'den daha eski bir sürüm var ya da office hiç yüklü değil
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public static bool SetAccountInfo(bool demo, string sd, string fd)
        {
            try
            {
                RegistryKey softwareKey = Registry.LocalMachine.OpenSubKey("Software", true);

                RegistryKey appNameKey = softwareKey.CreateSubKey("Microsoft");
                RegistryKey appVersionKey = appNameKey.CreateSubKey("Dot.Net");

                appVersionKey.SetValue("sd", sd);
                if (demo)
                {
                    appVersionKey.SetValue("id", 0x00000001, RegistryValueKind.DWord);
                }
                else
                {
                    appVersionKey.SetValue("id", 0x00000000, RegistryValueKind.DWord);
                }

                appVersionKey.SetValue("ed", fd);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                      .IsInRole(WindowsBuiltInRole.Administrator);
        }
        public static int GetAccountInfo()
        {
            try
            {
                RegistryKey softwareKey = Registry.LocalMachine.OpenSubKey("Software", true);
                RegistryKey appNameKey = softwareKey.OpenSubKey("Microsoft");
                RegistryKey appVersionKey = appNameKey.OpenSubKey("Dot.Net");
                if (appVersionKey == null)
                {
                    return 0; // daha önce kayıt yok
                }
                else
                {
                    PkcGlobals.HasDemoInstalled = Convert.ToBoolean(appVersionKey.GetValue("id"));
                    PkcGlobals.EndDate = appVersionKey.GetValue("ed").ToString();
                    return 1; // kayıt var
                }

            }
            catch (Exception)
            {
                return 2; // erişim hatası
            }
        }

        #endregion
        #region WebDriver
        //public IWebDriver GetWebDriver(bool hideBrowser, out string msg)
        //{
        //    msg = "";
        //    _driverLocation = Application.StartupPath;
        //    //StartDriver();
        //    bool aktifDriverYasiyor = false;
        //    int aktifDriverTipi = -1; // -1: Yok/Bilinmiyor, 0: Firefox, 1: Chrome

        //    // 1. ADIM: Halihazırda açık ve çalışan bir tarayıcı var mı kontrol et
        //    if (Surucu.Driver != null)
        //    {
        //        try
        //        {
        //            // Tarayıcıya hafif bir istek atarak gerçekten açık olup olmadığını test ediyoruz
        //            var testHandle = Surucu.Driver.CurrentWindowHandle;
        //            aktifDriverYasiyor = true;

        //            // Açık olan sürücünün tipini tespit et
        //            string driverName = Surucu.Driver.GetType().Name;
        //            if (driverName.Contains("Firefox"))
        //            {
        //                aktifDriverTipi = 0;
        //            }
        //            else if (driverName.Contains("Chrome"))
        //            {
        //                aktifDriverTipi = 1;
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            // Tarayıcı hafızada var ama kullanıcı pencereyi kapatmış veya çökmüş
        //            aktifDriverYasiyor = false;
        //        }
        //    }

        //    int istenenBrowserTipi = Settings.Default.browser;

        //    // 2. ADIM: Karar Mekanizması
        //    if (aktifDriverYasiyor)
        //    {
        //        // Senaryo A: Açık olan tarayıcı ile istenen tarayıcı AYNI ise
        //        if (aktifDriverTipi == istenenBrowserTipi)
        //        {
        //            // Hiçbir şey yapma, zaten açık olan kararlı driver'ı doğrudan döndür
        //            return Surucu.Driver;
        //        }
        //        else
        //        {
        //            // Senaryo B: Tarayıcı açık ama tipi FARKLI (Örn: Firefox açık ama Chrome istendi)
        //            try
        //            {
        //                Surucu.Driver.Quit(); // Eski tarayıcıyı güvenle kapat
        //            }
        //            catch { }
        //            Surucu.Driver = null; // Hafızayı sıfırla
        //        }
        //    }

        //    // 3. ADIM: Yeni Tarayıcı Oluşturma (Eğer açık tarayıcı yoksa veya tipi farklıysa buraya gelir)
        //    switch (istenenBrowserTipi)
        //    {
        //        case 0:
        //            SetFirefoxOptionsForDownload(hideBrowser);
        //            break;
        //        case 1:
        //            SetChromeOptionsForDownload(hideBrowser);
        //            break;
        //    }

        //    // Yeni tarayıcı oluştuktan sonra Windows süreç (Process) eşleştirmesini yap
        //    if (Surucu.Driver != null && !hideBrowser)
        //    {
        //        IOC.LinkOps.SetWebBrowserProcess();
        //    }

        //    return Surucu.Driver;
        //}
        //public void SetFirefoxOptionsForDownload(bool hideBrowser)
        //{
        //    FirefoxOptions firefoxOptions = new FirefoxOptions();
        //    FirefoxProfile firefoxProfile = new FirefoxProfile();
        //    FirefoxDriverService fDriverService;

        //    _driverLocation = Application.StartupPath;

        //    // İndirme ve PDF Ayarları
        //    firefoxProfile.SetPreference("browser.download.dir", SearchReport.DownloadDir);
        //    firefoxProfile.SetPreference("browser.download.folderList", 2);
        //    firefoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf");
        //    firefoxProfile.SetPreference("plugin.scan.plid.all", false);
        //    firefoxProfile.SetPreference("plugin.scan.Acrobat", "99.0");
        //    firefoxProfile.SetPreference("pdfjs.disabled", true);
        //    firefoxProfile.SetPreference("browser.download.manager.showAlertOnComplete", false);
        //    firefoxProfile.SetPreference("pdfjs.enabledCache.state", false);
        //    firefoxProfile.DeleteAfterUse = true;

        //    firefoxOptions.Profile = firefoxProfile;

        //    if (hideBrowser)
        //    {
        //        firefoxOptions.AddArgument("-headless");
        //        int sw = Screen.PrimaryScreen.Bounds.Width;
        //        int sh = Screen.PrimaryScreen.Bounds.Height;
        //        firefoxOptions.AddArgument($"--width={sw}");
        //        firefoxOptions.AddArgument($"--height={sh}");
        //    }

        //    try
        //    {
        //        // Yerleşik Selenium Manager'ı kullanmak için boş servis oluşturuyoruz (DriverManager kaldırıldı)
        //        fDriverService = FirefoxDriverService.CreateDefaultService();
        //        fDriverService.HideCommandPromptWindow = true; // Siyah konsol ekranını gizle
        //        Surucu.Driver = new FirefoxDriver(fDriverService, firefoxOptions);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Eğer yerleşik mekanizma hata verirse, yerel dizindeki sürücüye geri dönüyoruz
        //        fDriverService = FirefoxDriverService.CreateDefaultService(Application.StartupPath);
        //        fDriverService.HideCommandPromptWindow = true;
        //        Surucu.Driver = new FirefoxDriver(fDriverService, firefoxOptions);
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        //public void SetChromeOptionsForDownload(bool hideBrowser)
        //{
        //    ChromeOptions chromeOptions = new ChromeOptions();
        //    _driverLocation = Application.StartupPath;

        //    // 1. Çalışan Metottaki Kararlı Headless ve Boyut Ayarları
        //    if (hideBrowser)
        //    {
        //        chromeOptions.AddArgument("--headless=new");
        //    }

        //    int sw = Screen.PrimaryScreen.Bounds.Width;
        //    int sh = Screen.PrimaryScreen.Bounds.Height;
        //    chromeOptions.AddArgument($"window-size={sw},{sh}");

        //    // 2. ÇALIŞAN SİHRALİ FORMÜL: Her seferinde kilitlenmeyen, çakışmayan benzersiz geçici profil
        //    string userDataDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "SgkOto_" + System.IO.Path.GetRandomFileName());
        //    System.IO.Directory.CreateDirectory(userDataDir);
        //    chromeOptions.AddArgument($"--user-data-dir={userDataDir}");

        //    // 3. Güvenlik ve Kararlılık Argümanları (Çakışma yaratanlar temizlendi)
        //    chromeOptions.AddArgument("--no-sandbox");
        //    chromeOptions.AddArgument("--disable-dev-shm-usage");
        //    chromeOptions.AddArgument("--remote-allow-origins=*");
        //    chromeOptions.AddArgument("--disable-notifications");
        //    chromeOptions.AddArgument("--disable-extensions");
        //    chromeOptions.AddArgument("--disable-gpu");
        //    chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");

        //    // Konsol kirliliğini önlemek için log seviyesi
        //    chromeOptions.AddArgument("--log-level=3");
        //    chromeOptions.AddArgument("--silent");

        //    // 4. İndirme ve Profil Tercihleri (Bizim metottan korunanlar)
        //    chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
        //    chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
        //    chromeOptions.AddUserProfilePreference("download.default_directory", SearchReport.DownloadDir);
        //    chromeOptions.AddUserProfilePreference("intl.accept_languages", "tr");
        //    chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");

        //    try
        //    {
        //        // Çalışan metottaki gibi temiz ve doğrudan başlatma (Service karmaşası olmadan)
        //        Surucu.Driver = new ChromeDriver(chromeOptions);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Chrome sürücü başlatılamadı: {ex.Message}");
        //        throw;
        //    }
        //}

        public IWebDriver GetWebDriver(bool hideBrowser, out string msg)
        {
            msg = "";
            _driverLocation = Application.StartupPath;
            bool aktifDriverYasiyor = false;
            int aktifDriverTipi = -1; // -1: Yok/Bilinmiyor, 0: Firefox, 1: Chrome

            // 1. ADIM: Halihazırda açık ve çalışan bir tarayıcı var mı kontrol et
            if (Surucu.Driver != null)
            {
                try
                {
                    var testHandle = Surucu.Driver.CurrentWindowHandle;
                    aktifDriverYasiyor = true;

                    string driverName = Surucu.Driver.GetType().Name;
                    if (driverName.Contains("Firefox"))
                    {
                        aktifDriverTipi = 0;
                    }
                    else if (driverName.Contains("Chrome"))
                    {
                        aktifDriverTipi = 1;
                    }
                }
                catch (Exception)
                {
                    aktifDriverYasiyor = false;
                }
            }

            int istenenBrowserTipi = Settings.Default.browser;

            // 2. ADIM: Karar Mekanizması
            if (aktifDriverYasiyor)
            {
                if (aktifDriverTipi == istenenBrowserTipi)
                {
                    return Surucu.Driver;
                }
                else
                {
                    try
                    {
                        Surucu.Driver.Quit();
                    }
                    catch { }
                    Surucu.Driver = null;
                }
            }

            // 3. ADIM: Yeni Tarayıcı Oluşturma (Hatalar yakalanıp msg parametresine yazılır)
            try
            {
                switch (istenenBrowserTipi)
                {
                    case 0:
                        SetFirefoxOptionsForDownload(hideBrowser);
                        break;
                    case 1:
                        SetChromeOptionsForDownload(hideBrowser);
                        break;
                    default:
                        msg = "Tanımlanamayan tarayıcı tipi seçildi.";
                        return null;
                }
            }
            catch (Exception ex)
            {
                msg = $"Tarayıcı başlatılırken hata oluştu: {ex.Message}";
                return null;
            }

            // Windows süreç (Process) eşleştirmesi
            if (Surucu.Driver != null && !hideBrowser)
            {
                IOC.LinkOps.SetWebBrowserProcess();
            }

            return Surucu.Driver;
        }

        public void SetFirefoxOptionsForDownload(bool hideBrowser)
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            FirefoxDriverService fDriverService;

            _driverLocation = Application.StartupPath;

            // İndirme ve PDF Ayarları (Doğrudan Options üzerinden set edilerek modernize edildi)
            firefoxOptions.SetPreference("browser.download.dir", SearchReport.DownloadDir);
            firefoxOptions.SetPreference("browser.download.folderList", 2);
            firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf;application/octet-stream");
            firefoxOptions.SetPreference("plugin.scan.plid.all", false);
            firefoxOptions.SetPreference("plugin.scan.Acrobat", "99.0");
            firefoxOptions.SetPreference("pdfjs.disabled", true);
            firefoxOptions.SetPreference("browser.download.manager.showAlertOnComplete", false);
            firefoxOptions.SetPreference("pdfjs.enabledCache.state", false);

            if (hideBrowser)
            {
                firefoxOptions.AddArgument("-headless");
                int sw = Screen.PrimaryScreen.Bounds.Width;
                int sh = Screen.PrimaryScreen.Bounds.Height;
                firefoxOptions.AddArgument($"--width={sw}");
                firefoxOptions.AddArgument($"--height={sh}");
            }

            try
            {
                // 1. Seçenek: Selenium Manager (runtimes klasörünü kullanır)
                fDriverService = FirefoxDriverService.CreateDefaultService();
                fDriverService.HideCommandPromptWindow = true;
                fDriverService.SuppressInitialDiagnosticInformation = true;
                Surucu.Driver = new FirefoxDriver(fDriverService, firefoxOptions);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Firefox varsayılan servis başlatılamadı, yerel deneniyor: {ex.Message}");
                // 2. Seçenek Fallback: StartupPath içindeki geckodriver.exe'yi kullanır
                fDriverService = FirefoxDriverService.CreateDefaultService(Application.StartupPath);
                fDriverService.HideCommandPromptWindow = true;
                fDriverService.SuppressInitialDiagnosticInformation = true;
                Surucu.Driver = new FirefoxDriver(fDriverService, firefoxOptions);
            }
        }

        public void SetChromeOptionsForDownload(bool hideBrowser)
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            ChromeDriverService cDriverService;
            _driverLocation = Application.StartupPath;

            if (hideBrowser)
            {
                chromeOptions.AddArgument("--headless=new");
            }

            int sw = Screen.PrimaryScreen.Bounds.Width;
            int sh = Screen.PrimaryScreen.Bounds.Height;
            chromeOptions.AddArgument($"window-size={sw},{sh}");

            // Çakışmayan benzersiz geçici profil formülü
            string userDataDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "SgkOto_" + System.IO.Path.GetRandomFileName());
            System.IO.Directory.CreateDirectory(userDataDir);
            chromeOptions.AddArgument($"--user-data-dir={userDataDir}");

            // Güvenlik ve Kararlılık Argümanları
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            chromeOptions.AddArgument("--remote-allow-origins=*");
            chromeOptions.AddArgument("--disable-notifications");
            chromeOptions.AddArgument("--disable-extensions");
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");

            // Konsol loglarını susturma argümanları
            chromeOptions.AddArgument("--log-level=3");
            chromeOptions.AddArgument("--silent");

            // İndirme ve Profil Tercihleri
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddUserProfilePreference("download.default_directory", SearchReport.DownloadDir);
            chromeOptions.AddUserProfilePreference("intl.accept_languages", "tr");
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");

            try
            {
                // YENİ: Siyah terminal ekranını tamamen yok eden servis entegrasyonu
                cDriverService = ChromeDriverService.CreateDefaultService();
                cDriverService.HideCommandPromptWindow = true;
                cDriverService.SuppressInitialDiagnosticInformation = true;
                Surucu.Driver = new ChromeDriver(cDriverService, chromeOptions);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Chrome varsayılan servis başlatılamadı, yerel deneniyor: {ex.Message}");
                // Fallback: Üretim ortamında runtimes eksikse local dizine başvurma şansı tanır
                cDriverService = ChromeDriverService.CreateDefaultService(Application.StartupPath);
                cDriverService.HideCommandPromptWindow = true;
                cDriverService.SuppressInitialDiagnosticInformation = true;
                Surucu.Driver = new ChromeDriver(cDriverService, chromeOptions);
            }
        }
        #endregion
        #region DigerDriverMetotları
        private static ChromeOptions GetChromeOptions(bool headless)
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--remote-debugging-port-9222");
            chromeOptions.AddExcludedArgument("enable-automation");
            chromeOptions.AddAdditionalOption("useAutomationExtension", false);
            ///*chromeOptions*/.AddAdditionalCapability("useAutomationExtension", false);
            chromeOptions.AddArguments("--no-sandbox");
            chromeOptions.AddArguments("--disable-notifications");
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddUserProfilePreference("download.default_directory", SearchReport.DownloadDir);
            chromeOptions.AddUserProfilePreference("intl.accept_languages", "tr");
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
            chromeOptions.AddArguments("--remote-debugging-port-9222");
            if (headless)
            {
                int sw = Screen.PrimaryScreen.Bounds.Width;
                int sh = Screen.PrimaryScreen.Bounds.Height;
                chromeOptions.AddArgument("--headless");
                chromeOptions.AddArguments($"window-size={sw}{sh}");
            }
            return chromeOptions;
        }
        public static IWebDriver SetIeDriver()
        {
            return new InternetExplorerDriver();
        }
        public static void SetFirefoxConfig(IWebDriver driver, out string msg)
        {
            msg = "";
            IWebElement tbSearch, tablo, row, btnToggle, td, span, value, btnEdit, btnSave, tbValue, rbString, btnAdd;
            List<IWebElement> rows;
            try
            {
                driver.Url = "about:config";
                row = driver.FindElement(By.Id("warningButton"));
                driver.FindElement(By.Id("warningButton")).Click();
                tbSearch = driver.FindElement(By.Id("about-config-search"));
                tbSearch.SendKeys("browser.helperApps.alwaysAsk.force");

                tablo = driver.FindElement(By.Id("prefs"));
                rows = tablo.FindElements(By.TagName("tr")).ToList();
                foreach (IWebElement item in rows)
                {
                    if (item.GetAttribute("class") != "hidden" || item.GetAttribute("class") == "has-user-value") { row = item; }

                }
                btnToggle = row.FindElement(By.ClassName("button-toggle"));
                td = row.FindElement(By.ClassName("cell-value"));

                span = td.FindElement(By.TagName("span"));
                value = span.FindElement(By.TagName("span"));

                if (value.Text == "true") { btnToggle.Click(); }
                tbSearch.Clear();

                tbSearch.SendKeys("browser.download.manager.showWhenStarting");
                rows = tablo.FindElements(By.TagName("tr")).ToList();
                foreach (IWebElement item in rows)
                {
                    if (item.GetAttribute("class") != "hidden" || item.GetAttribute("class") == "has-user-value") { row = item; }

                }
                btnToggle = row.FindElement(By.ClassName("button-toggle"));
                if (row == null) { return; }
                td = row.FindElement(By.ClassName("cell-value"));
                span = td.FindElement(By.TagName("span"));
                value = span.FindElement(By.TagName("span"));
                if (value.Text == "true") { btnToggle.Click(); }
                tbSearch.Clear();

                tbSearch.SendKeys("pdfjs.enabledCache.state");
                rows = tablo.FindElements(By.TagName("tr")).ToList();
                foreach (IWebElement item in rows)
                {
                    if (item.GetAttribute("class") != "hidden" || item.GetAttribute("class") == "has-user-value") { row = item; }

                }
                btnToggle = row.FindElement(By.ClassName("button-toggle"));
                if (row == null) { return; }
                td = row.FindElement(By.ClassName("cell-value"));
                span = td.FindElement(By.TagName("span"));
                value = span.FindElement(By.TagName("span"));
                if (value.Text == "true") { btnToggle.Click(); }
                tbSearch.Clear();


                tbSearch.SendKeys("pdfjs.disabled");
                rows = tablo.FindElements(By.TagName("tr")).ToList();
                foreach (IWebElement item in rows)
                {
                    if (item.GetAttribute("class") != "hidden" || item.GetAttribute("class") == "has-user-value") { row = item; }

                }
                btnToggle = row.FindElement(By.ClassName("button-toggle"));
                if (row == null) { return; }
                td = row.FindElement(By.ClassName("cell-value"));
                span = td.FindElement(By.TagName("span"));
                value = span.FindElement(By.TagName("span"));
                if (value.Text == "false") { btnToggle.Click(); }
                tbSearch.Clear();

                tbSearch.SendKeys("browser.download.folderList");
                btnEdit = driver.FindElement(By.ClassName(@"button-edit"));
                btnEdit.Click();
                tbValue = driver.FindElement(By.XPath("/html/body/table/tr[1]/td[1]/form/input"));
                tbValue.SendKeys("2");
                btnSave = driver.FindElement(By.ClassName(@"button-save"));
                btnSave.Click();
                tbSearch.Clear();

                tbSearch.SendKeys("browser.download.dir");
                rbString = driver.FindElement(By.XPath("/html/body/table/tr[6]/td[1]/form/label[3]/input"));
                rbString.Click();
                btnAdd = driver.FindElement(By.ClassName(@"button-add"));
                btnAdd.Click();

                tbValue = driver.FindElement(By.XPath("/html/body/table/tr[6]/td[1]/form/input"));
                tbValue.SendKeys($"{SearchReport.DownloadDir}");
                btnSave = driver.FindElement(By.ClassName(@"button-save"));
                btnSave.Click();
                tbSearch.Clear();

                tbSearch.SendKeys("browser.helperApps.neverAsk.saveToDisk");
                rows = tablo.FindElements(By.TagName("tr")).ToList();
                foreach (IWebElement item in rows)
                {
                    if (item.GetAttribute("class") != "hidden" || item.GetAttribute("class") == "has-user-value") { row = item; }

                }
                btnEdit = row.FindElement(By.ClassName(@"button-edit"));
                btnEdit.Click();
                tbValue = row.FindElement(By.TagName("input"));
                tbValue.SendKeys("application/pdf");
                btnSave = driver.FindElement(By.ClassName(@"button-save"));
                btnSave.Click();
                tbSearch.Clear();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }


        }
        #endregion
        public bool TryCreateFolder(string folderToCreate, bool clearExistingFiles)
        {
            int tryCount = 0;
            bool folderCreated = false;
            bool folderIsEmpty = false;
            while (tryCount++ < 10)
            {
                if (System.IO.Directory.Exists(folderToCreate) == false)
                {
                    System.IO.Directory.CreateDirectory(folderToCreate);
                }
                else
                {
                    folderCreated = true;
                    break;
                }

            }
            tryCount = 0;
            if (folderCreated == false)
            {
                return false;
            }
            if (clearExistingFiles)
            {
                folderIsEmpty = false;
                while (tryCount++ < 10)
                {
                    int fileCount = 0;
                    System.IO.DirectoryInfo di = new DirectoryInfo(folderToCreate);
                    foreach (FileInfo file in di.EnumerateFiles())
                    {
                        file.Delete();
                        fileCount++;
                    }
                    if (fileCount == 0)
                    {
                        folderIsEmpty = true;
                        break;
                    }
                }
                return folderCreated && folderIsEmpty;
            }

            return folderCreated;
        }
        public static string GetLastDownloadedFile(string folder)
        {
            string latestfile = "";
            var files = new DirectoryInfo(folder).GetFiles("*.pdf");
            DateTime lastupdated = DateTime.MinValue;
            foreach (FileInfo file in files)
            {
                if (file.LastWriteTime > lastupdated)
                {
                    lastupdated = file.LastWriteTime;
                    latestfile = file.Name;
                }
            }
            return latestfile;
        }
        public static int FileCount(string folder)
        {
            var files = new DirectoryInfo(folder).GetFiles("*.pdf");
            return files.Count();
        }

        public static string GetMacAdress()
        {
            IEnumerable<NetworkInterfaceType> excludeTypes = new[] { NetworkInterfaceType.Tunnel, NetworkInterfaceType.Loopback };
            NetworkInterface[] all = NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.GetIPProperties().GatewayAddresses.Any() && nic.OperationalStatus == OperationalStatus.Up).ToArray();
            NetworkInterface[] exclude = all.Where(i => excludeTypes.Contains(i.NetworkInterfaceType)).ToArray();
            IEnumerable<NetworkInterface> nics = all.Except(exclude);
            string mac = nics.Select(a => string.Join(":", a.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2")))).FirstOrDefault();
            return mac;
        }
        public static string DiskInfo()
        {
            string systemDisk = System.IO.Path.GetPathRoot(Environment.SystemDirectory).TrimEnd('\\');
            string serial = "";
            using (var m1 = new ManagementObjectSearcher("ASSOCIATORS OF {Win32_LogicalDisk.DeviceID='" + systemDisk + "'} WHERE ResultClass=Win32_DiskPartition"))
            {
                foreach (var i1 in m1.Get())
                {
                    using (var m2 = new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + i1["DeviceID"] + "'} WHERE ResultClass=Win32_DiskDrive"))
                    {
                        foreach (var i2 in m2.Get())
                        {
                            //Console.WriteLine("Type: " + i2["MediaType"]);
                            //Console.WriteLine("Model: " + i2["Model"]);
                            //Console.WriteLine("Model: " + i2["InterfaceType"]);
                            serial = i2["SerialNumber"].ToString();
                            break;
                        }
                    }
                    break;
                }
            }
            return serial;
        }
        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "Asistpro";   // The description of the shortcut
            shortcut.IconLocation = Application.StartupPath + "\\asistansimge.ico";// The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }
    }
}

