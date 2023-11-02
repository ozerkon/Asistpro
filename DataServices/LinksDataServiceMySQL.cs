using Dapper;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServices
{
    public class LinksDataServiceMySql : DbBaseMySql, ILinksDataService
    {
        List<Links> mainlist = new List<Links>();
        public class Commands : DbBaseMySql
        {

            public int Insert(Links l)
            {
                string query = "INSERT INTO links(grp, sgrp, vers, head, url, vurl, `desc`, tags, cmd, fav) VALUES (@grp, @sgrp, @vers, @head, @url, @vurl, @desc, @tags, @cmd, @fav)";
                
                return ConLocal.Execute(query, new
                {
                    grp = l.Grp,
                    sgrp = l.Sgrp,
                    vers = l.Vers,
                    head = l.Head,
                    url = l.Url,
                    vurl = l.Vurl,
                    desc = l.Desc,
                    tags = l.Tags,
                    cmd = l.Cmd,
                    fav = l.Fav
                });
            }
            public int Delete(int id)
            {
                string query = "DELETE FROM links WHERE id = @id";
                return ConLocal.Execute(query, new { id = id });

            }
            public int Update(Links l)
            {
                string query = "UPDATE links SET grp=@grp, sgrp = @sgrp, vers = @vers, head = @head, " +
               "url = @url, vurl = @vurl, `desc` = @desc, tags = @tags, cmd = @cmd, fav = @fav " +
               "WHERE id = @conditionId";

                return ConLocal.Execute(query, new
                {
                    grp = l.Grp,
                    sgrp = l.Sgrp,
                    vers = l.Vers,
                    head = l.Head,
                    url = l.Url,
                    vurl = l.Vurl,
                    desc = l.Desc,
                    tags = l.Tags,
                    cmd = l.Cmd,
                    fav = l.Fav,
                    conditionId = l.Id
                });

            }
        }

        Commands cmd = new Commands();
        public List<Links> GetAllLinks(out string msg)
        {

            msg = "";
            try
            {
                string sql = "SELECT * FROM links";
                List<Links> lst = ConLocal.QueryAsync<Links>(sql).Result.ToList();
                
                if(lst == null || lst.Count == 0)
                {
                    lst = GetAllLinksFromRs(out msg);
                    AddLinks(lst, out msg);
                }
                mainlist = lst;
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        public List<Links> GetAllLinksFromRs(out string msg)
        {
            msg = "";
            List<Links> lst = null;
            try
            {
                lst = ConRemote.Query<Links>("SELECT id, grp, sgrp AS sgrp, vers, head, url, vurl, `desc`, tags, cmd, fav FROM links").ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public (string, string) GetCommandFromDb(int id, out string msg)
        {
            msg = "";
            try
            {
                Links lnk = GetAllLinks(out msg).Where(x => x.Id == id).FirstOrDefault();
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
                string sql = "SELECT id, grp, sgrp AS sgrp, vers, head, url, vurl, `desc`, tags, cmd, fav FROM links WHERE id = @aydi";
                Links lnk = ConLocal.Query<Links>(sql, new { aydi = id }).FirstOrDefault();
                if(lnk != null)
                {
                    return true;
                }
                //List<Links> link = GetAllLinks(out msg);
                //foreach (var l in link)
                //{
                //    if (l.id == id)
                //    {
                //        return true;
                //    }
                //}
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
                string sql = $"SELECT head FROM links WHERE head = @p1";
                Links link = ConLocal.QueryAsync<Links>(sql, new { p1 = head }).Result.FirstOrDefault();// GetAllLinks(out msg);
                
                if (link != null)
                {
                    return true;
                }
                return false;
            }
            catch (System.Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }
        public List<string> GetDescriptions(out string msg) /*string[], List<string> e çevrildi*/
        {
            msg = ""; List<string> descs = new List<string>();
            try
            {
                List<Links> list = GetAllLinks(out msg).ToList();
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
                Links link = GetAllLinks(out msg).Where(x => x.Id == id).FirstOrDefault();
                return link;
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
                    isOk = cmd.Update(l) > 0 ? true : false;
                    result = (isOk) ? 2 : 1; // güncelleme olursa result değeri 2 olsun
                }
                else
                {
                    isOk = cmd.Insert(l) == 1 ? true : false;
                    result = (isOk) ? 1 : -1;
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
                ConLocal.ExecuteAsync("TRUNCATE links");
                foreach (Links l in lst)
                {
                    if (ExistLink(l.Head, out msg))
                    {
                        isOk = cmd.Update(l) > 0 ? true : false;
                        result += (isOk) ? 2 : 1; // güncelleme olursa result değeri 2 olsun
                    }
                    else
                    {
                        // l.fav = Convert.ToBoolean(l.fav);
                        isOk = cmd.Insert(l) > 0 ? true : false;
                        result += (isOk) ? 1 : -1;
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
                isOk = cmd.Delete(l.Id) > 0 ? true : false;
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
                lnk.Fav = isFav ? true : false;
                return cmd.Update(lnk) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }

        public bool ResetIdColumn(out string msg)
        {
            msg = "";
            try
            {
                ConLocal.Execute("TRUNCATE TABLE links");
                foreach (var l in LinkGlobals.LstLinks)
                {
                    cmd.Insert(l);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
            return true;
        }
    }
}
