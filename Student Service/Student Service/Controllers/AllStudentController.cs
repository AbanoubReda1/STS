using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Student_Service.Models;

namespace Student_Service.Controllers
{
    public class AllStudentController : Controller
    {
        // GET: AllStudent
        private MydataEntities dc = new MydataEntities();
        public ActionResult AllUser()
        {
            return View(dc.Students.ToList());
        }

        // GET: AllStudent/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AllStudent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AllStudent/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AllStudent/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AllStudent/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AllStudent/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AllStudent/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
