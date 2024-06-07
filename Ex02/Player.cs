
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
        private List<CardsWithIndex> m_ComputerAiMemoryList; // delete "= null" ???? 

        public Player(string i_PlayerName, int i_Score, bool i_IsComputer) // ctor
        {
            m_PlayerName = i_PlayerName;
            m_Score = i_Score;
            m_IsComputer = i_IsComputer;
            m_ComputerAiMemoryList = i_IsComputer ? new List<CardsWithIndex>(26) : null;
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
    }
}
