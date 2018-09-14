using System.Web.Mvc;
using Ecobit_Blockchain_Frontend.Exceptions;
using Ecobit_Blockchain_Frontend.DataAccess.Interfaces;
using Ecobit_Blockchain_Frontend.Models;
using Ecobit_Blockchain_Frontend.Utils;
using Ecobit_Blockchain_Frontend.Utils.Auth;

namespace Ecobit_Blockchain_Frontend.Controllers
{
    [AuthenticationFilterAttribute]
    public class UserController : Controller
    {
        private readonly IUserDao _userDao;

        public UserController() : this(DependencyFactory.Resolve<IUserDao>())
        {
        }

        public UserController(IUserDao userDao)
        {
            _userDao = userDao;
        }
        
        /// <summary>
        /// Gets all users on the system and sends out a view
        /// </summary>
        /// <returns>View with all the users on the system</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// Redirects to update page to update a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The view of the update profile page</returns>
        
        [HttpGet]
        public ActionResult Update(string id)
        {
            var user = _userDao.Read(id);
            
            return user.IsNotValid() ? View("NotFound") : View(user);
        }

        /// <summary>
        /// Update user data.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The view of the user profile</returns>
        [HttpPost]
        public ActionResult Update(User user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("InvalidEditUser", "Gegevens niet correct ingevuld.");
                return RedirectToAction("Update", user.Companyname);
            }
            
            _userDao.Update(user);
            return View("Profile", user);

        }
        
        /// <summary>
        /// Get the user profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The view of the user profile</returns>
        [HttpGet]
        public ActionResult Profile(string id)
        {
            var user = _userDao.Read(id);

            return user.IsNotValid() ? View("NotFound") : View(user);
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The view with a list of all the users</returns>
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var user = _userDao.Read(id);
            
            if (user.IsNotValid())
            {
                return View("NotFound");
            }
            
            _userDao.Delete(user);
            return RedirectToAction("Index", "User");
        }

        
        /// <summary>
        /// Returns create view
        /// </summary>
        /// <returns>The view that creates a new user</returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The view of the user profile</returns>
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("InvalidCreateUser", "Gegevens niet correct ingevuld.");
                return View("Create", user);
            }
            
            try
            {
                _userDao.Create(user);
            }
            catch (UserException e)
            {
                return View("Error", e);
            }

            return View("Profile", user);
        }
    }
}