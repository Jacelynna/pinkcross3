using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using PinkCrossDonationRecord.Models;

namespace PinkCrossDonationRecord.Controllers
{
    public class DonationRecord : Controller
    {
        public IActionResult Index()
        {
            DataTable dt = DBUtl.GetTable("SELECT * FROM DonationRecord");
            return View("Index", dt.Rows);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Models.DonationRecord record)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invaild Input";
                ViewData["MsgType"] = "warning";
                return View("Create");
            }
            else
            {
                string insert = @"INSERT DonationRecord(DonationRecord_id,DateOfDonation,ModeOfDonation,DonationAmount,PurposeOfDonation,DonorType,CompanyName,ReceiptNumber,DateOfReceipt,TaxExemptionStatus) VALUES ('{0}','{1:yyyy-MM-dd}','{2}','{3}','{4}','{5}','{6}','{7}','{8:yyyy-MM-dd}','{9}')";
                if (DBUtl.ExecSQL(insert, record.DonationRecord_id,record.DateOfDonation, record.ModeOfDonation, record.DonationAmount,record.PurposeOfDonation, record.DonorType, record.CompanyName, record.ReceiptNumber, record.DateOfReceipt, record.TaxExemptionStatus) ==1)
                {
                    TempData["Message"] = "Donation Record Created";
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
        public IActionResult Edit(int DonationRecord_id)
        {
            string select = "SELECT * FROM DonationRecord WHERE DonationRecord_id = {0}";
            List<Models.DonationRecord> list = DBUtl.GetList<Models.DonationRecord>(select, DonationRecord_id);
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
        public IActionResult Edit(Models.DonationRecord record)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invaild Input";
                ViewData["MsgType"] = "warning";
                return View("Edit");
            }
            else
            {
                string update = @"UPDATE DonationRecord_id SET DonationRecord_id = '{0}', DateOfDonation = '{1}', ModeOfDonation = '{2}', DonationAmount = '{3}', PurposeOfDonation = '{4}', DonationAmount = '{5}', CompanyName= '{6}', ReceiptNumber = '{7}', DateOfReceipt = '{8}', TaxExemptionStatus = '{9}'  WHERE ID = {0}";

                int res = DBUtl.ExecSQL(update, record.DonationRecord_id, record.DateOfDonation, record.ModeOfDonation, record.DonationAmount, record.PurposeOfDonation, record.DonationAmount, record.CompanyName, record.ReceiptNumber, record.DateOfReceipt, record.TaxExemptionStatus);

if (res == 1)
                {
                    TempData["Message"] = "DonationRecord Updated";
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

        [Authorize(Roles = "Staff")]
        public IActionResult Delete(int DonationRecord_id)
        {
            string sql = @"SELECT * FROM DonationRecord WHERE DonationRecord_id = {0}";
            DataTable ds = DBUtl.GetTable(sql, DonationRecord_id);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Donation Record does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                string delete = "DELETE FROM DonationRecord WHERE DonationRecord_id = {0}";
                int res = DBUtl.ExecSQL(delete, DonationRecord_id);
                if (res == 1)
                {
                    TempData["Message"] = "DonationRecord deleted";
                    TempData["MsgType"] = "warning";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("Index");
        }
    }
}