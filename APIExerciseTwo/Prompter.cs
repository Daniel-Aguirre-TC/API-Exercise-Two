using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
                        optionIndex = optionIndex == 0? 1: --optionIndex; break;                       
                    case ConsoleKey.DownArrow:
                        optionIndex = optionIndex == 2 ? 0 : ++optionIndex; break;

                    case ConsoleKey.Enter: selecting = false; break;

                    default:
                        giveHint = true;
                        break;
                }
            }
            return optionIndex;
        }

        public static string ShowSearchPage(string searchType, CitiesRepository citiesRepo)
        {
            Screen.SetTopSpacing(2);
            var header = $"What {searchType} are you looking for?";
            var searchBox = new string[]
            {
                    header,
                    "         ___________________________________________________       ",
                    "Enter    |                                                 |       ",
                    "Your    |  #############################################  |   <---",
                    "Search   |_________________________________________________|       ","",
            };
            var footer = new string[]
            {
                    "", "Created by Daniel Aguirre.", "",
            };           
            var gettingInput = true;
            var inputSoFar = new StringBuilder("");
            
            while(gettingInput)
            {
                Screen.ClearRows();
                // update the search line
                var searchLine = new Regex(@"^.*\| (.*) \|").Match(searchBox[3]).Groups.Values.Select(x => x.ToString()).ToArray();
                var inputToShow = inputSoFar.Length > searchLine[1].Length ? inputSoFar.ToString().Substring(inputSoFar.Length - searchLine[1].Length).ToString() : inputSoFar.ToString();
                searchBox[3] = searchBox[3].Replace(searchLine[1], MatchLengthCentered(inputToShow, searchLine[1]));
                //print the screenI 

                Screen.AddToRows(PadToCenter(searchBox));

                if (inputSoFar.Length > 0)
                {

                var citiesFound = citiesRepo.CitiesByName(inputSoFar.ToString());
                citiesFound.OrderBy(x => searchType == "city"? x.Name : searchType == "state" ? x.State : x.Zip).ToList().
                    ForEach(x => Screen.AddToRows(PadToCenter(LocationLine(x.Zip, x.State, x.Name))));
                }
                Screen.ReprintWith(PadToCenter(footer));


                // store then switch on input
                var input = Console.ReadKey().KeyChar;
                switch (input)
                {
                    case (char)ConsoleKey.Enter:
                        gettingInput = false;
                        break;
                    case (char)ConsoleKey.UpArrow:

                        break;
                    case (char)ConsoleKey.DownArrow:

                        break;
                    case (char)ConsoleKey.Backspace:
                        if (inputSoFar.Length >= 1)
                        {
                        inputSoFar.Remove(inputSoFar.Length-1, 1);
                        }
                        break;
                    case (char)ConsoleKey.Escape:

                        break;
                    case (char)ConsoleKey.Spacebar:
                    case char x when char.IsLetterOrDigit(x):
                        inputSoFar.Append(input);
                            break;

                    default:
                        break;
                }
            }
            //todo: select a city found by arrow instead of by if there is more than one
            return inputSoFar.ToString();
        }



















        static string LocationLine(string zip, string state, string city)
        {
            var stateWidth = 23;
            var editedState = state.PadLeft(stateWidth / 2 + state.Length / 2).PadRight(stateWidth);
            var cityWidth = 30;
            // 5 chars wide for the zip
            // 16 chars wide for the state
            // city padded to match the length of zip and editedState
            return $"{zip}  --- {editedState} --- {city.PadRight(cityWidth)}";

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

        static string MatchLengthCentered(string text, string targetLength)
        {
            return text.PadLeft(targetLength.Length / 2 + text.Length / 2).PadRight(targetLength.Length);
        }

    }
}
