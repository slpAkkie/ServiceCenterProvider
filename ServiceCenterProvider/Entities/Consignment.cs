using System;
using System.Collections.Generic;

namespace ServiceCenterProvider.Entities
{
    class Consignment
    {
        public int Id;
        public Dictionary<Product, int> Products;

        public Consignment(int Number)
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
            Console.WriteLine($"Накладная №{this.Id}");
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
