using System;
using Microsoft.Data.SqlClient;

namespace lab_2_1_var_5
{
    public class Search
    {
        public static void SearchProduct(DatabaseConnection conn)
        {
            using (SqlConnection connection = conn.GetConnection())
            {
                try
                {
                    connection.Open();

                    Console.WriteLine("Введіть назву товару для пошуку:");
                    string productName = Console.ReadLine();

                    string selectQuery = @"
                        SELECT p.Id, p.Name, p.Sale, p.Description, p.Count, c.Name AS CategoryName
                        FROM Products p
                        JOIN Categories c ON p.CategoryId = c.Id
                        WHERE p.Name LIKE '%' + @Name + '%'";

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", productName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Результати пошуку:");
                            bool found = false;
                            while (reader.Read())
                            {
                                found = true;
                                Console.WriteLine($"Id: {reader["Id"]} | Назва: {reader["Name"]} | Ціна: {reader["Sale"]} | Опис: {reader["Description"]} | Кількість: {reader["Count"]} | Категорія: {reader["CategoryName"]}");
                            }
                            if (!found)
                            {
                                Console.WriteLine("Товари не знайдені.");
                            }
                        }
                        Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Помилка: " + ex.Message);
                }
            }
        }
        public static void SearchCategories(DatabaseConnection conn)
        {
            using (SqlConnection connection = conn.GetConnection())
            {
                try
                {
                    connection.Open();

                    string categoryQuery = "SELECT Id, Name FROM Categories";
                    Console.WriteLine("-----------------------------------------------------------");
                    Console.WriteLine("Категорії:");
                    using (SqlCommand categoryCommand = new SqlCommand(categoryQuery, connection))
                    using (SqlDataReader categoryReader = categoryCommand.ExecuteReader())
                    {
                        while (categoryReader.Read())
                        {
                            Console.WriteLine($"Id: {categoryReader["Id"]} | Назва: {categoryReader["Name"]}");
                        }
                    }
                    Console.WriteLine("-----------------------------------------------------------");

                    Console.WriteLine("Введіть ID категорії для пошуку товарів:");
                    int categoryId;
                    while (!int.TryParse(Console.ReadLine(), out categoryId))
                    {
                        Console.WriteLine("Неправильний ввід. Будь ласка, введіть число для ID категорії.");
                    }

                    string selectQuery = @"
                        SELECT p.Id, p.Name, p.Sale, p.Description, p.Count
                        FROM Products p
                        WHERE p.CategoryId = @CategoryId";

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryId", categoryId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Результати пошуку товарів у категорії:");
                            bool found = false;
                            while (reader.Read())
                            {
                                found = true;
                                Console.WriteLine($"Id: {reader["Id"]} | Назва: {reader["Name"]} | Ціна: {reader["Sale"]} | Опис: {reader["Description"]} | Кількість: {reader["Count"]}");
                            }
                            if (!found)
                            {
                                Console.WriteLine("Товари не знайдені для цієї категорії.");
                            }
                        }
                        Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Помилка: " + ex.Message);
                }
            }
        }

    }
}
