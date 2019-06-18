using NF.DAL.Data;
using NF.Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NF.DAL
{
  public  class UserUrlHistoryDLL
  {
      #region [SaveUserUrlHistory]
      public void SaveUserUrlHistory(int loginId, string url, string urlDescription, string rawData)
        {


            using (var context = new DatabaseContext())
            {
                var sql = new StringBuilder();
                sql.Append("nfs_SaveUserUrlHistory @LoginId={0}, ");
                sql.Append("@Url={1}, @UrlDescription={2}, @RawData={3} ");
                
                context.Database.ExecuteSqlCommand(sql.ToString(),
                   loginId,
                    url,
                    urlDescription,
                   rawData
                    );
            }
        }
        #endregion


      #region [GetUserUrlHistory]
      public List<UserUrlHistoryView> GetUserUrlHistory(int LoginId)
      {
          using (var context = new DatabaseContext())
          {
              return context.Database.SqlQuery<UserUrlHistoryView>("[nfs_GetUserUrlHistory] @LoginId={0}",
                 LoginId
                  ).ToList();
          }
      }
      #endregion

    }
}
