using MvcApp.Models;
using System;
using System.Linq;

namespace MvcApp.Data
{
    public static class ProductSeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Products.Any())
                return;

            var products = new Product[]
            {
                new Product
                {
                    Name = "Ноутбук Machenike",
                    Price = 69999,
                    Category = "Электроника",
                    Description = "Игровой ноутбук",
                    CreatedDate = DateTime.Now,
                    InStock = true
                },
                new Product
                {
                    Name = "Смартфон POCOX8",
                    Price = 24999,
                    Category = "Электроника",
                    Description = "Современный смартфон",
                    CreatedDate = DateTime.Now,
                    InStock = true
                }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}