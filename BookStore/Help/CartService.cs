using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Help
{
    public class CartService
    {
        private readonly AppDbContext appDbContext;

        public CartService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        #region Cart

        public int AddProductsToCart(int customerId, int productId, int quantity)
        {
            using (var tran = appDbContext.Database.BeginTransaction())
            {
                try
                {
                    AddCart(customerId);
                    Cart cart = appDbContext.Carts.FirstOrDefault(x => x.CustomerId == customerId && x.IsCheckOut == false);
                    AddCartItem(productId, quantity, cart);
                    appDbContext.SaveChanges();
                    tran.Commit();
                    return cart.Quantity;
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        public void AddCartWithCustomer(int customerId, Cart cart)
        {
            using (var tran = appDbContext.Database.BeginTransaction())
            {
                try
                {
                    var newCart = new Cart(customerId);
                    newCart.Quantity = cart.Quantity;
                    newCart.CreationTime = cart.CreationTime;
                    newCart.Value = cart.Value;
                    appDbContext.Carts.Add(newCart);
                    appDbContext.SaveChanges();
                    foreach (var item in cart.CartItems)
                    {
                        var cartitem = new CartItem
                        {
                            CartId = newCart.Id,
                            BookId = item.BookId,
                            Quantity = item.Quantity
                        };
                        appDbContext.CartItems.Add(cartitem);
                    }
                    appDbContext.SaveChanges();
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                }
            }
        }

        public void AddCart(int customerId)
        {
            Cart cart = appDbContext.Carts.FirstOrDefault(x => 
            x.CustomerId == customerId && x.IsCheckOut == false);

            if (cart == null)
            {
                cart = new Cart(customerId);
                appDbContext.Carts.Add(cart);
            };

            appDbContext.SaveChanges();
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
        #endregion

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
