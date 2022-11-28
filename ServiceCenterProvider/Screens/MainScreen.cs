using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace ServiceCenterProvider.Screens
{
    class MainScreen : IScreen
    {
        private const string SERIALIZATION_TEMP = "serialize.dat.tmp";

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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Выберите действие");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("1. Создание / Редактирование заявки");
                Console.WriteLine("2. Оформление / Просмотр накладной");
                Console.WriteLine("3. Просмотр результата");
                Console.WriteLine("4. Очистить заявки");
                Console.WriteLine("5. Очистить накладные");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Пустая строка, чтобы завершить");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine();

                Console.Write(">>> ");
                string choose = Console.ReadLine();
                Console.Clear();

                switch (choose)
                {
                    case "1": this.OpenNewRequestScreen(); break;
                    case "2": this.OpenNewConsignmentScreen(); break;
                    case "3": this.Calculate(); break;
                    case "4": this.Container.RequestRepository.Items.Clear(); break;
                    case "5": this.Container.ConsignmentRepository.Items.Clear(); break;
                    default: this.IsClose = true; break;
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
            Dictionary<int, Dictionary<string, int>> Initials;
            this.SaveConsignmentsInitials(out Initials);

            foreach (Entities.Request Request in this.Container.RequestRepository.Items)
            {
                foreach (KeyValuePair<Entities.Product, int> _Products in Request.Products)
                {
                    bool IsUnloaded = false;
                    Console.Write($"Заявка №{Request.Id}. ");

                    Console.Write($"{_Products.Key.Name}: заказано - {_Products.Value}");
                    int Remain = _Products.Value;
                    IEnumerable<Entities.Consignment> Consignemnts = from c in this.Container.ConsignmentRepository.Items
                                                                     where (from p in c.Products where p.Key.Code == _Products.Key.Code select p).Count() >= 1
                                                                     select c;

                    List<Entities.Consignment> ConsignmentsList = Consignemnts.ToList();

                    foreach (Entities.Consignment Consignemnt in ConsignmentsList)
                    {
                        int Amount;
                        if (Consignemnt.Products.TryGetValue(_Products.Key, out Amount))
                        {
                            if (Amount == 0)
                            {
                                continue;
                            }

                            IsUnloaded = true;

                            int Diff, OldRemain = Remain;

                            if (Amount > Remain)
                            {
                                Consignemnt.Products[_Products.Key] = Amount - Remain;
                                Remain = 0;
                            }
                            else
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

                    if (!IsUnloaded)
                    {
                        Console.Write(", отгружено - нет");
                    }

                    Console.WriteLine();
                }
            }
            Console.ReadKey();
            Console.Clear();

            this.RestoreConsignmentsInitials(Initials);

            this.IsClose = false;
        }

        private void SaveConsignmentsInitials(out Dictionary<int, Dictionary<string, int>> Initials)
        {
            Initials = new Dictionary<int, Dictionary<string, int>>();
            foreach (Entities.Consignment _Consignment in this.Container.ConsignmentRepository.Items)
            {
                Dictionary<string, int> _ConsignmentInitials;
                if (!Initials.TryGetValue(_Consignment.Id, out _ConsignmentInitials))
                {
                    _ConsignmentInitials = new Dictionary<string, int>() { };
                    Initials.Add(_Consignment.Id, _ConsignmentInitials);
                }

                foreach (KeyValuePair<Entities.Product, int> _ConsignmentProduct in _Consignment.Products)
                {
                    _ConsignmentInitials.Add(_ConsignmentProduct.Key.Code, _ConsignmentProduct.Value);
                }
            }
        }

        private void RestoreConsignmentsInitials(in Dictionary<int, Dictionary<string, int>> Initials)
        {
            Dictionary<Entities.Product, int> NewConsignmentProducts;
            foreach (Entities.Consignment _Consignment in this.Container.ConsignmentRepository.Items)
            {
                Dictionary<string, int> _ConsignmentInitials;
                if (Initials.TryGetValue(_Consignment.Id, out _ConsignmentInitials))
                {
                    NewConsignmentProducts = new Dictionary<Entities.Product, int>();
                    foreach (KeyValuePair<Entities.Product, int> _Products in _Consignment.Products)
                    {
                        NewConsignmentProducts.Add(_Products.Key, Initials[_Consignment.Id][_Products.Key.Code]);
                    }
                    _Consignment.Products = NewConsignmentProducts;
                }
            }
        }
    }
}
