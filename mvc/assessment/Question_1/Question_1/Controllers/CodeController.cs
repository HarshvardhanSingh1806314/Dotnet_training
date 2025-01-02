using Question_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Question_1.Controllers
{
    public class CodeController : Controller
    {
        private NorthwindEntities _db;

        public CodeController()
        {
            _db = new NorthwindEntities();
        }

        // GET: Code
        public ActionResult GetCustomersResidingInGermany()
        {
            List<Customer> customerListInGermany = _db.Customers.Where(c => c.Country == "Germany").ToList();
            return View(customerListInGermany);
        }

        [HttpGet]
        public ActionResult GetCustomerDetailsWithId10248()
        {
            Customer customerDetail = _db.Customers.Find(_db.Orders.Find(10248).CustomerID);
            return View(customerDetail);
        }
    }
}