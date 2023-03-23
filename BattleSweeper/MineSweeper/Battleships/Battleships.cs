using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games.Battleships
{

    
    public class Battleships
    {
        public EventHandler<int>? ShipSunk;
        List<int> remainingPieces = new List<int>(new int[] { 4, 3, 2, 2, 2 });
        Grid<BattleshipTile> Board; 
        List<int> shipLengths = new List<int>();
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
                return MoveResult.Success;
            else if (wasIllegal == false)
                return MoveResult.Illegal;
            else
                return MoveResult.Failure;
        }
        public bool shootExecution(Point coord)
        {
            if (Board[coord].hasBeenShot == true)
            {
                return false;
            }
            
            Board[coord].hasBeenShot = true;

            if (Board[coord].hasBomb == true)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        Point delta_coord = new(coord.X + dx, coord.Y + dy);

                            shootExecution(delta_coord);
                        
                    }
                }
                return true;
            }

            if (Board[coord].ship > 0)
            {
                remainingPieces[Board[coord].ship]--;
                if (remainingPieces[Board[coord].ship] == 0)
                    ShipSunk?.Invoke(this, Board[coord].ship);
                hit = true;
                return true;
            }
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
                    if (Board[coord.Y + i, coord.X].ship > 0)
                        return MoveResult.Illegal;
                }
            }
                
            else {
                if (shipLengths[n] + coord.X > 10)
                    return MoveResult.Illegal;

                    for (int i = 0; i < shipLengths[n]; i++)
                {
                    if (Board[coord.Y, coord.X + i].ship > 0)
                        return MoveResult.Illegal;
                }
            }
            ///If nothing is in the way the ship is placed
            if (Vertical == true)
            {
                for (int i = 0; i < shipLengths[n]; i++)
                    Board[coord.Y, coord.X + i].ship = n;
            }

            else
            {
                for (int i = 0; i < shipLengths[n]; i++)
                    Board[coord.Y + 1, coord.X].ship = n;
            }
            ///Increments index for what ship is next to be placed, and returns success.
            n++;
            return MoveResult.Success;
        }
        public void constructBoard(List<Point> bombPositions) {
           

            Board = new Grid<BattleshipTile>(new System.Drawing.Size(10,10));
            shipLengths = remainingPieces.GetRange(0,remainingPieces.Count);

            foreach (Point bomb in bombPositions)
            {

                Board[bomb].hasBomb = true;

            }

        }   
    }

    

}
