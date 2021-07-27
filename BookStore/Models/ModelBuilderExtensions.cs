using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData
                (
                    new Book
                    {
                        Id = 1,
                        Name = ".NET Charts",
                        Desc = "Brings powerful charting capabilities to your .NET applications.",
                        UnitPrice = 99,
                        Image = "/images/Charts.png",
                        Tags = "power | chart | application"
                    },

                    new Book
                    {
                        Id = 2,
                        Name = ".NET PayPal",
                        Desc = "Integrate your .NET apps with PayPal the easy way!",
                        UnitPrice = 69,
                        Image = "/images/PayPal.png",
                        Tags = "integrate | pay | app"
                    },

                    new Book
                    {
                        Id = 3,
                        Name = ".NET ML",
                        Desc = "Supercharged .NET machine learning libraries.",
                        UnitPrice = 299,
                        Image = "/images/ML.png",
                        Tags = "machine | learn | ml"
                    },

                    new Book
                    {
                        Id = 4,
                        Name = ".NET Analytics",
                        Desc = "Performs data mining and analytics easily in .NET.",
                        UnitPrice = 299,
                        Image = "/images/Analytics.png",
                        Tags = "data | mining | analytic"
                    },

                    new Book
                    {
                        Id = 5,
                        Name = ".NET Logger",
                        Desc = "Logs and aggregates events easily in your .NET apps.",
                        UnitPrice = 49,
                        Image = "/images/Logger.png",
                        Tags = "log | event | aggregate | app"
                    },

                    new Book
                    {
                        Id = 6,
                        Name = ".NET Numerics",
                        Desc = "Powerful numerical methods for your .NET simulations. ",
                        UnitPrice = 199,
                        Image = "/images/Numerics.png",
                        Tags = "power | num | simulat"
                    }
                );

        }
    }
}
