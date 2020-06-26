using System.Linq;
using System.Web.Mvc;
using UserRegistration.Models;
using System.Web.Security;
using System.Data.Entity;
using System.Net;
using System.Linq;

namespace UserRegistration.Controllers
{
   // [AllowAnonymous]
  
    public class User_InfoController : Controller
    {
      

        //Registration
        [HttpGet]
        
        // public ActionResult AddorEdit(int id=0)
        public ActionResult AddorEdit(int id = 0)
        {
            User_Info usermodel = new User_Info();
            return View(usermodel);
        }
        [HttpPost]
       
        public ActionResult AddorEdit(User_Info usermodel)
        {
            //save user details into User_Info table
            using (DbModels dbmodel = new DbModels())
            {
                //for removing duplicate name
                if(dbmodel.User_Info.Any(x => x.UserName == usermodel.UserName))
                {
                    ViewBag.DuplicateMessage = "Username already exist";
                  //  return RedirectToAction("login");
                    return View("AddorEdit", new User_Info());
                }
                dbmodel.User_Info.Add(usermodel);
                dbmodel.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful..";
           // return RedirectToAction("login");
            return View("AddorEdit", new User_Info());
        }

        // Login

        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
       
        public ActionResult login(User_Info usermodel)
        {
            using (DbModels dbmodel = new DbModels())
            {
                var usr = dbmodel.User_Info.Single(u => u.UserName == usermodel.UserName && u.Password == usermodel.Password);
                if(usr != null)
                {
                    Session["UserID"] = usr.UserID.ToString();
                    Session["UserName"] = usr.UserName.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError(" ", "Username or Password is wrong");
                }
            }
                return View();
        }

        public ActionResult LoggedIn()
        {
            if(Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login");
            }
        }

        public ActionResult logout()
        {
            Session.Abandon();
          //  FormsAuthentication.SignOut(); 
            return RedirectToAction("login");
        }

        // Retrive all data from database
        // GET: /User_Info/Index
        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            using (DbModels dbmodel = new DbModels())
            {
                var list = dbmodel.User_Info.ToList();
                return View(list);
            }
        }

        // GET: /User_Info/Details
        // Show All the Information of Selected Person

        
        
        public ActionResult Details(int id)
        {
            using (DbModels dbmodel = new DbModels())
            {
                var details = dbmodel.User_Info.Where(d => d.UserID == id).FirstOrDefault();

                return View(details);
            }
        }


        /*   ******Another Approach of get details of an individual******
         *   
         *    public ActionResult Details(int? id)
               {
                   using (DbModels dbmodel = new DbModels())
                   {
                       if (id == null)
                       {
                           return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                       }
                       var details = dbmodel.User_Info.Find(id);
                       if (details == null)
                       {
                           return HttpNotFound();
                       }
                       return View(details);
                   }
              }
              */

        // Update/Edit data
        // when click on Update button the rqst comes here & give necessary fields to do the action for the selected id
       
        [HttpGet]
     //   [Authorize(Roles = "Admin, Customer")]
        public ActionResult Update(int id)
        {
            using (DbModels dbmodel = new DbModels())
            {
                var update = dbmodel.User_Info.Where(u => u.UserID == id).FirstOrDefault();
                return View(update);
            }
        }
        //after change or update data it should be saved in DB so it comes in this portion with [HttpPost]

        [HttpPost]
     //   [Authorize(Roles = "Admin, Customer")]
        public ActionResult Update(int id,User_Info usermodel)
        {
             try
             {
                  using (DbModels dbmodel = new DbModels())
                  {
                      dbmodel.Entry(usermodel).State = EntityState.Modified;
                      dbmodel.SaveChanges();
                  }
                  return RedirectToAction("List");//after update,the updated value shown in List,so back to /User_Info/List

             }
             catch
             {
                 return View(); //Update.cshtml pg e rtn krbe
             }
            
        }

        // Delete data

        [HttpGet]
     //   [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            using (DbModels dbmodel = new DbModels())
            {
                var delete = dbmodel.User_Info.Where(d => d.UserID == id).FirstOrDefault();
                return View(delete);
            }
        }
        [HttpPost]
     //   [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id,User_Info usermodel)
        {
            try
            {
                using (DbModels dbmodel = new DbModels())
                {
                    var dlt = dbmodel.User_Info.Where(d => d.UserID == id).FirstOrDefault();
                    dbmodel.User_Info.Remove(dlt);
                    dbmodel.SaveChanges();
                }
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }
    }
}


