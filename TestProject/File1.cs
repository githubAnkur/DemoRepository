using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NF.DAL.Data;
using NF.Model;
using NF.Model.Views;
using NF.Common;

namespace NF.DAL
{
    public partial class UsersFilesData
    {
        public void Save(UsersFilesEntity item)
        {
            using (var context = new DatabaseContext())
            {
                tblUsers_Files e;

                if (item.tblUsersFilesID == 0)
                {
                    e = new tblUsers_Files();
                    e.tblLogins_ID = item.tblLoginsID;
                    e.Filename = item.Filename;
                    e.Description = item.Description;
                    e.UploadedBy_ID = item.UploadedByID;
                    e.Filesize = item.Filesize;
                    e.UploadDate = item.UploadDate;
                    e.isFromLink = item.isFromLink;
                    e.LinkAdress = item.LinkAdress;
                    context.tblUsers_Files.Add(e);
                }
                else
                {
                    e = context.tblUsers_Files.SingleOrDefault(o => o.tblUsers_Files_ID == item.tblUsersFilesID);

                    e.Description = item.Description;
                    if (!string.IsNullOrEmpty(item.Filename))
                    {
                        e.Filename = item.Filename;
                        e.UploadedBy_ID = item.UploadedByID;
                        e.Filesize = item.Filesize;
                        e.UploadDate = item.UploadDate;
                        e.isFromLink = item.isFromLink;
                        e.LinkAdress = item.LinkAdress;
                    }
                }

                context.SaveChanges();


                if (item.tblUsersFilesID == 0)
                {
                    if (item.isFromLink != true)
                    { e.Filename = string.Format("{0}_{1}", e.tblUsers_Files_ID, e.Filename); }
                    else
                    { e.Filename = item.Filename; }
                    context.SaveChanges();
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.Filename))
                    {
                        if (item.isFromLink != true)
                        { e.Filename = string.Format("{0}_{1}", e.tblUsers_Files_ID, e.Filename); }
                        else
                        { e.Filename = item.Filename; }
                        context.SaveChanges();
                    }
                }

