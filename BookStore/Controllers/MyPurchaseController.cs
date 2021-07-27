using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations;
using BookStore.Help;

namespace BookStore.Controllers
{
    public class MyPurchaseController : Controller
    {
        private readonly AppDbContext appDbContext;

        public MyPurchaseController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public IActionResult Index(int cartid = 0, int customerid = 0)
        {
            Session session = appDbContext.Sessions.FirstOrDefault(x => x.Id == HttpContext.Request.Cookies["sessionId"]);
            if (session == null)
            {
                return RedirectToAction("login", "account");
            }
            if (customerid == 0)
            {            
                // current cart date
                List<Cart> cartpruchased = appDbContext.Carts.Where(x => x.Id == cartid).ToList();
                DateTime dt = new DateTime();
                foreach (Cart ct in cartpruchased)
                {
                    ct.CheckoutTime = DateTime.Now;
                    dt = ct.CheckoutTime;
                    if (ct.IsCheckOut == false)
                    {
                        ct.IsCheckOut = true;
                        appDbContext.SaveChanges();
                    }
                }

                //current cartitem act
                List<CartItem> crtitems = appDbContext.CartItems.Where(x => x.CartId == cartid).ToList();
                foreach (CartItem crtitem in crtitems)
                {
                    crtitem.CheckoutTime = dt;
                    List<PurcahsedActivationCode> pdtacts = new List<PurcahsedActivationCode> { };
                    for (int i = 0; i < crtitem.Quantity; i++)
                    {
                        PurcahsedActivationCode pdtact = new PurcahsedActivationCode();
                        pdtact.CartItemId = crtitem.Id;
                        pdtact.ActivationCode = GetAct();
                        pdtacts.Add(pdtact);
                    }

                    crtitem.ActivationCodes = pdtacts;
                    appDbContext.SaveChanges();
                }

                int cusid = 0;
                foreach ( Cart ctp in cartpruchased)
                {
                    cusid = ctp.CustomerId;
                }

                List<Cart> historycart = appDbContext.Carts.Where(x => x.CustomerId == cusid && x.IsCheckOut == true).ToList();

                List<int> hiscartid = new List<int> { };

                foreach (Cart ct in historycart)
                {
                    hiscartid.Add(ct.Id);
                }

                List<CartItem> hispdt = new List<CartItem> { };
                foreach (int hsit in hiscartid)
                {
                    List<CartItem> temps = appDbContext.CartItems.Where(x => x.CartId == hsit).ToList();
                    foreach(CartItem temp in temps)
                    {
                        hispdt.Add(temp);
                    }
                }
               ViewData["hispdt"] = hispdt;
                return View();
            }
            else
            {
                List<Cart> historycart = appDbContext.Carts.Where(x => x.CustomerId == customerid && x.IsCheckOut == true).ToList();

                List<int> hiscartid = new List<int> { };

                foreach (Cart ct in historycart)
                {
                    hiscartid.Add(ct.Id);
                }

                List<CartItem> hispdt = new List<CartItem> { };
                foreach (int hsit in hiscartid)
                {
                    List<CartItem> temps = appDbContext.CartItems.Where(x => x.CartId == hsit).ToList();
                    foreach (CartItem temp in temps)
                    {
                        hispdt.Add(temp);
                    }
                }
                ViewData["hispdt"] = hispdt;
                return View();
            }

        }























        public static string GetAct()
        {
            string s1 = "e" + GenerateCheckCodeNum(4);
            string s2 = GenerateCheckCodeNum(3);
            string s3 = GenerateRandomLetter(4);
            string s4 = GenerateCheckCodeNum(3);
            string code = s1 + "-" + s2 + "-" + s3 + "-" + s4;
            return code;
        }
        public static string GenerateRandomLetter(int Length)
        {
            char[] Pattern = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            string result = "";
            int n = Pattern.Length;
            Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
        public static string GenerateCheckCodeNum(int codeCount)
        {
            int rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = random.Next();
                str += ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }
    }
}
