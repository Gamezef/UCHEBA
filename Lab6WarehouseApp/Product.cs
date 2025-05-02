using System;

namespace Lab6WarehouseApp // <--- Убедитесь, что это имя вашего проекта
{
    // Базовый класс для всех товаров
    public abstract class Product
    {
        public string Name { get; set; }
        private decimal _price;

        public decimal Price
        {
            get => _price;
            set
            {
                // Проверка на отрицательную цену, выбрасываем пользовательское исключение
                if (value < 0)
                {
                    throw new InvalidProductDataException("Цена товара не может быть отрицательной!");
                }
                _price = value;
            }
        }

        // Абстрактный метод для отображения информации, должен быть реализован в производных классах
        public abstract void DisplayInfo();

        // Виртуальный метод, может быть переопределен, но не обязан
        public virtual string GetProductType()
        {
            // Возвращает имя типа класса
            return GetType().Name;
        }
    }
}