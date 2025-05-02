using System;
using System.Collections.Generic; // Для List и Dictionary
using System.Linq; // Для LINQ

namespace Lab6WarehouseApp // <--- Убедитесь, что это имя вашего проекта
{
    // Класс для управления складом, использует коллекции Product
    public class Warehouse
    {
        private List<Product> _allProducts = new List<Product>(); // <-- Используем Product
        private Dictionary<string, Product> _productLookup = new Dictionary<string, Product>(); // <-- Используем Product

        // Метод для добавления товара на склад
        public void AddProduct(Product product) // <-- Принимает Product
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Нельзя добавить на склад пустой товар.");
            }
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                 // Выбрасываем пользовательское исключение InvalidProductDataException
                 throw new InvalidProductDataException("Название товара не может быть пустым.");
            }
             if (_productLookup.ContainsKey(product.Name))
            {
                 // Выбрасываем стандартное исключение InvalidOperationException
                 throw new InvalidOperationException($"Товар с названием '{product.Name}' уже есть на складе.");
            }

            _allProducts.Add(product);
            _productLookup.Add(product.Name, product);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Товар '{product.Name}' добавлен на склад.");
            Console.ResetColor();
        }

        // Метод для отображения информации обо всех товарах
        public void DisplayAllProducts()
        {
            if (_allProducts.Count == 0)
            {
                Console.WriteLine("Склад пуст.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- Все товары на складе ---");
            Console.ResetColor();
            foreach (var product in _allProducts) // <-- Перебираем коллекцию Product
            {
                product.DisplayInfo(); // Демонстрация полиморфизма (вызывается метод DisplayInfo соответствующего типа: Electronics или FoodItem)
                Console.WriteLine("---");
            }
        }

        // Метод для поиска товара по имени с использованием Dictionary
        public Product FindProductByName(string name) // <-- Возвращает Product
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            _productLookup.TryGetValue(name, out Product foundProduct); // <-- Используем Product
            return foundProduct;
        }

        // Метод для поиска товаров дороже определенной цены с использованием LINQ
        public List<Product> FindProductsMoreExpensiveThan(decimal price) // <-- Работает с коллекцией Product
        {
            var expensiveProducts = _allProducts.Where(p => p.Price > price).ToList(); // <-- Используем LINQ на коллекции Product
            return expensiveProducts;
        }

         // Метод для поиска просроченных продуктов (для FoodItem) с использованием LINQ
        public List<FoodItem> FindExpiredFoodItems() // <-- Возвращает коллекцию FoodItem
        {
            // Используем OfType<FoodItem>() для фильтрации коллекции Product только до FoodItem
            var expiredItems = _allProducts.OfType<FoodItem>().Where(f => f.IsExpired()).ToList();
             return expiredItems;
        }

        // Метод для группировки товаров по типу с использованием LINQ
        public void GroupProductsByType()
        {
             if (_allProducts.Count == 0)
            {
                Console.WriteLine("Склад пуст.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- Группировка товаров по типу ---");
            Console.ResetColor();

            // Группируем коллекцию Product по результату виртуального метода GetProductType
            var grouped = _allProducts.GroupBy(p => p.GetProductType());

            foreach (var group in grouped)
            {
                Console.WriteLine($">>> Тип: {group.Key}");
                foreach (var product in group) // <-- Перебираем товары внутри группы (они тоже Product)
                {
                    Console.WriteLine($"- {product.Name} ({product.Price:C})");
                }
            }
        }
    }
}