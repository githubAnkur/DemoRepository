using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NF.Common;
using NF.DAL.Data;
using NF.Model;
using NF.Model.Views;

namespace NF.DAL
{
    public partial class VendorCostData
    {
        #region SaveNotToExceed
        public static void SaveNotToExceed(decimal? NotToexceed, int woid)
        {
            using (var context = new DatabaseContext())
            {

                var e = context.tblVendorCosts.SingleOrDefault(o => o.tbljb_schedules_ID == woid);
                if (e != null)
                {
                    e.NotToExceed = (NotToexceed == 0) ? null : NotToexceed;
                }

                else
                {
                    e = new tblVendorCost();
                    e.tbljb_schedules_ID = woid;
                    e.NotToExceed = (NotToexceed == 0) ? null : NotToexceed;
                    context.tblVendorCosts.Add(e);

                }

                context.SaveChanges();

            }
        }
        #endregion

        #region SavePO

        public static void SavePO(string poid, int woid)
        {
            using (var context = new DatabaseContext())
            {
                

                var e = context.tblVendorCosts.SingleOrDefault(o => o.tbljb_schedules_ID == woid);
                if (e != null)
                {
                    e.PurchaseOrderID = poid == "0" ? null: poid;
                }

                else
                {
                    e = new tblVendorCost();
                    e.tbljb_schedules_ID = woid;
                    e.PurchaseOrderID = poid == "0" ? null : poid;
                    context.tblVendorCosts.Add(e);

                }

                context.SaveChanges();

            }
        }
        #endregion

        #region GetVendorCostItems

        public static List<VendorCostItemsView> GetVendorCostItems(int woid)
        {
            List<VendorCostItemsView> list = new List<VendorCostItemsView>();

            using (var context = new DatabaseContext())
            {
                var items = from t1 in context.tblVendorCostItems
                            from t2 in context.tblRatetypes.Where(o => o.tblRatetype_ID == t1.tblRatetype_ID)
                            from t3 in context.tblRefNoes.Where(o => o.tblRefNo_ID == t1.tblRefNo_ID).DefaultIfEmpty()
                            where 1 == 1
                                      && (t1.tbljb_schedules_ID == woid)
                            select new
                       {
                           VendorCostItemId = t1.tblVendorCostItem_ID,
                           WOID = t1.tbljb_schedules_ID,
                           Qty = t1.QTY,
                           DateOfService = t1.DateOfService,
                           RateTypeID = t1.tblRatetype_ID,
                           RateTypeName = t2.RateType,
                           Description = t1.Description,
                           Rate = t1.Rate,
                           Amount = t1.Amount,
                           Resale = t1.Resale,
                           SalePrice = t1.Saleprice,
                           Loginid = t1.tblLogins_ID,
                           TotalSale = t1.TotalSale,
                           RefNo = t3.RefNo,
                           RefNoID = t1.tblRefNo_ID,
                           InvoiceNumber = t1.InvoiceNumber
                        };

                //    var items = context.tblVendorCostItems.Where(o => o.tbljb_schedules_ID == woid).ToList();
                foreach (var item in items)
                {
                    list.Add(new VendorCostItemsView
                    {
                        VendorCostItemId = item.VendorCostItemId,
                        WOID = item.WOID,
                        Qty = item.Qty,
                        DateOfService = item.DateOfService,
                        RateTypeID = item.RateTypeID,
                        RateTypeName = item.RateTypeName,
                        Description = item.Description,
                        Rate = item.Rate,
                        Amount = item.Amount,
                        Resale = item.Resale,
                        SalePrice = item.SalePrice,
                        Loginid = item.Loginid,
                        TotalSale = item.TotalSale,
                        RefNo = item.RefNo ?? "",
                        RefNoID = item.RefNoID,
                        InvoiceNumber = item.InvoiceNumber

                    });
                }
            }
            return list;
        }
        #endregion

        #region SaveVendorCostItem
        public static VendorCostItemsEditView SaveVendorCostItem(VendorCostItemsView item, int Loginid)
        {
            using (var context = new DatabaseContext())
            {
                VendorCostItemsEditView EditView = new VendorCostItemsEditView();
                if (item != null)
                {                    
                    if (item.VendorCostItemId > 0)
                    {
                        var e = context.tblVendorCostItems.SingleOrDefault(o => o.tblVendorCostItem_ID == item.VendorCostItemId);
                        if (e != null)
                        {
                            EditView.OldQty = e.QTY;
                            EditView.OldDescription = e.Description;
                            EditView.NewQty = item.Qty;
                            EditView.NewDescription = item.Description;
                            
                            e.tblRatetype_ID = item.RateTypeID;
                            e.DateOfService = item.DateOfService;
                            e.Description = item.Description;
                            e.QTY = item.Qty;
                            e.Rate = item.Rate;
                            e.Amount = item.Amount;
                            e.Saleprice = item.SalePrice;
                            e.TotalSale = item.TotalSale;
                            e.Resale = item.Resale;
                            e.tblRefNo_ID = item.RefNoID;
                            e.InvoiceNumber = item.InvoiceNumber;
                        }
                    }

                    else
                    {

                        var e = new tblVendorCostItem();
                        e.tbljb_schedules_ID = item.WOID;
                        e.tblRatetype_ID = item.RateTypeID;
                        e.DateOfService = item.DateOfService;
                        e.Description = item.Description;
                        e.QTY = item.Qty;
                        e.Rate = item.Rate;
                        e.Amount = item.Amount;
                        e.tblLogins_ID = Loginid;
                        e.Saleprice = item.SalePrice;
                        e.TotalSale = item.TotalSale;
                        e.Resale = item.Resale;
                        e.tblRefNo_ID = item.RefNoID;
                        e.InvoiceNumber = item.InvoiceNumber;

                        context.tblVendorCostItems.Add(e);
                    }
                    context.SaveChanges();
                   
                }
                return EditView;
            }
        }

