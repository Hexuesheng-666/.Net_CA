using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class WishlistController : Controller
    {

        private readonly IBookRepository bookRepository;
        private readonly AppDbContext appDbContext;

        public WishlistController(IBookRepository bookRepository, AppDbContext appDbContext)
        {
            this.bookRepository = bookRepository;
            this.appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session thisSession = appDbContext.Sessions.FirstOrDefault(x => x.Id == sessionId);
            if (thisSession != null)
                ViewData["username"] = thisSession.Customer.UserName.ToString();

            Session session = appDbContext.Sessions.FirstOrDefault(x => x.Id == HttpContext.Request.Cookies["sessionId"]);

            Customer currentCustomer = appDbContext.Customers.FirstOrDefault(x => x.Id == session.Customer.Id);

            List<Wishlist_AJAX> DisplayWishlistItems = appDbContext.Wishlist_AJAX.Where(x => x.Customer.Id == session.Customer.Id).ToList();

            ViewData["DisplayWishlistItems"] = DisplayWishlistItems;

            Cart cart = appDbContext.Carts.FirstOrDefault(x => x.CustomerId == session.Customer.Id && x.IsCheckOut == false);

            if (cart != null)
            {
                int count = cart.Quantity;
                ViewData["Count"] = count;
            }

            return View();
        }

        public IActionResult AddToWishlist([FromBody] Wishlist_AJAX wishlistAJAX)
        {
            Session session = appDbContext.Sessions.FirstOrDefault(x => x.Id == HttpContext.Request.Cookies["sessionId"]);

            if (session == null)
            {
                return RedirectToAction("login", "account");
            }

            Customer currentCustomer = appDbContext.Customers.FirstOrDefault(x => x.Id == session.Customer.Id);

            int wishlistIDInt = Int32.Parse(wishlistAJAX.wishlistID);
            Book addWishlistItem = appDbContext.Books.FirstOrDefault(x => x.Id == wishlistIDInt);

            if (wishlistAJAX.WishlistStatus == true)
            {
                Wishlist_AJAX newWishlistItem = new Wishlist_AJAX
                {
                    wishlistID = wishlistAJAX.wishlistID,
                    sessionID = session,
                    item = addWishlistItem,
                    Customer = currentCustomer
                };

                appDbContext.Add(newWishlistItem);
                appDbContext.SaveChanges();
            }

            if (wishlistAJAX.WishlistStatus == false)
            {
                List<Wishlist_AJAX> deleteWishlistItem = appDbContext.Wishlist_AJAX.Where(x => x.Customer.Id == session.Customer.Id && x.wishlistID == wishlistAJAX.wishlistID).ToList();

                foreach (Wishlist_AJAX deleteItem in deleteWishlistItem)
                    appDbContext.Wishlist_AJAX.Remove(deleteItem);

                appDbContext.SaveChanges();
            }

            return View();
        }
    }
}
