using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWebApplication.Models;
using System.Web.Security;
using DemoWebApplication.Providers;
using System.Net;
using System.Data.Entity.Validation;

namespace DemoWebApplication.Controllers
{
    public class AdminController : Controller
    {
        DemoWebAppDBContext _context = new DemoWebAppDBContext();
        CustomMembershipProvider cmp;
        CustomRoleProvider crp;

        [Authorize(Roles = "Admin")]
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                cmp = new CustomMembershipProvider();
                if (cmp.ValidateUser(user.Email, user.Password))
                {
                    //clear any other tickets that are already in the response
                    Response.Cookies.Clear();

                    DateTime expiration = DateTime.Now.AddMinutes(1);
                    if (user.RememberMe)
                    {
                        //expiration = DateTime.Now.AddMonths(1);
                        expiration = DateTime.Now.AddMinutes(10);
                    }

                    FormsAuthentication.Initialize();
                    FormsAuthenticationTicket tkt = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, expiration, user.RememberMe, FormsAuthentication.FormsCookiePath);

                    HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(tkt));
                    
                    

                    ////set the new expiry date – to thirty days from now
                    ////DateTime expiryDate = DateTime.Now.AddDays(30);
                    //DateTime expiryDate = DateTime.Now.AddMinutes(1);

                    ////create a new forms auth ticket
                    //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, user.Email, DateTime.Now, expiryDate, user.RememberMe, String.Empty);

                    ////encrypt the ticket
                    //string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    ////create a new authentication cookie – and set its expiration date
                    //HttpCookie authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    //authenticationCookie.Expires = ticket.Expiration;

                    ////add the cookie to the response.
                    //Response.Cookies.Add(authenticationCookie);


                    FormsAuthentication.SetAuthCookie(user.Email, user.RememberMe);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Login details are wrong.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Email or Password.");
            }
            return View(user);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterViewModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var crypto = new SimpleCrypto.PBKDF2();
                    var encryptPassword = crypto.Compute(user.Password);
                    string hostName = Dns.GetHostName(); // Retrive the Name of HOST
                    // Get the IP
                    string userIPAddr = Dns.GetHostByName(hostName).AddressList[0].ToString();

                    User newUser = new User
                    {
                        Email = user.Email,
                        UserName=user.Email,
                        Password = encryptPassword,
                        PasswordSalt = crypto.Salt,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IPAddress = userIPAddr,
                        ModifiedDate=DateTime.Now
                    };

                    _context.Users.Add(newUser);
                    _context.SaveChanges();

                    crp = new CustomRoleProvider();
                    crp.AddUserToRole(newUser.ID, "User");

                    FormsAuthentication.SetAuthCookie(newUser.Email, false);

                    return RedirectToAction("Login", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Register failed.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Admin");
        }
    }
}