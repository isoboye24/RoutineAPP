using Microsoft.Extensions.DependencyInjection;
using RoutineAPP.AllForms;
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
using System.Windows.Forms.DataVisualization.Charting;

namespace RoutineAPP
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();

            // DbContext 
            services.AddScoped<RoutineDBEntities>();

            // Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMonthRepository, MonthRepository>();
            services.AddScoped<IDailyRoutineRepository, DailyRoutineRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IGraphRepository, GraphRepository>();

            // Services
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMonthService, MonthService>();
            services.AddScoped<IDailyRoutineService, DailyRoutineService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IGraphService, GraphService>();
            services.AddScoped<IDashboardService, DashboardService>();

            // Date Provider
            services.AddScoped<IDateProvider, SystemDateProvider>();

            // Forms
            services.AddTransient<FormDashboard>();
            services.AddTransient<FormCategoryList>();
            services.AddTransient<FormCategory>();
            services.AddTransient<FormReportsBoard>();
            services.AddTransient<FormDailyRoutineList>();
            services.AddTransient<FormDailyRoutine>();
            services.AddTransient<FormCommentList>();
            services.AddTransient<FormGraphs>();
            services.AddTransient<FormTaskList>();
            services.AddTransient<FormTask>();
            services.AddTransient<FormDeletedData>();

            bool databaseWasCreated;
            string databaseName = EnsureDatabaseExists(out databaseWasCreated);

            if (databaseWasCreated)
            {
                RunSchemaScript(databaseName);
            }

            var provider = services.BuildServiceProvider();

            System.Windows.Forms.Application.Run(provider.GetRequiredService<FormDashboard>());
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