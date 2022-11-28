using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterProvider.Repositories
{
    class ProductRepository
    {
        public List<Entities.Product> Items;

        public ProductRepository()
        {
            this.Items = new List<Entities.Product>();
        }

        public void New(string Name, string Code)
        {
            this.Items.Add(new Entities.Product(Name, Code));
        }

        public Entities.Product Find(string ProductNameOrCode)
        {
            IEnumerable<Entities.Product> result = from p in this.Items
                                                   where p.Name == ProductNameOrCode || p.Code == ProductNameOrCode
                                                   select p;

            if (result.Count() == 0)
            {
                return null;
            }

            return result.First();
        }

        public void Print()
        {
            foreach (Entities.Product _Product in this.Items)
            {
                Console.WriteLine($"{_Product.Code}. {_Product.Name} ");
            }
        }
    }
}
