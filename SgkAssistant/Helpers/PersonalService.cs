using Models.Common;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using SgkAssistant.Forms.Defs;

namespace SgkAssistant.Helpers
{
    public class PersonalService
    {
        private Personal personal { get; set; }
        private DateTime lastPeriodStart = new DateTime();
        private List<LeavePeriod> lstLeavePeriods = new List<LeavePeriod>();
        private List<LeavePeriod> lstLeavePeriodsOfPersonal = new List<LeavePeriod>();
        private List<Leaves> PersonalLeavesAll = new List<Leaves>();
        private bool isUpdate = false;
        private decimal timeval = 0, existingAlvtimeval = 0, timevalOfDeletedLeave = 0;
        public string msg { get; set; } = "";
        public LeavePeriod activePeriod, prevPeriod;
        public void AddPersonal(Personal personal, out string msg)
        {
            msg = "";
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan span = DateTime.Now.Subtract(personal.Dtr.AddDays(2));
            int years = Convert.ToInt32(span.Days / 365.25) < 0 ? 0 : (zeroTime + span).Year - 1;
            if (years < 15)
            {
                DialogResult confirmResult = RadMessageBox.Show("15 yaşından küçük personel ekliyorsunuz, devam edilsin mi?", "15 yaşından küçük personel!", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (confirmResult == DialogResult.No)
                {
                    msg = "İşlem iptal edildi";  return;
                }
            }
            else if (years == 15)
            {
                DialogResult confirmResult = RadMessageBox.Show("Çocuk işçi (15 yaş) ekliyorsunuz, devam edilsin mi?", "Çocuk işçi!", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (confirmResult == DialogResult.No)
                {
                    msg = "İşlem iptal edildi"; return;
                }
            }
            else if (years > 15 && years <= 18)
            {
                DialogResult confirmResult = RadMessageBox.Show("Genç işçi (15-18 yaş arası) ekliyorsunuz, devam edilsin mi?", "Genç işçi!", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (confirmResult == DialogResult.No)
                {
                    msg = "İşlem iptal edildi"; return;
                }
            }

            int result = IOC.PersonalDataService.AddPersonal(personal, out msg);
            if (result == 1)
            {
                if (GlobalVars.Personals != null)
                {
                    GlobalVars.Personals.Add(personal);
                }
                else
                {
                    msg = "Personals list not initialized";
                    return;
                }
                this.personal = null;
                PersonalChange.HasChanged = true; PersonalChange.HasChangedForDialogBox= true; 
                SetPeriods(personal);
                IOC.LeavePeriodDataService.AddLeavePeriods(lstLeavePeriods, out msg);
                msg = $"{personal.Tcno} kimlik numaralı personel eklendi";
            }
            else if (result == 2)
            {
                PersonalChange.HasChanged = true; PersonalChange.HasChangedForDialogBox= true;
                SetPeriods(personal);
                IOC.LeavePeriodDataService.AddLeavePeriods(lstLeavePeriods, out msg);
                msg = $"{personal.Tcno} kimlik numaralı personelin bilgileri güncellendi";
            
            }
            else
            {
                msg = $"Bir hata oluştu: {msg}";
                PersonalChange.HasChanged = false;
            }
        }
        public decimal CalculateDeservedLeave(Personal personal, DateTime tarih, decimal unPaid, bool last = false )
        {
            decimal his = 0;

            TimeSpan spanAge = tarih.Subtract(personal.Dtr);
            int age = spanAge.TotalDays < 0 ? 0 : (int)(spanAge.TotalDays / 365.25);

            TimeSpan spanYears = tarih.AddDays(1).Subtract(personal.Igt);
            double workedYear = spanYears.TotalDays < 0 ? 0 : spanYears.TotalDays / 365.25;
            workedYear = Math.Floor(workedYear);
            workedYear = last ? workedYear + 1 : workedYear;

            TimeSpan spanDays = tarih.Subtract(lastPeriodStart);
            int workedDay = spanDays.TotalDays < 0 ? 0 : (int)spanDays.TotalDays;
            if(unPaid > 0)
            {
                spanDays = tarih.AddDays(366 - Convert.ToDouble(unPaid)).Subtract(tarih);
                workedDay = spanDays.TotalDays < 0 ? 0 : (int)spanDays.TotalDays;
            }

            if (age < 19 || (age > 50 && workedYear <= 15))
            {
                his = unPaid == 0 ? 20 : Math.Round(Convert.ToDecimal(workedDay) * 20 / 365m - unPaid);
            }
            else if (workedYear < 1)
            {
                his = 0;
            }
            else if (workedYear > 15)
            {
                his = unPaid == 0 ? 26 : Math.Round(Convert.ToDecimal(workedDay) * 26 / 365);
            }
            else if (workedYear >= 1 && workedYear <= 5)
            {
                his = unPaid == 0 ? 14 : Math.Round(Convert.ToDecimal(workedDay) * 14 / 365);
            }
            else if (workedYear >= 6 && workedYear <= 15)
            {
                his = unPaid == 0 ? 20 : Math.Round(Convert.ToDecimal(workedDay) * 20 / 365);
            }

            if (last)
            {
                his = Math.Round(Convert.ToDecimal(workedDay) * his / 365);
            }
            return his;
        }
        public decimal CalculateUsedLeave(string tcno)
        {
            decimal ki = 0;
            personal = (from x in GlobalVars.Personals where x.Tcno == tcno select x).FirstOrDefault();
            List<Leaves> leaves = (from x in GlobalVars.Leaves where x.Pid == personal.Id select x).ToList();
            if (leaves == null || leaves.Count == 0) return -1;
            foreach (Leaves alv in leaves)
            {
                if (alv.Enddate.Date < DateTime.Now.Date)
                {
                    ki += alv.Timeval;
                }
                else if (alv.Startdate.Date <= DateTime.Now.Date && alv.Enddate.Date >= DateTime.Now.Date)
                {
                    if (alv.Ph)
                    {
                        TimeSpan gunler = DateTime.Now.Date - alv.Startdate.Date;
                        ki += Convert.ToDecimal(gunler.Days);
                    }
                    else
                    {
                        for (DateTime day = alv.Startdate.Date; day.Date <= DateTime.Now.Date; day = day.AddDays(1))
                        {
                            if (GlobalVars.PublicHolidays == null)
                            {
                                break;
                            }
                            foreach (PublicHolidays ph in GlobalVars.PublicHolidays)
                            {
                                if ((ph.Day.Date == day.Date && ph.Ft) || day.Date.DayOfWeek == DayOfWeek.Sunday) continue;
                                else if (ph.Day.Date == day.Date && !ph.Ft) ki += 0.5m;
                                else ki += 1;
                            }
                        }
                    }
                }
            }
            return ki;
        }
        public decimal GetMaxLeaveDay(Personal personal)
        {
            decimal maxLeaveDays = 0;
            lstLeavePeriods = new List<LeavePeriod>();
            DateTime lastPeriodStart = GetLastPeriodStart(personal);

            int donem = 1;
            for (DateTime day = personal.Igt.Date; day <= lastPeriodStart.AddDays(-365); day = day.AddDays(365))
            {
                decimal his = CalculateDeservedLeave(personal, day.Date.AddDays(365), 0);
                LeavePeriod lp = new LeavePeriod()
                {
                    Tcno = personal.Tcno,
                    Period = $"{donem}. DÖNEM",
                    Startdate = day,
                    Enddate = day.AddDays(365),
                    His = his,
                    Ki = his,
                    Srk = 0,
                };

                day = day.AddDays(1);
                donem++;
                lstLeavePeriods.Add(lp);
            }
            //decimal hisCurrent = CalculateDeservedLeave(personal, DateTime.Now.Date, 0, true);
            //LeavePeriod lpCurrent = new LeavePeriod()
            //{
            //    tcno = personal.Tcno,
            //    period = $"{donem}. DÖNEM",
            //    startdate = lastPeriodStart,
            //    enddate = DateTime.Now.Date,
            //    his = hisCurrent,
            //    ki = 0,
            //    srk = hisCurrent,
            //};
            //lstLeavePeriods.Add(lpCurrent);
            maxLeaveDays = (from x in lstLeavePeriods select x.His).Sum();
            return maxLeaveDays;
        }
        public void SetPersonalLeaves(List<Leaves> _PersonalLeavesAll)
        {
            PersonalLeavesAll = _PersonalLeavesAll;
        }
        public List<Leaves> GetPersonalLeaves()
        {
            return PersonalLeavesAll;
        }
        public void RemoveLeaveFromList(long id)
        {
            PersonalLeavesAll.RemoveAll(x => x.Id == id);
        }
        public void AddLeaveToList(Leaves leaves)
        {
            PersonalLeavesAll.Add(leaves);
        }
        public void SetPersonalLeavePeriods(string tcno , out string msg)
        {
            lstLeavePeriodsOfPersonal = IOC.LeavePeriodDataService.GetLeavePeriodsByTcno(tcno, out msg);
        }
        public List<LeavePeriod> GetPersonalLeavePeriods()
        {
            return lstLeavePeriodsOfPersonal;
        }
        public List<LeavePeriod> GetLeavePeriods()
        {
            return lstLeavePeriods;
        }
        public void SetPeriods(Personal personal)
        {
            string msg = ""; 
            lstLeavePeriods = new List<LeavePeriod>();
            DateTime lastPeriodStart = GetLastPeriodStart(personal);
            decimal timevalUnpaid = 0, timevalPaidTotal = 0;
            int donem = 1;
            List<Leaves> leaves = IOC.LeaveDataService.GetLeavesByPersonalId(personal.Id, out msg);
            //if (leaves == null) { IOC.PersonalDataService.DeletePersonal(personal.Id, out msg); return; }
            for (DateTime day = personal.Igt.Date; day <= lastPeriodStart.AddDays(-365); day = day.AddDays(365))
            {
                decimal timevalPaid = 0;
                DateTime sd = day.Date;
                DateTime ed = day.AddDays(365).Date;
                if(leaves != null)
                {
                    timevalUnpaid = (from q in leaves where !q.Paid && q.Startdate >= sd && q.Enddate <= ed select q.Timeval).Sum();
                    timevalPaid = (from q in leaves where q.Paid && q.Startdate >= sd && q.Enddate <= ed select q.Timeval).Sum();
                }

                decimal his = CalculateDeservedLeave(personal, day.Date.AddDays(Convert.ToDouble(365)), timevalUnpaid);
                LeavePeriod lp = new LeavePeriod()
                {
                    Tcno = personal.Tcno,
                    Period = $"{donem}. DÖNEM",
                    Startdate = day,
                    Enddate = day.AddDays(365),
                    His = his,
                    Ki = his , 
                    Srk = 0 ,
                };

                day = day.AddDays(1);
                donem++;
                lstLeavePeriods.Add(lp);
                timevalPaidTotal += timevalPaid;
            }
            
            // alttaki satırı geçmişe dönük izin girilebilecekse aç
            //timevalUnpaid = (from q in leaves where !q.Paid && q.Startdate >= lastPeriodStart.Date && q.Enddate <= DateTime.Now.Date select q.Timeval).Sum();
            DateTime lastDate = personal.Active ? DateTime.Now : personal.Ict;
            // alttaki satırı geçmişe dönük izin girilebilecekse aç
            //decimal hisCurrent = CalculateDeservedLeave(personal, lastDate, timevalUnpaid, true);
            LeavePeriod lpCurrent = new LeavePeriod()
            {
                Tcno = personal.Tcno,
                Period = $"{donem}. DÖNEM",
                Startdate = lastPeriodStart,
                Enddate = lastDate,
                His = 0, // hisCurrent, 
                Ki = 0,
                Srk = 0, //hisCurrent, 
            };
            lstLeavePeriods.Add(lpCurrent);
            //personal.Tih -= timevalPaidTotal;
            if (personal.Tih > 0)
            {
                decimal tih = personal.Tih;
                int i = lstLeavePeriods.Count - 1;
                while (tih > 0)
                {
                    if (tih >= lstLeavePeriods[i - 1].Ki)
                    {
                        lstLeavePeriods[i - 1].Srk = lstLeavePeriods[i - 1].Ki;
                        lstLeavePeriods[i - 1].Ki = 0;
                        tih -= lstLeavePeriods[i - 1].His;
                    }
                    else
                    {
                        lstLeavePeriods[i - 1].Ki -= tih; 
                        lstLeavePeriods[i - 1].Srk += tih;
                        tih = 0;
                    }
                    i--;
                }
            }
        }
        public decimal SetTimeVal(bool ph, DateRange dr)
        {
            decimal timeval = 0;
            if (ph)
            {
                timeval = Convert.ToInt32((dr.Ed.Date - dr.Sd.Date).TotalDays) + 1;
            }
            else
            {
                for (DateTime day = dr.Sd.Date; day <= dr.Ed.Date; day = day.AddDays(1))
                {
                    if (day.DayOfWeek == DayOfWeek.Sunday) continue;
                    List<PublicHolidays> phs = (from x in GlobalVars.PublicHolidays where x.Day.Date == day.Date select x).ToList();
                    if (phs == null || phs.Count == 0) // gün, resmi tatile denk gelmiyorsa
                    {
                        timeval += 1;
                    }
                    else if (phs != null && phs.Count == 1 && !phs[0].Ft)// gün, bir tane resmi tatile denk geliyorsa
                    {
                        timeval += 0.5m;
                    }
                    else if (phs != null && phs.Count > 1) // gün, birden fazla resmi tatile denk geliyorsa
                    {
                        bool ft = false;
                        foreach (PublicHolidays p in phs) 
                        {
                            if (p.Ft) { ft = true; break; } // bu tatillerden en az birisi tam gün ise
                        }
                        if (!ft) timeval += 0.5m; 
                    }
                }
            }
            return timeval;
        }
        public DateTime GetLastPeriodStart(Personal personal)
        {
            lastPeriodStart = personal.Igt;
            if (personal.Igt.Month < DateTime.Now.Month)
            {
                while (lastPeriodStart.Year < DateTime.Now.Year)
                {
                    lastPeriodStart = lastPeriodStart.AddDays(366);
                }
            }
            else if (personal.Igt.Month > DateTime.Now.Month)
            {
                while (lastPeriodStart.Year < DateTime.Now.Year - 1)
                {
                    lastPeriodStart = lastPeriodStart.AddDays(366);
                }
            }
            else
            {
                if (personal.Igt.Day < DateTime.Now.Day)
                {
                    while (lastPeriodStart.Year < DateTime.Now.Year)
                    {
                        lastPeriodStart = lastPeriodStart.AddDays(366);
                    }
                }
                else if (personal.Igt.Day > DateTime.Now.Day)
                {
                    while (lastPeriodStart.Year < DateTime.Now.Year - 1)
                    {
                        lastPeriodStart = lastPeriodStart.AddDays(366);
                    }
                }
                else
                {
                    while (lastPeriodStart.Year < DateTime.Now.Year)
                    {
                        lastPeriodStart = lastPeriodStart.AddDays(366);
                    }
                }
                if (lastPeriodStart.Date > DateTime.Now.Date) lastPeriodStart = lastPeriodStart.AddDays(-366);
            }

            return lastPeriodStart;
        }
        public void AddOrUpdateLeaves(Leaves LEAVE, out string msg)
        {
            personal = (from q in GlobalVars.Personals where q.Id == LEAVE.Pid select q).FirstOrDefault();
            decimal usedTimevalOfPersonal = 0; // personelin kaydedilmiş ücretli ve avans olmayan izinlerin toplam gün sayısı
            decimal kisTotal = 0; // periodlardaki kullanılmayan izinlerin toplamı
            timevalOfDeletedLeave = 0;
            timeval = IOC.PersonalService.SetTimeVal(LEAVE.Ph, new DateRange() { Sd = LEAVE.Startdate.Date, Ed = LEAVE.Enddate.Date });
            SetPersonalLeaves(IOC.LeaveDataService.GetLeavesByPersonalId(personal.Id, out msg)); // personelin kayıtlı bütün izinlerini al

            List<Leaves> leavesSEPERATED = IOC.PersonalService.IsLeaveSeperated(LEAVE, out msg);
            SetPersonalLeavePeriods(personal.Tcno, out msg);
            if (leavesSEPERATED == null) { msg = $"Hata: {msg}"; return; }

            DialogResult dialogResult = DialogResult.OK;

            if (leavesSEPERATED.Count > 1)
            {
                dialogResult = RadMessageBox.Show(
                    $"Eklemeye çalıştığınız izin {leavesSEPERATED.Count} farklı çalışma dönemine denk geldiği için birbirini takip eden {leavesSEPERATED.Count} yrı izin olarak tanımlanması gerekir",
                    $"İzin {leavesSEPERATED.Count} farklı çalışma dönemine denk geliyor!", MessageBoxButtons.OK,
                    RadMessageIcon.Info, $"Personel Adı: {personal.Ads}\r\nPeronal Tcno: {personal.Tcno}\r\nİzin Tarihi: {LEAVE.Startdate:dd.MM.yyyy} - {LEAVE.Enddate:dd.MM.yyyy}\r\nDÖNEMLER:\r\n{msg}");
                msg = "iptal"; return;
            }

            Leaves existLeave; bool willUpd;  string mes;
            
            (existLeave, willUpd, mes) = IsLeaveExists(LEAVE, out msg);
            if(mes.Contains("iptal") || mes.Contains("hata")) { msg = mes; return; }
            LEAVE.Id = existLeave != null? existLeave.Id : 0; if (LEAVE.Id > 0) { isUpdate = true; timevalOfDeletedLeave = existLeave.Timeval; }

            if (isUpdate)
            {
                int resultDel = IOC.LeaveDataService.DeleteLeave(LEAVE.Id, out msg);
                if (resultDel == -1) { msg = $"Hata: {msg}"; return; }
                personal.Tih += timevalOfDeletedLeave;
                IOC.PersonalService.RemoveLeaveFromList(LEAVE.Id);
                IOC.PersonalService.UpdatePersonalAfterLeavesAdd(personal, out msg);
            }

            List<Leaves> toBeAdded = CalculateTimeOut(personal.Tcno, personal.Ads, LEAVE);
            if (toBeAdded == null)
            {
                msg = $"{personal.Ads} için {LEAVE.Startdate:dd.MM.yyyy}-{LEAVE.Enddate:dd.MM.yyyy} arası {LEAVE.Timeval} günlük izin ekleme iptal edildi";
                if (isUpdate)
                { // işlem iptal edildiğinde silinmiş varsa geri yerine koy
                    IOC.LeaveDataService.AddLeave(LEAVE, out msg);
                    personal.Tih -= LEAVE.Timeval;
                    IOC.PersonalService.AddLeaveToList(LEAVE);
                    IOC.PersonalService.UpdatePersonalAfterLeavesAdd(personal, out msg);
                }
                return;
            }
            int addRes = 0, updRes = 0;
            foreach (Leaves alv in toBeAdded)
            {
                usedTimevalOfPersonal = 0;
                kisTotal = (from x in GetPersonalLeavePeriods() select x.Ki).Sum();

                addRes += IOC.LeaveDataService.AddLeave(alv, out msg);
                usedTimevalOfPersonal = (from x in PersonalLeavesAll where x.Paid select x.Timeval).Sum() + (alv.Paid ? alv.Timeval : 0);
                if (usedTimevalOfPersonal > kisTotal && alv.Paid) { personal.Tih -= usedTimevalOfPersonal - kisTotal; }
                AddLeaveToList(alv);
                UpdatePersonalAfterLeavesAdd(personal, out msg);
                updRes = isUpdate ? 1 : 0;
            }

            if (addRes == 0 && updRes == 0) { msg = $"{personal.Ads} için {LEAVE.Startdate:dd.MM.yyyy}-{LEAVE.Enddate:dd.MM.yyyy} arası {LEAVE.Timeval} günlük izin zaten var"; }
            else if (addRes > 0 && updRes == 0) { msg = $"{personal.Tcno} kimlik numaralı personel için izin eklendi";  }
            else if (addRes == 0 && updRes > 0) { msg = $"{personal.Tcno} kimlik numaralı personelin izni güncellendi"; }
            else if (addRes < 0 || updRes < 0) { msg = $"Hata: {msg}";  }
            LeavesChanged.HasChanged = true; LeavesChanged.HasChangedForDialog = true;
        }
        public void UpdatePersonalAfterLeavesAdd(Personal personal, out string msg)
        {
            try
            {
                decimal totalHisBeforeUpdate, totalHisAfterUpdate;
                totalHisBeforeUpdate = (from x in lstLeavePeriods select x.His).Sum();
                IOC.PersonalDataService.AddPersonal(personal, out msg);
                SetPeriods(personal);
                IOC.LeavePeriodDataService.AddLeavePeriods(lstLeavePeriods, out msg);
                SetPersonalLeavePeriods(personal.Tcno, out msg);
                SetPersonalLeaves(IOC.LeaveDataService.GetLeavesByPersonalId(personal.Id, out msg));
                totalHisAfterUpdate = (from x in lstLeavePeriods select x.His).Sum();
                if (totalHisBeforeUpdate != totalHisAfterUpdate)
                {
                    if(totalHisBeforeUpdate > totalHisAfterUpdate) personal.Tih -= totalHisBeforeUpdate - totalHisAfterUpdate;
                    if(totalHisBeforeUpdate < totalHisAfterUpdate) personal.Tih += totalHisAfterUpdate - totalHisBeforeUpdate;
                    IOC.PersonalDataService.AddPersonal(personal, out msg);
                    SetPeriods(personal);
                    IOC.LeavePeriodDataService.AddLeavePeriods(lstLeavePeriods, out msg);
                    SetPersonalLeavePeriods(personal.Tcno, out msg);
                    SetPersonalLeaves(IOC.LeaveDataService.GetLeavesByPersonalId(personal.Id, out msg));
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            
        }
        public (bool, string) GetLeavesTcnoAll( out string msg)
        {
            bool isOk = true; string rgvTitle = "";
            GlobalVars.PersonalsForLeave = (from x in GlobalVars.Personals where x.Tcno == SearchReport.KimlikNo select x).ToList();
            if (GlobalVars.PersonalsForLeave == null || GlobalVars.PersonalsForLeave.Count == 0) { msg = $"{SearchReport.KimlikNo} kimlik numarası personel veri tabanında kayıtlı değil"; return (false, msg); }
            try
            {
                GlobalVars.Leaves = IOC.LeaveDataService.GetLeavesByPersonalId(GlobalVars.PersonalsForLeave[0].Id, out msg);

                SetPeriods(GlobalVars.PersonalsForLeave[0]);
                int res = IOC.LeavePeriodDataService.AddLeavePeriods(lstLeavePeriods, out msg);
                if(res < 0) { msg = $"Hata: {msg}"; return (false, msg); }
                GlobalVars.LeavePeriods = lstLeavePeriods; //IOC.leavePeriodDataService.GetLeavePeriodByTcno(SearchReport.KimlikNo, out msg);

                if (GlobalVars.LeavePeriods == null) { msg = $"Hata: {msg}"; return (false, msg); }
                else if (GlobalVars.LeavePeriods.Count == 0) { return (false, $"{SearchReport.KimlikNo} kimlik numaralı personelin çalışma dönemleri kaydı bulunamadı"); }

                if (GlobalVars.Leaves == null) { msg = $"Hata: {msg}"; return (false, msg); }
                else if (GlobalVars.Leaves.Count == 0)
                {
                    rgvTitle = $"{SearchReport.KimlikNo} kimlik numaralı personelin {GlobalVars.LeavePeriods.Count} adet çalışma dönemi içinde izin kaydı bulunamadı";
                }
                else
                {
                    rgvTitle = $"{SearchReport.KimlikNo} kimlik numaralı personelin {GlobalVars.LeavePeriods.Count} adet çalışma dönemi içinde {GlobalVars.Leaves.Count} adet izin kaydı bulundu";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                isOk = false; rgvTitle = "";
            }
            return (isOk, rgvTitle);
        }
        public (bool, string) GetLeavesTcnoAndDate(DateTime sd, DateTime ed, out string msg)
        {
            bool isOk = true; string rgvTitle = "";
            List<LeavePeriod> leavePeriods = new List<LeavePeriod>();
            GlobalVars.PersonalsForLeave = (from x in GlobalVars.Personals where x.Tcno == SearchReport.KimlikNo select x).ToList();
            if (GlobalVars.PersonalsForLeave == null || GlobalVars.PersonalsForLeave.Count == 0) { msg = $"{SearchReport.KimlikNo} kimlik numarası personel veri tabanında kayıtlı değil"; return (false, msg); }
            try
            {
                GlobalVars.Leaves = IOC.LeaveDataService.GetLeavesByPidAndDate(GlobalVars.PersonalsForLeave[0].Id, sd, ed, out msg);
                if (GlobalVars.Leaves == null) { msg = $"Hata: {msg}"; return (false, msg); }
                
                DateTime minSD = (from x in GlobalVars.Leaves orderby x.Startdate select x.Startdate).FirstOrDefault();
                DateTime maxEd = (from x in GlobalVars.Leaves orderby x.Enddate descending select x.Enddate).FirstOrDefault();

                SetPeriods(GlobalVars.PersonalsForLeave[0]);

                int res = IOC.LeavePeriodDataService.AddLeavePeriods(lstLeavePeriods, out msg);
                if (res < 0) { msg = $"Hata: {msg}"; return (false, msg); }
                GlobalVars.LeavePeriods = (from x in GlobalVars.LeavePeriods where x.Startdate >= sd select x).ToList();

                if (GlobalVars.LeavePeriods == null) { msg = $"Hata: {msg}"; return (false, msg); }
                else if (GlobalVars.LeavePeriods.Count == 0) { return (false, $"{SearchReport.KimlikNo} kimlik numaralı personelin çalışma dönemleri kaydı bulunamadı"); }

                if (GlobalVars.Leaves.Count == 0)
                {
                    rgvTitle = $"{SearchReport.KimlikNo} kimlik numaralı personelin {GlobalVars.LeavePeriods.Count} adet çalışma dönemi içinde izin kaydı bulunamadı";
                }
                else
                {
                    rgvTitle = $"{SearchReport.KimlikNo} kimlik numaralı personelin {GlobalVars.LeavePeriods.Count} adet çalışma dönemi içinde {GlobalVars.Leaves.Count} adet izin kaydı bulundu";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                isOk = false; rgvTitle = "";
            }
            return (isOk, rgvTitle);
        }
        public (bool, string) GetLeavesCompanyAll(out string msg)
        {
            bool isOk = true; string rgvTitle = ""; 
            GlobalVars.LeavePeriods = new List<LeavePeriod>();
            GlobalVars.PersonalsForLeave = (from x in GlobalVars.Personals where GlobalVars.Iynos.Contains(x.Cid) select x).ToList();
            if (GlobalVars.PersonalsForLeave == null || GlobalVars.PersonalsForLeave.Count == 0) { 
                msg = GlobalVars.Iynos.Count == 1 ? $"{GlobalVars.IzinComp.CompanyName} adlı firmaya ait personel kaydı bulunamadı" : $"Seçilen {GlobalVars.Iynos.Count} adet firmaya ait personel kaydı bulunamadı"; 
                return (false, msg); }
            try
            {
                GlobalVars.Leaves = IOC.LeaveDataService.GetLeavesByCompanies(GlobalVars.Iynos, out msg);
                foreach (Personal prs in GlobalVars.PersonalsForLeave)
                {
                    SetPeriods(prs);
                    int res = IOC.LeavePeriodDataService.AddLeavePeriods(lstLeavePeriods, out msg);
                    GlobalVars.LeavePeriods.AddRange(lstLeavePeriods);
                }

                if (GlobalVars.LeavePeriods == null) { msg = $"Hata: {msg}"; return (false, msg); }
                else if (GlobalVars.LeavePeriods.Count == 0) { return (false, GlobalVars.Iynos.Count == 1 ? $"{GlobalVars.IzinComp.CompanyName} adlı firmanın personellerine ait çalışma dönemi kayıtları bulunamadı" : $"Seçilen {GlobalVars.Iynos.Count} firmaya ait çalışma dönemi kayıtları bulunamadı"); }

                if (GlobalVars.Leaves == null) { msg = $"Hata: {msg}"; return (false, msg); }
                else
                    rgvTitle = GlobalVars.Iynos.Count == 1 ? $"{GlobalVars.IzinComp.CompanyName} adlı firmada {GlobalVars.PersonalsForLeave.Count} adet personel bulundu" : $"Seçilen {GlobalVars.Iynos.Count} adet firmada {GlobalVars.PersonalsForLeave.Count} adet personel bulundu";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                isOk = false; rgvTitle = "";
            }
            return (isOk, rgvTitle);
        }
        public (bool, string) GetLeavesByCompaniesAndDate(DateTime sd, DateTime ed, out string msg)
        {
            bool isOk = true; string rgvTitle = "";
            GlobalVars.LeavePeriods = new List<LeavePeriod>();
            List<LeavePeriod> leavePeriods = new List<LeavePeriod>();
            GlobalVars.PersonalsForLeave = (from x in GlobalVars.Personals where GlobalVars.Iynos.Contains(x.Cid) && x.Igt <= ed select x).ToList();
            if (GlobalVars.PersonalsForLeave == null || GlobalVars.PersonalsForLeave.Count == 0) {
                msg = GlobalVars.Iynos.Count == 1 ? $"{sd:dd.MM.yyyy} - {ed:dd.MM.yyyy} tarihleri arasında, {GlobalVars.IzinComp.CompanyName} adlı firmaya ait personel kaydı bulunamadı" : $"{sd:dd.MM.yyyy} - {ed:dd.MM.yyyy} tarihleri arasında, seçilen {GlobalVars.Iynos.Count} adet firmaya ait personel kaydı bulunamadı";
                return (false, msg);
            }
            try
            {
                GlobalVars.Leaves = IOC.LeaveDataService.GetLeavesByCompaniesAndDate(GlobalVars.Iynos, sd, ed, out msg);
                foreach (Personal personal in GlobalVars.PersonalsForLeave)
                {
                    SetPeriods(personal);
                    int res = IOC.LeavePeriodDataService.AddLeavePeriods(lstLeavePeriods, out msg);
                    GlobalVars.LeavePeriods.AddRange(lstLeavePeriods);
                }
                List<long> pids = (from x in GlobalVars.Personals where x.Cid == GlobalVars.IzinComp.Id select x.Id).ToList();
                GlobalVars.LeavePeriods = (from x in GlobalVars.LeavePeriods where x.Startdate >= sd select x).ToList();

                if (GlobalVars.LeavePeriods == null) { msg = $"Hata: {msg}"; return (false, msg); }
                else if (GlobalVars.LeavePeriods.Count == 0) { return (false, GlobalVars.Iynos.Count == 1 ? $"{sd:dd.MM.yyyy} - {ed:dd.MM.yyyy} tarihleri arasında, {GlobalVars.IzinComp.CompanyName} adlı firmanın personellerine ait çalışma dönemi kayıtları bulunamadı" : $"{sd:dd.MM.yyyy} - {ed:dd.MM.yyyy} tarihleri arasında, seçilen {GlobalVars.Iynos.Count} adet firmanın personellerine ait çalışma dönemi kayıtları bulunamadı"); }

                
                if (GlobalVars.Leaves == null) { msg = $"Hata: {msg}"; return (false, msg); }
                else
                    rgvTitle = GlobalVars.Iynos.Count == 1 ? $"{sd:dd.MM.yyyy} - {ed:dd.MM.yyyy} tarihleri arasında, {GlobalVars.IzinComp.CompanyName} adlı firmada {GlobalVars.PersonalsForLeave.Count} adet personel bulundu" : $"{sd:dd.MM.yyyy} - {ed:dd.MM.yyyy} tarihleri arasında, seçilen {GlobalVars.Iynos.Count} firmada {GlobalVars.PersonalsForLeave.Count} adet personel bulundu";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                isOk = false; rgvTitle = "";
            }
            return (isOk, rgvTitle);
        }
        public (Leaves, bool, string)  IsLeaveExists(Leaves leave, out string msg)
        {
            msg = "";
            existingAlvtimeval = 0;
            //List<Leaves> alvs = PersonalLeavesAll; // IOC.LeaveDataService.GetLeavesByPersonalId(leaves.Pid, out msg); 
            if (PersonalLeavesAll == null ) { return (null, false, $"Veri tabanı hatası"); }
            else if (PersonalLeavesAll != null)
            {
                foreach (Leaves alv in PersonalLeavesAll)
                {
                    if (leave.Startdate.Date >= alv.Startdate.Date && leave.Startdate.Date <= alv.Enddate.Date || leave.Enddate.Date >= alv.Startdate.Date && leave.Enddate.Date <= alv.Enddate.Date)
                    {
                        string ads = (from x in GlobalVars.Personals where x.Id == leave.Pid select x.Ads).FirstOrDefault();
                        string tcno = (from x in GlobalVars.Personals where x.Id == leave.Pid select x.Tcno).FirstOrDefault();
                        DialogResult confirmResult = RadMessageBox.Show($"\"{tcno} kimlik numaralı {ads} için eklemeye çalıştığınız {leave.Startdate:dd.MM.yyyy} - {leave.Enddate:dd.MM.yyyy} tarihleri arasındaki izin \" {alv.Startdate:dd.MM.yyyy} - {alv.Enddate:dd.MM.yyyy} tarihleri arasındaki başka bir izinle çakışıyor. Var olan izin güncellensin mi?", "İzin tarihleri çakışması!", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                        if (confirmResult == DialogResult.Yes)
                        {
                            isUpdate = true;
                            leave.Id = alv.Id;
                            leave.Trxtype = "2";
                            existingAlvtimeval = alv.Timeval;
                            return (alv, isUpdate, "");
                        }
                        else
                        {
                            return (null, false, "İşlem iptal edildi");
                        }
                    }
                }
            }
            return (null, false, "");
        }

        public List<Leaves> IsLeaveSeperated(Leaves leave,  out string msg)
        {
            msg = "";
            List<Leaves> leaves = new List<Leaves>();
            string tcno = (from x in GlobalVars.Personals where x.Id == leave.Pid select x.Tcno).FirstOrDefault();
            
            List<LeavePeriod> leavePeriods = IOC.LeavePeriodDataService.GetLeavePeriodsByTcno(tcno, out msg);
            if (leavePeriods == null) return null;

            string firstPeriod = (from x in leavePeriods where leave.Startdate >= x.Startdate && leave.Startdate <= x.Enddate select x.Period).FirstOrDefault();
            string lastPeriod = (from x in leavePeriods where leave.Enddate >= x.Startdate && leave.Enddate <= x.Enddate select x.Period).FirstOrDefault();
            if (firstPeriod == lastPeriod) { leaves.Add(leave); return leaves; }
            else
            {
                int first = Convert.ToInt32(firstPeriod.Substring(0, firstPeriod.IndexOf(".")));
                int last  = Convert.ToInt32(lastPeriod.Substring(0, lastPeriod.IndexOf(".")));
                
                DateTime sd = leave.Startdate;
                int count = first;

                for (int i = first; i <= last; i++)
                {
                    LeavePeriod lp = (from x in leavePeriods where x.Period.StartsWith($"{i}.") select x).FirstOrDefault();
                    
                    DateTime ed = new DateTime();

                    if (count == first) ed = lp.Enddate;
                    else if(count == last) ed = leave.Enddate;
                    else ed = lp.Enddate;

                    decimal timeval = SetTimeVal(leave.Ph, new DateRange() { Sd = sd, Ed = ed });

                    Leaves lv = new Leaves() { Cid = leave.Cid, Cu = leave.Cu, Docref = leave.Docref, Startdate = sd, Enddate = ed, Notes = leave.Notes, Paid = leave.Paid, Ph = leave.Ph, Pid = leave.Pid, Trxdate = DateTime.Now, Cd = DateTime.Now, Timeunit = 1, Trxtype = "1", Timeval = timeval };
                    sd = lv.Enddate.AddDays(1);
                    leaves.Add(lv);
                    msg += $"  {i}. DÖNEM,\tBaşlangıç Tarih: {lp.Startdate:dd.MM.yyyy}\tBitiş Tarih: {lp.Enddate:dd.MM.yyyy}\r\n"; 
                    count++;
                }
            }
            return leaves;
        }

        public List<Leaves> CalculateTimeOut(string tcno, string ads,  Leaves alv)
        {
            decimal his, kis, kalan;
            string periodMes;
            LeavePeriod period;
            List<Leaves> returnLeaves = new List<Leaves>();
            if (!alv.Paid) { returnLeaves.Add(alv); return returnLeaves; }
            (his, kis, period) = GetDeservedDayOfPeriod(tcno, alv); alv.Period = period.Period;
            kalan = his - kis;
            if(alv.Id > 0) { kalan += existingAlvtimeval; };
            if(period.Period == "1. DÖNEM") { kalan = 0; periodMes = $"Eklemeye çalıştığınız {alv.Startdate:dd.MM.yyyy} - {alv.Enddate:dd.MM.yyyy} aralığındaki {alv.Timeval} günlük izin, personelin 1. çalışma dönemine denk geldiği için ücretli yıllık izin olarak eklenemez."; }else { periodMes = $"Eklemeye çalıştığınız {alv.Startdate:dd.MM.yyyy} - {alv.Enddate:dd.MM.yyyy} aralığındaki {alv.Timeval} günlük izin, bu dönem için kalan izin gününden ({kalan} gün kaldı) daha fazla."; }
            if (alv.Timeval > kalan)
            {
                
                string ques = $"<html>{periodMes} \r\nFazla olan günler ({alv.Timeval - kalan} gün) ücretsiz izin ya da avans izin olarak eklenebilir?\r\n\r\n<strong>Asistpro'nun ne yapmasını istersiniz?</strong></html>";
                
                string desc = $"Personelin TC kimlik numarası: {tcno}\r\nPersonelin adı soyadı: {ads}\r\nÇalışma Dönemi: {period.Period}\r\nÇalışma dönemi aralığı: {period.Startdate:dd.MM.yyyy} - {period.Enddate:dd.MM.yyyy}\r\nDönemde kullanılmış ücretli izin: {kis} gün\r\nKullanılabilecek ücretli izin: {kalan} gün";
                FTimeOut fTimeOut = new FTimeOut(ques, desc);

                DialogResult dr = fTimeOut.ShowDialog();

                if (dr == DialogResult.Yes)  // bir ücretli (kalan 0 değilse) bir ücretisz izin ekle
                {
                    if(kalan == 0) {
                        alv.Paid = false; returnLeaves.Add(alv); alv.Notes += " (Ücretsiz izin)";
                    }
                    else
                    {
                        Leaves lvPaid = new Leaves() { Cd = alv.Cd, Cid = alv.Cid, Cu = alv.Cu, Docref = alv.Docref, Notes = alv.Notes, Paid = true, Ph = alv.Ph, Timeunit = alv.Timeunit, Trxdate = alv.Trxdate, Trxtype = "1", Timeval = kalan  , Pid = alv.Pid, Period = period.Period };
                        DateRange dateRange =  GenerateNewLeave(new DateRange() { Sd = alv.Startdate, Ed = alv.Enddate }, kalan, alv.Ph);
                        lvPaid.Startdate = dateRange.Sd; lvPaid.Enddate = dateRange.Ed;

                        Leaves lvUnPaid = new Leaves() { Cd = alv.Cd, Cid = alv.Cid, Cu = alv.Cu, Docref = alv.Docref, Notes = $"{alv.Notes} (Ücretsiz izin)", Paid = false, Ph = alv.Ph, Timeunit = alv.Timeunit, Trxdate = alv.Trxdate, Trxtype = "1", Timeval = alv.Timeval - kalan , Pid = alv.Pid, Period = period.Period };
                        dateRange = GenerateNewLeave(new DateRange() { Sd = lvPaid.Enddate.AddDays(1), Ed = alv.Enddate }, alv.Timeval - kalan, alv.Ph);
                        lvUnPaid.Startdate = dateRange.Sd; lvUnPaid.Enddate = dateRange.Ed;
                        returnLeaves.Add(lvPaid); returnLeaves.Add(lvUnPaid);
                    }
                }
                else if (dr == DialogResult.No) // bir ücretli (kalan 0 değilse) bir avans izin ekle (AVANS İZİN NASIL OLACAK????)
                {
                    if (kalan == 0)
                    {
                        alv.Paid = true; alv.Notes += " (Avans izin olarak eklendi)"; alv.Avans = true;  returnLeaves.Add(alv);
                    }
                    else
                    {
                        Leaves lvPaid = new Leaves() { Cd = alv.Cd, Cid = alv.Cid, Cu = alv.Cu, Docref = alv.Docref, Notes = alv.Notes, Paid = true, Ph = alv.Ph, Timeunit = alv.Timeunit, Trxdate = alv.Trxdate, Trxtype = "1", Timeval = kalan  , Pid = alv.Pid, Period = period.Period };
                        DateRange dateRange = GenerateNewLeave(new DateRange() { Sd = alv.Startdate, Ed = alv.Enddate }, kalan, alv.Ph);
                        lvPaid.Startdate = dateRange.Sd; lvPaid.Enddate = dateRange.Ed;

                        Leaves lvAdvance = new Leaves() { Avans = true, Cd = alv.Cd, Cid = alv.Cid, Cu = alv.Cu, Docref = alv.Docref, Notes = $"{alv.Notes} (Avans izin olarak eklendi)", Paid = true, Ph = alv.Ph, Timeunit = alv.Timeunit, Trxdate = alv.Trxdate, Trxtype = "1", Timeval = alv.Timeval - kalan  , Pid = alv.Pid, Period = period.Period };
                        dateRange = GenerateNewLeave(new DateRange() { Sd = lvPaid.Enddate.AddDays(1), Ed = alv.Enddate }, alv.Timeval - kalan, alv.Ph);
                        lvAdvance.Startdate = dateRange.Sd; lvAdvance.Enddate = dateRange.Ed;
                        returnLeaves.Add(lvPaid); returnLeaves.Add(lvAdvance);
                    }
                }
                else if (dr == DialogResult.Cancel) // işlemi iptal et
                {
                    return null;
                }
            }
            else
            {
                returnLeaves.Add(alv);
            }
            return returnLeaves;
        }
        public (decimal, decimal, LeavePeriod) GetDeservedDayOfPeriod(string tcno, Leaves leave)
        {
            string msg = "";
            //lstLeavePeriodsOfPersonal = IOC.LeavePeriodDataService.GetLeavePeriodsByTcno(tcno, out msg);
            activePeriod = (from q in lstLeavePeriodsOfPersonal where q.Tcno == tcno && q.Startdate <= leave.Startdate && q.Startdate <= leave.Enddate && q.Enddate >= leave.Startdate && q.Enddate >= leave.Enddate select q).FirstOrDefault();
            int currentPeriod = Convert.ToInt32(activePeriod.Period.Substring(0, activePeriod.Period.IndexOf("."))) - 1;
            prevPeriod = currentPeriod != 0 ? (from x in lstLeavePeriodsOfPersonal where x.Period == $"{currentPeriod}. DÖNEM" select x).FirstOrDefault() : activePeriod;
            Personal prs = (from x in GlobalVars.Personals where x.Tcno == tcno select x).FirstOrDefault();
            List<Leaves> leaves = IOC.LeaveDataService.GetLeavesByPidAndDate(prs.Id, activePeriod.Startdate, activePeriod.Enddate, out msg);
            decimal kis = (from q in leaves where q.Pid == leave.Pid && q.Startdate >= activePeriod.Startdate && q.Enddate <= activePeriod.Enddate && q.Paid select q.Timeval).Sum();

            return (prevPeriod.His, kis, activePeriod);
        }
        public void SetNegativTih()
        {
            foreach (Personal personal in GlobalVars.PersonalsForLeave)
            {
                
            }
        }
        public DateRange GenerateNewLeave(DateRange dr, decimal timeval, bool ph)
        {
            
            DateTime ed = new DateTime();
            if (ph) ed = dr.Sd.AddDays(Convert.ToDouble(timeval));
            else
            {
                List<PublicHolidays> phs = (from x in GlobalVars.PublicHolidays where x.Day >= dr.Sd && x.Day <= dr.Ed select x).ToList();
                decimal gun = 0;
                for (DateTime day = dr.Sd.Date; day <= dr.Ed.Date; day = day.AddDays(1))
                {
                    if (day.DayOfWeek == DayOfWeek.Sunday) continue;
                    List<PublicHolidays> phss = (from x in phs where x.Day.Date == day.Date select x).ToList();
                    if (phss == null || phss.Count == 0) gun += 1;
                    else
                    {
                        bool ft = false;
                        foreach (PublicHolidays phsss in phss)
                        {
                            if (phsss.Ft) { ft = true; break; }
                        }
                        if (!ft) gun += 0.5m;
                    }
                    if (Math.Round(gun) == Math.Round(timeval)) { ed = day; break; }
                }

            }
            return new DateRange() { Sd = dr.Sd, Ed = ed };
        }
        public void AskBranchs(int cid)
        {
            DialogResult result = RadMessageBox.Show($"{GlobalVars.IzinComp.CompanyName} adlı firmaya bağlı diğer firmalar da aramaya dahil edilsin mi?", $"{GlobalVars.IzinComp.CompanyName}", MessageBoxButtons.YesNo, RadMessageIcon.Question);
            if (result == DialogResult.Yes)
                GlobalVars.Iynos.AddRange((from x in GlobalVars.Companies where x.Fm == cid select x.Id).ToList());
            else
                GlobalVars.Iynos.Add(cid);
            GlobalVars.Iynos = GlobalVars.Iynos.Distinct().ToList();
        }
    }
}