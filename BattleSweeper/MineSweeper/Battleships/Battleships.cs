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
        
        List<int> remainingPieces = new List<int>(new int[] { 4, 3, 2, 2, 2 });
        Grid<BattleshipTile> Board; 
        List<int> shipLengths = new List<int>();
        int n = 0;

        /*public MoveResult shoot(Point coord)
        {
            if (Board[coord].hasBeenShot == true)
            {
                return MoveResult.Illegal;
            }
            if (Board[coord].hasBomb == true)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        Point delta_coord = new(coord.X + dx, coord.Y + dy);

                            shoot(delta_coord);
                        
                    }
                }
                return MoveResult.Success;
            }
        
        }
        */
        public MoveResult placeShip(Point coord, bool Vertical)
        {

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
