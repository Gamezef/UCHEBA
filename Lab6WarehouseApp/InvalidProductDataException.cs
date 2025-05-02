using System;

namespace Lab6WarehouseApp // <--- Убедитесь, что это имя вашего проекта
{
    // Пользовательское исключение для некорректных данных о товаре
    public class InvalidProductDataException : Exception
    {
        public InvalidProductDataException()
        {
        }

        public InvalidProductDataException(string message)
            : base(message)
        {
        }

        public InvalidProductDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}