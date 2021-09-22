using System;
using System.Collections.Generic;
using System.Text;

namespace APIExerciseTwo
{
    static class ApplicationManager
    {

        static bool ApplicationRunning { get; set; }

        static ApplicationManager()
        {
            ApplicationRunning = true;
        }


        public static void StartApplication()
        {
            var weatherMan = new OpenWeatherMapAPI();
            Prompter.StartingScreen();
            

            while (ApplicationRunning)
            {
                var searchType = Prompter.GetSearchType();

                switch (searchType)
                {
                    // search by city
                    case 0:

                        break;
                    // search by state
                    case 1:
                    // search by zip
                    case 2:

                        Screen.ClearThenPrint("I'm sorry, this feature is not yet set up.");
                        break;


                }
                
                


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

            }

            static void GetSearchType()
            {
                // what would you like to search by?
                // --> city <--   state    zip



            }


        }

    }
}
