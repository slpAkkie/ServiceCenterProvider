using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterProvider.Screens
{
    class ConsignmentsScreen : IScreen
    {
        private Container Container;

        private bool IsClose = false;
        public ConsignmentsScreen(Container _Container)
        {
            this.Container = _Container;
        }
        
        public void Run()
        {
            while (!this.IsClose)
            {
                if (this.Container.ConsignmentRepository.Items.Count > 0)
                {
                    Console.WriteLine("Накладные: ");
                    this.Container.ConsignmentRepository.Print();
                    Console.WriteLine();
                }

                Console.WriteLine("Создание / Просмотр накладной");
                Console.Write("Номер накладной: №");
                int ConsignmentNumber = Convert.ToInt32(Console.ReadLine()); // TODO: Ошибка преобразования / переполнения
                Console.Clear();

                Entities.Consignment Consignment = this.Container.ConsignmentRepository.Find(ConsignmentNumber);
                if (Consignment == null)
                {
                    this.CreateNew(ConsignmentNumber);
                }
                else
                {
                    this.View(Consignment);
                }

                this.IsClose = true;
            }

            Console.Clear();
        }

        public void CreateNew(int ConsignmentNumber)
        {
            Entities.Consignment Consignment = this.Container.ConsignmentRepository.New(ConsignmentNumber);

            this.AskForProducts(Consignment);
        }

        public void AskForProducts(Entities.Consignment Consignment)
        {
            string ChosenProduct = "";

            do
            {
                if (Consignment.Products.Count > 0)
                {
                    Consignment.PrintProducts();
                    Console.WriteLine();
                }

                Console.WriteLine("Список товаров");
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
                        Consignment.AddProduct(FoundProduct, Amount);
                        Console.Clear();
                    }
                }
            } while (!String.IsNullOrEmpty(ChosenProduct));

            Console.Clear();
        }

        public void View(Entities.Consignment Consignment)
        {
            Consignment.Print();
            Consignment.PrintProducts();

            Console.ReadKey();
        }
    }
}
