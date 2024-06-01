using System;
using GameLogics;
using Ex02.ConsoleUtils;

namespace GameInterface
{
    public class GameView
    {
        private const int k_NumOfPlayers = 2;

        public void WelcomeMessage()
        {
            Console.WriteLine("\n                             Welcome To The ");
            Console.WriteLine("\r\n  __  __                                    ____                        _ \r\n |  \\/  | ___ _ __ ___   ___  _ __ _   _   / ___| __ _ _ __ ___   ___  | |\r\n | |\\/| |/ _ \\ '_ ` _ \\ / _ \\| '__| | | | | |  _ / _` | '_ ` _ \\ / _ \\ | |\r\n | |  | |  __/ | | | | | (_) | |  | |_| | | |_| | (_| | | | | | |  __/ |_|\r\n |_|  |_|\\___|_| |_| |_|\\___/|_|   \\__, |  \\____|\\__,_|_| |_| |_|\\___| (_)\r\n                                   |___/                                  \r\n");
            Console.WriteLine();
            Console.Write("                        Press Any Key To Continue ...");
            Console.ReadKey(true); 
            Screen.Clear();
        }


        public Player[] GetPlayersDetails()
        {
            Player[] playersArray = new Player[k_NumOfPlayers];

            Console.WriteLine("Please enter the player's name:");
            playersArray[0] = new Player();
            playersArray[0].Name = Console.ReadLine();

            Console.WriteLine("Do you wish to play against the computer? click (y)/(n)");
            string userChoice = Console.ReadLine().ToLower();
            while (userChoice != "y" && userChoice != "n")
            {
                Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                userChoice = Console.ReadLine().ToLower();
            }

            playersArray[1] = new Player();
            if (userChoice == "n")
            {
                Console.WriteLine("Please enter 2nd player's name:");
                playersArray[1].Name = Console.ReadLine();
            }
            else
            {
                playersArray[1].Name = "Computer";
                playersArray[1].IsComputer = true;
            }

            return playersArray;
        }

        public int GetNumberOfRows()
        {
            int numOfRows;
            Console.Write("Enter number of rows (4 or 6): ");

            while (!int.TryParse(Console.ReadLine(), out numOfRows) || (numOfRows != 4 && numOfRows != 6))
            {
                Console.Write("Invalid input. Enter number of rows: ");
            }

            return numOfRows;
        }

        public int GetNumberOfColumns()
        {
            int numOfColumns;
            Console.Write("Enter number of columns (4 or 6): ");

            while (!int.TryParse(Console.ReadLine(), out numOfColumns) || (numOfColumns != 4 && numOfColumns != 6))
            {
                Console.Write("Invalid input. Enter number of columns: ");
            }

            return numOfColumns;
        }

        public void GetPlayerTurn(ref int o_RowChosen, ref int o_ColumnChosen, int i_NumOfRows, int i_NumOfColumns)
        {
            Console.Write("Choose a card (e.g., A1 or E3) or press Q to quit: ");
            string playerInput;
            char columnChosen;
            int rowChosen;

            while (true)
            {
                playerInput = Console.ReadLine().ToUpper();

                if (playerInput == "Q")
                {
                    Environment.Exit(0); // Exit the program
                }

                // Check bounds
                if (playerInput.Length == 2 && char.IsLetter(playerInput[0]) && char.IsDigit(playerInput[1])
                    && (columnChosen = playerInput[0]) >= 'A' && columnChosen < 'A' + i_NumOfColumns
                    && int.TryParse(playerInput[1].ToString(), out rowChosen) && rowChosen >= 1 && rowChosen <= i_NumOfRows)
                {
                    o_RowChosen = rowChosen - 1;
                    o_ColumnChosen = columnChosen - 'A';
                    break;
                }

                Console.Write($"Invalid input. Choose a card (e.g., A1 or E3, within A-{(char)('A' + i_NumOfColumns - 1)} and 1-{i_NumOfRows}) or press Q to quit: ");
            }
        }

        public void DisplayBoard(Board i_Board)
        {
            // Display column headers
            Console.Write("   ");
            
            for (int col = 0; col < i_Board.NumOfColumns; col++)
            {
                Console.Write($"  {(char)('A' + col)} ");
            }

            Console.WriteLine();
            Console.Write("  ");
            for (int col = 0; col < i_Board.NumOfColumns; col++)
            {
                Console.Write("====");
            }

            Console.WriteLine("=");

            for (int row = 0; row < i_Board.NumOfRows; row++)
            {
                // Display row number
                Console.Write($"{row + 1} |");

                for (int col = 0; col < i_Board.NumOfColumns; col++)
                {
                    Card cardToPrint = i_Board.Cards[row, col];
                    if (cardToPrint.CardStatus == eCardStatus.CurrentlyFacedUp || cardToPrint.CardStatus == eCardStatus.PermanentlyFacedUp)
                    {
                        Console.Write($" {cardToPrint.Letter} |");
                    }
                    else
                    {
                        Console.Write("   |");
                    }
                }

                Console.WriteLine();
                Console.Write("  ");

                for (int col = 0; col < i_Board.NumOfColumns; col++)
                {
                    Console.Write("====");
                }

                Console.WriteLine("=");
            }
        }
    }
}
