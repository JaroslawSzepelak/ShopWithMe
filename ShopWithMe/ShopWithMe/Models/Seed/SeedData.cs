using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopWithMe.Models.Products;

namespace ShopWithMe.Models.Seed
{
    public static class SeedData
    {
        public static void EnsurePopulated(IServiceProvider serviceProvider)
        {
            using var context = new DefaultContext(
                serviceProvider.GetRequiredService<DbContextOptions<DefaultContext>>());

            /*
                context.Products.AddRange(
                    new Product { Name = "Smartfon Galaxy Z", Description = "Nowoczesny smartfon z elastycznym ekranem.", Price = 3999.00m },
                    new Product { Name = "Laptop Pro X", Description = "Ultracienki laptop z wydajnym procesorem.", Price = 5999.00m },
                    new Product { Name = "Telewizor UltraHD 65\"", Description = "Telewizor 4K z obsługą HDR.", Price = 2999.00m },
                    new Product { Name = "Głośnik Bluetooth Bass", Description = "Przenośny głośnik z głębokim basem.", Price = 299.00m },
                    new Product { Name = "Słuchawki bezprzewodowe AirSound", Description = "Wysokiej jakości dźwięk bez przewodów.", Price = 699.00m },
                    new Product { Name = "Konsola GameBox 360", Description = "Nowa generacja konsoli do gier.", Price = 2499.00m },
                    new Product { Name = "Aparat cyfrowy FotoSharp", Description = "Lustrzanka z 24MP matrycą i obiektywem.", Price = 3499.00m },
                    new Product { Name = "Mikser kuchenny MixMaster", Description = "Wielofunkcyjny mikser kuchenny o dużej mocy.", Price = 499.00m },
                    new Product { Name = "Robot sprzątający CleanBot", Description = "Automatyczny robot do odkurzania i mopowania.", Price = 1599.00m },
                    new Product { Name = "Smartwatch FitTime", Description = "Inteligentny zegarek z funkcjami fitness.", Price = 799.00m },
                    new Product { Name = "Lodówka Side-by-Side", Description = "Nowoczesna lodówka z systemem No Frost.", Price = 4499.00m },
                    new Product { Name = "Oczyszczacz powietrza PureAir", Description = "Skutecznie usuwa zanieczyszczenia i alergeny.", Price = 999.00m },
                    new Product { Name = "Tablet TabView 10\"", Description = "Tablet z ekranem 10-calowym i dużą baterią.", Price = 1299.00m },
                    new Product { Name = "Kamera sportowa ActionCam 4K", Description = "Kamera sportowa z obsługą 4K.", Price = 899.00m },
                    new Product { Name = "Ekspres do kawy Barista Pro", Description = "Automatyczny ekspres do kawy z młynkiem.", Price = 2299.00m },
                    new Product { Name = "Zmywarka AquaClean", Description = "Nowoczesna zmywarka z systemem suszenia.", Price = 1899.00m },
                    new Product { Name = "Pralka EcoWash 9kg", Description = "Ekonomiczna pralka o dużej pojemności.", Price = 2199.00m },
                    new Product { Name = "Klimatyzator CoolAir", Description = "Mobilny klimatyzator z funkcją chłodzenia i grzania.", Price = 1599.00m },
                    new Product { Name = "Monitor UltraWide 34\"", Description = "Szeroki monitor z rozdzielczością 1440p.", Price = 1999.00m },
                    new Product { Name = "Mysz gamingowa SpeedMouse", Description = "Precyzyjna mysz do gier z podświetleniem LED.", Price = 149.00m },
                    new Product { Name = "Klawiatura mechaniczna ProKey", Description = "Klawiatura mechaniczna dla graczy z RGB.", Price = 249.00m },
                    new Product { Name = "Wiertarka udarowa PowerDrill", Description = "Mocna wiertarka udarowa do prac budowlanych.", Price = 399.00m },
                    new Product { Name = "Zestaw garnków KitchenPro", Description = "Komplet garnków ze stali nierdzewnej.", Price = 299.00m },
                    new Product { Name = "Blender ręczny BlendFast", Description = "Wydajny blender ręczny do koktajli i zup.", Price = 99.00m },
                    new Product { Name = "Odkurzacz bezprzewodowy CleanFlex", Description = "Lekki i wydajny odkurzacz bezprzewodowy.", Price = 899.00m },
                    new Product { Name = "Waga kuchenna SmartScale", Description = "Precyzyjna waga kuchenna z funkcją tarowania.", Price = 49.00m },
                    new Product { Name = "Suszarka do włosów HairPro", Description = "Profesjonalna suszarka do włosów z jonizacją.", Price = 199.00m },
                    new Product { Name = "Prostownica do włosów SilkStyle", Description = "Prostownica do włosów z ceramiczną powłoką.", Price = 149.00m },
                    new Product { Name = "Czajnik elektryczny FastBoil", Description = "Szybko gotujący czajnik z podświetleniem LED.", Price = 99.00m },
                    new Product { Name = "Zestaw noży kuchennych SharpSet", Description = "Komplet noży kuchennych z ostrzarką.", Price = 249.00m },
                    new Product { Name = "Zegar ścienny ModernTime", Description = "Nowoczesny zegar ścienny z dużymi cyframi.", Price = 79.00m },
                    new Product { Name = "Lampka biurkowa LED Flex", Description = "Lampka biurkowa z regulowanym ramieniem.", Price = 129.00m },
                    new Product { Name = "Ładowarka indukcyjna FastCharge", Description = "Szybka ładowarka bezprzewodowa.", Price = 59.00m },
                    new Product { Name = "Gimbal do smartfona ProStabilizer", Description = "Stabilizator do smartfona z Bluetooth.", Price = 349.00m },
                    new Product { Name = "Sokowirówka FreshJuice", Description = "Sokowirówka do owoców i warzyw.", Price = 249.00m },
                    new Product { Name = "Fotel gamingowy GamerChair", Description = "Ergonomiczny fotel dla graczy.", Price = 599.00m },
                    new Product { Name = "Hulajnoga elektryczna eScoot 500", Description = "Lekka hulajnoga elektryczna do jazdy miejskiej.", Price = 1999.00m },
                    new Product { Name = "Lodówka turystyczna CoolBox", Description = "Przenośna lodówka na wakacyjne wyjazdy.", Price = 399.00m },
                    new Product { Name = "Drukarka 3D PrintMaster", Description = "Drukarka 3D do domowego użytku.", Price = 1499.00m },
                    new Product { Name = "Powerbank 20000mAh", Description = "Duża pojemność dla długotrwałego zasilania.", Price = 99.00m },
                    new Product { Name = "Zestaw do fondue", Description = "Komplet do przygotowania fondue dla całej rodziny.", Price = 129.00m },
                    new Product { Name = "Kamera IP SecurityCam", Description = "Kamera IP do monitoringu domowego.", Price = 349.00m },
                    new Product { Name = "Radio cyfrowe DAB+ TuneIn", Description = "Przenośne radio z technologią DAB+.", Price = 199.00m },
                    new Product { Name = "Grill elektryczny GrillMaster", Description = "Grill elektryczny do użytku domowego.", Price = 249.00m },
                    new Product { Name = "Kosa spalinowa GardenPro", Description = "Mocna kosa do koszenia trawy i chwastów.", Price = 799.00m },
                    new Product { Name = "Zegarek GPS AdventureWatch", Description = "Zegarek z GPS do turystyki i sportu.", Price = 899.00m },
                    new Product { Name = "Basen ogrodowy PoolFun", Description = "Duży basen ogrodowy na lato.", Price = 799.00m },
                    new Product { Name = "Kociołek żeliwny CampingCook", Description = "Kociołek do gotowania w plenerze.", Price = 159.00m },
                    new Product { Name = "Zestaw narzędzi ToolKit 120 elementów", Description = "Kompletny zestaw narzędzi do domu.", Price = 299.00m },
                    new Product { Name = "Mata do jogi YogaMat", Description = "Antypoślizgowa mata do jogi i ćwiczeń.", Price = 49.00m }
                );
                context.SaveChanges();
            */
        }
    }
}
