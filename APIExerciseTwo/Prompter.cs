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
            Screen.ClearThenPrint(PadToCenter(new string[]
            {
                "Thanks for using my OpenWeatherMap API application.","",
                "Created by Daniel Aguirre"
            }));
            Console.ReadKey();

        }
 

        /// <summary> Handles the loop needed to obtain user input for what type of location they are searching for. </summary>
        /// <returns> 0 - 2 depending on which option is selected. 0 = City, 1 = State, 2 = Zip</returns>
        public static int GetSearchType()
        {
            // option Index will represent which option the user is currently on
            var optionIndex = 0;

            // selecting will be true until the user clicks enter,
            // and is used to keep the player on this screen until an option is selected.
            var selecting = true;
            

            while (selecting)
            {
                // set the padding for ceiling to center this message vertically.
                // magic number 3 is to offset for displaying 5 rows of strings, half would be 2.5, so I rounded up to three.
                PadCeiling(Console.WindowHeight / 2 - 3);                                             
                Screen.ClearRows();
                Screen.AddToRows(PadToCenter("What would you like to search by?"));                     // row 1
                Screen.AddToRows("");                                                                   // row 2
                Screen.AddToRows(PadToCenter(optionIndex == 0 ? "City <-- " : "City     "));            // row 3
                Screen.AddToRows(PadToCenter(optionIndex == 1 ? "State <--" : "State    "));            // row 4
                Screen.ReprintWith(PadToCenter(optionIndex == 2 ? "Zip <--  " : "Zip      "));          // row 5
                
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        optionIndex = optionIndex == 0? 1: --optionIndex; break;       
                        
                    case ConsoleKey.DownArrow:
                        optionIndex = optionIndex == 2 ? 0 : ++optionIndex; break;

                    case ConsoleKey.Enter: 
                        selecting = false; break;

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
                    "Pressing Enter will return search results for the first location listed.\n"
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

                    switch(searchType)
                    {
                        case "city":
                            var citiesFound = citiesRepo.CitiesByName(inputSoFar.ToString()).OrderBy(x => x.Name).ToList();
                            citiesFound.ForEach(x => Screen.AddToRows(PadToCenter(LocationLine(x.Zip, x.State, x.Name))));
                            break;

                        case "state":
                            var statesFound = citiesRepo.GetState(inputSoFar.ToString()).OrderBy(x => x.StateName).ToList();
                            statesFound.ForEach(x => { Screen.AddToRows(PadToCenter(x.StateName)); });
                            break;

                        case "zip code":
                            var zipsFound = citiesRepo.CitiesByZip(inputSoFar.ToString()).OrderBy(x => x.Zip).ToList();
                            zipsFound.ForEach(x => Screen.AddToRows(PadToCenter(LocationLine(x.Zip, x.State, x.Name))));
                            break;
                    }

                }
                Screen.ReprintWith(PadToCenter(footer));

                // store then switch on input
                var input = Console.ReadKey().KeyChar;
                switch (input)
                {
                    case (char)ConsoleKey.Enter:
                        if (inputSoFar.Length > 0)
                        {
                            try
                            {
                                return searchType == "city" ? citiesRepo.CitiesByName(inputSoFar.ToString()).OrderBy(x => x.Name).First().Name
                                 : searchType == "state" ? citiesRepo.GetState(inputSoFar.ToString()).OrderBy(x => x.StateName).First().StateName
                                                        : citiesRepo.CitiesByZip(inputSoFar.ToString()).OrderBy(x => x.Zip).First().Name;
                            }
                            catch (Exception)
                            {
                                break;
                            }
                        }
                        else break;

                    case (char)ConsoleKey.Backspace:
                        if (inputSoFar.Length >= 1)
                        {
                        inputSoFar.Remove(inputSoFar.Length-1, 1);
                        }
                        break;
                    case (char)ConsoleKey.Spacebar:
                    case char x when char.IsLetterOrDigit(x):
                        inputSoFar.Append(input);
                            break;

                    default:
                        break;
                }
            }
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
