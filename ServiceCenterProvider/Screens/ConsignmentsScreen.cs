using System;
using System.Linq;

namespace ServiceCenterProvider.Screens
{
    class ConsignmentsScreen : IScreen
    {
        private Application App;

        private bool IsClose = false;
        public ConsignmentsScreen(Application _Container)
        {
            this.App = _Container;
        }

        public void Run()
        {
            while (!this.IsClose)
            {
                Output.GreenLine("Раздел накладных");

                if (this.App.ConsignmentRepository.Items.Count > 0)
                {
                    Console.WriteLine();
                    Output.BlueLine("Накладные в системе");
                    this.App.ConsignmentRepository.Print();
                    Console.WriteLine();
                }

                Console.WriteLine("Для создания накладной введите новый номер, или существующий для редактирования");
                Console.WriteLine();
                Output.DarkLine("Введите 0, чтобы вернуться назад");
                Console.WriteLine();

                int ConsignmentNumber;
                while (true)
                {
                    Console.Write(">>> ");
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
                        Output.RedLine("Вы ввели неверное значение");
                        Console.WriteLine();
                    }
                }

                Console.Clear();

                Entities.Consignment Consignment = this.App.ConsignmentRepository.Find(ConsignmentNumber);
                if (Consignment == null)
                {

                    Entities.Consignment _Consignment = this.CreateNew(ConsignmentNumber);
                    if (_Consignment.Products.Count() == 0)
                    {
                        this.App.ConsignmentRepository.Items.Remove(_Consignment);
                    }
                }
                else
                {
                    this.View(Consignment);
                }
            }

            Console.Clear();
        }

        public Entities.Consignment CreateNew(int ConsignmentNumber)
        {
            Entities.Consignment Consignment = this.App.ConsignmentRepository.New(ConsignmentNumber);

            this.AskForProducts(Consignment);

            return Consignment;
        }

        public void AskForProducts(Entities.Consignment Consignment)
        {
            string ChosenProduct = "";

            do
            {
                Output.BlueLine($"Накладная №{Consignment.Id}");

                if (Consignment.Products.Count > 0)
                {
                    Consignment.PrintProducts();
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine();
                }

                Output.GreenLine("Список товаров");
                this.App.ProductRepository.Print();
                Console.WriteLine();
                Console.WriteLine("Для добавления товара введите наименование или код");
                Console.WriteLine();
                Output.DarkLine("Пустая строка, чтобы завершить");
                Console.WriteLine();
                Console.Write(">>> ");
                ChosenProduct = Console.ReadLine();
                if (!String.IsNullOrEmpty(ChosenProduct))
                {
                    Console.Clear();

                    Entities.Product FoundProduct = this.App.ProductRepository.Find(ChosenProduct);
                    if (FoundProduct == null)
                    {
                        Output.RedLine("Такого товара в списке нет пожалуйста, посмотрите еще раз");
                        Console.WriteLine();
                    }
                    else
                    {
                        int Amount;

                        while (true)
                        {
                            Output.BlueLine($"Выбраный товар: {FoundProduct.Name}. Код товара: {FoundProduct.Code}");
                            Console.WriteLine();
                            Console.WriteLine("Введите количество выбранного товара");
                            Console.WriteLine();
                            Console.Write(">>> ");
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
                                Console.Clear();
                                Output.RedLine("Вы ввели неверное значение");
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
