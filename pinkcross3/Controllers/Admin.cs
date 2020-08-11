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
    public class Admin : Controller
    {
        public IActionResult Index()
        {
            DataTable dt = DBUtl.GetTable("SELECT * FROM [User]");
            return View("Index", dt.Rows);
        }
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Models.User AA)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("Create");
            }
            else
            {
                String insert = @"INSERT INTO [User](Username_id,Password,Role) VALUES ('{0}',HASHBYTES('SHA1','{1}'), '{2}')";
                if (DBUtl.ExecSQL(insert, AA.Username_id,AA.Password,AA.Role) == 1)
                {
                    ViewData["Message"] = "Register successfully";
                    ViewData["MsgType"] = "success";
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                }
            }
            return View("Login");
        }

    }
}
