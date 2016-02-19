using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTP.Accounts;
using LTP.Accounts.Data;
using Model;

namespace Administration.Controllers
{
    public class UserManageController : Controller
    {
        //
        // GET: /UserManage/

        public ActionResult Index()
        {
            Common.Common_GetPagedList<AccountsUsers> au = new Common.Common_GetPagedList<AccountsUsers>(PubConstant.ConnectionString);

            int count = 0;
            List<AccountsUsers> list = au.ExecuteStoredProcedureWithParms("Accounts_Users", "*", "UserID DESC", "", 20, 1, out count) as List<AccountsUsers>;

            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string UserName, string PassWord)
        {
            try
            {
                LTP.Accounts.Bus.User user = new LTP.Accounts.Bus.User();
                user.UserName = UserName;
                user.Password = LTP.Accounts.Bus.AccountsPrincipal.EncryptPassword(PassWord);
                user.TrueName = "";
                user.UserType = "";
                user.Sex = "1";
                user.Phone = "";
                user.Email = "";
                user.DepartmentID = "1";
                user.EmployeeID = 0;
                int id = user.Create();
                return Content("用户id:" + id);
            }
            catch(Exception ex)
            {
                return Content(ex.ToString());
            }
        }

    }
}
