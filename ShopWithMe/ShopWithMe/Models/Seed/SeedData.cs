using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models.Products;
using ShopWithMe.Models.ProductCategories;
using ShopWithMe.Tools.Services;

namespace ShopWithMe.Models.Seed
{
    public static class SeedData
    {
        public static void EnsurePopulated(IServiceProvider serviceProvider)
        {
            using var context = new DefaultContext(
                serviceProvider.GetRequiredService<DbContextOptions<DefaultContext>>(), serviceProvider.GetRequiredService<UserService>());

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

            if (!context.Products.Any())
            {
                var categoryIds = context.ProductCategories.ToDictionary(c => c.Name, c => c.Id);

                var products = new[]
                {
                    new Product
                    {
                        Name = "Smartfon Galaxy Z",
                        Lead = "Nowoczesny smartfon z elastycznym ekranem.",
                        Description = "Smartfon Galaxy Z to rewolucyjny krok w technologii mobilnej, oferujący elastyczny ekran umożliwiający składanie urządzenia. Dzięki najnowszemu procesorowi, wytrzymałej baterii oraz niezwykle wydajnemu aparatowi fotograficznemu, jest to idealny wybór dla nowoczesnego użytkownika.",
                        Price = 3999.00m,
                        CategoryId = categoryIds["Elektronika"],
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
                    },
                                    new Product
                    {
                        Name = "Smartfon Galaxy S20",
                        Lead = "Smartfon z zaawansowaną kamerą.",
                        Description = "Galaxy S20 to flagowy model smartfona z zaawansowaną kamerą, 120Hz wyświetlaczem i wyjątkową wydajnością. Idealny wybór dla wymagających użytkowników.",
                        Price = 4599.00m,
                        CategoryId = categoryIds["Elektronika"]
                    },
                    new Product
                    {
                        Name = "Telewizor LED 55\"",
                        Lead = "Telewizor LED 4K.",
                        Description = "Nowoczesny telewizor LED z rozdzielczością 4K i wbudowaną funkcją Smart TV.",
                        Price = 2499.00m,
                        CategoryId = categoryIds["Elektronika"]
                    },
                    new Product
                    {
                        Name = "Laptop Gamingowy GamerPro",
                        Lead = "Laptop do gier z mocnym procesorem.",
                        Description = "Laptop GamerPro wyposażony w procesor Intel i7 i kartę graficzną RTX 3080. Zapewnia płynność gier na najwyższych ustawieniach.",
                        Price = 7499.00m,
                        CategoryId = categoryIds["Gaming"]
                    },
                    new Product
                    {
                        Name = "Konsola PlayStation 5",
                        Lead = "Konsola nowej generacji.",
                        Description = "PS5 oferuje niesamowitą grafikę i szybkość ładowania dzięki najnowszym technologiom.",
                        Price = 2499.00m,
                        CategoryId = categoryIds["Gaming"]
                    },
                    new Product
                    {
                        Name = "Robot sprzątający RoboClean 5000",
                        Lead = "Zaawansowany robot do sprzątania.",
                        Description = "RoboClean 5000 to robot sprzątający z funkcją mopowania i możliwością sterowania przez aplikację.",
                        Price = 2199.00m,
                        CategoryId = categoryIds["Dom i Ogród"]
                    },
                    new Product
                    {
                        Name = "Hulajnoga elektryczna FastScoot",
                        Lead = "Szybka hulajnoga elektryczna.",
                        Description = "FastScoot to hulajnoga elektryczna z dużymi kołami i szybkością do 30 km/h.",
                        Price = 1899.00m,
                        CategoryId = categoryIds["Sport i Rekreacja"]
                    },
                    new Product
                    {
                        Name = "Kamera sportowa ActionCam 4K",
                        Lead = "Kamera sportowa 4K.",
                        Description = "ActionCam 4K to kompaktowa kamera do rejestrowania wideo w wysokiej jakości, idealna dla sportowców.",
                        Price = 1299.00m,
                        CategoryId = categoryIds["Sport i Rekreacja"]
                    },
                    new Product
                    {
                        Name = "Drukarka 3D MiniPrint",
                        Lead = "Kompaktowa drukarka 3D.",
                        Description = "MiniPrint to mała i wydajna drukarka 3D, idealna do tworzenia modeli w domowych warunkach.",
                        Price = 999.00m,
                        CategoryId = categoryIds["Druk 3D i Akcesoria"]
                    },
                    new Product
                    {
                        Name = "Robot kuchenny KitchenMaster",
                        Lead = "Wszechstronny robot kuchenny.",
                        Description = "KitchenMaster to robot, który pomoże w kuchni: sieka, miesza, miksuje i gotuje.",
                        Price = 799.00m,
                        CategoryId = categoryIds["Kuchnia"]
                    },
                    new Product
                    {
                        Name = "Smartfon Galaxy S20",
                        Lead = "Smartfon z zaawansowaną kamerą.",
                        Description = "Galaxy S20 to flagowy model smartfona z zaawansowaną kamerą, 120Hz wyświetlaczem i wyjątkową wydajnością. Idealny wybór dla wymagających użytkowników.",
                        Price = 4599.00m,
                        CategoryId = categoryIds["Elektronika"]
                    },
                    new Product
                    {
                        Name = "Telewizor LED 55\"",
                        Lead = "Telewizor LED 4K.",
                        Description = "Nowoczesny telewizor LED z rozdzielczością 4K i wbudowaną funkcją Smart TV.",
                        Price = 2499.00m,
                        CategoryId = categoryIds["Elektronika"]
                    },
                    new Product
                    {
                        Name = "Laptop Gamingowy GamerPro",
                        Lead = "Laptop do gier z mocnym procesorem.",
                        Description = "Laptop GamerPro wyposażony w procesor Intel i7 i kartę graficzną RTX 3080. Zapewnia płynność gier na najwyższych ustawieniach.",
                        Price = 7499.00m,
                        CategoryId = categoryIds["Gaming"]
                    },
                    new Product
                    {
                        Name = "Tablet ProTab 10.5",
                        Lead = "Tablet z dużym ekranem i pojemną baterią.",
                        Description = "ProTab 10.5 to idealny tablet do pracy i rozrywki, wyposażony w 10.5-calowy wyświetlacz i baterię 10000 mAh.",
                        Price = 1999.00m,
                        CategoryId = categoryIds["Elektronika"]
                    },
                    new Product
                    {
                        Name = "Zestaw słuchawek Bluetooth AudioPro",
                        Lead = "Wysoka jakość dźwięku w kompaktowej formie.",
                        Description = "AudioPro to bezprzewodowe słuchawki Bluetooth oferujące krystaliczny dźwięk i do 30 godzin pracy na baterii.",
                        Price = 599.00m,
                        CategoryId = categoryIds["Elektronika"]
                    },
                    new Product
                    {
                        Name = "Pralka EcoWash 900",
                        Lead = "Energooszczędna pralka z dużą pojemnością.",
                        Description = "EcoWash 900 to pralka o pojemności 9 kg z programami oszczędzania wody i energii.",
                        Price = 2299.00m,
                        CategoryId = categoryIds["AGD"]
                    },
                    new Product
                    {
                        Name = "Odkurzacz Cyclone V10",
                        Lead = "Bezprzewodowy odkurzacz o dużej mocy.",
                        Description = "Cyclone V10 to lekki i wydajny odkurzacz bezprzewodowy z długim czasem pracy.",
                        Price = 1499.00m,
                        CategoryId = categoryIds["AGD"]
                    },
                    new Product
                    {
                        Name = "Mikser kuchenny EasyMix 300",
                        Lead = "Kompaktowy mikser do codziennych zastosowań.",
                        Description = "EasyMix 300 to lekki i łatwy w użyciu mikser z różnymi prędkościami pracy.",
                        Price = 199.00m,
                        CategoryId = categoryIds["Kuchnia"]
                    },
                    new Product
                    {
                        Name = "Grill elektryczny BBQMaster 2000",
                        Lead = "Grilluj w domu i na tarasie.",
                        Description = "BBQMaster 2000 to kompaktowy grill elektryczny z powłoką zapobiegającą przywieraniu.",
                        Price = 899.00m,
                        CategoryId = categoryIds["Dom i Ogród"]
                    },
                    new Product
                    {
                        Name = "Piła łańcuchowa PowerSaw 500",
                        Lead = "Lekka i wydajna piła łańcuchowa.",
                        Description = "PowerSaw 500 idealnie nadaje się do cięcia drewna w ogrodzie lub na działce.",
                        Price = 799.00m,
                        CategoryId = categoryIds["Narzędzia i Warsztat"]
                    },
                    new Product
                    {
                        Name = "Torba sportowa ActiveBag",
                        Lead = "Funkcjonalna torba sportowa o dużej pojemności.",
                        Description = "ActiveBag to torba wykonana z trwałych materiałów, idealna na siłownię lub podróż.",
                        Price = 199.00m,
                        CategoryId = categoryIds["Sport i Rekreacja"]
                    },
                    new Product
                    {
                        Name = "Lampa warsztatowa LED WorkLight",
                        Lead = "Przenośna lampa do pracy w trudnych warunkach.",
                        Description = "WorkLight LED to solidna lampa z regulowaną jasnością i odpornością na kurz i wodę.",
                        Price = 299.00m,
                        CategoryId = categoryIds["Narzędzia i Warsztat"]
                    },
                    new Product
                    {
                        Name = "Fotel biurowy Ergofit",
                        Lead = "Ergonomiczny fotel biurowy z regulacją wysokości.",
                        Description = "Ergofit zapewnia komfort pracy dzięki podparciu lędźwiowemu i wygodnemu siedzisku.",
                        Price = 599.00m,
                        CategoryId = categoryIds["Dom i Ogród"]
                    },
                    new Product
                    {
                        Name = "Zegarek sportowy SportWatch Lite",
                        Lead = "Minimalistyczny zegarek dla aktywnych osób.",
                        Description = "SportWatch Lite monitoruje puls, kroki i spalone kalorie, idealny na trening i codzienne użytkowanie.",
                        Price = 399.00m,
                        CategoryId = categoryIds["Sport i Rekreacja"]
                    },
                    new Product
                    {
                        Name = "Lodówka Side-by-Side CoolMaster",
                        Lead = "Duża lodówka do rodzinnego użytku.",
                        Description = "CoolMaster oferuje technologię No Frost, dużą pojemność i energooszczędność klasy A++.",
                        Price = 3999.00m,
                        CategoryId = categoryIds["AGD"]
                    },
                    new Product
                    {
                        Name = "Kamera domowa SmartCam 360",
                        Lead = "Obrotowa kamera do monitoringu.",
                        Description = "SmartCam 360 pozwala na monitorowanie domu w czasie rzeczywistym za pomocą aplikacji mobilnej.",
                        Price = 599.00m,
                        CategoryId = categoryIds["Dom i Ogród"]
                    },
                    new Product
                    {
                        Name = "Rower górski TrailMaster",
                        Lead = "Lekki i wytrzymały rower górski.",
                        Description = "TrailMaster to rower idealny na trudne szlaki i codzienne przejażdżki.",
                        Price = 2499.00m,
                        CategoryId = categoryIds["Sport i Rekreacja"]
                    },
                    new Product
                    {
                        Name = "Zestaw kluczy mechanicznych ToolSet Pro",
                        Lead = "Profesjonalny zestaw kluczy dla majsterkowiczów.",
                        Description = "ToolSet Pro zawiera 50 elementów, idealny do warsztatu i prac domowych.",
                        Price = 499.00m,
                        CategoryId = categoryIds["Narzędzia i Warsztat"]
                    },
                    new Product
                    {
                        Name = "Kamera samochodowa RoadSafe 300",
                        Lead = "Kamera rejestrująca trasę w wysokiej jakości.",
                        Description = "RoadSafe 300 nagrywa w rozdzielczości Full HD, zapewniając bezpieczeństwo na drodze.",
                        Price = 399.00m,
                        CategoryId = categoryIds["Motoryzacja"]
                    },
                    new Product
                    {
                        Name = "Smartfon Pixel 7",
                        Lead = "Smartfon z czystym Androidem i zaawansowaną kamerą.",
                        Description = "Pixel 7 to flagowy smartfon Google z najnowszym systemem Android, który zapewnia płynne działanie i wyjątkową jakość zdjęć dzięki technologii AI.",
                        Price = 3699.00m,
                        CategoryId = categoryIds["Elektronika"]
                    },
                    new Product
                    {
                        Name = "Głośnik Bluetooth SoundBox Pro",
                        Lead = "Kompaktowy głośnik o dużej mocy.",
                        Description = "SoundBox Pro to przenośny głośnik Bluetooth z wodoodporną konstrukcją i dźwiękiem 360 stopni.",
                        Price = 499.00m,
                        CategoryId = categoryIds["Elektronika"]
                    },
                    new Product
                    {
                        Name = "Piekarnik MasterBake 3000",
                        Lead = "Nowoczesny piekarnik z termoobiegiem.",
                        Description = "MasterBake 3000 oferuje precyzyjne pieczenie z termoobiegiem i intuicyjnym panelem dotykowym.",
                        Price = 1999.00m,
                        CategoryId = categoryIds["AGD"]
                    },
                    new Product
                    {
                        Name = "Zmywarka EcoWash 500",
                        Lead = "Oszczędna zmywarka z dużą pojemnością.",
                        Description = "EcoWash 500 to energooszczędna zmywarka klasy A++ z miejscem na 12 kompletów naczyń.",
                        Price = 2599.00m,
                        CategoryId = categoryIds["AGD"]
                    },
                    new Product
                    {
                        Name = "Gra RPG Fantasy Quest",
                        Lead = "Ekscytująca gra RPG dla fanów fantastyki.",
                        Description = "Fantasy Quest przenosi graczy do magicznego świata pełnego przygód i walk z potworami.",
                        Price = 249.00m,
                        CategoryId = categoryIds["Gaming"]
                    },
                    new Product
                    {
                        Name = "Konsola przenośna PlayGo",
                        Lead = "Przenośna konsola do gier.",
                        Description = "PlayGo to idealne rozwiązanie dla graczy w podróży, oferująca długą żywotność baterii i szeroką bibliotekę gier.",
                        Price = 1299.00m,
                        CategoryId = categoryIds["Gaming"]
                    },
                    new Product
                    {
                        Name = "Szafka na narzędzia ToolBox XL",
                        Lead = "Praktyczna szafka do warsztatu.",
                        Description = "ToolBox XL zapewnia miejsce na wszystkie narzędzia, z dodatkowymi szufladami i zamkiem na klucz.",
                        Price = 999.00m,
                        CategoryId = categoryIds["Narzędzia i Warsztat"]
                    },
                    new Product
                    {
                        Name = "Zestaw wkrętaków ProfiDriver",
                        Lead = "Profesjonalne wkrętaki do każdej pracy.",
                        Description = "ProfiDriver to zestaw 20 wkrętaków z ergonomicznie zaprojektowanymi uchwytami i końcówkami magnetycznymi.",
                        Price = 149.00m,
                        CategoryId = categoryIds["Narzędzia i Warsztat"]
                    },
                    new Product
                    {
                        Name = "Krzesło ogrodowe Relax",
                        Lead = "Wygodne krzesło do ogrodu lub na taras.",
                        Description = "Relax to krzesło wykonane z lekkiego aluminium i tkaniny odpornej na warunki atmosferyczne.",
                        Price = 399.00m,
                        CategoryId = categoryIds["Dom i Ogród"]
                    },
                    new Product
                    {
                        Name = "Basen nadmuchiwany FamilyPool",
                        Lead = "Duży basen dla całej rodziny.",
                        Description = "FamilyPool to idealny basen na lato, łatwy w montażu i wystarczająco duży, aby pomieścić całą rodzinę.",
                        Price = 999.00m,
                        CategoryId = categoryIds["Dom i Ogród"]
                    },
                    new Product
                    {
                        Name = "Rolki SpeedRoller 300",
                        Lead = "Szybkie rolki dla miłośników sportów.",
                        Description = "SpeedRoller 300 to lekkie i wytrzymałe rolki z łożyskami ABEC-7, idealne do szybkiej jazdy.",
                        Price = 349.00m,
                        CategoryId = categoryIds["Sport i Rekreacja"]
                    },
                    new Product
                    {
                        Name = "Namiot turystyczny CampSafe 4",
                        Lead = "Przestronny namiot dla 4 osób.",
                        Description = "CampSafe 4 oferuje wodoodporną konstrukcję i łatwy montaż, idealny na wyjazdy rodzinne.",
                        Price = 899.00m,
                        CategoryId = categoryIds["Sport i Rekreacja"]
                    },
                    new Product
                    {
                        Name = "Fotograf aparat Snapshot Pro",
                        Lead = "Aparat kompaktowy z funkcją HDR.",
                        Description = "Snapshot Pro to aparat z 20 Mpix matrycą, doskonały do codziennego użytku i podróży.",
                        Price = 1599.00m,
                        CategoryId = categoryIds["Elektronika"]
                    },
                    new Product
                    {
                        Name = "Monitor UltraWide 34\"",
                        Lead = "Ultraszeroki monitor do pracy i gier.",
                        Description = "Monitor UltraWide 34\" zapewnia wyjątkową przestrzeń roboczą i immersyjne wrażenia podczas grania.",
                        Price = 2999.00m,
                        CategoryId = categoryIds["Elektronika"]
                    },
                    new Product
                    {
                        Name = "Lodówka turystyczna CoolBox 24L",
                        Lead = "Przenośna lodówka na wyjazdy.",
                        Description = "CoolBox 24L utrzymuje żywność w niskiej temperaturze przez wiele godzin, idealna na pikniki i podróże.",
                        Price = 399.00m,
                        CategoryId = categoryIds["Dom i Ogród"]
                    },
                    new Product
                    {
                        Name = "Zestaw garnków PremiumCook",
                        Lead = "Garnki z powłoką nieprzywierającą.",
                        Description = "PremiumCook to zestaw 10 garnków wykonanych z najwyższej jakości materiałów, idealnych do gotowania i pieczenia.",
                        Price = 599.00m,
                        CategoryId = categoryIds["Kuchnia"]
                    },
                    new Product
                    {
                        Name = "Parowar SteamMaster",
                        Lead = "Parowar do zdrowego gotowania.",
                        Description = "SteamMaster pozwala na szybkie i zdrowe gotowanie potraw, zachowując ich wartości odżywcze.",
                        Price = 399.00m,
                        CategoryId = categoryIds["AGD"]
                    },
                    new Product
                    {
                        Name = "Kask rowerowy SafeRide",
                        Lead = "Bezpieczny kask rowerowy.",
                        Description = "SafeRide to lekki i wygodny kask rowerowy, zapewniający bezpieczeństwo podczas jazdy.",
                        Price = 199.00m,
                        CategoryId = categoryIds["Sport i Rekreacja"]
                    },
                    new Product
                    {
                        Name = "Ekspres do kawy AromaBrew",
                        Lead = "Nowoczesny ekspres do kawy.",
                        Description = "AromaBrew przygotowuje doskonałą kawę dzięki precyzyjnemu zaparzaniu i funkcji spieniania mleka.",
                        Price = 799.00m,
                        CategoryId = categoryIds["AGD"]
                    },
                    new Product
                    {
                        Name = "Wentylator biurkowy CoolAir",
                        Lead = "Kompaktowy wentylator do biura i domu.",
                        Description = "CoolAir to cichy i wydajny wentylator z możliwością regulacji kąta i prędkości.",
                        Price = 149.00m,
                        CategoryId = categoryIds["Dom i Ogród"]
                    }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
