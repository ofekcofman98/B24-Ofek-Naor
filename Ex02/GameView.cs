using System;
using GameLogics;
using Ex02.ConsoleUtils;
using GameControl;
using System.Threading;
using System.Collections.Generic;

namespace GameInterface
{
    public class GameView
    {
        private const int k_NumOfPlayers = 2;
        private const int k_MaximumBoardSize = 6;
        private const int k_MinimumBoardSize = 4;
        private const int k_AmountOfLetters = 26;

        private GameController m_MemoryGame;

        public GameView()
        {
            m_MemoryGame = new GameController();
        }

        public void StartGame()
        {
            welcomeMessage();

            bool continuePlaying = true;

            m_MemoryGame.Players = getPlayersDetails();

            while (continuePlaying)
            {
                getGameSettings(); 
                startNewRound(); 
                printGameResults();
                continuePlaying = checkIfPlayerWantsAnotherRound();
            }
        }

        private void getGameSettings()
        {
            getNumberOfRows(out int numOfRows);
            getNumberOfColumns(numOfRows, out int numOfColumns);
            m_MemoryGame.CreateNewRound(numOfRows, numOfColumns, k_AmountOfLetters); 
        }

        private void startNewRound() 
        {

            while(!m_MemoryGame.IsRoundOver)
            {
                // code is duplicated
                
                Screen.Clear();
                displayBoard();

                getPlayerTurn(out int rowChosen1, out int columnChosen1, out bool isQuitGame);
                if(isQuitGame)
                {
                    break;
                }

                m_MemoryGame.FlipUpCard(rowChosen1, columnChosen1);
                Screen.Clear(); 
                displayBoard();

                getPlayerTurn(out int rowChosen2, out int columnChosen2, out isQuitGame);
                if(isQuitGame)
                {
                    break;
                }

                m_MemoryGame.FlipUpCard(rowChosen2, columnChosen2);
                Screen.Clear();
                displayBoard();

                tryMatchCards(rowChosen1, columnChosen1, rowChosen2, columnChosen2);
            }
        }

        private void getPlayerTurn(out int o_RowChosen, out int o_ColumnChosen, out bool o_QuitGame)
        {
            o_RowChosen = -1;
            o_ColumnChosen = -1;
            o_QuitGame = false;

            Console.Write("Choose a card (e.g., A1 or C3) or press Q to quit: ");

            while (true)
            {
                string playerInput = Console.ReadLine().ToUpper();

                if (playerInput == "Q")
                {
                    o_QuitGame = true;
                    m_MemoryGame.QuitRound();
                    break;
                }

                if (checkTurnInputValid(playerInput, out o_RowChosen, out o_ColumnChosen))
                {
                    break;
                }

                Console.Write($"Choose a card (e.g., A1 or C3, within A-{(char)('A' + m_MemoryGame.WidthOfBoard - 1)} and"
                              + $" 1-{m_MemoryGame.HeightOfBoard}) or press Q to quit: ");
            }
        }

        private bool checkTurnInputValid(string i_PlayerInput, out int o_RowChosen, out int o_ColumnChosen)
        {
            o_RowChosen = -1;
            o_ColumnChosen = -1;
            bool isValid = false;


            if (i_PlayerInput.Length != 2)
            {
                Console.WriteLine("Invalid input length. Please enter in the format ColRow (e.g., A1).");
            }
            else
            {
                char columnChar = i_PlayerInput[0];
                char rowChar = i_PlayerInput[1];

                if (!char.IsLetter(columnChar))
                {
                    Console.WriteLine("Invalid column. The first character must be a letter.");
                }

                else if (!char.IsDigit(rowChar))
                {
                    Console.WriteLine("Invalid row. The second character must be a digit.");
                }
                else
                {
                    
                    if (!int.TryParse(rowChar.ToString(), out int rowChosen) || !m_MemoryGame.IsCardChosenInBounds(rowChosen - 1, columnChar - 'A'))
                    {

                        Console.WriteLine($"Invalid input. The row must be within 1-{m_MemoryGame.HeightOfBoard},"
                                          + $" and column must be within A-{(char)('A' + m_MemoryGame.WidthOfBoard - 1)}.");
                    }
                    else if (m_MemoryGame.CheckIfCardRevealed(rowChosen - 1, columnChar - 'A'))
                    {
                        Console.WriteLine("Card is already revealed, please choose a card that has yet to be revealed.");
                    }
                    else
                    {
                        o_RowChosen = rowChosen - 1;
                        o_ColumnChosen = columnChar - 'A';
                        isValid = true;
                    }
                }
            }

            return isValid;
        }

        private void tryMatchCards(int i_RowChosen1, int i_ColumnChosen1, int i_RowChosen2, int i_ColumnChosen2)
        {
            if (m_MemoryGame.AreCardsMatched(i_RowChosen1, i_ColumnChosen1, i_RowChosen2, i_ColumnChosen2))
            {
                Console.WriteLine($"Cards are Matched! 1 Points added to {m_MemoryGame.GetCurrentPlayerName()}");
                m_MemoryGame.ExecuteSuccessfullMatch(i_RowChosen1, i_ColumnChosen1, i_RowChosen2, i_ColumnChosen2);
            }
            else
            {
                Console.WriteLine("Cards are not matched!");
                m_MemoryGame.ExecuteFailedMatch(i_RowChosen1, i_ColumnChosen1, i_RowChosen2, i_ColumnChosen2);
            }
            Thread.Sleep(2000);
        }

