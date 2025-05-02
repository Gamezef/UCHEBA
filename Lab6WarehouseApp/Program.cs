using System;
using System.Collections.Generic; // Для List и Dictionary (используются в Warehouse)
using System.Linq; // Для LINQ (используется в Warehouse)
using System.Globalization; // Для TryParse (используется в методах ввода)
using System.Threading; // Для Thread.Sleep (для паузы в меню)

namespace Lab6WarehouseApp // <--- Убедитесь, что это имя вашего проекта
{
    // Основной исполняемый класс
    class Program
    {
        // Создаем статический экземпляр класса Warehouse (учет склада)
        static Warehouse warehouse = new Warehouse(); // <--- Используем класс Warehouse

        static void Main(string[] args)
        {
            Console.WriteLine("=== Система учета товаров на складе ===\n");

            // Добавим несколько товаров для примера при старте программы
            AddSampleProducts();

            // Основной цикл меню программы
            while (true)
            {
                PrintMenu(); // Показать опции меню
                string choice = Console.ReadLine(); // Считать выбор пользователя

                // Обработка выбора пользователя
                switch (choice)
                {
                    case "1":
                        AddNewProductConsole(); // Добавить товар через консоль
                        break;
                    case "2":
                        warehouse.DisplayAllProducts(); // Показать все товары со склада (через класс Warehouse)
                        break;
                    case "3":
                        FindProductByNameConsole(); // Найти товар по имени через консоль
                        break;
                    case "4":
                        FindExpensiveProductsConsole(); // Найти дорогие товары через консоль (использует Warehouse)
                        break;
                    case "5":
                         FindExpiredFoodItemsConsole(); // Найти просроченные продукты через консоль (использует Warehouse)
                         break;
                    case "6":
                         warehouse.GroupProductsByType(); // Группировать товары по типу (через Warehouse)
                         break;
                    case "7":
                        Console.WriteLine("Выход из программы...");
                        Thread.Sleep(500); // Небольшая пауза
                        return; // Выход из метода Main и завершение программы
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Неизвестная команда. Попробуйте снова.");
                        Console.ResetColor();
                        break;
                }
                 Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                 Console.ReadKey(); // Ждем нажатия перед следующим показом меню
                 Console.Clear(); // Очищаем консоль для чистого отображения меню
            }
        }

        // Вспомогательный метод для отображения текста меню в консоли
        static void PrintMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Добавить новый товар");
            Console.WriteLine("2. Показать все товары");
            Console.WriteLine("3. Найти товар по имени");
            Console.WriteLine("4. Найти товары дороже указанной цены");
            Console.WriteLine("5. Показать просроченные продукты питания");
            Console.WriteLine("6. Группировать товары по типу");
            Console.WriteLine("7. Выход");
            Console.Write("Ваш выбор: ");
            Console.ResetColor();
        }

