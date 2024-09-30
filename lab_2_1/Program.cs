using System;
using Microsoft.Data.SqlClient;

namespace lab_2_1lab_2_1_var_5
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseConnection conn = new DatabaseConnection();

            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("------Виберіть опцію------");
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine("1 - Пошук товару");
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine("2 - Пошук по категорії");
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine("3 - Керування товаром");
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine("4 - Керування категоріями");
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine("0 - Вихід");
                Console.WriteLine("-----------------------------------------------------------");

                int swt;
                if (int.TryParse(Console.ReadLine(), out swt))
                {
                    switch (swt)
                    {
                        case 1:
                            {
                                Search.SearchProduct(conn);
                                break;
                            }
                        case 2:
                            {
                                Search.SearchCategories(conn);
                                break;
                             }
                        case 3:
                            {
                                Manage.ManegeProduct(conn);
                                break;
                            }
                        case 4:
                            {
                                Manage.ManageCategory(conn);
                                break;
                            }
                        case 0:
                            {
                                running = false;
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                                break;
                            }
                    }
                }
                else
                {
                    Console.WriteLine("Неправильний ввід. Введіть число.");
                }

                //Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                //Console.ReadKey();
            }
        }
    }
}
