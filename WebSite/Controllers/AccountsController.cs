using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;
using LTP.Accounts;
using LTP.Accounts.Data;
using WebSite.Helper;
using System.Configuration;
namespace WebSite.Controllers
{
    public class AccountsController : Controller
    {
        //
        // GET: /Accounts/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            
                //通过数据库查询验证

              
            var userData =  LTP.Accounts.Bus.AccountsPrincipal.ValidateLogin(model.UserName, model.Password);
            
             if (userData != null)
                {
                    //验证成功，用户名密码正确，构造用户数据（可以添加更多数据，这里只保存用户Id）
                  

                    //保存Cookie
                    MyFormsAuthentication<LTP.Accounts.Bus.AccountsPrincipal>.SetAuthCookie(model.UserName, userData, model.RememberMe);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        public ActionResult Register()
        {

            return View();
        }

         public ActionResult RegisterOn(LogOnModel model)
         {
          
             LTP.Accounts.Bus.User user = new LTP.Accounts.Bus.User();
             user.UserName = model.UserName;
             user.Password = LTP.Accounts.Bus.AccountsPrincipal.EncryptPassword(model.Password);
             user.TrueName = "";
             user.UserType = "";
             user.Sex = "1";
             user.Phone = "";
             user.Email = "";
             user.DepartmentID = "1";
             user.EmployeeID = 0;
             user.Create();

             return View("Index");
         }

         public ActionResult SignOut()
         {
             MyFormsAuthentication<LTP.Accounts.Bus.AccountsPrincipal>.DeleteAuthCookie();
             return    RedirectToAction("About", "Home");
         }
     
    }
}
