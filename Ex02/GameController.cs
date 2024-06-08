using GameLogics;
using GameInterface;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GameControl
{
    public class GameController
    {
        private const int k_AddedPointsForMatchedCards = 1;

        private Player[] m_Players;
        private Board m_Board;
        private int m_WidthOfBoard;
        private int m_HeightOfBoard;
        List<int> m_FacedDownCardIndexList; // for the computer to choose wisely
        private bool m_IsRoundOver;
        private int m_CurrentPlayerTurn;

        public void CreateNewRound(int i_NumOfRows, int i_NumOfColumns, int i_RangeOfIDs)
        {
            m_Board = new Board(i_NumOfRows, i_NumOfColumns, i_RangeOfIDs);
            m_HeightOfBoard = i_NumOfRows;
            m_WidthOfBoard = i_NumOfColumns;
            m_FacedDownCardIndexList = new List<int>(m_WidthOfBoard * m_HeightOfBoard);

            for (int i = 0; i < i_NumOfRows * i_NumOfColumns; i++)
            {
                m_FacedDownCardIndexList.Add(i);
            }

            foreach(Player player in Players)
            {
                player.Score = 0;
            }

            m_CurrentPlayerTurn = 0;
            m_IsRoundOver = false;
        }

        public bool ValidateEvenAmountOfCards(int i_NumOfRows, int i_NumOfColumns)
        {
            return (i_NumOfColumns * i_NumOfRows) % 2 == 0;
        }

        public bool IsCardChosenInBounds(int i_RowChosen, int i_ColumnChosen) // CHECK IF WORKS!
        {
            return (i_RowChosen >= 0 && i_RowChosen < m_HeightOfBoard) && 
                   (i_ColumnChosen >= 0 && i_ColumnChosen < m_WidthOfBoard);
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

        private void checkForWinner()
        {
            int totalScoreOfAllPlayers = 0;
            int numOfPairs = (m_Board.NumOfColumns * m_Board.NumOfRows) / 2;

            foreach (Player player in Players)
            {
                totalScoreOfAllPlayers += player.Score;
            }

            m_IsRoundOver = totalScoreOfAllPlayers == numOfPairs; 
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

            updateFacedDownCardIndexList(i_RowChosen1, i_ColumnChosen1, i_RowChosen2, i_ColumnChosen2);

            checkForWinner();
        }
        public void ExecuteFailedMatch(int i_RowChosen1, int i_ColumnChosen1, int i_RowChosen2, int i_ColumnChosen2)
        {
            Board.Cards[i_RowChosen1, i_ColumnChosen1].FlipDown();  
            Board.Cards[i_RowChosen2, i_ColumnChosen2].FlipDown();
            changePlayer();
            if (Players[m_CurrentPlayerTurn].IsComputer)
            {
                computerTurn();
            }
        }

        private void computerTurn()
        {
            // FIX Add meanings to names
            genrateComputerTurn(out int row1, out int col1, out int row2, out int col2);

            while(AreCardsMatched(row1, col1, row2, col2))
            {
                ExecuteSuccessfullMatch(row1, col1, row2, col2);
                if(!IsRoundOver)
                {
                    genrateComputerTurn(out row1, out col1, out row2, out col2);
                }
            }

            ExecuteFailedMatch(row1, col1, row2, col2);   
        }

        private void genrateComputerTurn(out int o_RowChosen1, out int o_ColumnChosen1, out int o_RowChosen2, out int o_ColumnChosen2)
        {
            Random randomGenerator = new Random();
            int index1 = randomGenerator.Next(m_FacedDownCardIndexList.Count);
            int index2;
            do
            {
                index2 = randomGenerator.Next(m_FacedDownCardIndexList.Count);
            } while (index1 == index2); // Move to function 

            o_RowChosen1 = findRowFromIndex(index1);
            o_ColumnChosen1 = findColFromIndex(index1);
            o_RowChosen2 = findRowFromIndex(index2);
            o_ColumnChosen2 = findColFromIndex(index2);
        }

        private void updateFacedDownCardIndexList(int i_RowChosen1, int i_ColumnChosen1, int i_RowChosen2, int i_ColumnChosen2)
        {
            int indexOfCard1 = findIndexFromChoosing(i_RowChosen1, i_ColumnChosen1);
            int indexOfCard2 = findIndexFromChoosing(i_RowChosen2, i_ColumnChosen2);
            m_FacedDownCardIndexList.Remove(indexOfCard1);
            m_FacedDownCardIndexList.Remove(indexOfCard2);
        }

        private int findIndexFromChoosing(int i_Row, int i_Column)
        {
            return i_Row * m_WidthOfBoard + i_Column;
        }

        private int findRowFromIndex(int i_BoardIndex)
        {
            return i_BoardIndex / m_WidthOfBoard;
        }

        private int findColFromIndex(int i_BoardIndex)
        {
            return i_BoardIndex % m_WidthOfBoard;
        }

        private void changePlayer()
        {
            m_CurrentPlayerTurn = (m_CurrentPlayerTurn + 1) % Players.Length;
        }

        public string GetCurrentPlayerName()
        {
            return m_Players[m_CurrentPlayerTurn].Name;
        }

        public Card GetCard(int i_Row, int i_Column)
        {
            return m_Board.Cards[i_Row, i_Column];
        }

        public void QuitRound()
        {
            m_IsRoundOver = true;
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
        }

        public Board Board
        {
            get
            {
                return m_Board;
            }
        }

        //public int CurrentPlayerTurn
        //{
        //    get
        //    {
        //        return m_CurrentPlayerTurn;
        //    }
        //}

        public int WidthOfBoard
        {
            get
            {
                return m_WidthOfBoard;
            }
        }

        public int HeightOfBoard
        {
            get
            {
                return m_HeightOfBoard;
            }
        }
    }
}
