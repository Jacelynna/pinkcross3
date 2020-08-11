using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace pinkcross3.Controllers
{
    public class DonorProfile : Controller
    {
        public IActionResult Index()
        {
            DataTable dt = DBUtl.GetTable("SELECT * FROM DonorProfile");
            return View("Index", dt.Rows);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Models.DonorProfile profile)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invaild Input";
                ViewData["MsgType"] = "warning";
                return View("Create");
            }
            else
            {
                string insert = @"INSERT DonorProfile(DonorName, Donornric, CompanyName, DonorNumber, DonorAddress) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')";
                if (DBUtl.ExecSQL(insert, profile.DonorName, profile.Donornric, profile.CompanyName, profile.DonorNumber, profile.DonorAddress) == 1)
                {
                    TempData["Message"] = "Donor Profile Created";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            string select = "SELECT * FROM DonorProfile WHERE ID = '{0}'";
            List<Models.DonorProfile> list = DBUtl.GetList<Models.DonorProfile>(select, Id);
            if (list.Count == 1)
            {
                return View(list[0]);
            }
            else 
            {
                TempData["Message"] = "Profile don't exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(Models.DonorProfile profile)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invaild Input";
                ViewData["MsgType"] = "warning";
                return View("Edit");
            }
            else
            {
                string update = @"UPDATE DonorProfile SET DonorName = '{1}', Donornric = '{2}', CompanyName = '{3}', DonorNumber = '{4}', DonorAddress = '{5}' WHERE ID = {0}";

                int res = DBUtl.ExecSQL(update, profile.DonorName, profile.Donornric, profile.CompanyName, profile.DonorNumber, profile.DonorAddress);

                if (res == 1)
                {
                    TempData["Message"] = "Donor Profile Updated";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            string select = @"SELECT * FROM DonorProfile WHERE ID={0}";
            DataTable ds = DBUtl.GetTable(select, id);
            if (ds.Rows.Count !=1)
            {
                TempData["Message"] = "Delete does not exist";
                TempData["MdgType"] = "warning";
            }
            else
            {
                string delete = "DELETE FROM DonorProfile WHERE ID={0}";
                int res = DBUtl.ExecSQL(delete, id);
                if (res == 1)
                {
                    TempData["Message"] = "Deleted";
                    TempData["MdgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MdgType"] = "danger";
                }
            }
            return RedirectToAction("Index");
        }
    }
}
