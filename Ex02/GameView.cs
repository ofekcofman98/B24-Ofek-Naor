using System;
using GameLogics;
using Ex02.ConsoleUtils;
using GameControl;
using System.Threading;

namespace GameInterface
{
    public class GameView
    {
        private const int k_NumOfPlayers = 2;
        private const int k_MaximumBoardSize = 6;
        private const int k_MinimumBoardSize = 4;
        private const int k_InputLength = 2;
        private const int k_AmountOfLetters = 26;
        private const int k_DefaultValueForOutIntVariables = -1;

        private GameController m_MemoryGame;

        public GameView()
        {
            m_MemoryGame = new GameController();
        }

        public void StartGame()
        {
            printWelcomeMessage();

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
                displayBoard();

                (int rowChosen1, int columnChosen1, int rowChosen2, int columnChosen2)? turnResult = m_MemoryGame.HandleTurn();

                if(turnResult.HasValue)
                {
                    displayBoard();
                    tryMatchCards(turnResult.Value.rowChosen1, turnResult.Value.columnChosen1, turnResult.Value.rowChosen2, turnResult.Value.columnChosen2);
                }
                else
                {
                    handleHumanTurn();
                }
            }
        }

        private void handleHumanTurn()
        {
            getPlayerTurn(out int rowChosen1, out int columnChosen1, out bool isQuitGame);

            if (!isQuitGame)
            {
                m_MemoryGame.FlipUpCard(rowChosen1, columnChosen1);
                displayBoard();

                getPlayerTurn(out int rowChosen2, out int columnChosen2, out isQuitGame);

                if (!isQuitGame)
                {
                    m_MemoryGame.FlipUpCard(rowChosen2, columnChosen2);
                    displayBoard();

                    tryMatchCards(rowChosen1, columnChosen1, rowChosen2, columnChosen2);
                }
            }
        }

        private void getPlayerTurn(out int o_RowChosen, out int o_ColumnChosen, out bool o_QuitGame)
        {
            o_RowChosen = k_DefaultValueForOutIntVariables;
            o_ColumnChosen = k_DefaultValueForOutIntVariables;
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

                Console.Write($"Choose a card (e.g., A1 or C3, within A-{(char)('A' + m_MemoryGame.GetWidthOfBoard() - 1)} and"
                              + $" 1-{m_MemoryGame.GetHeightOfBoard()}) or press Q to quit: ");
            }
        }

        private bool checkTurnInputValid(string i_PlayerInput, out int o_RowChosen, out int o_ColumnChosen)
        {
            o_RowChosen = k_DefaultValueForOutIntVariables;
            o_ColumnChosen = k_DefaultValueForOutIntVariables;
            bool isValid = false;


            if (i_PlayerInput.Length != k_InputLength)
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

                        Console.WriteLine($"Invalid input. The row must be within 1-{m_MemoryGame.GetHeightOfBoard()},"
                                          + $" and column must be within A-{(char)('A' + m_MemoryGame.GetWidthOfBoard() - 1)}.");
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

            string player1Score = string.Format("{0}: {1} points", player1.Name, player1.Score);
            string player2Score = string.Format("{0}: {1} points", player2.Name, player2.Score);
            Console.WriteLine(player1Score);
            Console.WriteLine(player2Score);

            string resultMessage;

            if (player1.Score > player2.Score)
            {
                resultMessage = string.Format("\nThe winner is {0} with {1} points!", player1.Name, player1.Score);
            }
            else if (player2.Score > player1.Score)
            {
                resultMessage = string.Format("\nThe winner is {0} with {1} points!", player2.Name, player2.Score);
            }
            else
            {
                resultMessage = "\nIt's a tie!";
            }

            Console.WriteLine(resultMessage);
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Screen.Clear();
        }

        private void printWelcomeMessage()
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
            string playerName1 = Console.ReadLine();

            playersArray[0] = new Player(playerName1, GameController.k_InitialScoreForPlayers, false);

            Console.Write("Do you wish to play against the computer? click (y)/(n): ");
            string userChoice = Console.ReadLine().ToLower();

            while (userChoice != "y" && userChoice != "n")
            {
                Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                userChoice = Console.ReadLine().ToLower();
            }

            if (userChoice == "n")
            {
                Console.WriteLine("Please enter 2nd player's name:");
                string playerName2 = Console.ReadLine();
                playersArray[1] = new Player(playerName2, GameController.k_InitialScoreForPlayers, false);
            }
            else
            {
                playersArray[1] = new Player("Computer", GameController.k_InitialScoreForPlayers, true);
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
            Screen.Clear();
            printPlayerTurn();
            
            Console.Write("   ");
            
            for (int column = 0; column < m_MemoryGame.GetWidthOfBoard(); column++)
            {
                Console.Write($"  {(char)('A' + column)} ");
            }

            Console.WriteLine();
            Console.Write("  ");
            for (int column = 0; column < m_MemoryGame.GetWidthOfBoard(); column++)
            {
                Console.Write("====");
            }

            Console.WriteLine("=");

            for (int row = 0; row < m_MemoryGame.GetHeightOfBoard(); row++)
            {
                
                Console.Write($"{row + 1} |");

                for (int column = 0; column < m_MemoryGame.GetWidthOfBoard(); column++)
                {

                    Card cardToPrint = m_MemoryGame.GetCard(row, column);

                    if (cardToPrint.IsFacedUp())
                    {
                        char displayChar = (char)('A' + cardToPrint.Id);
                        Console.Write($" {displayChar} |");
                    }
                    else
                    {
                        Console.Write("   |");
                    }
                }

                Console.WriteLine();
                Console.Write("  ");

                for (int column = 0; column < m_MemoryGame.GetWidthOfBoard(); column++)
                {
                    Console.Write("====");
                }

                Console.WriteLine("=");
            }
            Console.WriteLine();
        }
    }
}