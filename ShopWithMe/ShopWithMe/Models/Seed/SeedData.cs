using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopWithMe.Models.Products;
using ShopWithMe.Models.ProductCategories;

namespace ShopWithMe.Models.Seed
{
    public static class SeedData
    {
        public static void EnsurePopulated(IServiceProvider serviceProvider)
        {
            using var context = new DefaultContext(
                serviceProvider.GetRequiredService<DbContextOptions<DefaultContext>>());

            // Usuń istniejące produkty
            /*
            if (context.Products.Any())
            {
                context.Products.RemoveRange(context.Products);
                context.SaveChanges();
            }
            */

            // Dodaj kategorie, jeśli nie istnieją
            if (!context.ProductCategories.Any())
            {
                var categories = new[]
                {
                    new ProductCategory { Name = "Elektronika" },
                    new ProductCategory { Name = "AGD" },
                    new ProductCategory { Name = "Gaming" },
                    new ProductCategory { Name = "Dom i Ogród" },
                    new ProductCategory { Name = "Sport i Rekreacja" },
                    new ProductCategory { Name = "Zdrowie i Uroda" },
                    new ProductCategory { Name = "Motoryzacja" },
                    new ProductCategory { Name = "Dziecko" },
                    new ProductCategory { Name = "Kuchnia" },
                    new ProductCategory { Name = "Narzędzia i Warsztat" },
                    new ProductCategory { Name = "Druk 3D i Akcesoria" },
                    new ProductCategory { Name = "Technologia i Akcesoria" },
                };

                context.ProductCategories.AddRange(categories);
                context.SaveChanges();
            }

            var categoryIds = context.ProductCategories.ToDictionary(c => c.Name, c => c.Id);

            var products = new[]
            {
                new Product
                {
                    Name = "Smartfon Galaxy Z",
                    Lead = "Nowoczesny smartfon z elastycznym ekranem.",
                    Description = "Smartfon Galaxy Z to rewolucyjny krok w technologii mobilnej, oferujący elastyczny ekran umożliwiający składanie urządzenia. Dzięki najnowszemu procesorowi, wytrzymałej baterii oraz niezwykle wydajnemu aparatowi fotograficznemu, jest to idealny wybór dla nowoczesnego użytkownika.",
                    Price = 3999.00m,
                    CategoryId = categoryIds["Elektronika"]
                },
                new Product
                {
                    Name = "Laptop Pro X",
                    Lead = "Ultracienki laptop z wydajnym procesorem.",
                    Description = "Laptop Pro X to elegancki, ultracienki laptop wyposażony w najnowszą generację procesorów Intel Core i7, zapewniający płynną pracę nawet w najbardziej wymagających zadaniach. Jego wyjątkowy design i długa żywotność baterii czynią go idealnym wyborem dla profesjonalistów i studentów.",
                    Price = 5999.00m,
                    CategoryId = categoryIds["Elektronika"]
                },
                new Product
                {
                    Name = "Telewizor UltraHD 65\"",
                    Lead = "Telewizor 4K z obsługą HDR.",
                    Description = "Ciesz się wyjątkową jakością obrazu dzięki Telewizorowi UltraHD 65\", który oferuje niesamowitą rozdzielczość 4K, obsługę HDR i wbudowane aplikacje streamingowe. Dzięki dużemu ekranowi możesz zanurzyć się w kinowej jakości obrazu z wygodą swojego domu.",
                    Price = 2999.00m,
                    CategoryId = categoryIds["Elektronika"]
                },
                new Product
                {
                    Name = "Robot sprzątający CleanBot",
                    Lead = "Automatyczny robot do odkurzania i mopowania.",
                    Description = "CleanBot to inteligentny robot sprzątający, który skutecznie usuwa kurz i zabrudzenia z podłóg dzięki zaawansowanym czujnikom i funkcji mopowania. Idealne rozwiązanie dla osób ceniących wygodę i oszczędność czasu.",
                    Price = 1599.00m,
                    CategoryId = categoryIds["Dom i Ogród"]
                },
                new Product
                {
                    Name = "Smartwatch FitTime",
                    Lead = "Inteligentny zegarek z funkcjami fitness.",
                    Description = "Smartwatch FitTime to doskonały towarzysz dla aktywnych osób. Monitoruj swoje kroki, puls, a nawet jakość snu. Dzięki stylowemu designowi i wytrzymałej baterii, możesz nosić go zarówno podczas treningów, jak i na co dzień.",
                    Price = 799.00m,
                    CategoryId = categoryIds["Sport i Rekreacja"]
                },
                new Product
                {
                    Name = "Konsola GameBox 360",
                    Lead = "Nowa generacja konsoli do gier.",
                    Description = "GameBox 360 to konsola nowej generacji, która oferuje niezwykłe wrażenia z gry dzięki zaawansowanej grafice, szybkiej wydajności i szerokiej bibliotece gier. To idealny wybór dla każdego gracza.",
                    Price = 2499.00m,
                    CategoryId = categoryIds["Gaming"]
                },
                new Product
                {
                    Name = "Fotel gamingowy GamerChair",
                    Lead = "Ergonomiczny fotel dla graczy.",
                    Description = "GamerChair zapewnia komfort i wsparcie podczas długich godzin grania. Dzięki regulowanym podłokietnikom, podparciu lędźwiowemu i stylowemu designowi jest to nieodzowny element wyposażenia każdego gracza.",
                    Price = 599.00m,
                    CategoryId = categoryIds["Gaming"]
                },
                new Product
                {
                    Name = "Zestaw garnków KitchenPro",
                    Lead = "Komplet garnków ze stali nierdzewnej.",
                    Description = "Zestaw garnków KitchenPro to wysokiej jakości stal nierdzewna, która zapewnia doskonałe rezultaty podczas gotowania. Ergonomiczne uchwyty i wielowarstwowe dno sprawiają, że gotowanie staje się przyjemnością.",
                    Price = 299.00m,
                    CategoryId = categoryIds["Kuchnia"]
                },
                new Product
                {
                    Name = "Hulajnoga elektryczna eScoot 500",
                    Lead = "Lekka hulajnoga elektryczna do jazdy miejskiej.",
                    Description = "Hulajnoga eScoot 500 to szybki i ekologiczny sposób poruszania się po mieście. Wyposażona w pojemną baterię i wygodny system składania, jest idealnym rozwiązaniem dla osób w ruchu.",
                    Price = 1999.00m,
                    CategoryId = categoryIds["Sport i Rekreacja"]
                },
                new Product
                {
                    Name = "Drukarka 3D PrintMaster",
                    Lead = "Drukarka 3D do domowego użytku.",
                    Description = "PrintMaster to drukarka 3D stworzona dla kreatywnych osób. Twórz precyzyjne modele i prototypy w zaciszu własnego domu. Prosta w obsłudze i niezwykle wszechstronna.",
                    Price = 1499.00m,
                    CategoryId = categoryIds["Druk 3D i Akcesoria"]
                }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
