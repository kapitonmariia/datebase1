using System;
using Microsoft.Data.SqlClient;

namespace lab_2_1_var_5
{
    public class Manage
    {
        public static void ManageCategory(DatabaseConnection conn)
        {
            using (SqlConnection connection = conn.GetConnection())
            {
                try
                {
                    connection.Open();

                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Categories')
                        BEGIN
                            CREATE TABLE Categories (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Name NVARCHAR(100) NOT NULL
                            );
                        END";

                    using (SqlCommand createTableCommand = new SqlCommand(createTableQuery, connection))
                    {
                        createTableCommand.ExecuteNonQuery();
                    }

                    bool running = true;
                    while (running)
                    {
                        Console.Clear();
                        Console.WriteLine("----КЕРУВАННЯ КАТЕГОРІЯМИ----");
                        Console.WriteLine("-----------------------------------------------------------");
                        Console.WriteLine("1 - Вивести список категорій");
                        Console.WriteLine("-----------------------------------------------------------");
                        Console.WriteLine("2 - Додати категорію");
                        Console.WriteLine("-----------------------------------------------------------");
                        Console.WriteLine("3 - Видалити категорію");
                        Console.WriteLine("-----------------------------------------------------------");
                        Console.WriteLine("0 - Назад");
                        Console.WriteLine("-----------------------------------------------------------");

                        int action;
                        if (int.TryParse(Console.ReadLine(), out action))
                        {
                            switch (action)
                            {
                                case 1:
                                    {
                                        string selectQuery = "SELECT Id, Name FROM Categories";
                                        using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                                        {
                                            using (SqlDataReader reader = selectCommand.ExecuteReader())
                                            {
                                                Console.WriteLine("Список категорій:");
                                                while (reader.Read())
                                                {
                                                    Console.WriteLine($"Id: {reader["Id"]} | Назва: {reader["Name"]}");
                                                }
                                            }
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        Console.WriteLine("Введіть назву нової категорії:");
                                        string categoryName = Console.ReadLine();
                                        string selectQuery = "SELECT Id, Name FROM Categories";

                                        string insertQuery = "IF NOT EXISTS (SELECT * FROM Categories WHERE Name = @Name) " +
                                                             "INSERT INTO Categories (Name) VALUES (@Name)";
                                        using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                                        {
                                            cmd.Parameters.AddWithValue("@Name", categoryName);
                                            int rowsAffected = cmd.ExecuteNonQuery();
                                            if (rowsAffected > 0)
                                            {
                                                Console.WriteLine("Категорію успішно створено.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Категорія вже існує.");
                                            }
                                        }
                                        break;
                                    }
                                case 3:
                                    {
                                        Console.WriteLine("Введіть назву категорії для видалення:");
                                        string categoryNameToDelete = Console.ReadLine();
                                        string deleteQuery = "DELETE FROM Categories WHERE Name = @Name";

                                        using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                                        {
                                            deleteCommand.Parameters.AddWithValue("@Name", categoryNameToDelete);
                                            int rowsAffected = deleteCommand.ExecuteNonQuery();
                                            if (rowsAffected > 0)
                                            {
                                                Console.WriteLine("Категорію успішно видалено.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Категорія не знайдена.");
                                            }
                                        }
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

        public static void ManegeProduct(DatabaseConnection conn)
        {
            using (SqlConnection connection = conn.GetConnection())
            {
                try
                {
                    connection.Open();

                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Products')
                        BEGIN
                            CREATE TABLE Products (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Name NVARCHAR(100) NOT NULL, 
                                Sale INT,
                                Description NVARCHAR(100) NOT NULL,
                                Count INT,
                                CategoryId INT NOT NULL,
                                FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
                            );
                        END";
                    using (SqlCommand createTableCommand = new SqlCommand(createTableQuery, connection))
                    {
                        createTableCommand.ExecuteNonQuery();
                    }

                    bool running = true;
                    while (running)
                    {
                        Console.Clear();
                        Console.WriteLine("----КЕРУВАННЯ ТОВАРОМ----");
                        Console.WriteLine("-----------------------------------------------------------");
                        Console.WriteLine("1 - Вивести список товару");
                        Console.WriteLine("-----------------------------------------------------------");
                        Console.WriteLine("2 - Додати товар");
                        Console.WriteLine("-----------------------------------------------------------");
                        Console.WriteLine("3 - Видалити товар");
                        Console.WriteLine("-----------------------------------------------------------");
                        Console.WriteLine("0 - Назад");
                        Console.WriteLine("-----------------------------------------------------------");

                        int action;
                        if (int.TryParse(Console.ReadLine(), out action))
                        {
                            switch (action)
                            {
                                case 1:
                                    {
                                        string selectQuery = @"
                                            SELECT p.Id, p.Name, p.Sale, p.Description, p.Count, c.Name AS CategoryName
                                            FROM Products p
                                            JOIN Categories c ON p.CategoryId = c.Id";
                                        using (SqlCommand command = new SqlCommand(selectQuery, connection))
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            Console.WriteLine("Список товарів:");
                                            while (reader.Read())
                                            {
                                                Console.WriteLine($"Id: {reader["Id"]} | Назва: {reader["Name"]} | Ціна: {reader["Sale"]} | Опис: {reader["Description"]} | Кількість: {reader["Count"]} | Категорія: {reader["CategoryName"]}");
                                            }
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        string selectQuery = "SELECT Id, Name FROM Categories";
                                        string insertProductQuery = @"
                                            INSERT INTO Products (Name, Sale, Description, Count, CategoryId)
                                            VALUES (@Name, @Sale, @Description, @Count, @CategoryId)";
                                        using (SqlCommand command = new SqlCommand(insertProductQuery, connection))
                                        {
                                            Console.WriteLine("Введіть імя товару: ");
                                            string Name = Console.ReadLine();
                                            Console.WriteLine("Введіть ціну товару: ");
                                            int Sale = Convert.ToInt32(Console.ReadLine());
                                            Console.WriteLine("Введіть опис товару: ");
                                            string Description = Console.ReadLine();
                                            Console.WriteLine("Введіть кількість товару: ");
                                            int Count = Convert.ToInt32(Console.ReadLine());
                                            Console.WriteLine("-----------------------------------------------------------");
                                            using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                                            {
                                                using (SqlDataReader reader = selectCommand.ExecuteReader())
                                                {
                                                    Console.WriteLine("Список категорій:");
                                                    while (reader.Read())
                                                    {
                                                        Console.WriteLine($"Id: {reader["Id"]} | Назва: {reader["Name"]}");
                                                    }
                                                }
                                            }
                                            Console.WriteLine("-----------------------------------------------------------");
                                            Console.WriteLine("Виберіть категорію (введіть ID): ");
                                            int CategoryId = Convert.ToInt32(Console.ReadLine());

                                            command.Parameters.AddWithValue("@Name", Name);
                                            command.Parameters.AddWithValue("@Sale", Sale);
                                            command.Parameters.AddWithValue("@Description", Description);
                                            command.Parameters.AddWithValue("@Count", Count);
                                            command.Parameters.AddWithValue("@CategoryId", CategoryId);
                                            command.ExecuteNonQuery();

                                            Console.WriteLine("Товар успішно додано.");
                                        }
                                        break;
                                    }
                                case 3:
                                    {
                                        Console.WriteLine("Введіть ID товару для видалення:");
                                        int productIdToDelete = Convert.ToInt32(Console.ReadLine());
                                        string deleteQuery = "DELETE FROM Products WHERE Id = @Id";

                                        using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                                        {
                                            deleteCommand.Parameters.AddWithValue("@Id", productIdToDelete);
                                            int rowsAffected = deleteCommand.ExecuteNonQuery();
                                            if (rowsAffected > 0)
                                            {
                                                Console.WriteLine("Товар успішно видалено.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Товар не знайдений.");
                                            }
                                        }
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
