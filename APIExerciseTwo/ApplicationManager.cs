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

        // When my ApplicationManager is first used it will call my static constructor
        static ApplicationManager()
        {
            // Initialize ApplicationRunning bool as true.
            ApplicationRunning = true;
            // Initialize IDbConnection using the connection string stored in my appsettings.json file.
            DbConnection = GetConnection();
        }


        public static void StartApplication()
        {
            // create an instance of CitiesRepo to access MySQL Database using my IDbConnection 
            var citiesRepo = new CitiesRepository(DbConnection);

            // create an instance OpenWeatherMapAPI that is used to request information from the API
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

        /// <returns>IDbConnection based on the "DefaultConnection" string stored in appsettings.json file</returns>
        static IDbConnection GetConnection()
        {
            // Use ConfigurationBuilder to initalize an IConfigurationRoot from by appsettings.json file.
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // obtain the connection string to access MySQL Database
            string connString = config.GetConnectionString("DefaultConnection");

            // return the IDbConnection created with the above connection string.
            return new MySqlConnection(connString);
        }
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