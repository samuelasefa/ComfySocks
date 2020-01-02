using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    public class IssueController : Controller
    {
        // GET: Issue
        public ActionResult IssueList()
        {
            return View();
        }

        public ActionResult NewIssueEntry()
        {
            return View();
        }
    }
}