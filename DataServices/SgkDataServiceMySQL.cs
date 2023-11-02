using Dapper;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServices
{
    public class SgkDataServiceMySql : DbBaseMySql, ISgkDataService
    {
        public class EtCommand : DbBaseMySql
        {
            public int Insert(SgkEt s)
            {
                string query = "INSERT INTO sgket (cn, tt, yil, ay, bt, ttr, cx, un, cd) VALUES(@cn, @tt, @yil, @ay, @bt, @ttr, @cx, @un, @cd)";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    tt = s.Tt,
                    yil = s.Yil,
                    ay = s.Ay,
                    bt = s.Bt,
                    ttr = s.Ttr,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd
                }).Result;
            }
            public int Delete(int cx)
            {
                string query = "DELETE FROM sgket WHERE cx = @cx";
                return ConLocal.ExecuteAsync(query, new { cx = cx }).Result;

            }
            public int Update(SgkEt s)
            {
                string query = @"UPDATE sgket SET 
                        cn = @cn,
                        tt = @tt,
                        yil = @yil,
                        ay = @ay,
                        bt = @bt,
                        ttr = @ttr,
                        cx = @cx,
                        un = @un,
                        cd = @cd
                    WHERE cx = @conditionId";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    tt = s.Tt,
                    yil = s.Yil,
                    ay = s.Ay,
                    bt = s.Bt,
                    ttr = s.Ttr,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd,
                    conditionId = s.Cx
                }).Result;

            }
        }
        public class MeCommand : DbBaseMySql
        {
            public int Insert(SgkMe s)
            {
                string query = "INSERT INTO sgkme (cn, byt, etr, tur, cx, un, cd) VALUES(@cn, @byt, @etr, @tur, @cx, @un, @cd)";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    byt = s.Byt,
                    etr = s.Etr,
                    tur = s.Tur,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd
                }).Result;
            }
            public int Delete(int cx)
            {
                string query = "DELETE FROM sgkme WHERE cx = @cx";
                return ConLocal.ExecuteAsync(query, new { cx = cx }).Result;

            }
            public int Update(SgkMe s)
            {
                string query = @"UPDATE sgkme SET 
                        cn = @cn,
                        byt = @byt,
                        etr = @etr,
                        tur = @tur,
                        cx = @cx,
                        un = @un,
                        cd = @cd
                    WHERE cx = @conditionId";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    byt = s.Byt,
                    etr = s.Etr,
                    tur = s.Tur,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd,
                    conditionId = s.Cx
                }).Result;

            }
        }
        public class CrCommand : DbBaseMySql
        {
            public int Insert(SgkCr s)
            {
                string query = "INSERT INTO sgkcr (cn, kn, ty, tn, bt, ba, gz, tm, tp cx, un, cd) VALUES(@cn, @kn, @ty, @tn, @bt, @ba, @gz, @tm, @tp, @cx, @un, @cd)";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    kn = s.Kn,
                    ty = s.Ty,
                    bt = s.Bt,
                    ba = s.Ba,
                    gz = s.Gz,
                    tm = s.Tm,
                    tp = s.Tp,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd
                }).Result;
            }
            public int Delete(int cx)
            {
                string query = "DELETE FROM sgkcr WHERE cx = @cx";
                return ConLocal.ExecuteAsync(query, new { cx = cx }).Result;

            }
            public int Update(SgkCr s)
            {
                string query = @"UPDATE sgkcr SET 
                        cn = @cn,
                        kn = @kn,
                        ty = @ty,
                        bt = @bt,
                        ba = @ba,
                        gz = @gz,
                        tm = @tm,
                        tp = @tp,
                        cx = @cx,
                        un = @un,
                        cd = @cd
                    WHERE cx = @conditionId";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    kn = s.Kn,
                    ty = s.Ty,
                    bt = s.Bt,
                    ba = s.Ba,
                    gz = s.Gz,
                    tm = s.Tm,
                    tp = s.Tp,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd,
                    conditionId = s.Cx
                }).Result;

            }
        }
        public class Command6661 : DbBaseMySql
        {
            public int Insert(Sgk6661 s)
            {
                string query = "INSERT INTO sgk6661 (cn, yil, ay, fgs, dt, tt, cx, un, cd)" +
                               "VALUES(@cn, @yil, @ay, @fgs, @dt, @tt, @cx, @un, @cd)";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    yil = s.Yil,
                    ay = s.Ay,
                    fgs = s.Fgs,
                    dt = s.Dt,
                    tt = s.Tt,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd
                }).Result;
            }
            public int Delete(int cx, string yil)
            {
                string query = "DELETE FROM sgk6661 WHERE cx = @cx AND yil = @yil";
                return ConLocal.ExecuteAsync(query, new { cx = cx, yil = yil }).Result;

            }
            public int Update(Sgk6661 s)
            {
                string query = @"UPDATE sgk6661 SET 
                    cn = @cn,
                    yil = @yil,
                    ay = @ay,
                    fgs = @fgs,
                    dt = @dt,
                    tt = @tt,
                    cx = @cx,
                    un = @un,
                    cd = @cd
                    WHERE cx = @conditionId";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    yil = s.Yil,
                    ay = s.Ay,
                    fgs = s.Fgs,
                    dt = s.Dt,
                    tt = s.Tt,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd,
                    conditionId = s.Cx
                }).Result;

            }
        }
        public class IglCommand : DbBaseMySql
        {
            public int Insert(SgkIgl s)
            {
                string query = "INSERT INTO sgkigl (cn, tc, ads, gc, tr, stn, ipc, isl, ist, isa, cx, un, cd)" +
                               "VALUES(@cn, @tc, @ads, @gc, @tr, @stn, @ipc, @isl, @ist, @isa, @cx, @un, @cd)";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    tc = s.Tc,
                    ads = s.Ads,
                    gc = s.Gc,
                    tr = s.Tr,
                    stn = s.Stn,
                    ipc = s.Ipc,
                    isl = s.Isl,
                    ist = s.Ist,
                    isa = s.Isa,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd
                }).Result;
            }
            public int Delete(int cx)
            {
                string query = "DELETE FROM sgkigl WHERE cx = @cx";
                return ConLocal.ExecuteAsync(query, new { cx = cx }).Result;

            }
            public int Update(SgkIgl s)
            {
                string query = @"UPDATE sgkigl SET 
                    cn = @cn,
                    tc = @tc,
                    ads = @ads,
                    gc = @gc,
                    tr = @tr,
                    stn = @stn,
                    ipc = @ipc,
                    isl = @isl,
                    ist = @ist,
                    isa = @isa,
                    cx = @cx,
                    un = @un,
                    cd = @cd
                     WHERE id = @conditionId";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    tc = s.Tc,
                    ads = s.Ads,
                    gc = s.Gc,
                    tr = s.Tr,
                    stn = s.Stn,
                    ipc = s.Ipc,
                    isl = s.Isl,
                    ist = s.Ist,
                    isa = s.Isa,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd,
                    conditionId = s.Id
                }).Result;

            }
        }
        public class HlpCommand : DbBaseMySql
        {
            public int Insert(SgkHlp s)
            {
                string query = "INSERT INTO sgkhlp (cn, tcno, ads, utl, itl, gun, ucg, eGun, gGun, cGun, icn, egn, mk, ya, bm, bt, kk, cx, cd, ttl, pdfid)" +
                               "VALUES(@cn, @tcno, @ads, @utl, @itl, @gun, @ucg, @eGun, @gGun, @cGun, @icn, @egn, @mk, @ya, @bm, @bt, @kk, @cx, @cd, @ttl, @pdfid)";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    tcno = s.Tcno,
                    ads = s.Ads,
                    utl = s.Utl,
                    itl = s.Itl,
                    gun = s.Gun,
                    ucg = s.Ucg,
                    eGun = s.EGun,
                    gGun = s.GGun,
                    cGun = s.CGun,
                    icn = s.Icn,
                    egn = s.Egn,
                    mk = s.Mk,
                    ya = s.Ya,
                    bm = s.Bm,
                    bt = s.Bt,
                    kk = s.Kk,
                    cx = s.Cx,
                    cd = s.Cd,
                    ttl = s.Ttl,
                    pdfid = s.Pdfid
                }).Result;
            }
            public int Delete(int cx)
            {
                string query = "DELETE FROM sgkhlp WHERE cx = @cx";
                return ConLocal.ExecuteAsync(query, new { cx = cx }).Result;

            }
            public int Update(SgkHlp s)
            {
                string query = @"UPDATE sgkhlp SET 
                    cn = @cn,
                    tcno = @tcno,
                    ads = @ads,
                    utl = @utl,
                    itl = @itl,
                    gun = @gun,
                    eGun = @egn,
                    gGun = @gGun,
                    cGun = @cGun,
                    icn = @icn,
                    egn = @egn,
                    mk = @mk,
                    ya = @ya,
                    bm = @bm,
                    bt = @bt,
                    kk = @kk,
                    cx = @cx,
                    cd = @cd,
                    ttl = @ttl,
                    pdfid = @pdfid
                    WHERE id = @conditionId";
                return ConLocal.Execute(query, new
                {
                    cn = s.Cn,
                    tcno = s.Tcno,
                    ads = s.Ads,
                    utl = s.Utl,
                    itl = s.Itl,
                    gun = s.Gun,
                    ucg = s.Ucg,
                    eGun = s.EGun,
                    gGun = s.GGun,
                    cGun = s.CGun,
                    icn = s.Icn,
                    egn = s.Egn,
                    mk = s.Mk,
                    ya = s.Ya,
                    bm = s.Bm,
                    bt = s.Bt,
                    kk = s.Kk,
                    cx = s.Cx,
                    cd = s.Cd,
                    ttl = s.Ttl,
                    pdfid = s.Pdfid,
                    conditionId = s.Id
                });

            }
        }
        public class HlCommand : DbBaseMySql
        {
            public int Insert(SgkHl s)
            {
                string query = "INSERT INTO sgkhl (cn, tya, hya, bt, bm, kn, tcs, tgs, tpt, pdfPath, cx, un, cd)" +
                               "VALUES(@cn, @tya, @hya, @bt, @bm, @kn, @tcs, @tgs, @tpt, @pdfPath, @cx, @un, @cd)";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    tya = s.Tya,
                    hya = s.Hya,
                    bt = s.Bt,
                    bm = s.Bm,
                    kn = s.Kn,
                    tcs = s.Tcs,
                    tgs = s.Tgs,
                    tpt = s.Tpt,
                    pdfPath = s.PdfPath,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd
                }).Result;
            }
            public int Delete(int cx)
            {
                string query = "DELETE FROM sgkhl WHERE cx = @cx";
                return ConLocal.ExecuteAsync(query, new { cx = cx }).Result;

            }
            public int Update(SgkHl s)
            {
                string query = @"UPDATE sgkhl SET 
                    cn = @cn,
                    tya = @tya,
                    hya = @hya,
                    bt = @bt,
                    bm = @bm,
                    kn = @kn,
                    tcs = @tcs,
                    tgs = @tgs,
                    tpt = @tpt,
                    pdfPath = @pdfPath,
                    cx = @cx,
                    un = @un,
                    cd = @cd
                     WHERE cx = @conditionId";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    tya = s.Tya,
                    hya = s.Hya,
                    bt = s.Bt,
                    bm = s.Bm,
                    kn = s.Kn,
                    tcs = s.Tcs,
                    tgs = s.Tgs,
                    tpt = s.Tpt,
                    pdfPath = s.PdfPath,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd,
                    conditionId = s.Cx
                }).Result;

            }
        }
        public class DbCommand : DbBaseMySql
        {
            public int Insert(SgkDb s)
            {
                string query = "INSERT INTO sgkdb (cn, yil, ay, drm, pb, pbGz, ipcb, ipcbGz, ekpb, ekpbGz, oivb, oivbGz, ib, ibGz, dvb, dvbGz, dpgzb, dpgzbGz, dekpgzb, sbt, sbtGz, cx, un, cd)" +
                               "VALUES(@cn, @yil, @ay, @drm, @pb, @pbGz, @ipcb, @ipcbGz, @ekpb, @ekpbGz, @oivb, @oivbGz, @ib, @ibGz, @dvb, @dvbGz, @dpgzb, @dpgzbGz, @dekpgzb, @sbt, @sbtGz, @cx, @un, @cd)";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    yil = s.Yil,
                    ay = s.Ay,
                    drm = s.Drm,
                    pb = s.Pb,
                    pbGz = s.PbGz,
                    ipcb = s.Ipcb,
                    ipcbGz = s.IpcbGz,
                    ekpb = s.Ekpb,
                    ekpbGz = s.EkpbGz,
                    oivb = s.Oivb,
                    oivbGz = s.OivbGz,
                    ib = s.Ib,
                    ibGz = s.IbGz,
                    dvb = s.Dvb,
                    dvbGz = s.DvbGz,
                    dpgzb = s.Dpgzb,
                    dpgzbGz = s.DpgzbGz,
                    dekpgzb = s.Dekpgzb,
                    sbt = s.Sbt,
                    sbtGz = s.SbtGz,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd
                }).Result;
            }
            public int Delete(int cx)
            {
                string query = "DELETE FROM sgkdb WHERE cx = @cx";
                return ConLocal.ExecuteAsync(query, new { cx = cx }).Result;

            }
            public int Update(SgkDb s)
            {
                string query = @"UPDATE sgkdb SET 
                    cn = @cn, 
                    yil = @yil, 
                    ay = @ay, 
                    drm = @drm, 
                    pb = @pb, 
                    pbGz = @pbGz, 
                    ipcb = @ipcb, 
                    ipcbGz = @ipcbGz, 
                    ekpb = @ekpb, 
                    ekpbGz = @ekpbGz, 
                    oivb = @oivb, 
                    oivbGz = @oivbGz, 
                    ib = @ib, 
                    ibGz = @ibGz, 
                    dvb = @dvb, 
                    dvbGz = @dvbGz, 
                    dpgzb = @dpgzb, 
                    dpgzbGz = @dpgzbGz, 
                    dekpgzb = @dekpgzb, 
                    sbt = @sbt, 
                    sbtGz = @sbtGz, 
                    cx = @cx, 
                    un = @un, 
                    cd = @cd
                    WHERE id = @conditionId";
                return ConLocal.ExecuteAsync(query, new
                {
                    cn = s.Cn,
                    yil = s.Yil,
                    ay = s.Ay,
                    drm = s.Drm,
                    pb = s.Pb,
                    pbGz = s.PbGz,
                    ipcb = s.Ipcb,
                    ipcbGz = s.IpcbGz,
                    ekpb = s.Ekpb,
                    ekpbGz = s.EkpbGz,
                    oivb = s.Oivb,
                    oivbGz = s.OivbGz,
                    ib = s.Ib,
                    ibGz = s.IbGz,
                    dvb = s.Dvb,
                    dvbGz = s.DvbGz,
                    dpgzb = s.Dpgzb,
                    dpgzbGz = s.DpgzbGz,
                    dekpgzb = s.Dekpgzb,
                    sbt = s.Sbt,
                    sbtGz = s.SbtGz,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd,
                    conditionId = s.Id
                }).Result;

            }
        }


        EtCommand etcmd = new EtCommand();
        MeCommand mecmd = new MeCommand();
        CrCommand crcmd = new CrCommand();
        Command6661 cmd6661 = new Command6661();
        IglCommand iglcmd = new IglCommand();
        HlpCommand hlpcmd = new HlpCommand();
        HlCommand hlcmd = new HlCommand();
        DbCommand dbcmd = new DbCommand();

        public List<SgkDb> GetAllDb(out string msg)
        {
            msg = "";
            try
            {
                List<SgkDb> lst = new List<SgkDb>();
                string query = "SELECT * FROM sgkdb";
                lst = ConLocal.Query<SgkDb>(query).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<SgkDb> GetAllDb(List<int> cxs, out string msg)
        {
            msg = "";
            List<SgkDb> lst = null;
            try
            {
                lst = GetAllDb(out msg).FindAll(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public List<SgkDb> GetAllDb(int cx, string yil, string ay, out string msg)
        {
            msg = "";
            List<SgkDb> lst = null;
            try
            {
                lst = GetAllDb(out msg).FindAll(q => q.Cx == cx && q.Yil == yil && q.Ay == ay);  //  Query.And(Query.EQ("cx", cx), Query.EQ("yil", yil), Query.EQ("ay", ay))).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int IsDbExists(int cx, string yil, string ay, out string msg)
        {
            msg = "";
            try
            {
                SgkDb item = new SgkDb();
                //item.cx = cx;
                //item.yil = yil;
                //item.ay = ay;
                string sql = $"SELECT id, cn, yil, ay, drm, pb, pbGz, ipcb, ipcbGz, ekpb, ekpbGz, oivb, oivbGz, ib, ibGz, dvb, dvbGz, dpgzb, dpgzbGz, dekpgzb, sbt, sbtGz, cx, un, cd from sgkdb WHERE cx = @cx AND yil = @yil AND ay = @ay";
                item = ConLocal.QueryAsync<SgkDb>(sql, new { cx, yil, ay }).Result.FirstOrDefault();
                //bool x = GetAllDBData(out msg).Contains(item); //   Query.And(Query.EQ("cx", cx), Query.EQ("yil", yil), Query.EQ("ay", ay)));
                if (item != null)
                {
                    return item.Id;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public (int, int) AddPd(List<SgkDb> lst, out string msg)
        {
            int resultEkle = 0, resultGuncelle = 0;
            List<SgkDb> lstGuncelle = new List<SgkDb>();
            List<SgkDb> lstUpdate = new List<SgkDb>();
            List<SgkDb> lstEkle = new List<SgkDb>();
            msg = "";
            try
            {
                foreach (SgkDb sd in lst)
                {
                    int isExists = IsDbExists(sd.Cx, sd.Yil, sd.Ay, out msg);
                    if (isExists == 0)
                    {
                        int sonuc = dbcmd.Insert(sd);
                        if (sonuc >= 1) resultEkle++;
                    }
                    else if (isExists >= 1)
                    {
                        sd.Id = isExists;
                        if (sd.Id == -1) { return (-1, -1); }
                        resultGuncelle = (dbcmd.Update(sd) > 0 ? true : false) ? resultGuncelle + 1 : resultGuncelle;
                    }
                    else
                    {
                        resultGuncelle = -1; resultEkle = -1;
                    }
                }
               
                //foreach (SgkDB sd in lst)
                //{
                //    lstGuncelle = lstGuncelle.Concat(GetAllDB(sd.cx, sd.yil, sd.ay, out msg)).ToList();

                //    int varmi = IsDBExists(sd.cx, sd.yil, sd.ay, out msg);
                //    if (varmi == 0)
                //    {
                //        lstEkle.Add(sd);
                //    }
                //}
                //if (lstGuncelle.Count > 0)
                //{
                //    List<int> ids = (from lg in lstGuncelle orderby lg.id select lg.id).ToList();
                //    List<int> cxs = (from lg in lstGuncelle orderby lg.id select lg.cx).ToList();
                //    List<string> yils = (from lg in lstGuncelle orderby lg.id select lg.yil).ToList();
                //    List<string> ays = (from lg in lstGuncelle orderby lg.id select lg.ay).ToList();

                //    lstUpdate = (from x in lst where cxs.Contains(x.cx) && yils.Contains(x.yil) && ays.Contains(x.ay) select x).ToList();
                //    foreach (SgkDB sdb in lstUpdate)
                //    {
                //        for (int i = 0; i < ids.Count; i++)
                //        {
                //            if (sdb.cx == cxs[i] && sdb.yil == yils[i] && sdb.ay == ays[i])
                //            {
                //                sdb.id = ids[i];
                //            }
                //        }

                //    }
                //    foreach (SgkDB d in lstUpdate)
                //    {
                //        resultGuncelle++;
                //        dbcmd.Update(d);
                //    }
                //}
                //foreach (SgkDB d in lstEkle)
                //{
                //    resultEkle++;
                //    dbcmd.Insert(d);
                //}
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return (-1, -1);
            }

            return (resultEkle, resultGuncelle);
        }
        
        public List<SgkEt> GetAllEt(out string msg)
        {
            msg = "";
            try
            {
                List<SgkEt> lst = new List<SgkEt>();
                string query = "SELECT * FROM sgket";
                lst = ConLocal.Query<SgkEt>(query).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<SgkEt> GetAllEt(List<int> cxs, out string msg)
        {
            msg = "";
            List<SgkEt> lst = new List<SgkEt>();
            try
            {
                lst = GetAllEt(out msg).FindAll(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int AddEt(List<SgkEt> lst, out string msg)
        {
            msg = "";
            try
            {
                foreach (SgkEt et in lst)
                {
                    etcmd.Delete(et.Cx);
                }
                int result = 0;
                foreach (SgkEt et in lst)
                {
                    etcmd.Insert(et);
                    result++;
                }
                return result;

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }

        }
        
        public List<SgkMe> GetAllMe(out string msg)
        {
            msg = "";
            try
            {
                List<SgkMe> lst = new List<SgkMe>();
                string query = "SELECT * FROM sgkme";
                lst = ConLocal.Query<SgkMe>(query).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<SgkMe> GetAllMe(List<int> cxs, out string msg)
        {
            msg = "";
            List<SgkMe> lst = null;
            try
            {
                lst = GetAllMe(out msg).FindAll(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int AddMe(List<SgkMe> lst, out string msg)
        {
            msg = "";
            try
            {
                foreach (SgkMe et in lst)
                {
                    mecmd.Delete(et.Cx);
                }
                int result = 0;
                foreach (SgkMe me in lst)
                {
                    mecmd.Insert(me);
                    result++;
                }
                return result;

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }

        }
        
        public List<SgkCr> GetAllCr(out string msg)
        {
            msg = "";
            try
            {
                List<SgkCr> lst = new List<SgkCr>();
                string query = "SELECT * FROM sgkcr";
                lst = ConLocal.Query<SgkCr>(query).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<SgkCr> GetAllCr(List<int> cxs, out string msg)
        {
            msg = "";
            List<SgkCr> lst = null;
            try
            {
                lst = GetAllCr(out msg).FindAll(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int AddCr(List<SgkCr> lst, out string msg)
        {
            msg = "";
            try
            {
                foreach (SgkCr et in lst)
                {
                    crcmd.Delete(et.Cx);
                }
                int result = 0;
                foreach (SgkCr cr in lst)
                {
                    crcmd.Insert(cr);
                    result++;
                }
                return result;

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }

        }

        public List<Sgk6661> GetAll6661(out string msg)
        {
            msg = "";
            try
            {
                List<Sgk6661> lst = new List<Sgk6661>();
                string query = "SELECT * FROM sgk6661";
                lst = ConLocal.Query<Sgk6661>(query).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<Sgk6661> GetAll6661(List<int> cxs, string ya, out string msg)
        {
            msg = "";
            List<Sgk6661> lst = null;
            try
            {
                lst = GetAll6661(out msg).FindAll(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                lst = (from x in lst where x.Yil == ya select x).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int Add6661(List<Sgk6661> lst, out string msg)
        {
            msg = "";
            try
            {
                Dictionary<int, string> cxYil =  (from x in lst select new { x.Cx, x.Yil }).Distinct().ToDictionary(x => x.Cx , x => x.Yil);
                foreach (KeyValuePair<int, string> aaab in cxYil)
                {
                    cmd6661.Delete(aaab.Key, aaab.Value);
                }
                int result = 0;
                foreach (Sgk6661 a in lst)
                {
                    cmd6661.Insert(a);
                    result++;
                }
                return result;

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }

        }
        
        public List<SgkIgl> GetAllIgl(out string msg)
        {
            msg = "";
            try
            {
                List<SgkIgl> lst = new List<SgkIgl>();
                string query = "SELECT * FROM sgkigl";
                lst = ConLocal.Query<SgkIgl>(query).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<SgkIgl> GetAllIgl(List<int> cxs, DateTime tr1, DateTime tr2, out string msg)
        {
            msg = "";
            List<SgkIgl> lst = null;
            string sql = $"SELECT * FROM sgkigl WHERE tr BETWEEN @p1 AND @p2 AND cx IN @p3"; 
            try
            {
                lst = ConLocal.Query<SgkIgl>(sql, new { p1 = tr1, p2 = tr2, p3= cxs.ToArray() }).ToList();
                //lst = GetAllIGLData(out msg).FindAll(q => cxs.Contains(Convert.ToInt32(q.cx))).ToList();
                //lst = (from x in lst where x.tr >= tr1 && x.ist <= tr2 select x).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int IsIglExists(SgkIgl igl, out string msg)
        {
            msg = "";
            try
            {
                string sql = $"SELECT id, cn, tc, ads, gc, tr, stn, ipc, isl, ist, isa, cx, un, cd FROM sgkigl WHERE cn = @cn AND tc = @tc AND ads = @ads AND gc = @gc AND tr = @tr AND stn = @stn AND ipc = @ipc AND isl = @isl AND ist = @ist AND isa = @isa AND cx = @cx";
                SgkIgl iglx = ConLocal.Query<SgkIgl>(sql, new {
                    cn = igl.Cn,
                    tc = igl.Tc,
                    ads = igl.Ads,
                    gc = igl.Gc,
                    tr = igl.Tr,
                    stn = igl.Stn,
                    ipc = igl.Ipc,
                    isl = igl.Isl,
                    ist = igl.Ist,
                    isa = igl.Isa,
                    cx = igl.Cx}).FirstOrDefault();
                if (iglx != null)
                {
                    return iglx.Id;
                }
                else
                {
                    return -10;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public int AddIgl(List<SgkIgl> lst, out string msg)
        {
            msg = ""; int result = 0;
            try
            {
                foreach (SgkIgl igl in lst)
                {
                    int iglId = IsIglExists(igl, out msg);
                    if (iglId == -10)
                    {
                        result += iglcmd.Insert(igl);
                    }
                    else if (iglId >= 0)
                    {
                        igl.Id = iglId;
                        result = (iglcmd.Update(igl) > 0 ? true : false) ? result + 1 : result;
                    }
                    else
                    {
                        result = -1;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }

        }

        public int RemoveUnapproveds(List<DateTime> tyas, out string msg)
        {
            msg = "";
            string queryHl = $"DELETE FROM sgkhl WHERE tya IN @tya AND pdfPath LIKE '%Onaysiz%'";
            string queryHlp = $"DELETE FROM sgkhlp WHERE ya IN @tya AND pdfid LIKE 'Onaysiz%'";
            try
            {
                int x = ConLocal.Execute(queryHl, new { tya = tyas.ToArray() });
                int y = ConLocal.Execute(queryHlp, new { tya = tyas.ToArray() });
                return x + y;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public List<SgkHl> GetAllHl(out string msg)
        {
            msg = "";
            try
            {
                List<SgkHl> lst = new List<SgkHl>();
                string query = "SELECT * FROM sgkhl";
                lst = ConLocal.Query<SgkHl>(query).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<SgkHl> GetAllHl(List<int> cxs, DateTime tr1, DateTime tr2, out string msg)
        {
            msg = "";
            List<SgkHl> lst = null;
            try
            {
                lst = GetAllHl(out msg).FindAll(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                lst = (from x in lst where x.Hya >= tr1 && x.Hya <= tr2 select x).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int GetHlId(SgkHl hl, out string msg)
        {
            msg = "";
            try
            {
                SgkHl temp = GetAllHl(out msg).Find(h => h.Cx == hl.Cx && h.Tya == hl.Tya && h.Hya == hl.Hya && h.Bt == hl.Bt && h.Bm == hl.Bm && h.Kn == hl.Kn && h.Tcs == hl.Tcs && h.Tgs == hl.Tgs && h.Tpt == hl.Tpt);

                if (temp == null)
                {
                    return 0;
                }
                else
                {
                    return temp.Id;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public int IsHlExists(SgkHl hl, out string msg)
        {
            msg = "";
            try
            {
                string sql = "SELECT cn, tya, hya, bt, bm, kn, tcs, tgs, tpt, pdfPath, cx, un, cd FROM sgkhl WHERE cn = @cn AND tya = @tya AND hya = @hya AND bt = @bt AND bm = @bm AND kn = @kn AND tcs = @tcs AND tgs = @tgs AND tpt = @tpt AND pdfPath = @pdfPath AND cx = @cx AND un = @un AND cd = @cd";
                SgkHl hlx = ConLocal.QueryAsync<SgkHl>(sql, new { cn = hl.Cn, tya = hl.Tya, hya = hl.Hya, bt = hl.Bt, bm = hl.Bm, kn = hl.Kn, tcs = hl.Tcs, tgs = hl.Tgs, tpt = hl.Tpt, pdfPath = hl.PdfPath, cx = hl.Cx, un = hl.Un, cd = hl.Cd }).Result.FirstOrDefault();
                //bool x = GetAllHLData(out msg).Contains(hl); //Exists(Query.And(Query.EQ("cx", hl.cx), Query.EQ("tya", hl.tya), Query.EQ("hya", hl.hya), Query.EQ("bt", hl.bt), Query.EQ("bm", hl.bm), Query.EQ("kn", hl.kn), Query.EQ("tcs", hl.tcs), Query.EQ("tgs", hl.tgs), Query.EQ("tpt", hl.tpt)));
                if (hlx != null)
                {
                    return hl.Id;
                }
                else
                {
                    return -10;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public int AddHl(List<SgkHl> lst, out string msg)
        {
            msg = ""; int result = 0;
            try
            {
                foreach (SgkHl hl in lst)
                {
                    int hlId = IsHlExists(hl, out msg);
                    if (hlId == -10)
                    {
                        int sonuc = hlcmd.Insert(hl);
                        if (sonuc >= 1) result++;
                    }
                    else if (hlId >= 0)
                    {
                        hl.Id = hlId;
                        result = (hlcmd.Update(hl) > 0 ? true : false) ? result + 1 : result;
                    }
                    else
                    {
                        result = -1;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }
        }
        public int DeleteHLs(List<int> cxs, DateTime tr1, DateTime tr2, out string msg) 
        {
            msg = "";
            int result = 0;
            string query = "DELETE FROM sgkhl WHERE tya BETWEEN @d1 AND @d2 AND cx IN @c1";
            try
            {
                result = ConLocal.Execute(query, new { d1 = tr1, d2 = tr2, c1 = cxs.ToArray() });
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                result = -1;
            }
            return result;
        }
        
        public List<SgkHlp> GetAllHlp(out string msg)
        {
            msg = "";
            try
            {
                List<SgkHlp> lst = new List<SgkHlp>();
                string query = "SELECT * FROM sgkhlp";
                lst = ConLocal.Query<SgkHlp>(query).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<SgkHlp> GetAllHlp(List<int> cxs, DateTime tr1, DateTime tr2, out string msg)
        {
            msg = "";
            List<SgkHlp> lst = null;
            tr1 = new DateTime(tr1.Year, tr1.Month, 1);
            tr2 = new DateTime(tr2.Year, tr2.Month, 1);
            try
            {
                lst = GetAllHlp(out msg).FindAll(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                lst = (from x in lst where x.Ya >= tr1 && x.Ya <= tr2 select x).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int GetHlpId(SgkHlp hlp, out string msg)
        {
            msg = "";
            try
            {
                SgkHlp temp = GetAllHlp(out msg).Find(h => h.Cx == hlp.Cx && h.Tcno == hlp.Tcno && h.Utl == hlp.Utl && h.Itl == hlp.Itl && h.Gun == hlp.Gun && h.EGun == hlp.EGun && h.GGun == hlp.GGun && h.CGun == hlp.CGun && h.Icn == hlp.Icn && h.Egn == hlp.Egn && h.Mk == hlp.Mk && h.Ya == hlp.Ya && h.Bm == hlp.Bm && h.Bt == hlp.Bt && h.Kk == hlp.Kk && h.Ttl == hlp.Ttl && h.Pdfid == hlp.Pdfid);
                
                if (temp == null)
                {
                    return 0;
                }
                else
                {
                    return temp.Id;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public int IsHlpExists(SgkHlp hlp, out string msg)
        {
            msg = "";
            try
            {
                string query = "SELECT id, cn, tcno, ads, utl, itl, gun, eGun, gGun, cGun, icn, egn, mk, ya, bm, bt, kk, cx, cd, ttl, pdfid FROM sgkhlp WHERE cn = @cn AND tcno = @tcno AND ads = @ads AND utl = @utl AND itl = @itl AND gun = @gun AND eGun = @eGun AND gGun = @gGun AND cGun = @cGun AND icn = @icn AND egn = @egn AND mk = @mk AND ya = @ya AND bm = @bm AND bt = @bt AND kk = @kk AND cx = @cx AND ttl = @ttl AND pdfid = @pdfid";
                SgkHlp x = ConLocal.Query<SgkHlp>(query, new {cn = hlp.Cn,  tcno = hlp.Tcno, ads = hlp.Ads,  utl = hlp.Utl,  itl = hlp.Itl,  gun = hlp.Gun,  eGun = hlp.EGun,  gGun = hlp.GGun,  cGun = hlp.CGun,  icn = hlp.Icn,  egn = hlp.Egn,  mk = hlp.Mk,  ya = hlp.Ya,  bm = hlp.Bm,  bt = hlp.Bt,  kk = hlp.Kk,  cx = hlp.Cx,  ttl = hlp.Ttl,  pdfid = hlp.Pdfid }).FirstOrDefault();

                if (x != null)
                {
                    return x.Id;
                }
                else
                {
                    return -10;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public int AddHlp(List<SgkHlp> lst, out string msg)
        {
            msg = ""; int result = 0;
            try
            {
                foreach (SgkHlp hlp in lst)
                {
                    int hlpId = IsHlpExists(hlp, out msg);
                    if (hlpId == -10)
                    {
                        int sonuc = hlpcmd.Insert(hlp);
                        if (sonuc >= 1) result++;
                    }
                    else if (hlpId >= 1)
                    {
                        hlp.Id = hlpId;
                        result = (hlpcmd.Update(hlp) > 0 ? true : false) ? result + 1 : result;
                    }
                    else
                    {
                        result = -1;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }
        }
        public int AddHlp(SgkHlp hlp, out string msg)
        {
            msg = ""; int resultAdd = 0, resultUd = 0;
            try
            {
                int hlpId = IsHlpExists(hlp, out msg);
                if (hlpId == -10)
                {
                    resultAdd = hlpcmd.Insert(hlp); 
                }
                else if (hlpId >= 0)
                {
                    hlp.Id = hlpId;
                    resultUd = hlpcmd.Update(hlp) ; msg = $"{resultAdd} adet kayıt eklendi, {resultUd} adet kayıt güncellendi";
                }
                return resultAdd;
                
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }
        }
        public int DeleteHlPs(List<int> cxs, DateTime tr1, DateTime tr2, out string msg) 
        {
            msg = "";
            int result = 0;
            string query = "DELETE FROM sgkhlp WHERE ya BETWEEN @d1 AND @d2 AND cx IN @c1";
            try
            {
                result = ConLocal.Execute(query, new { d1 = tr1, d2 = tr2, c1 = cxs.ToArray() });
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                result = -1;
            }
            return result;
        }
        public void RemoveDuplicate() { }
                
    }
}
