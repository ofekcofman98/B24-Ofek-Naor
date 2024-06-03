using System;
using GameLogics;
using Ex02.ConsoleUtils;
using System.Collections.Generic;

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
            Console.Write("                       Press Any Key To Continue ...");
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
            Console.Write("Enter number of rows (4 - 6): ");

            while (!int.TryParse(Console.ReadLine(), out numOfRows) || numOfRows < 4 || numOfRows > 6)
            {
                Console.Write("Invalid input. Enter number of rows (4 - 6): ");
            }

            return numOfRows;
        }

        public int GetNumberOfColumns(int i_NumOfRows)
        {
            int numOfColumns;
            Console.Write("Enter number of columns (4 - 6): ");

            while (!int.TryParse(Console.ReadLine(), out numOfColumns) || numOfColumns < 4 || numOfColumns > 6 || (numOfColumns * i_NumOfRows) % 2 != 0)
            {
                Console.Write("Invalid input. Ensure the total number of cards is even and columns are between 4 and 6: ");
            }

            return numOfColumns;
        }

        public void GetPlayerTurn(ref int o_RowChosen, ref int o_ColumnChosen, int i_NumOfRows, int i_NumOfColumns)
        {
            Console.Write("Choose a card (e.g., A1 or E3) or press Q to quit: ");

            while (true)
            {
                string playerInput = Console.ReadLine().ToUpper();

                if (playerInput == "Q")
                {
                    Environment.Exit(0); 
                }

                if (checkTurnInputValid(playerInput, i_NumOfRows, i_NumOfColumns, out o_RowChosen, out o_ColumnChosen))
                {
                    break;
                }

                Console.Write($"Choose a card (e.g., A1 or E3, within A-{(char)('A' + i_NumOfColumns - 1)} and 1-{i_NumOfRows}) or press Q to quit: ");
            }
        }

        private bool checkTurnInputValid(string i_PlayerInput, int i_NumOfRows, int i_NumOfColumns, out int o_RowChosen, out int o_ColumnChosen)
        {
            o_RowChosen = -1;
            o_ColumnChosen = -1;
            bool isValid = true;

            if (i_PlayerInput.Length != 2)
            {
                Console.WriteLine("Invalid input length. Please enter in the format ColRow (e.g., A1).");
                isValid = false;
            }
            else
            {
                char columnChosen = i_PlayerInput[0];
                char rowChar = i_PlayerInput[1];

                if (!char.IsLetter(columnChosen))
                {
                    Console.WriteLine("Invalid column. The first character must be a letter.");
                    isValid = false;
                }
                else if (!char.IsDigit(rowChar))
                {
                    Console.WriteLine("Invalid row. The second character must be a digit.");
                    isValid = false;
                }
                else
                {
                    if (!int.TryParse(rowChar.ToString(), out int rowChosen) || rowChosen < 1 || rowChosen > i_NumOfRows)
                    {
                        Console.WriteLine($"Invalid row. The row must be within 1-{i_NumOfRows}.");
                        isValid = false;
                    }
                    else if (columnChosen < 'A' || columnChosen >= 'A' + i_NumOfColumns)
                    {
                        Console.WriteLine($"Invalid column. The column must be within A-{(char)('A' + i_NumOfColumns - 1)}.");
                        isValid = false;
                    }
                    else
                    {
                        o_RowChosen = rowChosen - 1;
                        o_ColumnChosen = columnChosen - 'A';
                    }
                }
            }

            return isValid;
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
                    if (cardToPrint.CardStatus == eCardStatus.CurrentlyFacedUp || cardToPrint.CardStatus == eCardStatus.PermanentlyFacedUp || cardToPrint.CardStatus == eCardStatus.FacedDown)
                    {
                        //Console.Write($" {cardToPrint.id} |");
                        char displayChar = (char)('A' + cardToPrint.id);
                        Console.Write($" {displayChar} |");

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
