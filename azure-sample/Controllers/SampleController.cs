﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace azure_sample.Controllers
{
    public class SampleController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "サンプル";

            return View();
        }
    }
}