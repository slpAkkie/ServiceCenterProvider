using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterProvider.Screens
{
    class RequestsScreen : IScreen
    {
        private Container Container;

        private bool IsClose = false;
        public RequestsScreen(Container _Container)
        {
            this.Container = _Container;
        }

        public void Run()
        {
            while (!this.IsClose)
            {
                if (this.Container.RequestRepository.Items.Count > 0)
                {
                    Console.WriteLine("Заявки: ");
                    this.Container.RequestRepository.Print();
                    Console.WriteLine();
                }

                Console.WriteLine("Создание / Редактирование зявки");
                Console.Write("Номер заявки: №");
                int RequestNumber = Convert.ToInt32(Console.ReadLine()); // TODO: Ошибка преобразования / переполнения
                Console.Clear();

                Entities.Request Request = this.Container.RequestRepository.Find(RequestNumber);
                if (Request == null)
                {
                    this.CreateNew(RequestNumber);
                } else
                {
                    this.Update(Request);
                }

                this.IsClose = true;
            }

            Console.Clear();
        }

        private void CreateNew(int RequestNumber)
        {
            Entities.Request Request = this.Container.RequestRepository.New(RequestNumber);

            this.AskForProducts(Request);
        }

        public void AskForProducts(Entities.Request Request)
        {
            string ChosenProduct = "";

            do
            {
                if (Request.Products.Count > 0)
                {
                    Request.PrintProducts();
                    Console.WriteLine();
                }

                Console.WriteLine("Список доступных к заказу деталей");
                this.Container.ProductRepository.Print();
                Console.WriteLine();
                Console.Write("Наименование товара (Пустая строка, чтобы завершить): ");
                ChosenProduct = Console.ReadLine();
                if (!String.IsNullOrEmpty(ChosenProduct))
                {
                    Entities.Product FoundProduct = this.Container.ProductRepository.Find(ChosenProduct);
                    if (FoundProduct == null)
                    {
                        Console.Clear();
                        Console.WriteLine("Такого товара в списке нет пожалуйста, посмотрите еще раз");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.Write("Количество: ");
                        int Amount = Convert.ToInt32(Console.ReadLine()); // TODO: Ошибка преобразования / переполнения
                        Request.AddProduct(FoundProduct, Amount);
                        Console.Clear();
                    }
                }
            } while (!String.IsNullOrEmpty(ChosenProduct));

            Console.Clear();
        }

        private void Update(Entities.Request Request)
        {
            Request.Print();
            Request.PrintProducts();

            Console.WriteLine();

            this.AskForProducts(Request);
        }
    }
}
