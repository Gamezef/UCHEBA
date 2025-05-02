using System;

namespace Lab6WarehouseApp // <--- Убедитесь, что это имя вашего проекта
{
    // Класс для продуктов питания, наследует от Product
    public class FoodItem : Product // <--- Наследуем от Product
    {
        public DateTime ExpiryDate { get; set; } // Срок годности

        // Переопределяем метод отображения информации
        public override void DisplayInfo()
        {
            Console.WriteLine($"Тип: Продукт питания");
            Console.WriteLine($"Название: {Name}"); // <-- Используем Name и Price из базового класса Product
            Console.WriteLine($"Цена: {Price:C}");
            Console.WriteLine($"Срок годности: {ExpiryDate.ToShortDateString()}");
            if (ExpiryDate < DateTime.Now)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("!!! Срок годности истек !!!");
                Console.ResetColor();
            }
        }

        // Метод для проверки срока годности (специфичен для продуктов питания)
        public bool IsExpired()
        {
            return ExpiryDate < DateTime.Now;
        }
    }
}