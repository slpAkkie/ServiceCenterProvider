using System.Collections.Generic;
using System.Linq;

namespace ServiceCenterProvider.Repositories
{
    class RequestRepository
    {
        public List<Entities.Request> Items;

        public RequestRepository()
        {
            this.Items = new List<Entities.Request>();
        }

        public Entities.Request New(int Number)
        {
            Entities.Request Request = new Entities.Request(Number);
            this.Items.Add(Request);

            return Request;
        }

        public void Print()
        {
            foreach (Entities.Request _Request in this.Items)
            {
                _Request.Print();
            }
        }

        public Entities.Request Find(int Number)
        {
            IEnumerable<Entities.Request> result = from r in this.Items
                   where r.Id == Number
                   select r;

            if (result.Count() == 0)
            {
                return null;
            }

            return result.First();
        }
    }
}
