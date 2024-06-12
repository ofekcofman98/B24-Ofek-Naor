using System.Collections.Generic;

namespace GameLogics
{
    public class Player
    {
        private const int k_AmountOfLastTurnsRemembered = 4;
        private const int k_OldestItemIndexInMemoryList = 0;

        private string m_PlayerName;
        private int m_Score;
        private bool m_IsComputer;
        private List<CardsLocationOnBoard> m_ComputerAiMemoryList;

        public Player(string i_PlayerName, int i_Score, bool i_IsComputer) 
        {
            m_PlayerName = i_PlayerName;
            m_Score = i_Score;
            m_IsComputer = i_IsComputer;

            if (i_IsComputer)
            {
                m_ComputerAiMemoryList = new List<CardsLocationOnBoard>();
            }
            else
            {
                m_ComputerAiMemoryList = null;
            }
        }

        public void RememberTurn(int i_CardId, int i_Row, int i_Column)
        {
            if (m_ComputerAiMemoryList.Count == k_AmountOfLastTurnsRemembered)
            {
                m_ComputerAiMemoryList.RemoveAt(k_OldestItemIndexInMemoryList); // Remove the oldest item in memory
            }
            m_ComputerAiMemoryList.Add(new CardsLocationOnBoard(i_CardId, i_Row, i_Column));
        }

        public bool FindCardInMemory(CardsLocationOnBoard i_CardToFind, out int o_RowChosen, out int o_ColumnChosen)
        {
            o_RowChosen = -1;
            o_ColumnChosen = -1;
            bool isInList = false;

            foreach(CardsLocationOnBoard cardWithLocation in m_ComputerAiMemoryList)
            {
                if(cardWithLocation.Id == i_CardToFind.Id && (cardWithLocation.Row != i_CardToFind.Row || cardWithLocation.Column != i_CardToFind.Column))
                {
                    o_RowChosen = cardWithLocation.Row;
                    o_ColumnChosen = cardWithLocation.Column;
                    isInList = true;
                }
            }

            return isInList;
        }
        
        public string Name
        {
            get
            {
                return m_PlayerName;
            }
            set
            {
                m_PlayerName = value;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }
            set
            {
                m_Score = value;
            }
        }

        public bool IsComputer
        {
            get
            {
                return m_IsComputer;
            }
            set
            {
                m_IsComputer = value;
            }
        }
    }
}
