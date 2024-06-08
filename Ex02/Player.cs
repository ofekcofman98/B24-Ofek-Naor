
using System;
using System.Collections;
using System.Collections.Generic;

namespace GameLogics
{
    public class Player
    {
        private string m_PlayerName;
        private int m_Score;
        private bool m_IsComputer;
        private List<CardsWithIndex> m_ComputerAiMemoryList;  // changed here 

        public Player(string i_PlayerName, int i_Score, bool i_IsComputer) // changed here 
        {
            m_PlayerName = i_PlayerName;
            m_Score = i_Score;
            m_IsComputer = i_IsComputer;
            if (i_IsComputer)
            {
                m_ComputerAiMemoryList = new List<CardsWithIndex>(26);
                for (int i = 0; i < 26; i++)
                {
                    m_ComputerAiMemoryList.Add(new CardsWithIndex(i));
                }
            }
            else
            {
                m_ComputerAiMemoryList = null; 
            }
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

        public List<CardsWithIndex> ComputerAiMemoryList // changed here
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

        public void RememberTurn(int cardId, int cardIndex) // changed here 
        {
            if (m_IsComputer)
            {
                CardsWithIndex cardInMemory = m_ComputerAiMemoryList[cardId];

                if (cardInMemory.ID == null)
                {
                    cardInMemory = new CardsWithIndex(cardId, cardIndex, null);
                }
                else
                {
                    cardInMemory.SetIndex(cardIndex);
                }

                m_ComputerAiMemoryList[cardId] = cardInMemory;
            }
        }
    }
}