        private void printPlayerTurn()
        {
            Console.WriteLine($"Now it is {m_MemoryGame.GetCurrentPlayerName()}'s turn: \n");
        }

        private bool checkIfPlayerWantsAnotherRound()
        {
            Screen.Clear();
            Console.Write("Do you wish to play another round? click (y/n): ");
            string userChoice = Console.ReadLine().ToLower();
            while (userChoice != "y" && userChoice != "n")
            {
                Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                userChoice = Console.ReadLine().ToLower();
            }

            return userChoice == "y";
        }

        private void printGameResults()
        {
            Screen.Clear();
            Console.WriteLine("Game Over!");
            Console.WriteLine("Final Scores:");

            Player[] playerStatistics = m_MemoryGame.Players;
            Player player1 = playerStatistics[0];
            Player player2 = playerStatistics[1];

            Console.WriteLine($"{player1.Name}: {player1.Score} points");
            Console.WriteLine($"{player2.Name}: {player2.Score} points");

            // Determine the winner
            if (player1.Score > player2.Score)
            {
                Console.WriteLine($"\nThe winner is {player1.Name} with {player1.Score} points!");
            }
            else if (player2.Score > player1.Score)
            {
                Console.WriteLine($"\nThe winner is {player2.Name} with {player2.Score} points!");
            }
            else
            {
                Console.WriteLine("\nIt's a tie!");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Screen.Clear();
        }


        private void welcomeMessage()
        {
            Console.WriteLine("\n                             Welcome To The ");
            Console.WriteLine("\r\n  __  __                                    ____                        _ \r\n |  \\/  | ___ _ __ ___   ___  _ __ _   _   / ___| __ _ _ __ ___   ___  | |\r\n | |\\/| |/ _ \\ '_ ` _ \\ / _ \\| '__| | | | | |  _ / _` | '_ ` _ \\ / _ \\ | |\r\n | |  | |  __/ | | | | | (_) | |  | |_| | | |_| | (_| | | | | | |  __/ |_|\r\n |_|  |_|\\___|_| |_| |_|\\___/|_|   \\__, |  \\____|\\__,_|_| |_| |_|\\___| (_)\r\n                                   |___/                                  \r\n");
            Console.WriteLine();
            Console.Write("                       Press Any Key To Continue ...");
            Console.ReadKey(true); 
            Screen.Clear();
        }

        private Player[] getPlayersDetails()
        {
            Player[] playersArray = new Player[k_NumOfPlayers];

            Console.WriteLine("Please enter the player's name:");
            playersArray[0] = new Player();
            playersArray[0].Name = Console.ReadLine();

            Console.Write("Do you wish to play against the computer? click (y)/(n): ");
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

        private void getNumberOfRows(out int io_NumOfRows)
        {
            Console.Write($"Enter number of rows ({k_MinimumBoardSize} - {k_MaximumBoardSize}): ");

            while (!int.TryParse(Console.ReadLine(), out io_NumOfRows) || !validateDimensionsInBound(io_NumOfRows))
            {
                Console.Write($"Invalid input. Enter number of rows ({k_MinimumBoardSize} - {k_MaximumBoardSize}): ");
            }
        }

        private void getNumberOfColumns(int i_NumOfRows, out int io_NumOfColumns) 
        {
            Console.Write($"Enter number of columns ({k_MinimumBoardSize} - {k_MaximumBoardSize}): ");

            while (!int.TryParse(Console.ReadLine(), out io_NumOfColumns) || !validateDimensionsInBound(io_NumOfColumns) 
                   || !m_MemoryGame.ValidateEvenAmountOfCards(i_NumOfRows, io_NumOfColumns))
            {
                Console.Write($"Invalid input. Ensure the total number of cards is even and columns are between {k_MinimumBoardSize} and {k_MaximumBoardSize}: ");
            }
        }

        private bool validateDimensionsInBound(int i_Dimension)
        {
            return i_Dimension >= k_MinimumBoardSize && i_Dimension <= k_MaximumBoardSize;
        }

        private void displayBoard()
        {
            printPlayerTurn();
            
            Console.Write("   ");
            
            for (int col = 0; col < m_MemoryGame.WidthOfBoard; col++)
            {
                Console.Write($"  {(char)('A' + col)} ");
            }

            Console.WriteLine();
            Console.Write("  ");
            for (int col = 0; col < m_MemoryGame.WidthOfBoard; col++)
            {
                Console.Write("====");
            }

            Console.WriteLine("=");

            for (int row = 0; row < m_MemoryGame.HeightOfBoard; row++)
            {
                
                Console.Write($"{row + 1} |");

                for (int col = 0; col < m_MemoryGame.WidthOfBoard; col++)
                {

                    Card cardToPrint = m_MemoryGame.GetCard(row, col);

                    if (cardToPrint.IsFacedUp())
                    {
                        char displayChar = (char)('A' + cardToPrint.ID);
                        Console.Write($" {displayChar} |");
                    }
                    else
                    {
                        Console.Write("   |");
                    }
                }

                Console.WriteLine();
                Console.Write("  ");

                for (int col = 0; col < m_MemoryGame.WidthOfBoard; col++)
                {
                    Console.Write("====");
                }

                Console.WriteLine("=");
            }
            Console.WriteLine();
        }
    }
}
