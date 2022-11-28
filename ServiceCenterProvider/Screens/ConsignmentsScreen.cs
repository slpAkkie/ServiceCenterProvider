using System;
using System.Linq;

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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Создание / Просмотр накладной");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("P.S. Для выхода введите 0");
                Console.ForegroundColor = ConsoleColor.Gray;
                int ConsignmentNumber;
                while (true)
                {
                    Console.Write("Номер накладной: №");
                    try
                    {
                        ConsignmentNumber = Convert.ToInt32(Console.ReadLine());
                        if (ConsignmentNumber == 0)
                        {
                            Console.Clear();
                            return;
                        }

                        if (ConsignmentNumber <= 0)
                        {
                            throw new Exception();
                        }
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Вы ввели неверное значение");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine();
                    }
                }

                Console.Clear();

                Entities.Consignment Consignment = this.Container.ConsignmentRepository.Find(ConsignmentNumber);
                if (Consignment == null)
                {
                    
                    Entities.Consignment _Consignment = this.CreateNew(ConsignmentNumber);
                    if (_Consignment.Products.Count() == 0)
                    {
                        this.Container.ConsignmentRepository.Items.Remove(_Consignment);
                    }
                }
                else
                {
                    this.View(Consignment);
                }

                this.IsClose = true;
            }

            Console.Clear();
        }

        public Entities.Consignment CreateNew(int ConsignmentNumber)
        {
            Entities.Consignment Consignment = this.Container.ConsignmentRepository.New(ConsignmentNumber);

            this.AskForProducts(Consignment);

            return Consignment;
        }

        public void AskForProducts(Entities.Consignment Consignment)
        {
            string ChosenProduct = "";

            do
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"Накладная №{Consignment.Id}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine();

                if (Consignment.Products.Count > 0)
                {
                    Consignment.PrintProducts();
                    Console.WriteLine();
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Список товаров");
                Console.ForegroundColor = ConsoleColor.Gray;
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
                        int Amount;
                        while (true)
                        {
                            Console.Write("Количество: ");
                            try
                            {
                                Amount = Convert.ToInt32(Console.ReadLine());
                                if (Amount <= 0)
                                {
                                    throw new Exception();
                                }
                                break;
                            }
                            catch (Exception)
                            {
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("Вы ввели неверное значение");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine();
                            }
                        }
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
