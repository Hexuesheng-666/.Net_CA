using BookStore.Help;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        public IActionResult ViewCart()
        {            
            Session session = appDbContext.Sessions.FirstOrDefault(x => x.Id == HttpContext.Request.Cookies["sessionId"]);
            if (session == null)
            {
                return RedirectToAction("index", "home");
            }
            Cart cart = appDbContext.Carts.FirstOrDefault(x => x.CustomerId == session.Customer.Id && x.IsCheckOut == false);
            if (cart == null)
            {
                return RedirectToAction("index", "home");
            }
            List<CartItem> itemincart = appDbContext.CartItems.Where(x => x.CartId == cart.Id).ToList();
            ViewData["itemincart"] = itemincart;
            return View();
        }

        private readonly AppDbContext appDbContext;

        public CartController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        #region not sure use or not

        //public int AddProductsToCart(int customerId, int productId, int quantity)
        //{
        //    using (var tran = appDbContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            AddCart(customerId);
        //            Cart cart = appDbContext.Carts.FirstOrDefault(x => x.CustomerId == customerId && x.IsCheckOut == false);
        //            AddCartItem(productId, quantity, cart);
        //            appDbContext.SaveChanges();
        //            tran.Commit();
        //            return cart.Quantity;
        //        }
        //        catch (Exception e)
        //        {
        //            tran.Rollback();
        //            throw new Exception(e.Message);
        //        }
        //    }
        //}

        //public void AddCartWithCustomer(int customerId, Cart cart)
        //{
        //    using (var tran = appDbContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var newCart = new Cart(customerId);
        //            newCart.Quantity = cart.Quantity;
        //            newCart.CreationTime = cart.CreationTime;
        //            newCart.Value = cart.Value;
        //            appDbContext.Carts.Add(newCart);
        //            appDbContext.SaveChanges();
        //            foreach (var item in cart.CartItems)
        //            {
        //                var cartitem = new CartItem
        //                {
        //                    CartId = newCart.Id,
        //                    BookId = item.BookId,
        //                    Quantity = item.Quantity
        //                };
        //                appDbContext.CartItems.Add(cartitem);
        //            }
        //            appDbContext.SaveChanges();
        //            tran.Commit();
        //        }
        //        catch (Exception)
        //        {
        //            tran.Rollback();
        //        }
        //    }
        //}

        //

        #endregion
        public IActionResult AddCart(int id)
        {
            Book book = appDbContext.Books.FirstOrDefault(x => x.Id == id);

            Session session = appDbContext.Sessions.FirstOrDefault(x => x.Id == HttpContext.Request.Cookies["sessionId"]);
            if (session == null)
            {
                return RedirectToAction("login", "account");
            }

            else
            {
                Cart cart = appDbContext.Carts.FirstOrDefault(x => x.CustomerId == session.Customer.Id && x.IsCheckOut == false);

                if (cart == null)
                {
                    cart = new Cart(session.Customer.Id);
                    appDbContext.Carts.Add(cart);
                };
                CartItem cartItem = new CartItem();
                cartItem.Book = book;
                cartItem.CartId = cart.Id;
                cartItem.Quantity = 1;
                cartItem.BookId = book.Id;
                cartItem.Cart = cart;
                if(appDbContext.CartItems.FirstOrDefault(x => x.BookId==cartItem.BookId && x.CartId ==cartItem.CartId)==null)
                {          
                    appDbContext.CartItems.Add(cartItem);
                    appDbContext.SaveChanges();
                }
                else
                {
                    appDbContext.CartItems.FirstOrDefault(x => x.BookId == cartItem.BookId && x.CartId == cartItem.CartId).Quantity += 1;
                    appDbContext.SaveChanges();
                }


                cart.Quantity += 1;
                appDbContext.SaveChanges();

                List<CartItem> itemincart = appDbContext.CartItems.Where(x => x.CartId == cart.Id).ToList();

                return RedirectToAction("index", "home");

            }
            
        }

        public IActionResult Remove(int cartid, int pdtid)
        {
            List<CartItem> itemtoremove = appDbContext.CartItems.Where(x => x.BookId == pdtid && x.CartId == cartid).ToList();
            List<Cart> carttoremove = appDbContext.Carts.Where(x => x.Id == cartid).ToList();
            int num = appDbContext.Carts.FirstOrDefault(x => x.Id == cartid).Quantity - itemtoremove[0].Quantity;
            if (num <= 0)
            {
                foreach (Cart ctr in carttoremove)
                {
                    appDbContext.Carts.Remove(ctr);
                    appDbContext.SaveChanges();
                }
            }
            else
            {
                foreach (CartItem cartit in itemtoremove)
                {
                    appDbContext.Carts.FirstOrDefault(x => x.Id == cartid).Quantity -= itemtoremove[0].Quantity;
                    appDbContext.CartItems.Remove(cartit);
                    appDbContext.SaveChanges();
                }
            }
            return RedirectToAction("ViewCart", "Cart");
        }

        public IActionResult Add(int cartid, int pdtid)
        {
            appDbContext.Carts.FirstOrDefault(x => x.Id == cartid).Quantity += 1;
            appDbContext.CartItems.FirstOrDefault(x => x.BookId == pdtid && x.CartId == cartid).Quantity += 1;
            appDbContext.SaveChanges();
            return RedirectToAction("ViewCart", "Cart");
        }

        public IActionResult Minus(int cartid, int pdtid)
        {
            appDbContext.Carts.FirstOrDefault(x => x.Id == cartid).Quantity -= 1;
            appDbContext.CartItems.FirstOrDefault(x => x.BookId == pdtid && x.CartId == cartid).Quantity -= 1;
            int num1 = appDbContext.Carts.FirstOrDefault(x => x.Id == cartid).Quantity;
            int num2 = appDbContext.CartItems.FirstOrDefault(x => x.BookId == pdtid && x.CartId == cartid).Quantity;
            appDbContext.SaveChanges();
            List<Cart> carttoremove = appDbContext.Carts.Where(x => x.Id == cartid).ToList();
            if (num1 <= 0)
            {
                foreach (Cart ctr in carttoremove)
                {
                    appDbContext.Carts.Remove(ctr);
                    appDbContext.SaveChanges();
                }
                return RedirectToAction("ViewCart", "Cart");
            }
            else
            {
                if (num2 <= 0)
                {
                    return RedirectToAction("Remove", "Cart", new { cartid = cartid, pdtid = pdtid });
                }
                else
                {
                    return RedirectToAction("ViewCart", "Cart");
                }
            }
        }


        public int GetNumberOfCartItem(int customerId)
        {
            Cart cart = appDbContext.Carts.FirstOrDefault(x =>
            x.CustomerId == customerId && x.IsCheckOut == false);

            return cart?.Quantity ?? 0;
            //same as above syntax.
            //if (cart != null)
            //{
            //    return cart.Quantity;
            //}
            //else
            //    return 0;
        }

        public void AddCartItem(int productId, int quantity, Cart cart)
        {
            Book book = appDbContext.Books.First(x => x.Id == productId);

            CartItem cartItem = appDbContext.CartItems.FirstOrDefault(x =>
            x.CartId == cart.Id && x.BookId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem(cart.Id, productId);
                appDbContext.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }
            if (cartItem.Quantity == 0)
            {
                appDbContext.CartItems.Remove(cartItem);
            }

            cart.Quantity += quantity;
            cart.Value += quantity * book.UnitPrice;
        }

        public Cart GetCartForCustomer(int customerId)
        {
            Cart cart = appDbContext.Carts.Where(cart =>
            cart.CustomerId == customerId && !cart.IsCheckOut).FirstOrDefault();

            if (cart != null)
            {
                cart.CartItems = appDbContext.CartItems.Where(x => x.CartId == cart.Id).ToList<CartItem>();
            }

            return cart ?? new Cart();
        }

        //public int CheckoutCart(int customerId)
        //{
        //    Cart cart = appDbContext.Carts.First(x => x.CustomerId == customerId && x.IsCheckOut == false);
        //    cart.IsCheckOut = true;
        //    cart.CheckoutTime = DateTime.Now;
        //    appDbContext.Carts.Update(cart);
        //    appDbContext.SaveChanges();

        //    List<CartItem> cartItems = appDbContext.CartItems.Where(x => x.CartId == cart.Id).ToList();
        //    foreach (var cartItem in cartItems)
        //    {
        //        for (int i = 0; i < cartItem.Quantity; i++)
        //        {
        //            string activationCode = GetActivationCode(cartItem.CartId, cartItem.BookId);
        //            PurcahsedActivationCode p = new PurcahsedActivationCode(cartItem.Id, activationCode);
        //            appDbContext.PurcahsedActivationCodes.Add(p);
        //        }
        //        cartItem.CheckoutTime = DateTime.Now;
        //        appDbContext.Update(cartItem);
        //    }
        //    appDbContext.SaveChanges();
        //    return cart.Id;
        //}
 

        public List<Cart> GetPurchasedHistory(int customerId)
        {
            var carts = appDbContext.Carts.Where(x => x.CustomerId == customerId && x.IsCheckOut).ToList();
            return carts;
        }

        public Cart GetCartById(int cartId)
        {
            Cart cart = appDbContext.Carts.First(x => x.Id == cartId);
            cart.CartItems = appDbContext.CartItems.Where(x => x.CartId == cartId).ToList();
            foreach (var item in cart.CartItems)
            {
                item.ActivationCodes = appDbContext.PurcahsedActivationCodes.Where(x =>
                x.CartItemId == item.Id).ToList();
            }
            return cart;
        }

        private static string GetActivationCode(int cartId, int productId)
        {
            return cartId + "-" + productId + "-" + Guid.NewGuid().ToString();
        }
    }
}