                item.tblUsersFilesID = e.tblUsers_Files_ID;
            }
        }

        public List<UsersFilesEntity> GetListByUser(
            int loginID,
            int take,
            int skip,
            string sortby,
            out int totalRows
            )
        {
            using (var context = new DatabaseContext())
            {
                var items = from t1 in context.tblUsers_Files
                            join t2 in context.tblLogins on t1.UploadedBy_ID equals t2.tblLogins_ID
                            join t3 in context.tblUserLevels on t2.tblUserLevels_ID equals t3.tblUserLevels_ID
                            where t1.tblLogins_ID == loginID
                            orderby t1.UploadDate descending
                            select new UsersFilesEntity
                            {
                                Description = t1.Description,
                                Filename = t1.Filename,
                                Filesize = t1.Filesize,
                                tblUsersFilesID = t1.tblUsers_Files_ID,
                                tblLoginsID = t1.tblLogins_ID,
                                UploadDate = t1.UploadDate,
                                UploadedByID = t1.UploadedBy_ID,
                                isFromLink=t1.isFromLink,
                                LinkAdress=t1.LinkAdress,
                                UploadedByName = t2.DisplayName,
                                UploadedByUserLevel = t3.UserLevel,
                                UploadedByPayrollLevel = t2.PayrollLevel
                            };

                string[] arr = sortby.Split(' ');
                if (arr.Length > 0)
                {
                    switch (arr[0])
                    {
                        case "FileName":
                            if (arr[1] == "desc")
                            {
                                items = from t1 in context.tblUsers_Files
                                        join t2 in context.tblLogins on t1.UploadedBy_ID equals t2.tblLogins_ID
                                        join t3 in context.tblUserLevels on t2.tblUserLevels_ID equals t3.tblUserLevels_ID
                                        where t1.tblLogins_ID == loginID
                                        orderby t1.Filename descending
                                        select new UsersFilesEntity
                                        {
                                            Description = t1.Description,
                                            Filename = t1.Filename,
                                            Filesize = t1.Filesize,
                                            tblUsersFilesID = t1.tblUsers_Files_ID,
                                            tblLoginsID = t1.tblLogins_ID,
                                            UploadDate = t1.UploadDate,
                                            UploadedByID = t1.UploadedBy_ID,
                                            isFromLink = t1.isFromLink,
                                            LinkAdress = t1.LinkAdress,
                                            UploadedByName = t2.DisplayName,
                                            UploadedByUserLevel = t3.UserLevel,
                                            UploadedByPayrollLevel = t2.PayrollLevel
                                        };
                            }
                            else
                            {
                                items = from t1 in context.tblUsers_Files
                                        join t2 in context.tblLogins on t1.UploadedBy_ID equals t2.tblLogins_ID
                                        join t3 in context.tblUserLevels on t2.tblUserLevels_ID equals t3.tblUserLevels_ID
                                        where t1.tblLogins_ID == loginID
                                        orderby t1.Filename
                                        select new UsersFilesEntity
                                        {
                                            Description = t1.Description,
                                            Filename = t1.Filename,
                                            Filesize = t1.Filesize,
                                            tblUsersFilesID = t1.tblUsers_Files_ID,
                                            tblLoginsID = t1.tblLogins_ID,
                                            UploadDate = t1.UploadDate,
                                            UploadedByID = t1.UploadedBy_ID,
                                            isFromLink = t1.isFromLink,
                                            LinkAdress = t1.LinkAdress,
                                            UploadedByName = t2.DisplayName,
                                            UploadedByUserLevel = t3.UserLevel,
                                            UploadedByPayrollLevel = t2.PayrollLevel
                                        };
                            }
                            break;
                        case "Description":
                            if (arr[1] == "desc")
                            {
                                items = from t1 in context.tblUsers_Files
                                        join t2 in context.tblLogins on t1.UploadedBy_ID equals t2.tblLogins_ID
                                        join t3 in context.tblUserLevels on t2.tblUserLevels_ID equals t3.tblUserLevels_ID
                                        where t1.tblLogins_ID == loginID
                                        orderby t1.Description descending
                                        select new UsersFilesEntity
                                        {
                                            Description = t1.Description,
                                            Filename = t1.Filename,
                                            Filesize = t1.Filesize,
                                            tblUsersFilesID = t1.tblUsers_Files_ID,
                                            tblLoginsID = t1.tblLogins_ID,
                                            UploadDate = t1.UploadDate,
                                            UploadedByID = t1.UploadedBy_ID,
                                            isFromLink = t1.isFromLink,
                                            LinkAdress = t1.LinkAdress,
                                            UploadedByName = t2.DisplayName,
                                            UploadedByUserLevel = t3.UserLevel,
                                            UploadedByPayrollLevel = t2.PayrollLevel
                                        };
                            }
                            else
                            {
                                items = from t1 in context.tblUsers_Files
                                        join t2 in context.tblLogins on t1.UploadedBy_ID equals t2.tblLogins_ID
                                        join t3 in context.tblUserLevels on t2.tblUserLevels_ID equals t3.tblUserLevels_ID
                                        where t1.tblLogins_ID == loginID
                                        orderby t1.Description
                                        select new UsersFilesEntity
                                        {
                                            Description = t1.Description,
                                            Filename = t1.Filename,
                                            Filesize = t1.Filesize,
                                            tblUsersFilesID = t1.tblUsers_Files_ID,
                                            tblLoginsID = t1.tblLogins_ID,
                                            UploadDate = t1.UploadDate,
                                            UploadedByID = t1.UploadedBy_ID,
                                            isFromLink = t1.isFromLink,
                                            LinkAdress = t1.LinkAdress,
                                            UploadedByName = t2.DisplayName,
                                            UploadedByUserLevel = t3.UserLevel,
                                            UploadedByPayrollLevel = t2.PayrollLevel
                                        };
                            }
                            break;
                        case "UploadedByName":
                            if (arr[1] == "desc")
                            {
                                items = from t1 in context.tblUsers_Files
                                        join t2 in context.tblLogins on t1.UploadedBy_ID equals t2.tblLogins_ID
                                        join t3 in context.tblUserLevels on t2.tblUserLevels_ID equals t3.tblUserLevels_ID
                                        where t1.tblLogins_ID == loginID
                                        orderby t2.DisplayName descending
                                        select new UsersFilesEntity
                                        {
                                            Description = t1.Description,
                                            Filename = t1.Filename,
                                            Filesize = t1.Filesize,
                                            tblUsersFilesID = t1.tblUsers_Files_ID,
                                            tblLoginsID = t1.tblLogins_ID,
                                            UploadDate = t1.UploadDate,
                                            UploadedByID = t1.UploadedBy_ID,
                                            isFromLink = t1.isFromLink,
                                            LinkAdress = t1.LinkAdress,
                                            UploadedByName = t2.DisplayName,
                                            UploadedByUserLevel = t3.UserLevel,
                                            UploadedByPayrollLevel = t2.PayrollLevel
                                        };
                            }
                            else
                            {
                                items = from t1 in context.tblUsers_Files
                                        join t2 in context.tblLogins on t1.UploadedBy_ID equals t2.tblLogins_ID
                                        join t3 in context.tblUserLevels on t2.tblUserLevels_ID equals t3.tblUserLevels_ID
                                        where t1.tblLogins_ID == loginID
                                        orderby t2.DisplayName
                                        select new UsersFilesEntity
                                        {
                                            Description = t1.Description,
                                            Filename = t1.Filename,
                                            Filesize = t1.Filesize,
                                            tblUsersFilesID = t1.tblUsers_Files_ID,
                                            tblLoginsID = t1.tblLogins_ID,
                                            UploadDate = t1.UploadDate,
                                            UploadedByID = t1.UploadedBy_ID,
                                            isFromLink = t1.isFromLink,
                                            LinkAdress = t1.LinkAdress,
                                            UploadedByName = t2.DisplayName,
                                            UploadedByUserLevel = t3.UserLevel,
                                            UploadedByPayrollLevel = t2.PayrollLevel
                                        };
                            }
                            break;
                        case "DateUploaded":
                            if (arr[1] == "desc")
                            {
                                items = from t1 in context.tblUsers_Files
                                        join t2 in context.tblLogins on t1.UploadedBy_ID equals t2.tblLogins_ID
                                        join t3 in context.tblUserLevels on t2.tblUserLevels_ID equals t3.tblUserLevels_ID
                                        where t1.tblLogins_ID == loginID
                                        orderby t1.UploadDate descending
                                        select new UsersFilesEntity
                                        {
                                            Description = t1.Description,
                                            Filename = t1.Filename,
                                            Filesize = t1.Filesize,
                                            tblUsersFilesID = t1.tblUsers_Files_ID,
                                            tblLoginsID = t1.tblLogins_ID,
                                            UploadDate = t1.UploadDate,
                                            UploadedByID = t1.UploadedBy_ID,
                                            isFromLink = t1.isFromLink,
                                            LinkAdress = t1.LinkAdress,
                                            UploadedByName = t2.DisplayName,
                                            UploadedByUserLevel = t3.UserLevel,
                                            UploadedByPayrollLevel = t2.PayrollLevel
                                        };
                            }
                            else
                            {
                                items = from t1 in context.tblUsers_Files
                                        join t2 in context.tblLogins on t1.UploadedBy_ID equals t2.tblLogins_ID
                                        join t3 in context.tblUserLevels on t2.tblUserLevels_ID equals t3.tblUserLevels_ID
                                        where t1.tblLogins_ID == loginID
                                        orderby t1.UploadDate
                                        select new UsersFilesEntity
                                        {
                                            Description = t1.Description,
                                            Filename = t1.Filename,
                                            Filesize = t1.Filesize,
                                            tblUsersFilesID = t1.tblUsers_Files_ID,
                                            tblLoginsID = t1.tblLogins_ID,
                                            UploadDate = t1.UploadDate,
                                            UploadedByID = t1.UploadedBy_ID,
                                            isFromLink = t1.isFromLink,
                                            LinkAdress = t1.LinkAdress,
                                            UploadedByName = t2.DisplayName,
                                            UploadedByUserLevel = t3.UserLevel,
                                            UploadedByPayrollLevel = t2.PayrollLevel
                                        };
                            }
                            break;
                        case "FileSize":
                            if (arr[1] == "desc")
                            {
                                items = from t1 in context.tblUsers_Files
                                        join t2 in context.tblLogins on t1.UploadedBy_ID equals t2.tblLogins_ID
                                        join t3 in context.tblUserLevels on t2.tblUserLevels_ID equals t3.tblUserLevels_ID
                                        where t1.tblLogins_ID == loginID
                                        orderby t1.Filesize descending
                                        select new UsersFilesEntity
                                        {
                                            Description = t1.Description,
                                            Filename = t1.Filename,
                                            Filesize = t1.Filesize,
                                            tblUsersFilesID = t1.tblUsers_Files_ID,
                                            tblLoginsID = t1.tblLogins_ID,
                                            UploadDate = t1.UploadDate,
                                            UploadedByID = t1.UploadedBy_ID,
                                            isFromLink = t1.isFromLink,
                                            LinkAdress = t1.LinkAdress,
                                            UploadedByName = t2.DisplayName,
                                            UploadedByUserLevel = t3.UserLevel,
                                            UploadedByPayrollLevel = t2.PayrollLevel
                                        };
                            }
                            else
                            {
                                items = from t1 in context.tblUsers_Files
                                        join t2 in context.tblLogins on t1.UploadedBy_ID equals t2.tblLogins_ID
                                        join t3 in context.tblUserLevels on t2.tblUserLevels_ID equals t3.tblUserLevels_ID
                                        where t1.tblLogins_ID == loginID
                                        orderby t1.Filesize
                                        select new UsersFilesEntity
                                        {
                                            Description = t1.Description,
                                            Filename = t1.Filename,
                                            Filesize = t1.Filesize,
                                            tblUsersFilesID = t1.tblUsers_Files_ID,
                                            tblLoginsID = t1.tblLogins_ID,
                                            UploadDate = t1.UploadDate,
                                            UploadedByID = t1.UploadedBy_ID,
                                            isFromLink = t1.isFromLink,
                                            LinkAdress = t1.LinkAdress,
                                            UploadedByName = t2.DisplayName,
                                            UploadedByUserLevel = t3.UserLevel,
                                            UploadedByPayrollLevel = t2.PayrollLevel
                                        };
                            }
                            break;
                        default:
                            break;
                    }
                }

                totalRows = items.Count();

                items = items.Skip(skip).Take(take);

                return items.ToList();
            }
        }
        public string GetFileName(int loginID, string filename, int userfileid)
        {
            string fn,name;
            using (var context = new DatabaseContext()){
                if (userfileid == 0)
                {
                    name = (from t in context.tblUsers_Files
                            where t.tblLogins_ID == loginID && t.Description == filename
                            select t.Description).FirstOrDefault();
                }
                else
                {
                    name = (from t in context.tblUsers_Files
                            where t.tblLogins_ID == loginID && t.Description == filename
                            && t.tblUsers_Files_ID != userfileid
                            select t.Description).FirstOrDefault();
                }

                if (name != null)
                {
                    fn = name;
                }
                else
                {
                    fn = null;
                }
                
                return fn;
                 }
            
        }
        public int GetFilesCount(int loginID)
        {
            using (var context = new DatabaseContext())
            {
                return context.tblUsers_Files.Select(o => o).Where(o => o.tblLogins_ID == loginID).Count();
            }
        }
        public string GetLinkAdress(int loginID, string linkAdress, int userfileid)
        {
            string la, name;
            using (var context = new DatabaseContext())
            {
                if (userfileid == 0)
                {
                    name = (from t in context.tblUsers_Files
                            where t.tblLogins_ID == loginID && t.LinkAdress == linkAdress
                            select t.LinkAdress).FirstOrDefault();
                }
                else
                {
                    name = (from t in context.tblUsers_Files
                            where t.tblLogins_ID == loginID && t.LinkAdress == linkAdress
                            && t.tblUsers_Files_ID != userfileid
                            select t.LinkAdress).FirstOrDefault();
                }

                if (name != null)
                {
                    la = name;
                }
                else
                {
                    la = null;
                }

                return la;
            }

        }
        public string GetUploadedFileName(int tbllogins_id, string filename, int userfileid)
        {
            string fn;
            using (var context = new DatabaseContext())
            {
                string name;
                if (userfileid == 0)
                {
                    name = (from t in context.tblUsers_Files
                            where t.tblLogins_ID == tbllogins_id && t.Filename == filename
                            select t.Filename).FirstOrDefault();
                }
                else
                {
                    name = (from t in context.tblUsers_Files
                            where t.tblLogins_ID == tbllogins_id && t.Filename == filename
                            && t.tblUsers_Files_ID != userfileid
                            select t.Filename).FirstOrDefault();
                }
                if (name != null)
                {
                    fn = name;
                }
                else
                {
                    fn = null;
                }

                return fn;
            }

        }
    }
}
