using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterProvider.Entities
{
    class Product
    {
        public string Name;
        public string Code;

        public Product(string Name, string Code)
        {
            this.Name = Name;
            this.Code = Code;
        }
    }
}
