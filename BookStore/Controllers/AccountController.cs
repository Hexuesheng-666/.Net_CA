using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext appDbContext;
        private readonly ICustomerRepository customerRepository;

        public AccountController(AppDbContext appDbContext, ICustomerRepository customerRepository)
        {
            this.appDbContext = appDbContext;
            this.customerRepository = customerRepository;
        }

        public IActionResult Login()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            if (sessionId != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            Customer customer = appDbContext.Customers.FirstOrDefault(x => 
                                x.UserName == model.UserName && x.Password == model.Password);

            if (customer == null)
            {
                ViewData["errMsg"] = "Invalid Login Attempt";
                return View("Login");
            }
            else
            {
                Session session = new Session()
                {
                    Id = Guid.NewGuid().ToString(),
                    Customer = customer
                };

                appDbContext.Sessions.Add(session);

                appDbContext.SaveChanges();

                Response.Cookies.Append("sessionId", session.Id);

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];

            appDbContext.Sessions.Remove(new Session() { Id = sessionId });

            appDbContext.SaveChanges();

            HttpContext.Response.Cookies.Delete("sessionId");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            if (sessionId != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            Customer customer = appDbContext.Customers.FirstOrDefault(x =>
                    x.UserName == model.UserName);

            if (customer != null)
            {
                ViewData["errMsg"] = "Current UserName: " + model.UserName + " is already in use";
                return View();
            }
            else
            {
                Customer cstmr = new Customer()
                {
                    UserName = model.UserName,
                    Password = model.Password
                };
                customerRepository.Add(cstmr);
            }

            return RedirectToAction("Login", "Account");

        }


        [HttpPost]
        [HttpGet]
        public IActionResult IsUserNameInUse(string username)
        {
            var user = appDbContext.Customers.FirstOrDefault(x=>x.UserName == username);

            if (user != null)
            {
                return Json($"UserName {username} is already in use");
                
            }
            else
            {
                return Json(true);
            }
        }
    }
}
