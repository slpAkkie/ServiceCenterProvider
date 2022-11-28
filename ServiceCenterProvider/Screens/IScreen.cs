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
