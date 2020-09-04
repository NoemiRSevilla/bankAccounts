using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using bankAccounts.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

// Other using statements
namespace HomeController.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult RegisterUser()
        {
            string UserInSession = HttpContext.Session.GetString("Email");
            if (UserInSession != null)
            {
                User retrievedUser = dbContext.users.FirstOrDefault(user => user.Email == UserInSession);
                return Redirect($"account/{retrievedUser.UserId}");
            }
            else
            {
                return View();
            }
        }

        [HttpPost("submit")]
        public IActionResult Submit(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.users.Any(u => u.Email == newUser.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");
                    // You may consider returning to the View at this point
                    return View("RegisterUser");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    dbContext.Add(newUser);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetString("Email", newUser.Email);
                    return Redirect($"account/{newUser.UserId}");
                }
            }
            else
            {
                TempData["First Name"] = newUser.FirstName;
                TempData["Last Name"] = newUser.LastName;
                TempData["Email"] = newUser.Email;
                return View("RegisterUser");
            }
        }

        [HttpGet("account/{UserId}")]
        public IActionResult ShowAccount(int UserId)
        {
            List<Transaction> AllTransactions = dbContext.transactions.Include(t => t.OwnerOfAccount).Where(u => u.UserId == UserId).ToList();

            User retrievedUser = dbContext.users.FirstOrDefault(user => user.UserId == UserId);

            int currentBalance = AllTransactions.Sum(c => c.Amount);

            ViewBag.currentBalance = currentBalance;
            ViewBag.AllTransactions = AllTransactions;
            ViewBag.retrievedUser = retrievedUser;
            return View();
        }

        [HttpPost("interact")]
        public IActionResult Interact(Transaction newT)
        {
            dbContext.Add(newT);
            dbContext.SaveChanges();
            return Redirect ($"account/{newT.UserId}");
        }

        [HttpGet("loginuser")]
        public IActionResult LoginUser()
        {
            string UserInSession = HttpContext.Session.GetString("Email");
            if (UserInSession != null)
            {
                var userInDb = dbContext.users.FirstOrDefault(u => u.Email == UserInSession);
                return Redirect($"account/{userInDb.UserId}");
            }
            else
            {
                return View("LoginUser");
            }
        }

        [HttpPost("submitlogin")]
        public IActionResult SubmitLogin(LoginUser userSubmission)
        {
            if (ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.users.FirstOrDefault(u => u.Email == userSubmission.Email);
                // If no user exists with provided email
                if (userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("Email", "You could not be logged in");
                    return View("LoginUser");
                }

                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();

                // verify provided password against hash stored in dbcopy
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);

                // result can be compared to 0 for failure
                if (result == 0)
                {
                    ModelState.AddModelError("Email", "You could not be logged in");
                    return View("LoginUser");
                }
                HttpContext.Session.SetString("Email", userSubmission.Email);

                return Redirect($"account/{userInDb.UserId}");
            }
            else
            {
                return View("LoginUser");
            }
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Register User");
        }
    }
}