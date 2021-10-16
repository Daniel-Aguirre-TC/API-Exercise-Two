using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace APIExerciseTwo
{
    static class ApplicationManager
    {
        static IDbConnection DbConnection;
        static bool ApplicationRunning { get; set; }

        static ApplicationManager()
        {
            ApplicationRunning = true;
            DbConnection = GetConnection();
        }


        public static void StartApplication()
        {

            var citiesRepo = new CitiesRepository(DbConnection);
            var weatherMan = new OpenWeatherMapAPI();
            Prompter.StartingScreen();
            
            while (ApplicationRunning)
            {
                string searchType = string.Empty;
                switch (Prompter.GetSearchType())
                {
                    // search by city
                    case 0: searchType = "city"; break;
                    // search by state
                    case 1: searchType = "state"; break;
                    // search by zip
                    case 2: searchType = "zip code"; break;
                }
                weatherMan.GetWeather(Prompter.ShowSearchPage(searchType, citiesRepo));
                Console.ReadKey();
            }
        }

        /*
        //ApplicationRunning = false;
        //Console.Write("\nPlease enter the name of the city you want to check: ");

        //weatherMan.GetWeather(Console.ReadLine());


        //Console.Write("\nWould you like to search for another city?\n\nY/N: ");
        //var input = Console.ReadKey().Key;

        //if (input == ConsoleKey.Y || input == ConsoleKey.Enter)
        //{
        //    ApplicationRunning = true;
        //    Console.WriteLine();
        //}
        */


        static IDbConnection GetConnection()
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connString = config.GetConnectionString("DefaultConnection");
            return new MySqlConnection(connString);

        }


    }
}
