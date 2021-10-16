using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Linq;

namespace APIExerciseTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            // set window size and hide Console cursor since we won't be using it.
            Console.SetWindowSize(100, 80);
            Console.CursorVisible = false;
            // start the application
            ApplicationManager.StartApplication();

        }
    }
}
