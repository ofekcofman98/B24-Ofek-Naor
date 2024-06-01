using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameLogics
{
    public class Board
    {
        private int m_NumOfRows;
        private int m_NumOfColumns;
        private Card[,] m_CardsMatrix;

        public Board(int i_NumOfRows, int i_NumOfColumns)
        {
            m_NumOfRows = i_NumOfRows;
            m_NumOfColumns = i_NumOfColumns;
            m_CardsMatrix = new Card[m_NumOfRows, m_NumOfColumns];
            

            BoardInitialization(); // Randomise letters (cards) 
        }

        public Card[,] Card
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

        public void BoardInitialization()
        {
            Random rand = new Random();
            int numOfCards = m_NumOfRows * m_NumOfColumns;
            int numOfUniqueLetters = numOfCards / 2; // int for sure? even though we know numOfCards is even

            List<char> lettersList = new List<char>(26);        // define for 26
            List<char> lettersInGameList = new List<char>(numOfCards);


            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                lettersList.Add(letter);
            }

            int letterIndex;
            for (int i = 0; i < numOfUniqueLetters; i++)
            {
                letterIndex = rand.Next(0, lettersList.Count);
                lettersInGameList.Add(lettersList[letterIndex]); // efficient?
                lettersInGameList.Add(lettersList[letterIndex]); // efficient?
                lettersList.RemoveAt(letterIndex);               // efficient?
            }

            // shuffle the list

            letterIndex = 0;
            for (int row = 0; row < m_NumOfRows; row++)
            {
                for (int col = 0; col  < m_NumOfColumns; col ++)
                {
                    m_CardsMatrix[row, col] = new Card(lettersInGameList[letterIndex++]);
                }

            }
        }
    }
}
