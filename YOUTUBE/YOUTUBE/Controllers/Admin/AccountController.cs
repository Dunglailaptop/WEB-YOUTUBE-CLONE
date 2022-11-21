using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YOUTUBE.Models;

namespace YOUTUBE.Controllers
{
   
    public class AccountController : Controller
    {
        // GET: Account\
        DataClasses1DataContext db = new DataClasses1DataContext();
        int id = 0;
        public ActionResult logins()
        {
            return View();
        }
        [HttpPost]
        public ActionResult logins(account acc,string returnUrl)
        {
           
          
            if (ModelState.IsValid)
            {
                acc.username = Request.Form["username"];
                acc.passwords = Request.Form["password"];
                loginsResult logins = new loginsResult();
                logins = db.logins(acc.username, acc.passwords).SingleOrDefault();
                    if (logins != null)
                    {
                        FormsAuthentication.SetAuthCookie(logins.username, true);

                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            id = logins.matk;
                            Session["taikhoan"] = logins;
                            Session["acc"] = logins.matk;
                            Session["username"] = logins.username;
                            if (logins.MaQL == 1)
                            {
                                Session["idadmin"] = logins.MaQL;
                            }

                            List<video> v = db.videos.ToList();
                            List<account> a = db.accounts.ToList();
                            List<User> u = db.Users.ToList();
                            var quey = (from aco in a
                                        join us in u on aco.matk equals us.matk
                                        where aco.matk == logins.matk
                                        select new innerjoin { accountt = aco, user = us }).SingleOrDefault();
                            Session["imageusers"] = quey.user.imageuser;
                            int ac = Convert.ToInt32(Session["acc"]);
                            List<User> uss = db.Users.ToList();
                            List<dangkykenh> ch = db.dangkykenhs.ToList();
                            List<account> accc = db.accounts.ToList();
                            var quy = from use in uss
                                      join ace in accc on use.matk equals ace.matk into table1
                                      from ace in table1.DefaultIfEmpty()
                                      join chh in ch on ace.matk equals chh.matk
                                      where chh.matkuser == ac
                                      select new innerjoin { dk = chh, user = use, accountt = ace };
                            Session["dskenhdy"] = quy.ToList();
                            channel c = db.channels.Where(x => x.matk == ac).SingleOrDefault();
                            if (c != null)
                            {
                                Session["kenh"] = c.MaDK;
                            }

                            //    return RedirectToAction("Index");
                            return RedirectToAction("LOADVIDEO", "VIDEO");
                        }

                    }
                //}
                   
            }
            else
            {
                ModelState.AddModelError("", "the user name or password is correct");
                ViewBag.Message = "vui lòng kiểm tra lại username và password khong chính xác";
                return View();
            }

            //if (logins != null)
            //{
            //    Session["acc"]=logins.matk;
            //    Session["username"]=logins.username;
            //    return RedirectToAction("Index");
            //}else
            ViewBag.Message = "vui lòng kiểm tra lại username và password khong chính xác";
            return View();
        }
        [Authorize(Roles = "phanquyen"), Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            List<laytkResult> result = new List<laytkResult>();
            result = db.laytk().ToList();
            return View(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(account ac)
        {
            account acc = new account();
            acc.username = ac.username;
            acc.passwords = ac.passwords;
            db.taoaccount(acc.username, acc.passwords);
            return RedirectToAction("Index");
        }
        public ActionResult Detail(int ?id)
        {
            detailtaikhoanResult accc = db.detailtaikhoan(id).SingleOrDefault();
            return View(accc);
            
        }

     
    
        public ActionResult dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult dangky(account acc)
        {
            
            acc.username = Request.Form["username"];
            acc.passwords = Request.Form["passwords"];
            string confrimpassword = Request.Form["confrim"];
            if (acc.username.Length < 8)
            {
                ViewBag.Message = "Tên đăng nhập phải có 8 ký tự";
                return View();
            }else if(acc.passwords.Length < 8)
            {
                ViewBag.Message = "Mật khẩu phải có 8 ký tự";
                return View();
            }else if(acc.username.Length>=8 && acc.passwords.Length>=8)
            {
                if (confrimpassword == acc.passwords)
                {
                    account ac = db.accounts.Where(x => x.username == acc.username).SingleOrDefault();
                    if (ac == null)
                    {
                        acc.MaQL = 2;
                        db.dangky(acc.username, acc.passwords, acc.MaQL);
                        account a=db.accounts.Where(x=>x.username == acc.username).SingleOrDefault();
                        if (a != null)
                        {
                            string hoten = "chuaco";
                            string sdt = "0";
                            string email = "chuaco";
                            string diachi = "chuaco";
                            string image = "/Content/img/user.jpg";
                            db.capnhatthongtinuser(hoten,sdt,email,diachi,image,a.matk);
                        }else
                        {
                            ViewBag.Message = "Tạo tài khoản thất bại";
                        }
                        return RedirectToAction("logins");
                    }
                    else
                    {
                        ViewBag.Message = "Tên đăng nhập đã tồn tại";
                        return View();
                    }

                }
                else
                {
                    ViewBag.Message = "Mật khẩu xác nhận sai";
                    return View();
                }
            }
         
           return View();
          
        }

        public ActionResult dangxuat()
        {

            //Roles.DeleteCookie();
            FormsAuthentication.SignOut();
            return RedirectToAction("logins", "Account");
        }
       public ActionResult khoaacc(int id)
        {
            Response.Cookies["khoaacc"].Value =Convert.ToString(id);
            return RedirectToAction("Index", "Account");
        }
    }
}