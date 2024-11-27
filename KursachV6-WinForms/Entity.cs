namespace SalesManagerApp
{
    public abstract class Entity
    {
        public int Id { get; set; } // Идентификатор сущности

        public Entity(int id)
        {
            Id = id;
        }

        // Абстрактный метод для получения информации о сущности
        public abstract string GetInfo();
    }
}
