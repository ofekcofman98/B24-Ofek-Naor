using System;
using GameLogics;
using Ex02.ConsoleUtils;
using GameControl;

namespace GameInterface
{
    public class GameView
    {
        private const int k_NumOfPlayers = 2;
        private const int k_AddedPointsForMatchedCards = 10;


        private GameController m_MemoryGame;
        public void StartGame()
        {
            welcomeMessage();

            bool continuePlaying = true;

            m_MemoryGame.Players = getPlayersDetails();

            while (continuePlaying)
            {
                getGameSettings();
                startNewRound(); // TO DO
                printGameResults();
                continuePlaying = checkIfPlayerWantsAnotherRound(); // TO DO

            }
        }

        private void getGameSettings()
        {
            getNumberOfRows(out int numOfRows);
            getNumberOfColumns(numOfRows, out int numOfColumns);
            m_MemoryGame.CreateNewRound(numOfRows, numOfColumns); // TO DO (create members for GameController etc)
        }

        private void startNewRound() 
        {
            while(!m_MemoryGame.IsRoundOver)
            {
                printPlayerTurn(); // TO DO
                displayBoard(m_MemoryGame.Board);
                getPlayerTurn(out int rowChosen1, out int columnChosen1, m_MemoryGame.Board.NumOfRows, m_MemoryGame.Board.NumOfColumns);
                m_MemoryGame.Board.Cards[rowChosen1, columnChosen1].FlipUp();
                displayBoard(m_MemoryGame.Board);

                getPlayerTurn(out int rowChosen2, out int columnChosen2, m_MemoryGame.Board.NumOfRows, m_MemoryGame.Board.NumOfColumns);
                m_MemoryGame.Board.Cards[rowChosen2, columnChosen2].FlipUp();
                displayBoard(m_MemoryGame.Board);

                tryMatchCards(rowChosen1, columnChosen1, rowChosen2, columnChosen2);
                m_MemoryGame.CheckForWinner();
            }
        }

        private void printPlayerTurn()
        {

        }

        private void tryMatchCards(int i_RowChosen1, int i_ColumnChosen1, int i_RowChosen2, int i_ColumnChosen2)
        {
            if(m_MemoryGame.AreCardsMatched(i_RowChosen1, i_ColumnChosen1, i_RowChosen2, i_RowChosen2))
            {
                Console.WriteLine($"Cards are Matched! 10 Points added to {m_MemoryGame.Players[m_MemoryGame.CurrentPlayerTurn % k_NumOfPlayers].Name}");
                m_MemoryGame.Board.Cards[i_RowChosen1, i_ColumnChosen1].RevealPermanently();
                m_MemoryGame.Board.Cards[i_RowChosen2, i_ColumnChosen2].RevealPermanently();
                m_MemoryGame.Players[m_MemoryGame.CurrentPlayerTurn % k_NumOfPlayers].Score += k_AddedPointsForMatchedCards;
            }
            else
            {
                Console.WriteLine("Cards are not matched!");
                m_MemoryGame.Board.Cards[i_RowChosen1, i_ColumnChosen1].FlipDown();
                m_MemoryGame.Board.Cards[i_RowChosen2, i_ColumnChosen2].FlipDown();
            }
        }

        private bool checkIfPlayerWantsAnotherRound()
        {

        }

        private void printGameResults()
        {

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

        private void getNumberOfRows(out int io_NumOfRows)
        {
            Console.Write("Enter number of rows (4 - 6): ");
            // Move dimensions check to logic
            while (!int.TryParse(Console.ReadLine(), out io_NumOfRows) || io_NumOfRows < 4 || io_NumOfRows > 6)
            {
                Console.Write("Invalid input. Enter number of rows (4 - 6): ");
            }
        }

        private int getNumberOfColumns(int i_NumOfRows, out int io_NumOfColumns)
        {
            Console.Write("Enter number of columns (4 - 6): ");
            // Move dimensions check to logic
            while (!int.TryParse(Console.ReadLine(), out io_NumOfColumns) || io_NumOfColumns < 4 || io_NumOfColumns > 6 || (io_NumOfColumns * i_NumOfRows) % 2 != 0)
            {
                Console.Write("Invalid input. Ensure the total number of cards is even and columns are between 4 and 6: ");
            }

            return io_NumOfColumns;
        }

        private void getPlayerTurn(out int o_RowChosen, out int o_ColumnChosen, int i_NumOfRows, int i_NumOfColumns)
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
                { // Move to logics 
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
                    else if(m_MemoryGame.CheckIfCardRevealed(rowChosen, columnChosen - 'A' + 1))
                    {
                        Console.WriteLine("Card is already revealed, please choose a card that has yet to be revealed.");
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

        private void displayBoard(Board i_Board)
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
