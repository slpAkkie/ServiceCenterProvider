using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ServiceCenterProvider
{
    class Container
    {
        public Repositories.ProductRepository ProductRepository;
        public Repositories.RequestRepository RequestRepository;
        public Repositories.ConsignmentRepository ConsignmentRepository;

        public Container()
        {
            this.ProductRepository = new Repositories.ProductRepository();

            this.RequestRepository = new Repositories.RequestRepository();
            this.ConsignmentRepository = new Repositories.ConsignmentRepository();
            using (StreamReader _StreamReader = new StreamReader("Products.csv"))
            {
                string Line;
                while ((Line = _StreamReader.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(Line))
                    {
                        continue;
                    }

                    string[] ProductData = Line.Split(';');
                    this.ProductRepository.New(ProductData[0], ProductData[1]);
                }
            }
        }

        public void Start()
        {
            Screens.MainScreen Screen = new Screens.MainScreen(this);
            Screen.Run();
        }
    }
}
