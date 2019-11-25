using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LanguageFeatures.Models;
using System.Text;

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public string Index()
        {
            return "Przejście do adresy URL pokazującego przykład";
        }

        public ViewResult AutoProperty()
        {
            // utworzenie nowego obiektu Product
            Product myProduct = new Product();

            // ustawienie wartości właściwości
            myProduct.Name = "Kajak";

            // odczytanie właściwości
            string productName = myProduct.Name;

            // wygenerowanie widoku
            return View("Result", (object)String.Format("Nazwa produktu: {0}", productName));
        }

        public ViewResult CreateProduct()
        {
            // tworzenie nowego obiektu Product
            Product myProduct = new Product
            {
                ProductID = 100,
                Name = "Kajak",
                Description = "Łódka jednoosobowa",
                Price = 275M,
                Category = "Sporty wodne"
            };


            return View("Result", (object)String.Format("Kategoria: {0}", myProduct.Category));
        }

        public ViewResult CreateCollection()
        {
            string[] stringArray = { "jabłko", "pomarańcza", "gruszka" };

            List<int> intList = new List<int> { 10, 20, 30, 40 };

            Dictionary<string, int> myDict = new Dictionary<string, int>
            {
                {"jabłko", 10 }, {"pomarańcza", 20}, {"gruszka", 30}
            };

            return View("Result", (object)stringArray[1]);
        }

        public ViewResult CreteCollection2()
        {
            ShoppingCart cart = new ShoppingCart
            {
                Products = new List<Product>
                {
                    new Product {Name = "Kajak", Price = 275M},
                    new Product {Name = "Kamizelka ratunkowa", Price = 48.95M},
                    new Product {Name = "Połka nożna", Price = 19.50M },
                    new Product {Name = "Flaga narożna", Price = 34.95M},
                }
            };

            decimal cartTotal = cart.TotalPrices();

            return View("Result",
                (object)String.Format("Razem: {0:c}", cartTotal));
        }

        public ViewResult UseExtensionEnumerable()
        {
            IEnumerable<Product> products = new ShoppingCart
            {
                Products = new List<Product>
                {
                    new Product {Name = "Kajak", Price = 275M},
                    new Product {Name = "Kamizelka ratunkowa", Price = 48.95M},
                    new Product {Name = "Połka nożna", Price = 19.50M },
                    new Product {Name = "Flaga narożna", Price = 34.95M}
                }
            };

            // tworzenie i wypełnianie tablicy obiektów Product
            Product[] productArray =
            {
                    new Product {Name = "Kajak", Price = 275M},
                    new Product {Name = "Kamizelka ratunkowa", Price = 48.95M},
                    new Product {Name = "Połka nożna", Price = 19.50M },
                    new Product {Name = "Flaga narożna", Price = 34.95M},
            };

            // pobranie całkowitej wartości produktów do koszyka
            decimal cartTotal = products.TotalPrices();
            decimal arrayTotal = products.TotalPrices();

            return View("Result",
                (object)String.Format("Razem koszyk: {0}, Razem talica: {1}", cartTotal, arrayTotal));
        }


        public ViewResult UseExtensionEnumerable2()
        {
            IEnumerable<Product> products = new ShoppingCart
            {
                Products = new List<Product>
                {
                    new Product {Name = "Kajak", Category="Sporty wodne", Price = 275M},
                    new Product {Name = "Kamizelka ratunkowa", Category="Sporty wodne", Price = 48.95M},
                    new Product {Name = "Piłka nożna", Category="Piłka nożna", Price = 19.50M},
                    new Product {Name = "Flaga narożna", Category="Piłka nożna", Price = 34.95M}
}
            };



            decimal total = 0;
            foreach (Product prod in products.
                Filter(prod => prod.Category == "Piłka nożna" || prod.Price > 20))
            {
                total += prod.Price;
            }
            return View("Result", (object)String.Format("Razem: {0}", total));
        }

        public ViewResult CreateAnonArray()
        {
            var oddsAndEnds = new[]
            {
                new { Name = "MVC", Category = "Wzorzec"},
                new { Name = "Kapelusz", Category = "Odzież"},
                new { Name = "Jabłko", Category = "Owoc"}
            };

            StringBuilder result = new StringBuilder();
            foreach (var item in oddsAndEnds)
            {
                result.Append(item.Name).Append(" ");
            }

            return View("Result", (object)result.ToString());
        }

        public ViewResult FindProducts()
        {
            Product[] products =
            {
                new Product {Name = "Kajak", Category = "Sporty wodne", Price = 275M},
                new Product {Name = "Kamizelka ratunkowa", Category="Sporty wodne", Price = 48.95M},
                new Product {Name = "Piłka nożna", Category="Piłka nożna", Price = 19.50M},
                new Product {Name = "Flaga narożna", Category="Piłka nożna", Price = 34.95M}
            };

            // definiowanie tablicy do przechowywania wyników
            Product[] results = new Product[3];

            // posortowanie tablicy
            Array.Sort(products, (item1, item2) =>
            {
                return Comparer<decimal>.Default.Compare(item1.Price, item2.Price);
            });

            // odczytanie pierwszych trzech pozycji w tablicy
            Array.Copy(products, results, 3);

            // przygotowanie danych wyjściowych
            StringBuilder result = new StringBuilder();
            foreach (Product p in results)
            {
                result.AppendFormat("Cena: {0} ", p.Price);
            }
            return View("Result", (object)result.ToString());

        }

        public ViewResult FindProductWithLinq()
        {
            Product[] products =
            {
                new Product {Name = "Kajak", Category = "Sporty wodne", Price = 275M},
                new Product {Name = "Kamizelka ratunkowa", Category="Sporty wodne", Price = 48.95M},
                new Product {Name = "Piłka nożna", Category="Piłka nożna", Price = 19.50M},
                new Product {Name = "Flaga narożna", Category="Piłka nożna", Price = 34.95M}
            };

            var foundProducts = from match in products
                                orderby match.Price descending
                                select new { match.Name, match.Price };

            // przygotowanie danych wyjściowych
            int count = 0;
            StringBuilder result = new StringBuilder();
            foreach (var p in foundProducts)
            {
                result.AppendFormat("Cena: {0} ", p.Price);
                if (++count == 3)
                {
                    break;
                }
            }

            return View("Result", (object)result.ToString());
        }

        //public ViewResult FindProductWithLinq2()
        //{
        //    Product[] products = {
        //        new Product {Name = "Kajak", Category="Sporty wodne", Price = 275M},
        //        new Product {Name = "Kamizelka ratunkowa", Category="Sporty wodne", Price = 48.95M},
        //        new Product {Name = "Piłka nożna", Category="Piłka nożna", Price = 19.50M},
        //        new Product {Name = "Flaga narożna", Category="Piłka nożna", Price = 34.95M}
        //    };

            //var foundProducts = products.OrderByDescending(e => e.Price)
            //    .Take(3)
            //    .Select(e => new { e.Name, e.Price });

            //StringBuilder result = new StringBuilder();
            //foreach (Product p in foundProducts)
            //{
            //    result.AppendFormat("Cena: {0} ", p.Price);
            //}

            //return View("Result", (object)result.ToString());
        //}
    }
}