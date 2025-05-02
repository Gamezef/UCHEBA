using System;

namespace Lab6WarehouseApp // <--- Убедитесь, что это имя вашего проекта
{
    // Класс для электроники, наследует от Product
    public class Electronics : Product // <--- Наследуем от Product
    {
        public int WarrantyPeriod { get; set; } // Срок гарантии в месяцах

        // Переопределяем метод отображения информации
        public override void DisplayInfo()
        {
            Console.WriteLine($"Тип: Электроника");
            Console.WriteLine($"Название: {Name}"); // <-- Используем Name и Price из базового класса Product
            Console.WriteLine($"Цена: {Price:C}");
            Console.WriteLine($"Гарантия: {WarrantyPeriod} мес.");
        }
    }
}