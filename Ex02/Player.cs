
using System.Collections.Generic;

namespace GameLogics
{
    public class Player
    {
        private string m_PlayerName;
        private int m_Score;
        private bool m_IsComputer;
        private List<CardsWithIndex> m_ComputerAiMemoryList;

        public Player(string i_PlayerName, int i_Score, bool i_IsComputer, int i_RangeOfIDs) 
        {
            m_PlayerName = i_PlayerName;
            m_Score = i_Score;
            m_IsComputer = i_IsComputer;
            if (i_IsComputer)
            {
                m_ComputerAiMemoryList = new List<CardsWithIndex>(i_RangeOfIDs);
                for (int i = 0; i < i_RangeOfIDs; i++)
                {
                    m_ComputerAiMemoryList.Add(new CardsWithIndex(i));
                }
            }
            else
            {
                m_ComputerAiMemoryList = null;
            }
        }

        public List<CardsWithIndex> ComputerAiMemoryList 
        {
            get
            {
                return m_ComputerAiMemoryList;
            }
            set
            {
                m_ComputerAiMemoryList = value;
            }
        }

        public void RememberTurn(int i_CardId, int i_CardIndex)  
        {
            CardsWithIndex cardInMemory = m_ComputerAiMemoryList[i_CardId];

            if (cardInMemory.ID == null)
            {
                cardInMemory = new CardsWithIndex(i_CardId, i_CardIndex, null);
            }
            else
            {
                cardInMemory.SetIndex(i_CardIndex);
            }

            m_ComputerAiMemoryList[i_CardId] = cardInMemory;
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
