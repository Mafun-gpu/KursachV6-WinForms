using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalesManagerApp
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public DateTime OrderDate { get; set; }
        public Dictionary<int, int> Products { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }

        public Order(int orderId, int clientId, DateTime orderDate)
        {
            OrderId = orderId;
            ClientId = clientId;
            OrderDate = orderDate;
            Products = new Dictionary<int, int>();
            TotalPrice = 0;
            Status = "Создан";
        }
        public string GetProductDetails(ProductManager productManager)
        {
            var productDetails = new StringBuilder();
            foreach (var product in Products)
            {
                var productName = productManager.GetProductById(product.Key)?.Name ?? "Неизвестно";
                productDetails.AppendLine($"{productName} x {product.Value}");
            }
            return productDetails.ToString();
        }

        public override string ToString()
        {
         
            string productData = string.Join(",", Products.Select(p => $"{p.Key}:{p.Value}"));
            return $"{OrderId};{ClientId};{OrderDate:yyyy-MM-dd};{productData};{TotalPrice};{Status}";
        }
    }
}
