using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace APIExerciseTwo
{
    class Screen
    {
        // number of empty rows to place at the top of the screen for spacing.
        static int DefaultTopSpacing = 2;
        static int TopSpacing { get; set; }
        static List<StringBuilder> Checkpoint;
        public static int RowCount { get => ScreenRows.Count(); }

        // List of StringBuilders used to create the screen.
        static List<StringBuilder> ScreenRows;

        static Screen()
        {
            TopSpacing = DefaultTopSpacing;
            ScreenRows = new List<StringBuilder>();
            Checkpoint = new List<StringBuilder>();
        }

        #region Top Spacing

        public static void ResetTopSpacing()
        {
            SetTopSpacing(DefaultTopSpacing);
        }

        public static void SetTopSpacing(int newSpacingHeight)
        {
            TopSpacing = newSpacingHeight;
        }

        public static int GetTopSpacing()
        {
            return TopSpacing;
        }

        #endregion

        #region Checkpoint

        /// <summary>
        /// Return a string array representing the current contents of the screen. You can return to this checkpoint by calling ClearThenPrint(string[] returned)
        /// </summary>
        /// <returns></returns>
        public static string[] GetTempCheckpoint()
        {
            return ScreenRows.Skip(TopSpacing).Select(x => x.ToString()).ToArray();
        }

        /// <summary>
        /// Set a checkpoint that you can revert the screen to by calling ResetToCheckpoint()
        /// </summary>
        public static void SetCheckpoint()
        {
            Checkpoint.Clear();
            ScreenRows.Skip(TopSpacing).ToList().ForEach(Checkpoint.Add);
        }
        /// <summary>
        /// Revert back to the screen as it was when SetCheckpoint() was last called.
        /// </summary>
        public static void ResetToCheckpoint()
        {
            ClearThenPrint(Checkpoint.Select(x => x.ToString()).ToArray());
        }

        #endregion

        #region Input Messages

        /// <summary>
        /// print the provided question at the bottom of the screen and then return true/false based on the users input.
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static bool ReprintWithYesNoQ(string question)
        {
            ReprintWith(question, Prompter.PadToCenter("Enter Yes/No: "));
            return new Regex(@"^y").IsMatch(Console.ReadLine().ToLower()) ? true : false;
        }

        /// <summary>
        /// print the provided question at the bottom of the screen and then return true/false based on the users input.
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static bool ReprintWithYesNoQ(string[] question)
        {
            ReprintWith(Prompter.PadToCenter(question), Prompter.PadToCenter("Enter Yes/No: "));
            return new Regex(@"^y").IsMatch(Console.ReadLine().ToLower()) ? true : false;
        }

        /// <summary>
        /// Clear the screen then Cycle through the newMessage until the user confirms their input. baseMessage reflects all strings above where the user types. inputRequestMessage reflects the text that appears directly in front of where the user types their response.
        /// </summary>
        /// <param name="newMessage"></param>
        /// <param name="inputRequestMessage"></param>
        /// <returns></returns>
        public static string ClearThenConfirmInput(string[] newMessage, string inputRequestMessage)
        {
            bool inputconfirmed = false;
            string input = string.Empty;
            while (!inputconfirmed)
            {
                ClearThenPrint(newMessage, inputRequestMessage);
                input = ConvertIfEnter(Console.ReadLine());
                Console.Clear();
                inputconfirmed = ReprintWithYesNoQ(new string[] { $"You entered: {input}.", "Is this correct? " });
            }
            return input;
        }

        /// <summary>
        /// Cycle through a message that is displayed below the current screen until the user confirms their input. baseMessage reflects all strings above where the user types. inputRequestMessage reflects the text that appears directly in front of where the user types their response.
        /// </summary>
        /// <param name="newMessage"></param>
        /// <param name="inputRequestMessage"></param>
        /// <returns></returns>
        public static string ReprintThenConfirmInput(string newMessage, string inputRequestMessage)
        {
            //Set checkpoint to before message is added to screen
            var tempScreenCheckpoint = GetTempCheckpoint();
            bool inputconfirmed = false;
            string input = string.Empty;
            // cycle until input is confirmed
            while (!inputconfirmed)
            {
                ClearRows();
                AddToRows(tempScreenCheckpoint);
                ReprintWith(newMessage, inputRequestMessage);
                input = ConvertIfEnter(Console.ReadLine());
                Console.Clear();
                inputconfirmed = ReprintWithYesNoQ(new string[] { "", $"You entered: {input}.", "Is this correct? ", "" });

            }
            return input;
        }

        /// <summary>
        /// Cycle through a message that is displayed below the current screen until the user confirms their input. baseMessage reflects all strings above where the user types. inputRequestMessage reflects the text that appears directly in front of where the user types their response.
        /// </summary>
        /// <param name="newMessage"></param>
        /// <param name="inputRequestMessage"></param>
        /// <returns></returns>
        public static string ReprintThenConfirmInput(string[] newMessage, string inputRequestMessage)
        {
            //Set checkpoint to before message is added to screen.
            var tempScreenCheckpoint = GetTempCheckpoint();
            bool inputconfirmed = false;
            string input = string.Empty;
            // cycle until input is confirmed
            while (!inputconfirmed)
            {
                ClearRows();
                Console.Clear();
                AddToRows(tempScreenCheckpoint);
                ReprintWith(newMessage, inputRequestMessage);
                input = ConvertIfEnter(Console.ReadLine());
                Console.Clear();
                inputconfirmed = ReprintWithYesNoQ(new string[] { "", $"You entered: {input}.", "", "Is this correct? ", "" });
                ClearRows();
            }
            return input;
        }

        /// <summary>
        /// Print the message provided on a loop until the player provides a valid input option. Numbers available is dependant on how many messages match @"[\d]+)"
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int ReprintWithOptionList(string[] message)
        {
            int maxNumber = message.Where(x => new Regex(@"^[\d]+\)").IsMatch(x)).Count();
            char input = ' ';
            bool firstLoop = true;
            int result = 0;
            do
            {
                ReprintWith(message);
                // if not the first loop, display invalid entry message.
                result = ValidateOptionEntry(ref input, ref firstLoop, ref maxNumber);

            }
            while (result == 0);
            // return result once a valid result is received
            return result;
        }

        /// <summary>
        /// Print the message provided on a loop until the player provides a valid input option. Numbers available is dependant on how many messages match @"[\d]+)"
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int ClearThenPrintOptionList(string[] message)
        {
            int maxNumber = message.Where(x => new Regex(@"^[\d]+\)").IsMatch(x)).Count();
            char input = ' ';
            bool firstLoop = true;
            int result = 0;
            do
            {
                ClearThenPrint(message);
                // if not the first loop, display invalid entry message.
                result = ValidateOptionEntry(ref input, ref firstLoop, ref maxNumber);

            }
            while (result == 0);
            // return result once a valid result is received
            return result;
        }

        #endregion

        #region Standard Messages

        /// <summary>
        /// Clear all rows of the 
        /// </summary>
        /// <param name="newMessage"></param>
        public static void ClearThenPrint(string newMessage)
        {
            ClearRows();
            AddToRows(newMessage);
            PrintScreen();
        }

        /// <summary>
        /// Clear all rows of the screen then .
        /// </summary>
        /// <param name="key"></param>
        public static void ClearThenPrint(string[] newMessage)
        {
            ClearRows();
            AddToRows(newMessage);
            PrintScreen();
        }

        /// <summary>
        /// Clear the screen, print the provided array of messages, then use Console.Write()
        /// </summary>
        /// <param name="key"></param>
        public static void ClearThenPrint(string[] newMessage, string inputRequestMessage)
        {
            ClearRows();
            AddToRows(newMessage);
            PrintScreen(inputRequestMessage);
        }

        /// <summary>
        /// Add a provided string to the list of StringRows then reprint the page with the provided requestingInput string before where the user types their response.
        /// </summary>
        /// <param name="newMessage"></param>
        /// <param name="requestingInput"></param>
        public static void ReprintWith(string newMessage, string inputRequestMessage)
        {
            AddToRows(newMessage);
            PrintScreen(inputRequestMessage);
        }

        /// <summary>
        /// Add a provided string to the list of StringRows then reprint the page with the provided requestingInput string before where the user types their response.
        /// </summary>
        /// <param name="newMessage"></param>
        /// <param name="requestingInput"></param>
        public static void ReprintWith(string newMessage)
        {
            AddToRows(newMessage);
            PrintScreen();
        }

        /// <summary>
        /// Add an array of strings to the list of StringRows then reprint the page.
        /// </summary>
        /// <param name="newMessages"></param>
        /// <param name="requestingInput"></param>
        public static void ReprintWith(string[] newMessages, string inputRequestMessage)
        {
            AddToRows(newMessages);
            PrintScreen(inputRequestMessage);
        }

        /// <summary>
        /// Add an array of strings to the list of StringRows then reprint the page.
        /// </summary>
        /// <param name="newMessages"></param>
        public static void ReprintWith(string[] newMessages)
        {
            AddToRows(newMessages);
            PrintScreen();
        }

        #endregion

        #region Internal Methods for Printing Screen.

        /// <summary>
        /// Helper for PrintOptionList() methods to validate that the entry received is a number between 1 and maxNumber
        /// </summary>
        /// <param name="input"></param>
        /// <param name="firstLoop"></param>
        /// <param name="maxNumber"></param>
        /// <returns></returns>
        static int ValidateOptionEntry(ref char input, ref bool firstLoop, ref int maxNumber)
        {
            var result = 0;
            if (!firstLoop)
            {
                ReprintWith(new string[] { "", $"I'm sorry, {ConvertIfEnter(input.ToString())} is not a valid entry." });
            }
            // get single char input from user
            input = Console.ReadKey().KeyChar;
            // if input is a one char digit
            if (new Regex(@"^[\d]$").IsMatch(input.ToString()))
            {
                // then parse the input
                result = int.Parse(input.ToString());
                // if result is less than zero and is greater than option count then set result to zero so that we can go back through loop.
                if (result < 0 | result > maxNumber) result = 0;
            }
            if (result == 0) firstLoop = false;
            return result;
        }

        /// <summary>
        /// Clears console and then prints all ScreenRows to the console. the provided inputRequestMessage will be printed below the last newMessage line, with a space between the two.
        /// </summary>
        /// <param name="inputRequesetMessage"></param>
        static void PrintScreen(string inputRequesetMessage)
        {
            Console.Clear();
            for (int i = 0; i < ScreenRows.Count; i++)
            {
                // if it's the last cycle of string rows.
                if (i == ScreenRows.Count - 1)
                {
                    // print last row and add two new lines for spacing
                    Console.WriteLine(ScreenRows[i] + "\n");
                    // then print the provided string before where the user types.
                    Console.Write(inputRequesetMessage);
                }
                else
                {
                    // otherwise if it's not the last row being printed then just keep printing rows
                    Console.WriteLine(ScreenRows[i]);
                }
                Console.CursorVisible = true;
            }
        }

        /// <summary>
        /// Clear the console and then use Console.WriteLine for each string in ScreenRows
        /// </summary>
        static void PrintScreen()
        {
            Console.Clear();
            foreach (var message in ScreenRows)
            {
                Console.WriteLine(message);
            }
        }

        /// <summary>
        /// Add the provided string to the list of ScreenRows
        /// </summary>
        /// <param name="message"></param>
        public static void AddToRows(string message)
        {
            ScreenRows.Add(new StringBuilder(message));
        }

        /// <summary>
        /// Add the provided array of messages to the ScreenRows
        /// </summary>
        /// <param name="message"></param>
        public static void AddToRows(string[] message)
        {
            foreach (var item in message)
            {
                AddToRows(item);
            }
        }

        /// <summary>
        /// Remove x many rows from bottom of the screen (most recent messages) based on countToRemove provided. Rows are inserted at the top of the screen to make up for those being removed.
        /// </summary>
        /// <param name="countToRemove"></param>
        static void RemoveLastRows(int countToRemove)
        {
            ScreenRows.RemoveRange(ScreenRows.Count - countToRemove, countToRemove);
        }

        /// <summary>
        /// Clear the rows on the screen and then add rows for TopSpacing
        /// </summary>
        public static void ClearRows()
        {
            ScreenRows.Clear();
            AddToRows(new string[TopSpacing]);
        }

        /// <summary>
        /// Take in Console.ReadLine and if it is enter then return "Enter", otherwise return sring
        /// </summary>
        /// <param name="readLine"></param>
        static string ConvertIfEnter(string consoleReadLine)
        {
            return consoleReadLine == "\r" ? "Enter" : consoleReadLine;
        }

        /// <summary>
        /// Take in Console.ReadLine and if it is enter then return "Enter", otherwise return sring
        /// </summary>
        /// <param name="readLine"></param>
        static string ConvertIfEnter(char consoleReadKey)
        {
            return consoleReadKey.ToString() == ConsoleKey.Enter.ToString() ? "Enter" : consoleReadKey.ToString();
        }

        #endregion

    }

}
