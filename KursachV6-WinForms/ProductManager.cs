using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SalesManagerApp
{
    public class ProductManager
    {
        private List<Product> products;

        public ProductManager()
        {
            products = LoadProducts();
        }

        private List<Product> LoadProducts()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.txt");
            List<Product> loadedProducts = new List<Product>();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    var parts = line.Split(';');
                    loadedProducts.Add(new Product(
                        int.Parse(parts[0]), parts[1], parts[2], decimal.Parse(parts[3]), int.Parse(parts[4])));
                }
            }

            return loadedProducts;
        }

        private void SaveProductsToFile()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.txt");
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                foreach (var product in products)
                {
                    writer.WriteLine(product.ToString());
                }
            }
        }

        public void AddProduct(string name, string description, decimal price, int stock)
        {
            if (price < 0 || stock < 0)
            {
                throw new ArgumentException("Цена и количество товара не могут быть меньше нуля.");
            }

            int newId = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
            products.Add(new Product(newId, name, description, price, stock));
            SaveProductsToFile();
        }

        public void DeleteProduct(int productId)
        {
            var product = products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                products.Remove(product);
                SaveProductsToFile(); 
            }
        }

        public void ListProducts()
        {
            Console.WriteLine("\nСписок товаров:");
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id}: {product.Name}, Цена: {product.Price}, Остаток: {product.Stock}");
            }
        }

        public void SearchProduct()
        {
            Console.Write("Введите название товара для поиска: ");
            string query = Console.ReadLine();

            var results = products
                .Where(p => p.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            if (results.Count > 0)
            {
                Console.WriteLine("\nНайденные товары:");
                foreach (var product in results)
                {
                    Console.WriteLine($"{product.Id}: {product.Name}, Цена: {product.Price}, Остаток: {product.Stock}");
                }
            }
            else
            {
                Console.WriteLine("Товары не найдены.");
            }
        }

        public bool ProductExists(int productId)
        {
            return products.Any(p => p.Id == productId);
        }

        public decimal GetProductPrice(int productId)
        {
            var product = products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                return product.Price;
            }
            else
            {
                throw new Exception("Товар с указанным ID не найден.");
            }
        }

        public bool ReduceStock(int productId, int quantity)
        {
            var product = products.FirstOrDefault(p => p.Id == productId);
            if (product == null || product.Stock < quantity)
            {
                return false; 
            }

            product.Stock -= quantity;
            SaveProductsToFile();
            return true;
        }

        public void IncreaseStock(int productId, int quantity)
        {
            var product = products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                product.Stock += quantity;
                SaveProductsToFile();
            }
        }

        public Product GetProductById(int productId)
        {
            return products.FirstOrDefault(p => p.Id == productId);
        }

        public List<Product> GetAllProducts()
        {
            return products;
        }

        public void EditProduct(int productId, string name, string description, decimal price, int stock)
        {
            if (price < 0 || stock < 0)
            {
                throw new ArgumentException("Цена и количество товара не могут быть меньше нуля.");
            }

            var product = products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                product.Name = name;
                product.Description = description;
                product.Price = price;
                product.Stock = stock;
                SaveProductsToFile();
            }
            else
            {
                throw new Exception("Товар с указанным ID не найден.");
            }
        }
    }
}
