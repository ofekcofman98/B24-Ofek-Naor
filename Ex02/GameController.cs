using GameLogics;
using GameInterface;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Diagnostics.Eventing.Reader;

namespace GameControl
{
    public class GameController
    {
        private const int k_AddedPointsForMatchedCards = 1;
        private const int k_OpponentPlayerIndex = 1;

        private Player[] m_Players;
        private Board m_Board;
        private int m_WidthOfBoard;
        private int m_HeightOfBoard;
        List<int> m_FacedDownCardIndexList; // for the computer to choose wisely
        private bool m_IsRoundOver;
        //private bool m_AI;
        private int m_CurrentPlayerTurn;

        public void CreateNewRound(int i_NumOfRows, int i_NumOfColumns)
        {
            m_Board = new Board(i_NumOfRows, i_NumOfColumns); // Need to change into a regular method (Board CreateBoard(), Board BoardInitialization())
            m_HeightOfBoard = i_NumOfRows;
            m_WidthOfBoard = i_NumOfColumns;
            m_FacedDownCardIndexList = new List<int>(m_WidthOfBoard * m_HeightOfBoard);
            for (int i = 0; i < i_NumOfRows * i_NumOfColumns; i++)
            {
                m_FacedDownCardIndexList.Add(i);
            }
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
            int indexOfCard1 = FindIndexFromChoosing(i_RowChosen1, i_ColumnChosen1);
            int indexOfCard2 = FindIndexFromChoosing(i_RowChosen2, i_ColumnChosen2);
            m_FacedDownCardIndexList.Remove(indexOfCard1);
            m_FacedDownCardIndexList.Remove(indexOfCard2);
        }
        public void ExecuteFailedMatch(int i_RowChosen1, int i_ColumnChosen1, int i_RowChosen2, int i_ColumnChosen2)
        {
            Board.Cards[i_RowChosen1, i_ColumnChosen1].FlipDown();
            Board.Cards[i_RowChosen2, i_ColumnChosen2].FlipDown();
            ChangePlayer();
            if (Players[m_CurrentPlayerTurn].IsComputer)
            {
                ComputerTurn();
            }
            int card1Id = Board.Cards[i_RowChosen1, i_ColumnChosen1].ID;
            int card2Id = Board.Cards[i_RowChosen2, i_ColumnChosen2].ID;
            int indexOfCard1 = FindIndexFromChoosing(i_RowChosen1, i_ColumnChosen1);
            int indexOfCard2 = FindIndexFromChoosing(i_RowChosen2, i_ColumnChosen2);

            if (Players[k_OpponentPlayerIndex].IsComputer)
            {
                ComputerRememberTurn(card1Id, indexOfCard1);
                ComputerRememberTurn(card2Id, indexOfCard2);
            }
            // check if player[current] == computer:
            //      remember those cards 
            //      computer turn 
            // perform computer move
        }

        public void ComputerRememberTurn(int cardId, int i_IndexOfCard)
        {
            int n = Players[k_OpponentPlayerIndex].ComputerAiMemoryList[cardId].Index1 ?? i_IndexOfCard;

            //if (IsCardIndexInAiMemory(cardId, out int o_CardIndex))
            //{
                ////Players[k_OpponentPlayerIndex].ComputerAiMemoryList[0]
            //}
            //else
            //{
                //CardsWithIndex = new CardsWithIndex(cardId,in)
                //Players[k_OpponentPlayerIndex].ComputerAiMemoryList.Add()
            //}
        }

        public bool IsCardIndexInAiMemory(int i_CardId, out int o_CardIndex)
        {
            //bool v_IsCardIndexInList = false;
            //o_CardIndex = -1;
            //for (int i = 0; i < Players[k_OpponentPlayerIndex].ComputerAiMemoryList.Count; i++)
            //{
            //    if (Players[k_OpponentPlayerIndex].ComputerAiMemoryList[i].ID == i_CardId)
            //    {
            //        v_IsCardIndexInList = true;
            //        o_CardIndex = i;
            //        break;
            //    }
            //}
            //return v_IsCardIndexInList;
        }

        public int FindIndexFromChoosing(int i_Row, int i_Col)
        {
            return i_Row * m_WidthOfBoard + i_Col;
        }


        public void ComputerTurn()
        {
            Random randomGenerator = new Random();
            int index1 = randomGenerator.Next(m_FacedDownCardIndexList.Count);
            //if (IsCardIndexInAiMemory())
            //{

            //}
            int index2;
            do
            {
                index2 = randomGenerator.Next(m_FacedDownCardIndexList.Count);
            } while (index1 == index2); // is it legel? random inside a while loop?
            // maybe remove index1 and then add back?
            int row1 = FindRowFromIndex(index1);
            int col1 = FindColFromIndex(index1);
            int row2 = FindRowFromIndex(index2);
            int col2 = FindColFromIndex(index2);
            if (AreCardsMatched(row1, col1, row2, col2))
            {
                ExecuteSuccessfullMatch(row1, col1, row2, col2);
            }
            else
            {
                ExecuteFailedMatch(row1, col1, row2, col2);
            }
        }
        public int FindRowFromIndex(int index)
        {
            return index / m_WidthOfBoard;
        }
        public int FindColFromIndex(int index)
        {
            return index % m_WidthOfBoard;
        }
        public void ChangePlayer()
        {
            m_CurrentPlayerTurn = (m_CurrentPlayerTurn + 1) % Players.Length;
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
