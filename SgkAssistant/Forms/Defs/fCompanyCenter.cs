using Models.Common;
using SgkAssistant.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SgkAssistant.Forms.Defs
{
    public partial class FCompanyCenter : Telerik.WinControls.UI.RadForm
    {
        private Company Company;
        private Dictionary<string, int> dc;
        private string msg; 
        private int result = 0;
        private int resTotal = 0;
        public FCompanyCenter(Company _Company)
        {
            InitializeComponent();
            Company = _Company;
        }

        private void FCompanyCenter_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            msg = ""; result = 0; resTotal = 0;
            lblQuestion.Text = $"* {Company.CompanyName} adlı firma merkez iken başka bir firmanın şubesi olarak ayarlandı.\r\n* Daha önceden {Company.CompanyName} adlı firmaya bağlı olan şubeler için yeni bir firma merkezi seçmeniz gerekiyor.\r\n* Bağlı olduğu merkez tanımlanmayan firmaların kendileri, merkez olarak tanımlanacaktır.";

            
            SetCompanyList();
            GetCompanyCenters(out _);
            rgvCompanyList.CellFormatting += RgvHelpers.GridViews_CellFormatting;
            if (dc != null && dc.Count > 0)
            {
                ddlCenters.DataSource = dc;
                ddlCenters.DisplayMember = "Key";
                ddlCenters.ValueMember = "Value";
            }
        }
        private void GetCompanyCenters(out string msg)
        {
            try
            {
                dc = IOC.CompanyDataService.GetCompanyCenters(out msg);
            }
            catch (Exception ex)
            {
                dc = null;
                msg = ex.Message.ToString();
            }
        }
        private void SetCompanyList()
        {
            rgvCompanyList.DataSource = (from x in GlobalVars.Companies where x.Id != x.Fm && x.Fm == Company.Id select x).ToList();
            int i = 0;
            rgvCompanyList.Columns[i++].HeaderText = "Firma ID";
            rgvCompanyList.Columns[i++].HeaderText = "Firma Adı";
            rgvCompanyList.Columns[i++].HeaderText = "SGK Kullanıcı Adı";
            rgvCompanyList.Columns[i++].HeaderText = "SGK Kullanıcı Kodu";
            rgvCompanyList.Columns[i++].HeaderText = "SGK Sistem Şifresi";
            rgvCompanyList.Columns[i++].HeaderText = "SGK İşyeri Şifresi";
            rgvCompanyList.Columns[i++].HeaderText = "Firma Merkezi";
            rgvCompanyList.Columns[i++].HeaderText = "GİB Kullanıcı Adı";
            rgvCompanyList.Columns[i++].HeaderText = "GİB Parola";
            rgvCompanyList.Columns[i++].HeaderText = "GİB Şifre";
            rgvCompanyList.Columns[i++].HeaderText = "Firma Sicil No";
            rgvCompanyList.Columns[i++].HeaderText = "Unvan";
            rgvCompanyList.Columns[i++].HeaderText = "Adres";
            rgvCompanyList.Columns[i++].HeaderText = "Bağlı Olduğu SGM";
            rgvCompanyList.Columns[i++].HeaderText = "Kanun Kapsamına Alınış";
            rgvCompanyList.Columns[i++].HeaderText = "Kanun Kapsamından Çıkış";
            rgvCompanyList.Columns[i++].HeaderText = "Özel Kod 1";
            rgvCompanyList.Columns[i++].HeaderText = "Özel Kod 2";
            rgvCompanyList.Columns[i++].HeaderText = "Özel Kod 3";
            rgvCompanyList.Columns[i++].HeaderText = "Özel Kod 4";
            rgvCompanyList.Columns[i++].HeaderText = "Özel Kod 5";
            rgvCompanyList.Columns[i++].HeaderText = "Ekleyen";
            rgvCompanyList.Columns[i++].HeaderText = "Ekleme Tarihi";

            if (rgvCompanyList.Columns[0].Name == "Id")
            {
                GridViewDecimalColumn decimalColumn = new GridViewDecimalColumn();
                decimalColumn.Name = "sira";
                decimalColumn.HeaderText = "No";
                decimalColumn.DecimalPlaces = 0;
                rgvCompanyList.Columns.Add(decimalColumn); // 23 
                                                   //rgvCompanyList.Columns.RemoveAt(0);
                                                   //rgvCompanyList.Columns.Move(22, 0);
                rgvCompanyList.Columns.Move(23, 0);
                rgvCompanyList.Columns.Move(1, 23);

            }

            for (int j = 0; j < rgvCompanyList.RowCount; j++)
            {
                rgvCompanyList.Rows[j].Cells["Sira"].Value = j + 1;
            }
            IOC.WinHelpers.HideColumns(rgvCompanyList);
            rgvCompanyList.Columns[0].MaxWidth = 60;
            rgvCompanyList.Columns[0].MinWidth = 60;
            rgvCompanyList.Columns[1].MinWidth = 200;
            rgvCompanyList.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        }
        private void btnSetFm_Click(object sender, EventArgs e)
        {
            resTotal = 0;
            if (rgvCompanyList.SelectedRows[0].Index < 0) { lblMessage.Text = "Lütfen önce merkezi belirnecek firmaları seçiniz"; return; }
            List<Company> SelectedCompanies = IOC.WinHelpers.GetSelectedCompaniesFromRgv(rgvCompanyList, out msg);

            if(SelectedCompanies != null && SelectedCompanies.Count > 0)
            {
                foreach (Company company in SelectedCompanies)
                {
                    company.Fm = Convert.ToInt32(ddlCenters.SelectedValue);
                    result = IOC.CompanyDataService.AddCompany(company, PackageHelper.Mcc, out msg);
                    if (result == 2)
                    {
                        resTotal += 1;
                        GlobalVars.Companies.Find(x => x.Id == company.Id).Fm = Convert.ToInt32(ddlCenters.SelectedValue);
                    }
                }
            }
            lblMessage.Text = $"{resTotal } adet firmanın merkezi {ddlCenters.SelectedText} olarak ayarlandı";
            SetCompanyList();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string msg; resTotal = 0;
            List<Company> UnSelectedCompanies = IOC.WinHelpers.GetAllCompaniesFromRgv(rgvCompanyList, out msg).FindAll(x => x.Fm == Company.Id).ToList();
            foreach (Company company in UnSelectedCompanies)
            {
                company.Fm = company.Id;
                result = IOC.CompanyDataService.AddCompany(company, PackageHelper.Mcc, out msg);
                if (result == 2)
                {
                    resTotal += 1;
                    GlobalVars.Companies.Find(x => x.Id == company.Id).Fm = Convert.ToInt32(ddlCenters.SelectedValue);
                }
            }
        }
    }
}
