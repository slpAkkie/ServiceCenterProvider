using System;
using System.Linq;

namespace ServiceCenterProvider.Screens
{
    class RequestsScreen : IScreen
    {
        private Application App;

        private bool IsClose = false;
        public RequestsScreen(Application _Container)
        {
            this.App = _Container;
        }

        public void Run()
        {
            while (!this.IsClose)
            {
                Output.GreenLine("Раздел заявок");

                if (this.App.RequestRepository.Items.Count > 0)
                {
                    Console.WriteLine();
                    Output.BlueLine("Заявки в системе");
                    this.App.RequestRepository.Print();
                    Console.WriteLine();
                }

                Console.WriteLine("Для создания заявки введите новый номер, или существующий для редактирования");
                Console.WriteLine();
                Output.DarkLine("Введите 0, чтобы вернуться назад");
                Console.WriteLine();

                int RequestNumber;
                while (true)
                {
                    Console.Write(">>> ");
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
                        Output.RedLine("Вы ввели неверное значение");
                        Console.WriteLine();
                    }
                }
                Console.Clear();

                Entities.Request Request = this.App.RequestRepository.Find(RequestNumber);
                if (Request == null)
                {
                    Entities.Request _Request = this.CreateNew(RequestNumber);
                    if (_Request.Products.Count() == 0)
                    {
                        this.App.RequestRepository.Items.Remove(_Request);
                    }
                }
                else
                {
                    this.Update(Request);
                }
            }

            Console.Clear();
        }

        private Entities.Request CreateNew(int RequestNumber)
        {
            Entities.Request Request = this.App.RequestRepository.New(RequestNumber);

            this.AskForProducts(Request);

            return Request;
        }

        public void AskForProducts(Entities.Request Request)
        {
            string ChosenProduct = "";

            do
            {
                Output.BlueLine($"Заявка №{Request.Id}");

                if (Request.Products.Count > 0)
                {
                    Request.PrintProducts();
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine();
                }

                Output.GreenLine("Список доступных к заказу деталей");
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
