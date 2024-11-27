using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SalesManagerApp
{
    public class OrderManager
    {
        private List<Order> orders;
        private ProductManager productManager;

        public OrderManager(ProductManager productManager)
        {
            this.productManager = productManager;
            orders = LoadOrders();
        }

        // Загрузка заказов из файла
        private List<Order> LoadOrders()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orders.txt");
            List<Order> loadedOrders = new List<Order>();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    var order = new Order(
                        int.Parse(parts[0]),
                        int.Parse(parts[1]),
                        DateTime.Parse(parts[2])
                    );

                    if (!string.IsNullOrEmpty(parts[3]))
                    {
                        var productData = parts[3].Split(',');
                        foreach (var product in productData)
                        {
                            var productParts = product.Split(':');
                            order.Products.Add(int.Parse(productParts[0]), int.Parse(productParts[1]));
                        }
                    }

                    order.TotalPrice = decimal.Parse(parts[4]);
                    order.Status = parts[5];
                    loadedOrders.Add(order);
                }
            }

            return loadedOrders;
        }

        // Сохранение заказов в файл
        private void SaveOrdersToFile()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orders.txt");
            using (var writer = new StreamWriter(filePath, false))
            {
                foreach (var order in orders)
                {
                    writer.WriteLine(order.ToString());
                }
            }
        }

        // Обновление информации о заказе
        public void UpdateOrder(Order updatedOrder)
        {
            var existingOrder = orders.FirstOrDefault(o => o.OrderId == updatedOrder.OrderId);
            if (existingOrder != null)
            {
                orders.Remove(existingOrder);
            }
            orders.Add(updatedOrder);
            SaveOrdersToFile();
        }

        public void SaveOrders()
        {
            SaveOrdersToFile();
        }

        public void CreateOrder(int clientId, Dictionary<int, int> products)
        {
            int newOrderId = orders.Count > 0 ? orders.Max(o => o.OrderId) + 1 : 1;

            // Рассчитываем общую стоимость
            decimal totalPrice = 0;
            foreach (var product in products)
            {
                var productDetails = productManager.GetProductById(product.Key);
                if (productDetails == null || productDetails.Stock < product.Value)
                {
                    throw new Exception("Недостаточно товара на складе или товар не найден.");
                }

                totalPrice += productDetails.Price * product.Value;
                productManager.ReduceStock(product.Key, product.Value);
            }

            var newOrder = new Order(newOrderId, clientId, DateTime.Now)
            {
                Products = products,
                TotalPrice = totalPrice,
                Status = "Создан"
            };

            orders.Add(newOrder);
            SaveOrdersToFile();
        }

        // Удаление заказа
        public void DeleteOrder(int orderId)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                orders.Remove(order);
                SaveOrdersToFile(); // Сохраняем изменения в файл
            }
            else
            {
                throw new Exception("Заказ не найден.");
            }
        }

        // Получить заказ по ID
        public Order GetOrderById(int orderId)
        {
            return orders.FirstOrDefault(o => o.OrderId == orderId);
        }

        // Получить все заказы
        public List<Order> GetAllOrders()
        {
            return orders;
        }

        // Метод для получения заказов за указанный период
        public List<Order> GetOrdersByPeriod(DateTime startDate, DateTime endDate)
        {
            // Обнуление времени в датах начала и конца периода
            var startOfDay = startDate.Date; // Убираем время, оставляя только дату
            var endOfDay = endDate.Date.AddDays(1).AddTicks(-1); // Добавляем конец дня

            // Фильтруем заказы по указанному периоду
            return orders.Where(o => o.OrderDate >= startOfDay && o.OrderDate <= endOfDay).ToList();
        }

        // Метод для получения активности клиентов
        public List<ClientReport> GetTopClients()
        {
            return orders
                .GroupBy(o => o.ClientId)
                .Select(g => new ClientReport
                {
                    ClientId = g.Key,
                    OrdersCount = g.Count(),
                    TotalSpent = g.Sum(o => o.TotalPrice),
                    ClientName = $"Клиент {g.Key}"
                })
                .OrderByDescending(r => r.TotalSpent)
                .ToList();
        }

        // Метод для получения популярности товаров
        public List<ProductReport> GetPopularProducts()
        {
            return orders
                .Where(o => o.Status == "Выдан") // Учитываем только выполненные заказы
                .SelectMany(o => o.Products)
                .GroupBy(p => p.Key)
                .Select(g => new ProductReport
                {
                    ProductId = g.Key,
                    ProductName = $"Товар {g.Key}", // Это пример, в реальности может быть загрузка данных о товаре
                    QuantitySold = g.Sum(p => p.Value),
                    TotalRevenue = g.Sum(p => p.Value * GetProductPrice(g.Key)) // Получение цены товара
                })
                .OrderByDescending(r => r.QuantitySold)
                .ToList();
        }

        // Пример метода для получения цены товара
        private decimal GetProductPrice(int productId)
        {
            // Здесь должна быть логика получения цены из ProductManager или базы данных
            return 100; // Пример, замените на вашу реализацию
        }

        // Пример структуры для клиентских отчётов
        public class ClientReport
        {
            public int ClientId { get; set; }
            public string ClientName { get; set; }
            public int OrdersCount { get; set; }
            public decimal TotalSpent { get; set; }
        }

        // Пример структуры для отчётов по товарам
        public class ProductReport
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int QuantitySold { get; set; }
            public decimal TotalRevenue { get; set; }
        }
    }
}
