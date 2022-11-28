using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterProvider.Screens
{
    interface IScreen
    {
        /// <summary>
        /// Запуск экрана.
        /// Обрабатывает весь жизненный цикл экрана.
        /// </summary>
        void Run();
    }
}
