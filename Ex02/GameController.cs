using GameLogics;
using GameInterface;

namespace GameControl
{
    public class GameController
    {
        private Player[] m_Players;
        private Board m_Board;
        private bool m_IsRoundOver;
        private int m_CurrentPlayerTurn;

        public void CreateNewRound(int i_NumOfRows, int i_NumOfColumns)
        {
            m_Board = new Board(i_NumOfRows, i_NumOfColumns); // Need to change into a regular method (Board CreateBoard(), Board BoardInitialization())
            m_IsRoundOver = false;
        }

        public bool CheckIfCardRevealed(int i_RowChosen, int i_ColumnChosen)
        {
            return m_Board.Cards[i_RowChosen, i_ColumnChosen].CardStatus == eCardStatus.PermanentlyFacedUp
                      || m_Board.Cards[i_RowChosen, i_ColumnChosen].CardStatus == eCardStatus.CurrentlyFacedUp;
        }

        public bool AreCardsMatched(int i_RowChosen1, int i_ColumnChosen1, int i_RowChosen2, int i_ColumnChosen2)
        {
            return m_Board.Cards[i_RowChosen1, i_ColumnChosen1].Letter
                   == m_Board.Cards[i_RowChosen2, i_ColumnChosen2].Letter;
        }

        public void CheckForWinner()
        {
            
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
