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
                Console.WriteLine("1. Создание / Редактирование зявки");
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
