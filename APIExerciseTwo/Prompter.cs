using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APIExerciseTwo
{
    static class Prompter
    {


        public static void StartingScreen()
        {
            PadCeiling(Console.WindowHeight / 2);
            Screen.ClearThenPrint(new string[]
            {
                "Thanks for taking using my OpenWeatherMap API application.","",
                "Created by Daniel Aguirre"
            });


        }
 
        public static int GetSearchType()
        {
            var giveHint = false;
            var optionIndex = 0;
            var selecting = true;
            
            while (selecting)
            {

                // set the padding for ceiling to center this message vertically.
                // magic number three is to offset for displaying five rows of strings, half would be 2.5, so I rounded up to three.
                PadCeiling(Console.WindowHeight / 2 - 3);
                if (giveHint == true)
                {
                    PadCeiling(Console.WindowHeight / 2 - 5);
                    Screen.AddToRows(PadToCenter(new string[]
                    {
                        "Please use the up/down arrows to select an option.","",
                        "Click enter when you are ready to confirm your selection.",""                    
                    }));
                }
                Screen.ClearRows();
                Screen.AddToRows(PadToCenter("What would you like to search by?"));
                Screen.AddToRows("");
                Screen.AddToRows(PadToCenter(optionIndex == 0 ? "City <-- " : "City     "));
                Screen.AddToRows(PadToCenter(optionIndex == 1 ? "State <--" : "State    "));
                Screen.ReprintWith(PadToCenter(optionIndex == 2 ? "Zip <--  " : "Zip      "));               
                
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        optionIndex = optionIndex == 0? 1: --optionIndex;
                        
                        break;
                    case ConsoleKey.DownArrow:
                        optionIndex = optionIndex == 2 ? 0 : ++optionIndex;

                        break;
                    case ConsoleKey.Enter:
                        selecting = false;
                        break;

                    default:
                        giveHint = true;
                        break;
                }
            }   
            return optionIndex;
        }

        public string AskForCity()
        {
            var gettingInput = true;
            var inputSoFar = new StringBuilder();
            while(gettingInput)
            {

                //TODO: Print page that populates matching cities while input is typed in one char at a time.

            }
            
        }

        static void CenterVertically(int nextMessageLength)
        {
            PadCeiling(Console.WindowHeight / 2 - (nextMessageLength / 2));
        }

        static void PadCeiling(int rowsToPad)
        {
            Screen.SetTopSpacing(rowsToPad);
        }


        static void PadCeiling(string[] message)
        {
            Screen.SetTopSpacing(Console.WindowHeight / 2 - message.Length);
        }


        /// <summary>
        /// Return the provided string array with padding to the left to center it horiziontally.
        /// </summary>
        /// <param name="textToCenter"></param>
        /// <returns></returns>
        public static string[] PadToCenter(string[] textToCenter)
        {
            return textToCenter.Select(x => PadToCenter(x)).ToArray();
        }

        /// <summary>
        /// Return the provided string with padding to the left to center it horiziontally.
        /// </summary>
        /// <param name="textToCenter"></param>
        /// <returns></returns>
        public static string PadToCenter(string textToCenter)
        {
            if (textToCenter == null)
            {
                return "";
            }
            return textToCenter.PadLeft((int)MathF.Round((Console.WindowWidth / 2) + (textToCenter.Length / 2)));
        }


    }
}
