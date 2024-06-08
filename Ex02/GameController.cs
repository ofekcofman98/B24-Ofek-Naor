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

        public bool IsCardChosenInBounds(int i_RowChosen, int i_ColumnChosen) 
        {
            return (i_RowChosen >= 0 && i_RowChosen < m_HeightOfBoard) && 
                   (i_ColumnChosen >= 0 && i_ColumnChosen < m_WidthOfBoard);
        }

        public bool CheckIfCardRevealed(int i_RowChosen, int i_ColumnChosen)
        {
            return m_Board.Cards[i_RowChosen, i_ColumnChosen].IsFacedUp();
        }

        public (int row1, int column1, int row2, int column2)? HandleTurn()
        {
            (int row1, int column1, int row2, int column2)? turnResult = null;

            if (Players[m_CurrentPlayerTurn].IsComputer)
            {
                (int row1, int column1, int row2, int column2) = generateComputerTurn();
                FlipUpCard(row1, column1);
                FlipUpCard(row2, column2);

                turnResult = (row1, column1, row2, column2);
            }

            return turnResult;
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

            int card1Id = Board.Cards[i_RowChosen1, i_ColumnChosen1].ID; // saving card's id for the computer to remember
            int card2Id = Board.Cards[i_RowChosen2, i_ColumnChosen2].ID; // saving card's id for the computer to remember

            foreach(Player player in Players)
            {
                if(player.IsComputer)
                {
                    player.RememberTurn(card1Id, i_RowChosen1, i_ColumnChosen1); 
                    player.RememberTurn(card2Id, i_RowChosen2, i_ColumnChosen2); 
                }
            }

            changePlayer();
        }

        private (int rorowChosen1w1, int columnChosen1, int rowChosen2, int columnChosen2) generateComputerTurn()
        {
            Random randomGenerator = new Random();
            int indexGenerated1 = randomGenerator.Next(m_FacedDownCardIndexList.Count);
            int cardIndex1 = m_FacedDownCardIndexList[indexGenerated1];
            int indexGenerated2;

            int rowChosen1 = findRowFromIndex(cardIndex1);
            int columnChosen1 = findColFromIndex(cardIndex1);
            CardsWithIndex cardToFind = new CardsWithIndex(m_Board.Cards[rowChosen1, columnChosen1].ID, rowChosen1, columnChosen1);
            
            if(Players[m_CurrentPlayerTurn].FindCardInMemory(cardToFind, out int rowChosen2, out int columnChosen2))
            {
                indexGenerated2 = findIndexFromChoosing(rowChosen2, columnChosen2);
            }
            else
            {
                do
                {
                    indexGenerated2 = randomGenerator.Next(m_FacedDownCardIndexList.Count);
                } while (indexGenerated1 == indexGenerated2);

                int cardIndex2 = m_FacedDownCardIndexList[indexGenerated2];
                rowChosen2 = findRowFromIndex(cardIndex2);
                columnChosen2 = findColFromIndex(cardIndex2);
            }

            return (rowChosen1, columnChosen1, rowChosen2, columnChosen2);
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
