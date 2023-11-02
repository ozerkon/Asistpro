using LiteDB;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace DataServices
{
    public class LinksDataServiceLdb : DbBaseLiteDb, ILinksDataService
    {

        public LinksDataServiceLdb()
        {
            LinksConnect = LdbConnect.GetCollection<Links>("links");

            LinksConnect.EnsureIndex(x => x.Head);
        }

        public List<Links> GetAllLinks(out string msg)
        {
            msg = "";
            List<Links> lst = null;
            try
            {
                lst = LinksConnect.FindAll().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public List<Links> GetAllLinksFromRs(out string msg)
        {
            msg = "";
            List<Links> lst = null;
            try
            {
                lst = ConRemote.Query<Links>("SELECT id, grp, sgrp AS sgrp, vers, head, url, vurl, 'desc', tags, cmd, fav FROM links").ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }

        public (string,string) GetCommandFromDb(int id, out string msg)
        {

            msg = "";
            try
            {
                Links lnk = LinksConnect.FindAll().Where(x => x.Id == id).FirstOrDefault();
                return (lnk.Cmd, lnk.Vurl);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return (null, null);
            }
        }

        public bool ExistLink(int id, out string msg)
        {
            msg = "";
            try
            {
                List<Links> link = GetAllLinks(out msg);
                foreach (var l in link)
                {
                    if (l.Id == id)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }
        public bool ExistLink(string head, out string msg)
        {
            msg = "";
            try
            {
                List<Links> link = GetAllLinks(out msg);
                foreach (var l in link)
                {
                    if (l.Head == head)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }
        public List<string> GetDescriptions(out string msg)
        {
            msg = ""; /*string[] descs = null;*/ List<string> descs = new List<string>();
            try
            {
                List<Links> list = LinksConnect.FindAll().ToList();
                foreach (Links item in list)
                {
                    descs.Add(item.Head);
                }
                return descs;
            }
            catch (System.Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        public Links GetLinkById(int id, out string msg)
        {
            msg = "";
            try
            {
                return LinksConnect.FindById(id);
            }
            catch (System.Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        public int AddLink(Links l, out string msg)
        {
            int result; bool isOk;
            try
            {
                if (ExistLink(l.Id, out msg))
                {
                    isOk = LinksConnect.Update(l);
                    result = (isOk) ? 2 : 1; // güncelleme olursa result değeri 2 olsun
                }
                else
                {
                    result = LinksConnect.Insert(l).AsInt32;
                    result = (result > 0) ? 1 : -1;
                }

            }
            catch (System.Exception ex)
            {
                msg = ex.Message.ToString();
                return 0;
            }
            return result;
        }
        public int AddLinks(List<Links> lst, out string msg)
        {
            msg = "";
            int result = 0; bool isOk;
            try
            {
                foreach (Links l in lst)
                {
                    if (ExistLink(l.Head, out msg))
                    {
                        isOk = LinksConnect.Update(l);
                        result += (isOk) ? 2 : 1; // güncelleme olursa result değeri 2 olsun
                    }
                    else
                    {
                        l.Fav = Convert.ToBoolean(l.Fav);
                        result += AddLink(l, out msg);
                    }
                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message.ToString();
                return 0;
            }
            return result;
        }
        public int DeleteLink(Links l, out string msg)
        {
            msg = ""; bool isOk;
            try
            {
                isOk = LinksConnect.Delete(l.Id);
                return (isOk) ? 1 : -1;
            }
            catch (System.Exception ex)
            {
                msg = ex.Message.ToString();
                return 0;
            }
        }

        public bool AddRemoveFav(int id, bool isFav, out string msg)
        {
            msg = "";
            try
            {
                Links lnk = GetLinkById(id, out msg);
                lnk.Fav = isFav;
                return LinksConnect.Update(lnk);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }

        public bool ResetIdColumn(out string msg)
        {
            bool result = false; msg = "";
            try
            {

                LdbConnect.Execute("DROP COLLECTION temp");
                foreach (var l in LinkGlobals.LstLinks)
                {
                    LdbConnect.Execute("INSERT INTO temp:INT VALUES {" + $"grp:'{l.Grp}', sgrp:'{l.Sgrp}', vers:'{l.Vers}', head:'{l.Head}', url:'{l.Url}', vurl:'{l.Vurl}', desc:'{l.Desc}', tags:'{l.Tags}', cmd:'{l.Cmd}', fav:{l.Fav}" + "}");

                }
                LdbConnect.Execute("DROP COLLECTION users");
                LdbConnect.Execute("RENAME COLLECTION temp TO links");
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();

                return false;
            }

            return result;
        }
    }
}
