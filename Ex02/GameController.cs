using GameLogics;
using GameInterface;

namespace GameControl
{
    public class GameController
    {
        private const int k_AddedPointsForMatchedCards = 1;

        private Player[] m_Players;
        private Board m_Board;
        private int m_WidthOfBoard;
        private int m_HeightOfBoard;
        private bool m_IsRoundOver;
        private int m_CurrentPlayerTurn;

        public void CreateNewRound(int i_NumOfRows, int i_NumOfColumns)
        {
            m_Board = new Board(i_NumOfRows, i_NumOfColumns); // Need to change into a regular method (Board CreateBoard(), Board BoardInitialization())
            m_HeightOfBoard = i_NumOfRows;
            m_WidthOfBoard = i_NumOfColumns;
            m_IsRoundOver = false;
        }

        public bool ValidateEvenAmountOfCards(int i_NumOfRows, int i_NumOfColumns)
        {
            return (i_NumOfColumns * i_NumOfRows) % 2 == 0;
        }

        public bool CheckIfCardRevealed(int i_RowChosen, int i_ColumnChosen)
        {
            return m_Board.Cards[i_RowChosen, i_ColumnChosen].IsFacedUp();
        }

        public bool AreCardsMatched(int i_RowChosen1, int i_ColumnChosen1, int i_RowChosen2, int i_ColumnChosen2)
        {
            return m_Board.Cards[i_RowChosen1, i_ColumnChosen1].ID
                   == m_Board.Cards[i_RowChosen2, i_ColumnChosen2].ID;
        }

        public void CheckForWinner()
        {
            int totalScoreOfAllPlayers = 0;

            foreach(Player player in Players)
            {
                totalScoreOfAllPlayers += player.Score;
            }

            m_IsRoundOver = totalScoreOfAllPlayers == (m_Board.NumOfColumns * m_Board.NumOfRows) / 2; // FIX TO "m_NumOfPairs"
        }

        public void FlipUpCard(int i_RowChosen, int i_ColumnChosen)
        {
            m_Board.Cards[i_RowChosen, i_ColumnChosen].FlipUp();
        }

        public void ExecuteSuccessfullMatch(int i_RowChosen1, int i_ColumnChosen1, int i_RowChosen2, int i_ColumnChosen2)
        {
            Board.Cards[i_RowChosen1, i_ColumnChosen1].RevealPermanently(); 
            Board.Cards[i_RowChosen2, i_ColumnChosen2].RevealPermanently();
            Players[m_CurrentPlayerTurn].Score += k_AddedPointsForMatchedCards; 
        }

        public void ExecuteFailedMatch(int i_RowChosen1, int i_ColumnChosen1, int i_RowChosen2, int i_ColumnChosen2)
        {
            Board.Cards[i_RowChosen1, i_ColumnChosen1].FlipDown();  
            Board.Cards[i_RowChosen2, i_ColumnChosen2].FlipDown();
            m_CurrentPlayerTurn = (m_CurrentPlayerTurn + 1) % Players.Length;
            // check if player[current] == computer
            // perform computer move
        }

        public Player[] Players
        {
            get
            {
                return m_Players;
            }
            set
            {
                m_Players = value;
            }
        }

        public bool IsRoundOver
        {
            get
            {
                return m_IsRoundOver;
            }
            set
            {
                m_IsRoundOver = value;
            }
        }

        public Board Board
        {
            get
            {
                return m_Board;
            }
        }

        public int CurrentPlayerTurn
        {
            get
            {
                return m_CurrentPlayerTurn;
            }
        }

    }
}
