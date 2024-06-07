using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;
using System.Threading;

namespace GameLogics
{
    public class Board
    {
        private int m_NumOfRows; // GameController also has these members... why both classes have them? 
        private int m_NumOfColumns;
        private Card[,] m_CardsMatrix;

        public Board(int i_NumOfRows, int i_NumOfColumns, int i_RangeOfIDs)
        {
            m_NumOfRows = i_NumOfRows;
            m_NumOfColumns = i_NumOfColumns;
            m_CardsMatrix = new Card[m_NumOfRows, m_NumOfColumns];


            BoardInitialization(i_RangeOfIDs); // Randomise letters (cards) 
        }

        public Card[,] Cards
        {
            get
            {
                return m_CardsMatrix;
            }
        }
        public int NumOfRows
        {
            get { return m_NumOfRows; }
            set { m_NumOfRows = value; }
        }

        public int NumOfColumns
        {
            get { return m_NumOfColumns; }
            set { m_NumOfColumns = value; }
        }


        public void BoardInitialization(int i_Range)
        {
            Random randomGenerator = new Random();
            int numOfCards = m_NumOfRows * m_NumOfColumns;
            int numOfUniqueId = numOfCards / 2; 

            List<int> idDomainList = new List<int>(i_Range);        
            List<int> idInGameList = new List<int>(numOfCards);

            for (int i = 0; i < i_Range; i++)
            {
                idDomainList.Add(i);
            }


            for (int i = 0; i < numOfUniqueId; i++)
            {
                int randomIndex = randomGenerator.Next(0, idDomainList.Count);
                int randomId = idDomainList[randomIndex];

                idInGameList.Add(randomId); // Add the ID twice to the game list
                idInGameList.Add(randomId);

                idDomainList.RemoveAt(randomIndex); // Remove the ID from the domain list
            }

            // shuffle the list
            ShuffleList(idInGameList);

            int cardId = 0;
            for (int row = 0; row < m_NumOfRows; row++)
            {
                for (int col = 0; col < m_NumOfColumns; col++)
                {
                    m_CardsMatrix[row, col] = new Card(idInGameList[cardId++]);
                }
            }
        }


        public static void ShuffleList<T>(List<T> i_List)
        {
            Random randomGenerator = new Random();
            int listSize = i_List.Count;
            while (listSize > 1) 
            {
                listSize--;
                int randomIndex = randomGenerator.Next(listSize + 1);
                T tempVariable = i_List[listSize];
                i_List[listSize] = i_List[randomIndex];
                i_List[randomIndex] = tempVariable; // FIX into swap() function

            }
        }
    }
}