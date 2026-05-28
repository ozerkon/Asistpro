using Dapper;
using Models.Common;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServices
{
    public class ihaleDataServis:DbBaseMySql
    {
        public int Insert(ihale i, out string msg)
        {
            msg = "";
            string query = @"INSERT INTO ihale 
                     (ihaleKayitNo, ihaleTuru, ihaleAdi, il, ihaleyiAlan, OnayTarihi, SozlesmeTarihi, 
                      idareAdi, sozlesmeBilgileri, yaklasikMaliyet, enYuksekTeklif, enDusukTeklif) 
                     VALUES 
                     (@ihaleKayitNo, @ihaleTuru, @ihaleAdi, @il, @ihaleyiAlan, @OnayTarihi, 
                      @SozlesmeTarihi, @idareAdi, @sozlesmeBilgileri, @yaklasikMaliyet, 
                      @enYuksekTeklif, @enDusukTeklif)";
            try
            {
                return ConLocal.Execute(query, new
                {
                    ihaleKayitNo = i.ihaleKayitNo,
                    ihaleTuru = i.ihaleTuru,
                    ihaleAdi = i.ihaleAdi,
                    il = i.il,
                    ihaleyiAlan = i.ihaleyiAlan,
                    OnayTarihi = i.OnayTarihi,
                    SozlesmeTarihi = i.SozlesmeTarihi,
                    idareAdi = i.idareAdi,
                    sozlesmeBilgileri = i.sozlesmeBilgileri,
                    yaklasikMaliyet = i.yaklasikMaliyet,
                    enYuksekTeklif = i.enYuksekTeklif,
                    enDusukTeklif = i.enDusukTeklif
                });
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
            
        }

        public List<ihale> IhaleAl(out string msg)
        {
            msg = "";
            List<ihale> alvs = new List<ihale>();
            string query = "SELECT * FROM ihale";
            try
            {
                alvs = ConLocal.Query<ihale>(query).ToList();
                return alvs;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

    }
}
