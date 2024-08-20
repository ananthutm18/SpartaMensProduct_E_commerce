using SpartaMensProduct.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpartaMensProduct.DAL;
using System.Web.UI.WebControls;
using System.Web.Security;
using SpartaMensProduct.Middleware;
using System.Reflection;

namespace SpartaMensProduct.Controllers
{


    /// <summary>
    /// user controller
    /// </summary>

    public class UserController : Controller
    {
        userDAL _userDal = new userDAL();
        orderDAL _orderDal = new orderDAL();


        // GET: User
        public ActionResult Index()
        {
            return View();
        }


        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User model, HttpPostedFileBase imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.CreatedAt = DateTime.Now;
                    //  model.PasswordHash = HashPassword(model.PasswordHash);

                    var user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        DateOfBirth = model.DateOfBirth,
                        City = model.City,
                        State = model.State,
                        Email = model.Email,
                        PasswordHash = model.PasswordHash,
                        CreatedAt = DateTime.Now,
                    };

                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        using (var binaryReader = new BinaryReader(imageFile.InputStream))
                        {
                            user.ImageData = binaryReader.ReadBytes(imageFile.ContentLength);
                        }
                    }


                    _userDal.CreateUser(user);

                    return RedirectToAction("Create");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }





        // GET: User/Edit
        public ActionResult Edit()
        {
            try
            {
                var user = new User();

                if (Session["UserId"] != null && int.TryParse(Session["UserId"].ToString(), out int userId))
                {
                    user = _userDal.GetUserById(userId);
                    // Use 'user' as needed


                }

                return View(user);
            }

            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User model, HttpPostedFileBase imageFile)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Account"); // Redirect to login if user is not authenticated
                }

                if (ModelState.IsValid)
                {

                    var user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        DateOfBirth = model.DateOfBirth,
                        City = model.City,
                        State = model.State,
                        Email = model.Email,
                        PasswordHash = model.PasswordHash,
                        CreatedAt = DateTime.Now,
                    };
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        using (var binaryReader = new BinaryReader(imageFile.InputStream))
                        {
                            user.ImageData = binaryReader.ReadBytes(imageFile.ContentLength);
                        }
                    }
                    else
                    {
                        user.ImageData = null;
                    }

                    user.UserId = (int)Session["UserId"]; // Ensure the user ID is set from the session
                    _userDal.EditUser(user);

                    return RedirectToAction("Account"); // Redirect to an appropriate view
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }
    



    //get Login


    // GET: User/Login
    public ActionResult Login()
        {
            try
            {


                //Setting no cache to prevent browser back button
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Response.Cache.SetAllowResponseInBrowserHistory(false);

                if (Session["UserId"] != null)
                {

                    if ((bool)Session["IsAdmin"])
                    {

                        return RedirectToAction("Index", "Admin");

                    }
                    else
                    {

                        return RedirectToAction("Index", "Home");

                    }
                    
                }

                return View();

            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }

        }


        // POST: User/Login

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var user = _userDal.ValidateUser(email, password);
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(user.Email, false);

                        Session["UserId"] = user.UserId;
                        Session["FirstName"] = user.FirstName;
                        Session["LastName"] = user.LastName;
                        Session["Email"] = user.Email;
                        Session["IsAdmin"] = user.IsAdmin;

                        if ((bool)Session["IsAdmin"])
                        {

                            return RedirectToAction("Index", "Admin");

                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }


        public ActionResult Logout()
        {
            _userDal.Logout(Session);
            return RedirectToAction("Index", "Home");
        }


        // GET: User/Account
        [SessionAdminAuthorize]
        public ActionResult Account()
        {
            int userId = (int)Session["UserId"];
            User user = _userDal.GetUserById(userId); // Get user details including orders
            return View(user);
        }


        public ActionResult MyOrders()
        {
            int userId = (int)Session["UserId"];
            var orders = _orderDal.GetOrdersByUserId(userId);
            return View(orders);
        }


    }
}