using System.Collections.Generic;
using System.Linq;
using System;

namespace ServiceCenterProvider.Repositories
{
    class ConsignmentRepository
    {
        public List<Entities.Consignment> Items;

        public ConsignmentRepository()
        {
            this.Items = new List<Entities.Consignment>();
        }

        public Entities.Consignment New(int Number)
        {
            Entities.Consignment Consignment = new Entities.Consignment(Number);
            this.Items.Add(Consignment);

            return Consignment;
        }

        public void Print()
        {
            foreach (Entities.Consignment _Consignment in this.Items)
            {
                _Consignment.Print();
            }
        }

        public Entities.Consignment Find(int Number)
        {
            IEnumerable<Entities.Consignment> result = from c in this.Items
                                                   where c.Id == Number
                                                   select c;

            if (result.Count() == 0)
            {
                return null;
            }

            return result.First();
        }
    }
}
