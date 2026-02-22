using RoutineAPP.Application.Services;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.Infrastructure.Data;
using RoutineAPP.Infrastructure.Repositories;
using System;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace RoutineAPP
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            var db = new RoutineDBEntities();

            ICategoryRepository categoryRepository = new CategoryRepository(db);
            ICategoryService categoryService = new CategoryService(categoryRepository);

            IMonthRepository monthRepository = new MonthRepository(db);
            IMonthService monthService = new MonthService(monthRepository);

            IDailyRoutineRepository DailyRoutineRepository = new DailyRoutineRepository(db);
            IDailyRoutineService dailyRoutineService = new DailyRoutineService(DailyRoutineRepository);

            IYearsRepository yearRepository = new YearsRepository(db);
            IYearsService yearService = new YearsService(yearRepository);

            ITaskRepository taskRepository = new TaskRepository(db);
            ITaskService taskService = new TaskService(taskRepository);


            bool databaseWasCreated;
            string databaseName = EnsureDatabaseExists(out databaseWasCreated);

            if (databaseWasCreated)
            {
                RunSchemaScript(databaseName);
            }


            System.Windows.Forms.Application.Run(new FormDashboard(categoryService, monthService, yearService, dailyRoutineService, taskService));
        }

        private static string EnsureDatabaseExists(out bool created)
        {
            created = false;

            string efConnection = ConfigurationManager
                .ConnectionStrings["RoutineDBEntities"]
                .ConnectionString;

            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder(efConnection);

            string sqlConnectionString = entityBuilder.ProviderConnectionString;

            SqlConnectionStringBuilder sqlBuilder =
                new SqlConnectionStringBuilder(sqlConnectionString);

            string databaseName = sqlBuilder.InitialCatalog;
            string dataSource = sqlBuilder.DataSource;

            string masterConnection =
                $"Data Source={dataSource};Initial Catalog=master;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(masterConnection))
            {
                connection.Open();

                string checkQuery =
                    $"IF DB_ID('{databaseName}') IS NULL BEGIN CREATE DATABASE [{databaseName}]; SELECT 1 END ELSE SELECT 0";

                using (SqlCommand cmd = new SqlCommand(checkQuery, connection))
                {
                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    if (result == 1)
                        created = true;
                }
            }

            return databaseName;
        }

        private static void RunSchemaScript(string databaseName)
        {
            string scriptPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "database",
                "CreateDatabase.sql");

            if (!File.Exists(scriptPath))
                return;

            string script = File.ReadAllText(scriptPath);

            string connectionString =
                $"Data Source=localhost\\SQLEXPRESS;Initial Catalog={databaseName};Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(script, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}