using System;
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
        }

        public void Start()
        {
            try
            {

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
            catch (Exception)
            {
                this.ProductRepository.New("Двигатель", "1");
                this.ProductRepository.New("Мост", "2");
                this.ProductRepository.New("Колеса", "3");
                this.ProductRepository.New("Кабина", "4");
            }

            Screens.MainScreen Screen = new Screens.MainScreen(this);
            Screen.Run();
        }
    }
}