        #endregion

        #region GetItem
        public static VendorCostView GetItem(int woid)
        {
            VendorCostView item = new VendorCostView();
            using (var context = new DatabaseContext())
            {
                var e = context.tblVendorCosts.SingleOrDefault(o => o.tbljb_schedules_ID == woid);
                if (e != null)
                {
                    item.NotToExceed = e.NotToExceed;
                    item.PurchaseOrderID = e.PurchaseOrderID;
                    item.Vendor = e.Vendor;
                    item.Vendors_ID = e.tblVendors_ID <= 0 ? 0 : e.tblVendors_ID;
                }
                var items = context.tblJB_SCHEDULES.SingleOrDefault(o => o.ID == woid);
                if (items != null)
                {

                    item.tblVendors_ID = items.tblVendors_ID <= 0 ? 0 : items.tblVendors_ID;
                    item.tblSites_ID = items.tblSites_ID;
                    item.ScheduleTypeID = items.ScheduleTypeID;
                    if (items.tblVendors_ID > 0 || item.Vendors_ID > 0)
                    {
                        var vendorid = item.tblVendors_ID > 0 ? item.tblVendors_ID : item.Vendors_ID;
                        var vendor = context.tblVendors.Where(o => o.tblVendors_ID == vendorid).FirstOrDefault();
                        item.VendorName = vendor.VendorName;
                    }
                    else
                    {
                        item.VendorName = "";
                    }
                }
                else
                {
                    var arcitems = context.arcJB_SCHEDULES.SingleOrDefault(o => o.ID == woid);
                    if (arcitems != null)
                    {

                        item.tblVendors_ID = arcitems.tblVendors_ID <= 0 ? 0 : arcitems.tblVendors_ID;
                        item.tblSites_ID = arcitems.tblSites_ID;
                        item.ScheduleTypeID = arcitems.ScheduleTypeID;
                        if (arcitems.tblVendors_ID > 0 || item.Vendors_ID > 0)
                        {
                            var vendorid = item.tblVendors_ID > 0 ? item.tblVendors_ID : item.Vendors_ID;
                            var vendor = context.tblVendors.Where(o => o.tblVendors_ID == vendorid).FirstOrDefault();
                            item.VendorName = vendor.VendorName;
                        }
                        else
                        {
                            item.VendorName = "";
                        }
                    }
                }
            }
            return item;

        }
        #endregion

        #region DeleteVendorCostItem

        public static void DeleteVendorCostItem(int id)
        {
            using (var context = new DatabaseContext())
            {
                var e = context.tblVendorCostItems.SingleOrDefault(o => o.tblVendorCostItem_ID ==id);
                if (e != null)
                {
                    context.tblVendorCostItems.Remove(e);
                    context.SaveChanges();
                }
            }
        }
        #endregion
        #region SavePO

        public static void SaveVendor(int woid, int id = 0)
        {
            using (var context = new DatabaseContext())
            {
                var e = context.tblVendorCosts.SingleOrDefault(o => o.tbljb_schedules_ID == woid);
                if (e != null)
                {
                    e.tblVendors_ID = id;
                }

                else
                {
                    e = new tblVendorCost();
                    e.tbljb_schedules_ID = woid;
                    e.tblVendors_ID = id;
                    context.tblVendorCosts.Add(e);
                }

                context.SaveChanges();

            }
        }
        #endregion
        #region CheckVendorEntries

        public static bool CheckVendorEntries(int id)
        {
            using (var context = new DatabaseContext())
            {
                bool e = (from t1 in context.tblVendorCosts
                          join t2 in context.tblVendorCostItems on t1.tbljb_schedules_ID equals t2.tbljb_schedules_ID
                          where t2.tbljb_schedules_ID == id
                          && t1.tblVendors_ID == 0
                          select t1.tbljb_schedules_ID).Any();

                return e;
            }
        }
        #endregion
    }
}
