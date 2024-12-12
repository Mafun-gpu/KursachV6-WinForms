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


        private List<Order> LoadOrders()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orders.txt");
            List<Order> loadedOrders = new List<Order>();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split(';');
                    if (parts.Length < 6)
                    {
                        Console.WriteLine($"Некорректная строка: {line}");
                        continue;
                    }

                    try
                    {
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
                                if (productParts.Length == 2)
                                {
                                    order.Products.Add(int.Parse(productParts[0]), int.Parse(productParts[1]));
                                }
                            }
                        }

                        order.TotalPrice = decimal.Parse(parts[4]);
                        order.Status = parts[5];
                        loadedOrders.Add(order);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при разборе строки: {line}. Ошибка: {ex.Message}");
                    }
                }

               
                loadedOrders = loadedOrders.Where(o => o != null).ToList();
            }

            return loadedOrders;
        }




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


        public void DeleteOrder(int orderId)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                orders.Remove(order);
                SaveOrdersToFile(); 
            }
            else
            {
                throw new Exception("Заказ не найден.");
            }
        }

        
        public Order GetOrderById(int orderId)
        {
            return orders.FirstOrDefault(o => o.OrderId == orderId);
        }

        
        public List<Order> GetAllOrders()
        {
            return orders;
        }

       
        public List<Order> GetOrdersByPeriod(DateTime startDate, DateTime endDate)
        {
           
            var startOfDay = startDate.Date; 
            var endOfDay = endDate.Date.AddDays(1).AddTicks(-1);

            
            return orders.Where(o => o.OrderDate >= startOfDay && o.OrderDate <= endOfDay).ToList();
        }

       
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

        
        public List<ProductReport> GetPopularProducts()
        {
            return orders
                .Where(o => o.Status == "Выдан") 
                .SelectMany(o => o.Products)
                .GroupBy(p => p.Key)
                .Select(g => new ProductReport
                {
                    ProductId = g.Key,
                    ProductName = $"Товар {g.Key}", 
                    QuantitySold = g.Sum(p => p.Value),
                    TotalRevenue = g.Sum(p => p.Value * GetProductPrice(g.Key)) 
                })
                .OrderByDescending(r => r.QuantitySold)
                .ToList();
        }

      
        private decimal GetProductPrice(int productId)
        {
           
            return 100; 
        }

      
        public class ClientReport
        {
            public int ClientId { get; set; }
            public string ClientName { get; set; }
            public int OrdersCount { get; set; }
            public decimal TotalSpent { get; set; }
        }

       
        public class ProductReport
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int QuantitySold { get; set; }
            public decimal TotalRevenue { get; set; }
        }
    }
}
