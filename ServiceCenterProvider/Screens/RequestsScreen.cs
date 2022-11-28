using System;
using System.Linq;

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

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Создание / Редактирование заявки");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("P.S. Для выхода введите 0");
                Console.ForegroundColor = ConsoleColor.Gray;

                int RequestNumber;
                while (true)
                {
                    Console.Write("Номер заявки: №");
                    try
                    {
                        RequestNumber = Convert.ToInt32(Console.ReadLine());
                        if (RequestNumber == 0)
                        {
                            Console.Clear();
                            return;
                        }

                        if (RequestNumber <= 0)
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

                Entities.Request Request = this.Container.RequestRepository.Find(RequestNumber);
                if (Request == null)
                {
                    Entities.Request _Request = this.CreateNew(RequestNumber);
                    if (_Request.Products.Count() == 0)
                    {
                        this.Container.RequestRepository.Items.Remove(_Request);
                    }
                } else
                {
                    this.Update(Request);
                }

                this.IsClose = true;
            }

            Console.Clear();
        }

        private Entities.Request CreateNew(int RequestNumber)
        {
            Entities.Request Request = this.Container.RequestRepository.New(RequestNumber);

            this.AskForProducts(Request);

            return Request;
        }

        public void AskForProducts(Entities.Request Request)
        {
            string ChosenProduct = "";

            do
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"Заявка №{Request.Id}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine();

                if (Request.Products.Count > 0)
                {
                    Request.PrintProducts();
                    Console.WriteLine();
                }

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Список доступных к заказу деталей");
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

            Console.WriteLine();

            this.AskForProducts(Request);
        }
    }
}
