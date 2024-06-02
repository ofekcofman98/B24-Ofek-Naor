using GameInterface;
using GameLogics;
﻿using GameLogics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class Program
    {
        public static void Main()
        {
            GameView game = new GameView();
            game.WelcomeMessage();
            game.GetPlayersDetails();
            int rows = game.GetNumberOfRows();
            int columns =  game.GetNumberOfColumns(rows);
            Board board = new Board(rows, columns);

            game.DisplayBoard(board);
            
        }
    }
}
