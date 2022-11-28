using System;

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