        // Вспомогательный метод для добавления нового товара через ввод в консоли
        static void AddNewProductConsole()
        {
            Console.WriteLine("\n--- Добавление нового товара ---");
            Console.Write("Введите тип товара (Electronics/FoodItem): ");
            string type = Console.ReadLine(); // Считываем тип

            Console.Write("Введите название товара: ");
            string name = Console.ReadLine(); // Считываем название

            Console.Write("Введите цену товара: ");
            decimal price;
            // Цикл для проверки корректности ввода цены (должно быть число)
             while (!decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.CurrentCulture, out price))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Некорректный ввод цены. Введите число: ");
                Console.ResetColor();
            }

            // Переменная для создаваемого товара
            Product newProduct = null; // <--- Используем базовый класс Product

            try // Блок для обработки исключений при создании и добавлении товара
            {
                // В зависимости от введенного типа, создаем экземпляр соответствующего класса
                if (type.Equals("Electronics", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Введите срок гарантии в месяцах: ");
                    int warranty;
                     while (!int.TryParse(Console.ReadLine(), out warranty))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Некорректный ввод гарантии. Введите целое число: ");
                         Console.ResetColor();
                    }
                    // Создаем экземпляр Electronics
                    newProduct = new Electronics { Name = name, Price = price, WarrantyPeriod = warranty }; // <--- Используем класс Electronics
                }
                else if (type.Equals("FoodItem", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Введите срок годности (ГГГГ-ММ-ДД): ");
                    DateTime expiryDate;
                     while (!DateTime.TryParse(Console.ReadLine(), out expiryDate))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Некорректный ввод срока годности. Введите дату (ГГГГ-ММ-ДД): ");
                         Console.ResetColor();
                    }
                     // Создаем экземпляр FoodItem
                    newProduct = new FoodItem { Name = name, Price = price, ExpiryDate = expiryDate }; // <--- Используем класс FoodItem
                }
                else // Если введен некорректный тип
                {
                     Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неизвестный тип товара.");
                     Console.ResetColor();
                    return; // Выходим из метода, не добавляя товар
                }

                // Если товар успешно создан, пытаемся добавить его на склад
                if(newProduct != null)
                {
                     warehouse.AddProduct(newProduct); // <--- Вызываем метод из класса Warehouse
                }
            }
            // Перехват конкретного пользовательского исключения
            catch (InvalidProductDataException ex) // <--- Перехватываем наше пользовательское исключение
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка данных товара: {ex.Message}");
                Console.ResetColor();
            }
             // Перехват стандартного исключения .NET
             catch (InvalidOperationException ex)
            {
                 Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка операции: {ex.Message}");
                Console.ResetColor();
            }
            // Перехват любых других исключений
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
                Console.ResetColor();
            }
        }

        // Вспомогательный метод для поиска товара по имени через ввод в консоли
        static void FindProductByNameConsole()
        {
            Console.WriteLine("\n--- Поиск товара по имени ---");
            Console.Write("Введите имя товара для поиска: ");
            string searchName = Console.ReadLine(); // Считываем имя для поиска

            // Ищем товар с помощью метода из класса Warehouse
            Product found = warehouse.FindProductByName(searchName); // <--- Используем метод из Warehouse

            // Выводим результат поиска
            if (found != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Товар найден:");
                 Console.ResetColor();
                found.DisplayInfo(); // Вызываем DisplayInfo найденного товара (полиморфизм)
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Товар с именем '{searchName}' не найден.");
                Console.ResetColor();
            }
        }

        // Вспомогательный метод для поиска дорогих товаров через ввод в консоли
        static void FindExpensiveProductsConsole()
        {
            Console.WriteLine("\n--- Поиск товаров дороже указанной цены ---");
            Console.Write("Введите минимальную цену: ");
             decimal minPrice;
             // Цикл для проверки корректности ввода цены
             while (!decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.CurrentCulture, out minPrice))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Некорректный ввод цены. Введите число: ");
                Console.ResetColor();
            }

            // Ищем дорогие товары с помощью метода из класса Warehouse
            List<Product> expensive = warehouse.FindProductsMoreExpensiveThan(minPrice); // <--- Используем метод из Warehouse

            // Выводим найденные товары
            if (expensive.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Товары дороже {minPrice:C}:");
                Console.ResetColor();
                foreach (var p in expensive) // <--- Перебираем коллекцию найденных товаров
                {
                    Console.WriteLine($"- {p.Name} ({p.Price:C})"); // Выводим базовую информацию
                }
            }
            else
            {
                Console.WriteLine($"Товаров дороже {minPrice:C} не найдено.");
            }
        }

         // Вспомогательный метод для поиска просроченных продуктов через консоль
        static void FindExpiredFoodItemsConsole()
        {
             Console.WriteLine("\n--- Поиск просроченных продуктов питания ---");
             // Ищем просроченные продукты с помощью метода из класса Warehouse
             List<FoodItem> expired = warehouse.FindExpiredFoodItems(); // <--- Используем метод из Warehouse

             // Выводим найденные просроченные продукты
             if (expired.Count > 0)
             {
                 Console.ForegroundColor = ConsoleColor.Red;
                 Console.WriteLine("Найдены просроченные продукты:");
                 Console.ResetColor();
                 foreach(var item in expired) // <--- Перебираем коллекцию найденных FoodItem
                 {
                     item.DisplayInfo(); // Вызываем DisplayInfo для просроченного продукта
                     Console.WriteLine("---");
                 }
             }
             else
             {
                 Console.WriteLine("Просроченных продуктов питания не найдено.");
             }
        }


        // Вспомогательный метод для добавления нескольких тестовых товаров при запуске
        static void AddSampleProducts()
        {
            try // Блок для обработки исключений при добавлении тестовых данных
            {
                // Добавляем тестовые товары, используя метод AddProduct класса Warehouse
                warehouse.AddProduct(new Electronics { Name = "Ноутбук", Price = 1200.50M, WarrantyPeriod = 24 }); // <--- Создаем Electronics
                warehouse.AddProduct(new FoodItem { Name = "Хлеб", Price = 1.50M, ExpiryDate = DateTime.Now.AddDays(3) }); // <--- Создаем FoodItem
                warehouse.AddProduct(new Electronics { Name = "Мышь", Price = 25.99M, WarrantyPeriod = 12 }); // <--- Создаем Electronics
                 warehouse.AddProduct(new FoodItem { Name = "Молоко", Price = 2.20M, ExpiryDate = DateTime.Now.AddDays(-2) }); // Просроченное для теста <--- Создаем FoodItem
                 warehouse.AddProduct(new FoodItem { Name = "Сыр", Price = 8.75M, ExpiryDate = DateTime.Now.AddDays(30) }); // <--- Создаем FoodItem
                 warehouse.AddProduct(new Electronics { Name = "Клавиатура", Price = 75.00M, WarrantyPeriod = 36 }); // <--- Создаем Electronics

                // Примеры, которые ВЫБРОСЯТ исключение при добавлении (если раскомментировать):
                // warehouse.AddProduct(new Electronics { Name = "Телефон", Price = -100M, WarrantyPeriod = 12 }); // Неверная цена
                 // warehouse.AddProduct(new Electronics { Name = "Ноутбук", Price = 1500M, WarrantyPeriod = 12 }); // Дубликат имени
            }
            catch (Exception ex) // Перехват любых ошибок при добавлении тестовых данных
            {
                // Просто выводим ошибку добавления тестовых данных
                Console.ForegroundColor = ConsoleColor.Red;
                 Console.WriteLine($"Ошибка при добавлении тестовых данных: {ex.Message}");
                Console.ResetColor();
            }
             Console.WriteLine("\nТестовые товары добавлены.");
        }
    }
}