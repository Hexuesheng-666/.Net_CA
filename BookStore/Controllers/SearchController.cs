using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class SearchController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly AppDbContext appDbContext;

        public SearchController(IBookRepository bookRepository, AppDbContext appDbContext)
        {
            this.bookRepository = bookRepository;
            this.appDbContext = appDbContext;
        }

        public IActionResult Index(string query)
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session session = appDbContext.Sessions.FirstOrDefault(x => x.Id == sessionId);
            if (session != null)
            {
                ViewData["username"] = session.Customer.UserName.ToString();

                Cart cart = appDbContext.Carts.FirstOrDefault(x => x.CustomerId == session.Customer.Id && x.IsCheckOut == false);

                if (cart != null)
                {
                    int count = cart.Quantity;
                    ViewData["Count"] = count;
                }

                List<Wishlist_AJAX> CustomerWishlist = appDbContext.Wishlist_AJAX.Where(x => x.Customer.Id == session.Customer.Id).ToList();
                ViewData["CustomerWishlist"] = CustomerWishlist;
            }
                
            if (query == null)
                return RedirectToAction("Index", "Home");

            var allProducts = bookRepository.GetAllBook();

            query = query.ToLower();
            ViewData["query"] = query;

            return View(allProducts);
        }
    }
}
