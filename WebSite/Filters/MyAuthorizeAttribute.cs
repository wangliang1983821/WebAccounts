using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSite.Helper;
using LTP.Accounts.Bus;
using System.Web.Mvc;
using WebSite.Models;
namespace WebSite.Filters
{
    //验证角色和用户名的类
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var user = httpContext.User as MyFormsPrincipal<AccountsPrincipal>;

            if (user == null)
            {
                HttpContext.Current.User =
                   MyFormsAuthentication<LTP.Accounts.Bus.AccountsPrincipal>.TryParsePrincipal(HttpContext.Current.Request);

                user = httpContext.User as MyFormsPrincipal<AccountsPrincipal>;
            }

            if (user != null)
                return (user.IsInRole(Roles) || user.IsInUser(Users));
         
            return false;
        }

        public string Permissions { get; set; }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
             
            //验证不通过,直接跳转到相应页面，注意：如果不使用以下跳转，则会继续执行Action方法
            filterContext.Result = new RedirectResult("/Accounts/Login");
        }
    }
}