using System;
using System.Collections.Generic;

namespace ServiceCenterProvider.Entities
{
    class Request
    {
        public int Id;
        public Dictionary<Product, int> Products;

        public Request(int Number)
        {
            this.Id = Number;
            this.Products = new Dictionary<Product, int>();
        }

        public void AddProduct(Product _Product, int Amount)
        {
            int _ProductAmount;
            if (this.Products.TryGetValue(_Product, out _ProductAmount))
            {
                this.Products[_Product] = _ProductAmount + Amount;
            }
            else
            {
                this.Products.Add(_Product, Amount);
            }
        }

        public void Print()
        {
            Console.WriteLine($"Заявка №{this.Id}");
        }

        public void PrintProducts()
        {
            foreach (KeyValuePair<Product, int> _Product in this.Products)
            {
                Console.WriteLine($"{_Product.Key.Name} - {_Product.Value} шт");
            }
        }
    }
}
