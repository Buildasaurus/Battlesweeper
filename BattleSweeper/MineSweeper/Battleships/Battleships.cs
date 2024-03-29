﻿using Avalonia.Layout;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games.Battleships
{

    
    public class Battleships : IBattleships
    {
        public EventHandler<int>? ShipSunk { get; set; }
		public EventHandler<bool>? GameOver { get; set; }

		public List<int> remainingPieces { get; set; } = new List<int>(new int[] { 4, 3, 2, 2, 2 });
        public Grid<BattleshipTile> Tiles { get; protected set; }
        public List<int> shipLengths { get; set; } = new List<int>();

        public Action<Point>? TileChanged { get; set; }

        int n = 0;
        bool hit = false;
        /// <summary>
        /// Is used when move is called. In two functions to secure correct swap of turns,
        /// when a bomb is hit, and the recursive call is used. To ensure this the hit bool is used.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public MoveResult shoot(Point coord)
        {
            hit = false;
            bool wasIllegal = shootExecution(coord);
            if (hit == true)
            {
                checkWin();
                return MoveResult.Success;
            }
            else if (wasIllegal == false)
                return MoveResult.Illegal;
            else
                return MoveResult.Failure;
        }

        public bool shootExecution(Point coord)
        {
            if (Tiles[coord].hasBeenShot == true)
            {
                return false;
            }
            if (Tiles[coord].ship >= 0) // set ship to hit. and count down how many are left
            {
                remainingPieces[Tiles[coord].ship]--;
				hit = true;
				if (remainingPieces[Tiles[coord].ship] == 0)
                    ShipSunk?.Invoke(this, Tiles[coord].ship);
            }

		    Tiles[coord].hasBeenShot = true;

            if (Tiles[coord].hasBomb == true) //Recursively shooting spots around bombs.
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        Point delta_coord = new(coord.X + dx, coord.Y + dy);
                        if (Tiles.inBounds(delta_coord))
                        {
                            shootExecution(delta_coord);
                        }
                    }
                }
                TileChanged?.Invoke(coord);
                return true;
            }

            if (Tiles[coord].ship >= 0)
            {
                TileChanged?.Invoke(coord);
                return true;
            }
            TileChanged?.Invoke(coord);
            return true;
        }
        
        public MoveResult placeShip(Point coord, bool Vertical)
        {
            ///Checks if all tiles have a ship and whether the move places a ship out of bouds
            if (Vertical == true) 
            {
                if (shipLengths[n] + coord.Y > 10)
                    return MoveResult.Illegal;

                for (int i = 0; i < shipLengths[n]; i++)
                {
                    if (Tiles[coord.X, coord.Y + i].ship >= 0)
                        return MoveResult.Illegal;
                }
            }
                
            else {
                if (shipLengths[n] + coord.X > 10)
                    return MoveResult.Illegal;

                    for (int i = 0; i < shipLengths[n]; i++)
                {
                    if (Tiles[coord.X + i, coord.Y].ship >= 0)
                        return MoveResult.Illegal;
                }
            }
            ///If nothing is in the way the ship is placed
            if (Vertical == true)
            {
                for (int i = 0; i < shipLengths[n]; i++)
                {
                    Tiles[coord.X, coord.Y + i].ship = n;
                    if (i == 0)
                        Tiles[coord.X, coord.Y + i].atStart = true;
                    if (i == shipLengths[n] - 1)
                        Tiles[coord.X, coord.Y + i].atEnd = true;
                    TileChanged?.Invoke(new(coord.X, coord.Y + i));
                }
            }

            else
            {
                for (int i = 0; i < shipLengths[n]; i++)
                {
                    Tiles[coord.X + i, coord.Y].ship = n;
                    if (i == 0)
                        Tiles[coord.X + i, coord.Y].atStart = true;
                    if (i == shipLengths[n] - 1)
                        Tiles[coord.X + i, coord.Y].atEnd = true;
                    Tiles[coord.X + i, coord.Y].horizontal = true;
                    TileChanged?.Invoke(new(coord.X + i, coord.Y));
                }
            }
            ///Increments index for what ship is next to be placed, and returns success.
            n++;
            return MoveResult.Success;
        }

        public void checkWin()
        {
			//check if have won, by checking that there are no ships in shiplenghts.
			if (remainingPieces.Sum() == 0)
				GameOver?.Invoke(this, true);
		}
		public void constructBoard(List<Point> bombPositions, List<int> shipLengths) 
        {
           

            Tiles = new Grid<BattleshipTile>(new System.Drawing.Size(10,10),new BattleshipTile(-1, false, false,false, false, false ));
            this.shipLengths = new List<int>(shipLengths);
            this.remainingPieces = new List<int>(shipLengths);

            foreach (Point bomb in bombPositions)
            {

                Tiles[bomb].hasBomb = true;

            }

        }   
    }

    

}
