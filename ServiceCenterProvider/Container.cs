using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            this.ProductRepository.New("Двигатель", "000001");
            this.ProductRepository.New("Мост", "000002");
            this.ProductRepository.New("Колеса", "000003");
            this.ProductRepository.New("Кабина", "000004");

            this.RequestRepository = new Repositories.RequestRepository();
            this.ConsignmentRepository = new Repositories.ConsignmentRepository();
        }

        public void Start()
        {
            Screens.MainScreen Screen = new Screens.MainScreen(this);
            Screen.Run();
        }
    }
}
