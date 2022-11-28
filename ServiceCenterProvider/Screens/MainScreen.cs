using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterProvider.Screens
{
    class MainScreen : IScreen
    {
        private Container Container;

        private bool IsClose = false;

        public MainScreen(Container _Container)
        {
            this.Container = _Container;
        }

        public void Run()
        {
            while (!this.IsClose)
            {
                Console.WriteLine("Выберите действие");
                Console.WriteLine("1. Создание / Редактирование заявки");
                Console.WriteLine("2. Оформление / Просмотр накладной");
                Console.WriteLine();
                Console.WriteLine("Пустая строка, чтобы завершить");
                Console.WriteLine();

                Console.Write(">>> ");
                string choose = Console.ReadLine();
                Console.Clear();

                switch (choose)
                {
                    case "1": this.OpenNewRequestScreen(); break;
                    case "2": this.OpenNewConsignmentScreen(); break;
                    case "": this.Calculate(); break;
                    default: this.NoSuchEntry(); break;
                }
            }

            Console.Clear();
        }

        private void OpenNewRequestScreen()
        {
            new RequestsScreen(this.Container).Run();
        }

        private void OpenNewConsignmentScreen()
        {
            new ConsignmentsScreen(this.Container).Run();
        }

        public void Calculate()
        {
            foreach (Entities.Request Request in this.Container.RequestRepository.Items)
            {
                foreach (KeyValuePair<Entities.Product, int> _Products in Request.Products)
                {
                    Console.Write($"Заявка №{Request.Id}. ");

                    Console.Write($"{_Products.Key.Name}: заказано - {_Products.Value}");
                    int Remain = _Products.Value;
                    IEnumerable<Entities.Consignment> Consignemnts = from c in this.Container.ConsignmentRepository.Items
                                                                     where (from p in c.Products where p.Key == _Products.Key select p).Count() >= 1
                                                                     select c;

                    List<Entities.Consignment> ConsignmentsList = Consignemnts.ToList();

                    if (ConsignmentsList.Count() == 0)
                    {
                        Console.Write(", отгружено - нет");
                    }

                    foreach (Entities.Consignment Consignemnt in ConsignmentsList)
                    {
                        int Amount;
                        if (Consignemnt.Products.TryGetValue(_Products.Key, out Amount)) {
                            int Diff, OldRemain = Remain;

                            if (Amount > Remain)
                            {
                                Consignemnt.Products[_Products.Key] = Amount - Remain;
                                Remain = 0;
                            } else
                            {
                                Remain -= Amount;
                                Consignemnt.Products[_Products.Key] = 0;
                            }

                            Diff = OldRemain - Remain;

                            Console.Write($", отгружено - {Diff} шт (накладная №{Consignemnt.Id})");
                        }

                        if (Remain <= 0)
                        {
                            break;
                        }
                    }

                    Console.WriteLine();
                }
            }
            Console.ReadKey();

            this.IsClose = true;
        }

        private void NoSuchEntry()
        {
            Console.WriteLine("Такого пункта в меню нет, пожалуйста посмотрите еще раз");
            Console.WriteLine();
        }
    }
}
