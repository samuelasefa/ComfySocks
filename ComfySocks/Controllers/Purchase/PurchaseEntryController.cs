using ComfySocks.DAL;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.PurchaseModel;
using ComfySocks.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers.Purchase
{
    public class PurchaseEntryController : Controller
    {
        MyContext db = new MyContext();
        // GET: PurchaseEntry
        /// <summary>
        /// Main page for entering new purchase records.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public ActionResult Index()
        {
            var vm = new PurchaseEntryVM();
            return View(vm);
        }

        /// <summary>
        /// Populate Supplier DropDown List
        /// </summary>
        /// <returns>Json data of supplier's list</returns>
        /// <remarks>The value is cached for 3 minutes so that it doesn't have to query to database again.</remarks>
        public JsonResult PopulateSupplier()
        {
            //hold list of suppliers
            List<Supplier> _supplierList = new List<Supplier>();

            //queries all the suppliers for its ID and Name Property
            var supplierList = (from s in db.Suppliers select new { s.ID, s.Name }).ToList();

            //save list of supplier to the _supplierList
            foreach (var item in supplierList) {
                _supplierList.Add(new Supplier
                {
                    ID = item.ID,
                    Name = item.Name
                });
            }
            //return the json result of _supplierList
            return Json(_supplierList, JsonRequestBehavior.AllowGet);
        }
        //populateing supplier invoice
        public JsonResult PopulateSupplierInvoice() {

            //holding list of supplier invoce no.
            List<Supplier> _supplierInvoice = new List<Supplier>();

            //queriy supplier invoice from supplierd database
            var supplierInvoice = (from si in db.Suppliers select new { si.InvoiceNumber }).ToList();

            //save supplier Invoice 
            foreach (var item in supplierInvoice)
            {
                _supplierInvoice.Add(new Supplier
                {
                    InvoiceNumber = item.InvoiceNumber
                });
            }
            return Json(supplierInvoice, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Populate Items DropDownList
        /// </summary>
        /// <returns>Json data of Item's list</returns>
        /// <remarks>The value is cached for 3 minutes 
        public JsonResult PopulateItem()
        {
            //queries all the items in the database
            var itemList = (from i in db.Items select new { i.ID, i.Description }).ToList();

            //hold the list of item
            List<Item> _item = new List<Item>();

            //save the list of items to the _item
            foreach (var item in itemList)
            {
                _item.Add(new Item
                {
                    ID = item.ID,
                    Description = item.Description
                });
            }
            //return the list of item in json form
            return Json(_item, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Post action for Saving data to database
        /// </summary>
        /// <param name="p">View model holding the entered data.</param>
        /// <returns>Returns value indicating if the data has been saved or failed to save.</returns>
        /// <remarks> Gets data as ViewModel rather than FormCollection.</remarks>
        [HttpPost]
        public JsonResult SavePurchase(PurchaseEntryVM p)
        {
            bool status = false;

            if (p != null)
            {
                //new purchase object using the data from the viewmodel
                Purchases purchase = new Purchases
                {
                    ID = p.ID,
                    Date = p.Date,
                    SupplierID = p.SupplierID,
                    Total = p.Total,
                    Vat = p.Vat,
                    GrandTotal = p.GrandTotal
                };

                purchase.PurchaseItems = new List<PurchaseItem>();
                //populating purchase item from purchaseItems within purchaseItem purchaseentryVm

                foreach (var i in p.PurchaseItems)
                {
                    purchase.PurchaseItems.Add(i);
                }
                status = true;
            }
            // return the status in form of json
            return new JsonResult { Data = new { status = status } };
        }
    }
}