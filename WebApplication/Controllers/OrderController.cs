using Internal.Models;
using Internal.Database;
using System.Collections.Generic;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
namespace Internal.Controllers
{
    //[Authorize]
    public class OrderController : Controller
    {
        private EntityConnection db = new EntityConnection();

        [HttpGet]
        public JsonResult GetFilteredListProductName(int CategoryID)
        {
            List<SelectListItem> ProductList = db.Products.Where(x => x.CategoryID == CategoryID).Select(x => new SelectListItem { Text = x.ProductName, Value = x.ProductID.ToString() }).ToList();
            return Json(ProductList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetShipperName(int ProductID)
        {
            OrderDetails ThisOrderDetail = db.OrderDetails.Where(x=>x.ProductID == ProductID).FirstOrDefault();
            return Content(ThisOrderDetail.Orders.Shippers.ShipperName);
        }

        [HttpGet]
        public ActionResult GetCustomerAddress(int CustomerID)
        {
            Customers ThisCustomer = db.Customers.Where(x => x.CustomerID == CustomerID).FirstOrDefault();
            return Content(ThisCustomer.Address + ", " +
                        ThisCustomer.PostalCode + ", " +
                        ThisCustomer.City + ", " +
                        ThisCustomer.Country);
        }

        [HttpGet]
        public ActionResult GetTotalCost(int ProductID, int OrderQuantity)
        {
            Products ThisProduct = db.Products.Where(x=>x.ProductID==ProductID).FirstOrDefault();
            double TotalCost = ThisProduct.Price * OrderQuantity;
            return Content(TotalCost.ToString());
        }

        public ActionResult Details(string Mode, int? OrderID)
        {
            if (!(Mode == "View" || Mode == "Edit"))
            {
                return RedirectToAction("Index");
            }

            ViewBag.DropDownList_CustomerName = db.Customers.OrderBy(x => x.CustomerName).Select(x => new SelectListItem { Text = x.CustomerName, Value = x.CustomerID.ToString() });
            ViewBag.DropDownList_EmployeeName = db.Employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.EmployeeID.ToString()
            });
            ViewBag.DropDownList_ProductCategory = db.Categories.OrderBy(x => x.CategoryName).Select(x => new SelectListItem { Text = x.CategoryName, Value = x.CategoryID.ToString() });

            var model = new OrderModel();
            if (OrderID == null)
            {
                if (Mode == "View")
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                OrderDetails orderDetails = db.OrderDetails.Where(x => x.OrderID == OrderID).FirstOrDefault();
                if (orderDetails != null)
                {
                    model.Address = orderDetails.Orders.Customers.Address + ", " +
                                    orderDetails.Orders.Customers.PostalCode + ", " +
                                    orderDetails.Orders.Customers.City + ", " +
                                    orderDetails.Orders.Customers.Country;
                    model.CustomerName = orderDetails.Orders.Customers.CustomerName;
                    model.EmployeeName = orderDetails.Orders.Employees.FirstName + " " +
                                        orderDetails.Orders.Employees.LastName;
                    model.OrderDate = orderDetails.Orders.OrderDate;
                    model.OrderedQuantity = orderDetails.Quantity;
                    model.TotalCost = (decimal)(orderDetails.Quantity * orderDetails.Products.Price);
                    model.ProductCategory = orderDetails.Products.Categories.CategoryName;
                    model.ProductName = orderDetails.Products.ProductName;
                    model.ShipperName = orderDetails.Orders.Shippers.ShipperName;
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public ActionResult Index(
        string Page,
        string Search,
        string Sort
        )
        {
            var m = db.OrderDetails.AsQueryable();
            m = m.OrderBy(x => x.Orders.OrderDate);
            if (int.TryParse(Page, out int page))
            {
                return View(m.ToPagedList(page, 15));
            }
            else
            {
                return View(m.ToPagedList(1, 15));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
