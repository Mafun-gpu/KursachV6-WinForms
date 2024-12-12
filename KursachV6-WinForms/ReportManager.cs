using System;
using System.Collections.Generic;
using System.Linq;
using KursachV6_WinForms;

namespace SalesManagerApp
{
    public class ReportManager
    {
        private List<Order> orders;
        private ClientManager clientManager;
        private ProductManager productManager;

        public ReportManager(OrderManager orderManager, ClientManager clientMgr, ProductManager productMgr)
        {
            orders = orderManager.GetAllOrders();
            clientManager = clientMgr;
            productManager = productMgr;
        }

        public void SalesReportByPeriod(DateTime startDate, DateTime endDate)
        {
            var filteredOrders = orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.Status == "Выдан")
                .ToList();

            decimal totalSales = filteredOrders.Sum(o => o.TotalPrice);
            int totalItemsSold = filteredOrders.Sum(o => o.Products.Sum(p => p.Value));
            decimal averageOrderValue = filteredOrders.Any() ? filteredOrders.Average(o => o.TotalPrice) : 0;
            decimal averageItemPrice = totalItemsSold > 0 ? totalSales / totalItemsSold : 0;

            Console.WriteLine($"\nОтчет по продажам с {startDate:yyyy-MM-dd} по {endDate:yyyy-MM-dd}:");
            Console.WriteLine($"Общая сумма продаж: {totalSales:C}");
            Console.WriteLine($"Общее количество проданных товаров: {totalItemsSold}");
            Console.WriteLine($"Средний чек (средняя сумма одного заказа): {averageOrderValue:C}");
            Console.WriteLine($"Средняя цена проданного товара: {averageItemPrice:C}");
        }

        public void ClientActivityReport()
        {
            Console.WriteLine("\nОтчет по активности клиентов:");
            var clientOrders = orders
                .Where(o => o.Status == "Выдан")
                .GroupBy(o => o.ClientId)
                .Select(g => new
                {
                    ClientId = g.Key,
                    OrderCount = g.Count(),
                    TotalAmount = g.Sum(o => o.TotalPrice),
                    AverageOrderValue = g.Average(o => o.TotalPrice)
                })
                .OrderByDescending(c => c.TotalAmount)
                .ToList();

            foreach (var client in clientOrders)
            {
                var clientInfo = clientManager.GetClientById(client.ClientId);
                Console.WriteLine($"Клиент: {clientInfo.FullName}, Заказов: {client.OrderCount}, Общая сумма: {client.TotalAmount:C}, Средний чек: {client.AverageOrderValue:C}");
            }

            if (clientOrders.Any())
            {
                var topClientByOrders = clientOrders.OrderByDescending(c => c.OrderCount).First();
                var topClientByAmount = clientOrders.First();
                var topClientInfo = clientManager.GetClientById(topClientByAmount.ClientId);

                Console.WriteLine($"\nКлиент с наибольшим количеством заказов: {clientManager.GetClientById(topClientByOrders.ClientId).FullName}, Заказов: {topClientByOrders.OrderCount}");
                Console.WriteLine($"Клиент с наибольшей суммой покупок: {topClientInfo.FullName}, Сумма: {topClientByAmount.TotalAmount:C}");
            }
        }

        public void ProductSalesReport()
        {
            Console.WriteLine("\nОтчет по продажам товаров:");
            var productSales = orders
                .Where(o => o.Status == "Выдан")
                .SelectMany(o => o.Products)
                .GroupBy(p => p.Key)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalSold = g.Sum(p => p.Value),
                    TotalAmount = g.Sum(p => p.Value * productManager.GetProductPrice(p.Key))
                })
                .OrderByDescending(p => p.TotalSold)
                .ToList();

            foreach (var product in productSales)
            {
                var productInfo = productManager.GetProductById(product.ProductId);
                Console.WriteLine($"Товар: {productInfo.Name}, Продано: {product.TotalSold} шт., Общая сумма: {product.TotalAmount:C}");
            }

            if (productSales.Any())
            {
                var topProduct = productSales.First();
                var bottomProduct = productSales.Last();

                Console.WriteLine($"\nСамый продаваемый товар: {productManager.GetProductById(topProduct.ProductId).Name}, Продано: {topProduct.TotalSold} шт.");
                Console.WriteLine($"Наименее продаваемый товар: {productManager.GetProductById(bottomProduct.ProductId).Name}, Продано: {bottomProduct.TotalSold} шт.");

                int totalProducts = productManager.GetAllProducts().Count();
                int soldProducts = productSales.Count();

                Console.WriteLine($"Процент товаров, которые были проданы хотя бы раз: {((decimal)soldProducts / totalProducts) * 100:F2}%");
            }
        }
    }
}
