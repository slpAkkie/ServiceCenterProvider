using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterProvider.Entities
{
    class Request
    {
        public int Number;
        public Dictionary<Product, int> Products;

        public Request(int Number)
        {
            this.Number = Number;
            this.Products = new Dictionary<Product, int>();
        }

        public void AddProduct(Product _Product, int Amount)
        {
            int _ProductAmount;
            if (this.Products.TryGetValue(_Product, out _ProductAmount))
            {
                this.Products[_Product] = _ProductAmount + Amount;
            } else
            {
                this.Products.Add(_Product, Amount);
            }
        }

        public void Print()
        {
            Console.WriteLine($"Заявка №{this.Number}");
        }

        public void PrintProducts()
        {
            foreach (KeyValuePair<Product, int> _Product in this.Products)
            {
                Console.WriteLine($"Заявка №{this.Number}, {_Product.Key.Name} - {_Product.Value} шт");
            }
        }
    }
}
